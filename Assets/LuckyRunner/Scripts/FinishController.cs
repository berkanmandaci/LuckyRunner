using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FinishController : MonoBehaviour
{



    [SerializeField] private GameObject fireWork;
    
    private int _finishIndex=1;

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Character")) return;
        
            var characterController = other.GetComponent<CharacterController>();
           
            if (characterController.IsBot&& _finishIndex==1)
                InputManager.isWin = false;
            else
            {
                InputManager.isWin = true;
                fireWork.SetActive(true);
            }
               

            characterController.FinishIndex = _finishIndex;
            characterController.inGameSpeed = 0;

            if (_finishIndex==1)
                characterController.WinAnim();
            else
                characterController.LoseAnim();

           
            
            _finishIndex++;
    }
}
