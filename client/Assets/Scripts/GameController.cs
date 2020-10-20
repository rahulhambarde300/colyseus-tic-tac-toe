using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class PlayerImage
{
    public Image panel;
    public TMPro.TextMeshProUGUI text;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor; 
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public Text[] buttons;
    public Text[,] buttonList;
    public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI gameOverText;
    public PlayerImage playerX; 
    public PlayerImage playerO; 
    public PlayerColor activePlayerColor; 
    public PlayerColor inactivePlayerColor;
    public bool myTurn;

    public string playerSide;
    private int moveCount;

    private void Awake()
    {
        
        playerSide = "X";
        gameOverPanel.SetActive(false);
        moveCount = 0;

        SetPlayerColors(playerX, playerO);
        buttonList = new Text[3,3];
        int index = 0;
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                buttonList[i,j] = buttons[index];
                index++;
            }
        }
        SetGameControllerReferenceOnButtons();
    }


    void SetGameControllerReferenceOnButtons()
    {
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
                buttonList[i,j].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void ChangeSide()
    {
        playerSide = playerSide == "X" ? "O" : "X";
        if(playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }

    public void SetValue(int i,int j, float value)
    {
       
        if((int)value == 1)
        {
            buttonList[i,j].text = "X";
        }
        else if((int)value == 2)
        {
            buttonList[i,j].text = "O";
        }
        buttonList[i, j].GetComponentInParent<Button>().interactable = false;

    }

    void SetPlayerColors(PlayerImage newPlayer, PlayerImage oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;

        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    public void EndTurn()
    {
        moveCount++;  
    }

    public void GameOver(string winningPlayer)
    {
        if(winningPlayer == "draw")
            SetGameOverText("It's a draw !!");
        else
            SetGameOverText("Player " + winningPlayer + " wins!!");
    }

    void SetGameOverText(string value)
    {
        SetBoardInteractable(false);
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    public void RestartGame()
    {
        playerSide = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);

        SetBoardInteractable(true);
        for (int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                buttonList[i,j].text = "";
            }
            
        }
        SetPlayerColors(playerX, playerO);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetBoardInteractable(bool value)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                buttonList[i,j].GetComponentInParent<Button>().interactable = value;
            }
            
        }
    }
}
