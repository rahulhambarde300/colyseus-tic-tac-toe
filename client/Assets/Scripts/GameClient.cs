using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using Colyseus.Schema;
using System;
using UnityEngine.UI;

public class GameClient : MonoBehaviour
{
    Room<State> room;
    Client client;
    int numPlayers = 0;
    public GameController gameController;
    public Text statusText;
    public GameObject lobbyPanel;

    public string roomName = "tictactoe";
    private void Awake()
    {
        client = new Client("wss://tactic-toe.herokuapp.com/");
        if(client != null)
        {
            Debug.Log(client);
        }
    }

    public async void connect()
    {
        if (room != null)
            return;
        room = await client.JoinOrCreate<State>(roomName);
        Debug.Log("joined successfully");
        
        
        room.State.players.OnAdd += (string player, string key) => {
            
            numPlayers++;
            if(numPlayers == 1)
            {
                gameController.playerSide = "X";
                //gameController.myTurn = true;
            }
            
            if (numPlayers == 2)
            {
                onJoin();
            }
        };

        room.State.board.OnChange += (float value, int index) =>
        {
            int x = index % 3;
            int y = (int)Mathf.Floor(index / 3);
            gameController.SetValue(x, y, value);
            
        };

        room.State.OnChange += (changes) =>
        {
            changes.ForEach((change) =>
            {
                if(change.Field == "currentTurn")
                {
                    nextTurn(change.Value);
                }
                else if(change.Field == "draw")
                {
                    drawGame();
                }
                else if(change.Field == "winner")
                {
                    Debug.Log("Game Over");
                    showWinner(change.Value);
                }
            });
        };

        




    }

    public void onJoin()
    {
        lobbyPanel.SetActive(false);
        Debug.Log(room.State.currentTurn+ " "+ room.SessionId);
        if(room.State.currentTurn == room.SessionId)
        {
            gameController.playerSide = "O";
        }
        else
        {
            gameController.playerSide = "X";
        }
        gameController.SetBoardInteractable(true);
        
        //gameController.myTurn = false;
        Debug.Log("I Joined");
    }

    async public void onSelect(Transform buttonText)
    {
        GameObject text = buttonText.gameObject;
        GameObject temp;
        int x=-1, y=-1;
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                temp = gameController.buttonList[i,j].gameObject;
                if(temp == text)
                {
                    x = i;
                    y = j;
                    break;
                }
                
            }
            if (x != -1 || y != -1)
            {
                break;
            }
        }
        await room.Send("action", new { x =  x, y = y });
    }

    void nextTurn(object playerId)
    {
        if ((string)playerId == room.SessionId)
        {
            statusText.text = "Your move!";
            gameController.myTurn = true;
            gameController.ChangeSide();
        }
        else
        {
            statusText.text = "Opponent's turn...";
            gameController.myTurn = false;
            gameController.ChangeSide();
        }
    }

    async void drawGame()
    {
        gameController.GameOver("draw");
        await room.Leave();
    }

    async void showWinner(object clientId)
    {
        if((string)clientId == room.State.currentTurn)
        {
            gameController.GameOver(gameController.GetPlayerSide());
        }
        else
        {
            if (gameController.GetPlayerSide() == "X")
                gameController.GameOver("O");
            else
                gameController.GameOver("X");
        }
        
        await room.Leave();
    }
    void onDispose()
    {

    }
}
