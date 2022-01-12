using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject diceButton;
    [SerializeField] private GameObject winGameObject;
    [SerializeField] private GameObject loseGameObject;

    public void StartUI()
    {
        startButton.SetActive(false);
        diceButton.SetActive(true);
    }

    public void FinishUI()
    {
        if (InputManager.isWin)
        {
            winGameObject.SetActive(true);
        }
        else
        {
            loseGameObject.SetActive(true);
        }
    }

    
    
}
