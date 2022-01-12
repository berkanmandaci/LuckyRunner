using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Tools obstacleTool;
    
    private void OnCollisionEnter(Collision other)
    {
        
      



       // Vector3 explosionPos = transform.position;
       //  Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
       //  foreach (Collider collider in colliders)
       //  {
       //      Rigidbody rb = collider.GetComponent<Rigidbody>();
       //      if (rb!=null)
       //      {
       //          rb.AddExplosionForce(force,explosionPos,radius,0.05f,ForceMode.Impulse);
       //      }
       //  }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Character")) return;

        var characterTool = other.GetComponent<CharacterController>().MyTool;
        if (characterTool!=obstacleTool) return;
        Debug.Log("TriggerExplosion");
        var explosionObject = transform.GetChild(transform.childCount-1);
        explosionObject.transform.parent = transform.parent;
        explosionObject.gameObject.SetActive(true);
        Destroy(gameObject);
    }
    
}
