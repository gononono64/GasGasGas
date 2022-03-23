using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerMovement : NetworkBehaviour
{

    float _moveSpeed;
    [SyncVar] bool isSetup = false;

    Vector3 lastPos;
    // Start is called before the first frame update
    public override void OnStartServer()
    {

    }

   

    public void Setup( float moveSpeed)
    {              
        _moveSpeed = moveSpeed;
        
        
        lastPos = transform.position;     

        isSetup = true; // dont forget to change this back
        
    }   


  
    
    void Update()
    {
        if (!isSetup) return;

        if (isLocalPlayer)
        {           
            HandleMovement();
        }

        if (isServer)
        {
            float distancePlyMovedPerTick = Mathf.Round(Vector3.Distance(lastPos, transform.position) * 1000) / 1000;
            float moveSpeedPerTick = Mathf.Round(_moveSpeed * Time.deltaTime * 1000) / 1000;

            //dirty verification
            if ( distancePlyMovedPerTick > moveSpeedPerTick * 2.0f)
            {
                //this.connectionToClient.Disconnect();
                //use this for verification of player movement  fix the if statement
            }
        }

        lastPos = transform.position;
    }

   

    private void HandleMovement()
    {
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");

        bool movingVert = vert >= 0.15 || vert <= -0.15;
        bool movingHorz = horz >= 0.15 || horz <= -0.15;        

        if (movingVert)        
            transform.position += transform.forward * vert * _moveSpeed * Time.deltaTime;
        if (movingHorz)
            transform.position += transform.right * horz * _moveSpeed * Time.deltaTime;       
        
    }
}

