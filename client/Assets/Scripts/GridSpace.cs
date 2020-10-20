using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    public GameClient gameClient;

    private GameController gameController;

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
        gameClient = FindObjectOfType<GameClient>();
    }

    public void SetSpace()
    {
        if (gameController.myTurn)
        {
            buttonText.text = gameController.GetPlayerSide();
            button.interactable = false;
            gameController.EndTurn();
            gameClient.onSelect(transform.GetChild(0));

            gameController.myTurn = false;
        }
        
    }

    
}
