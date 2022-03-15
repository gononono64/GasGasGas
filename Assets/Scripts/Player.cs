using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Player : NetworkBehaviour
{

    [SerializeField] Camera _camera;
    [SerializeField] float _camMoveSpeed = 20;
    [SerializeField] float _moveSpeed = 20;


    Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            _camera.gameObject.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Locked;
        lastPos = transform.position;
    }

   
    
    void Update()
    {   
        if (isLocalPlayer)
        {
            HandleCamera();
            HandleMovement();
        }

        if (isServer)
        {
            float distancePlyMovedPerTick = Mathf.Round(Vector3.Distance(lastPos, transform.position) * 1000) / 1000;
            float moveSpeedPerTick = Mathf.Round(_moveSpeed * Time.deltaTime * 1000) / 1000;

            if ( distancePlyMovedPerTick > moveSpeedPerTick * 2.0f)
            {
                this.connectionToClient.Disconnect();
            }
        }

        lastPos = transform.position;
    }

    float rotationX = 0;
    float rotationY = 0;
    private void HandleCamera()
    {
        rotationX += Input.GetAxis("Mouse X") * Time.deltaTime * _camMoveSpeed;
        rotationY += Input.GetAxis("Mouse Y") * Time.deltaTime * _camMoveSpeed;
        rotationY = Mathf.Clamp(rotationY, -90, 90);        
        _camera.transform.rotation = Quaternion.Euler(-rotationY, _camera.transform.rotation.eulerAngles.y, _camera.transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(0, rotationX, 0);
        
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

