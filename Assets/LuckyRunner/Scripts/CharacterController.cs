using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private float firstSpeed;
    [SerializeField] private float jumpSpeed;
    
    [SerializeField] private GameObject skateL;
    [SerializeField] private GameObject skateR;
    [SerializeField] private GameObject hammer;
    [SerializeField] private GameObject machete; 
    
    [SerializeField] private float forceValue;
    [SerializeField] private List<GameObject> toolList;
    [SerializeField] private Transform targetTransform;
    
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private GameManager gameManager;
   
    
    private bool inObstacle;
    private bool inGameDice;

    public int FinishIndex;
    public bool IsBot;
    public Tools MyTool;
    public Tools ObstaclePassTool;
    public float inGameSpeed;
    
     
     

     private void Start()
    {
        inGameSpeed = firstSpeed;
        MyTool =Tools.Empty;
        ObstaclePassTool = Tools.Empty;
        var randomTime = Random.Range(5f, 8f);
        var randomStartTime = Random.Range(3f, 5f);
        if (!IsBot) return;
        InvokeRepeating(nameof(RollDice),randomStartTime,randomTime);
    }
    
    void Update()
    {
        if (!InputManager.isStart) return;
            Move();
        // if (IsBot)
        //     StartCoroutine(RandomDice());

       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tool"))
            TriggerDice(other.gameObject);
        if (other.CompareTag("Obstacle"))
            TriggerObstacle(other.gameObject);
        if (other.CompareTag("ObstacleHammer")&& MyTool==Tools.Hammer)
            ObstacleHammerPass();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Exit Obstacle");
            NormalSpeed();
            ObstaclePassTool = Tools.Empty;
            ResetAnims();
        }
    }

    private void Move()
    {
        transform.position += transform.forward * (inGameSpeed * Time.deltaTime);
        characterAnimator.SetBool("isRun", true);
    }
    
    private void CheckTool()
    {
        if (ObstaclePassTool ==Tools.Empty) return;
        

        if (ObstaclePassTool==Tools.Hammer)
        {
            if (ObstaclePassTool!=MyTool)
            {
                ObstacleHammerFail();
            }
        }

        if (ObstaclePassTool==Tools.Skate)
        {
            if (ObstaclePassTool==MyTool)
            {
                ObstacleSkatePass();
            }
            else
            {
                SlowCharacter();
                ObstacleSkateFail();
            }
        }

        if (ObstaclePassTool==Tools.Machete)
        {
            if (ObstaclePassTool==MyTool)
            {
                ObstacleSkatePass();
            }
            else
            {
                SlowCharacter();
                ObstacleMacheteFail();
            }
        }
    }
    private void SlowCharacter()
    {
        Debug.Log("Character Slow");
        inGameSpeed = firstSpeed / 1.5f;
    }
    
    private void NormalSpeed()
    {
        Debug.Log("Character Normal Speed");
        inGameSpeed = firstSpeed;
    }

    private void ResetTools()
    {
        skateL.SetActive(false);
        skateR.SetActive(false);
        hammer.SetActive(false);
        machete.SetActive(false);
        ResetAnims();

    }
    private void ResetAnims()
    {
        characterAnimator.SetBool("isSkateIce",false);
        characterAnimator.SetBool("isIce",false);
        characterAnimator.SetBool("isMachete",false);
    }
    
    private void ObstacleSkatePass()
    {
        Debug.Log("Character Boost");
        inGameSpeed = firstSpeed * 1.25f;
        characterAnimator.SetBool("isSkateIce",true);
    }
    
    private void ObstacleSkateFail()
    {
        Debug.Log("Character Slide Ice");
        characterAnimator.SetBool("isIce",true);
    }
    private void ObstacleMachetePass()
    {
        Debug.Log("Character Machete Attack");
        // characterAnimator.SetBool("isMachete",true);
    }
    
    private void ObstacleMacheteFail()
    {
        Debug.Log("Character Mach Ice");
        characterAnimator.SetBool("isMachete",true);
    }
    
    private void ObstacleHammerPass()
    {
        Debug.Log("Character Jump Attack");
        characterAnimator.SetTrigger("isAttack");
    }
    
    private void ObstacleHammerFail()
    {
        Debug.Log("Character Jump");
        inGameSpeed = firstSpeed/jumpSpeed;
        characterAnimator.SetTrigger("isJump");
    }
    
    private void TriggerDice(GameObject diceGo)
    {
        Debug.Log("Trigger Dice");
        var diceData = diceGo.GetComponent<ToolData>();
        var diceTool = diceData.passTool;
        MyTool = diceTool;
        ResetTools();
        if (MyTool == Tools.Hammer)
            hammer.SetActive(true);
        
        if (MyTool == Tools.Machete)
            machete.SetActive(true);
        
        if (MyTool == Tools.Skate)
        {
            skateL.SetActive(true);
            skateR.SetActive(true);
        }
        Destroy(diceGo.gameObject);
        CheckTool();
        inGameDice = false;
    }
    private void TriggerObstacle(GameObject obstacleGo)
    {
        Debug.Log("Trigger Obstacle");
        var obstacleData = obstacleGo.GetComponent<ToolData>();
        ObstaclePassTool = obstacleData.passTool;
        inObstacle = true;
        CheckTool();
    }

    public void RollDice()
    {
       
        if (inGameDice) return;
        
        Debug.Log("RollDice: RandomTool");
        var randIndex = Random.Range(0, 3);
        
        var dice = Instantiate(toolList[randIndex],targetTransform.position,Quaternion.identity);
        var diceRb = dice.GetComponent<Rigidbody>();
        // diceRb.velocity=Vector3.forward*3;
        diceRb.AddForce(transform.forward*(forceValue*100));
        inGameDice = true;
    }
    IEnumerator RandomDice()
    {
        RollDice();
        yield return new WaitForSecondsRealtime(Random.Range(2, 6));
        
    }

    public void WinAnim()
    {
        characterAnimator.SetTrigger("isFinish");
        characterAnimator.SetBool("isWin",true);
        if (!IsBot)
        {
            playableDirector.Play();
            Invoke(nameof(FinishGame),6f);
        }
            
    }
    public void LoseAnim()
    {
        characterAnimator.SetTrigger("isFinish");
        characterAnimator.SetBool("isWin",false);
        if (!IsBot)
        {
            playableDirector.Play();
            Invoke(nameof(FinishGame),3f);
        }
    }

    private void FinishGame()
    {
        gameManager.FinishGame();
    }
}
