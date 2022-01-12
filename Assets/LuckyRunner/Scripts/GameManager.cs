using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIController uıController;
    [SerializeField] private float levelLenght;
    [SerializeField] private List<GameObject> obstacleList;
    [SerializeField] private GameObject finishGo;
   

    private void Start()
    {
       // CreateLevel();
    }

    public void StartGame()
    {
        if (InputManager.isStart) return;
        
        uıController.StartUI();
        
        InputManager.isStart = true;
        Debug.Log("Game Start");
        
    }
    
    public void FinishGame()
    {
        if (!InputManager.isStart) return;
        
        uıController.FinishUI();
        
        InputManager.isStart = false;
        Debug.Log("Finish Game");
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene("LuckyRunner");
    }

    private void CreateLevel()
    {
        var instancetePos = new Vector3(-1.5f, 0, 30);
        while (levelLenght>0)
        {
            var obstacleIndex = Random.Range(0, obstacleList.Count);
            var randomObstacle = obstacleList[obstacleIndex];
            Instantiate(randomObstacle, instancetePos, Quaternion.identity);
            instancetePos.z += 30;
            levelLenght--;
        }

        instancetePos.z -= 25;
        Instantiate(finishGo, instancetePos, Quaternion.identity);
    }
}
