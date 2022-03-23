using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : NetworkBehaviour
{
    public NetworkConnectionToClient conn;
    public GameObject model;
    [SerializeField] float _moveSpeed = 20f;
    [SerializeField] [SyncVar] public Vector3 startPos;
    [SyncVar] public bool hasInitiallySpawned = false;
    public Health health;
    PlayerMovement _movement;
    MenuManager _menuManager;



    // Start is called before the first frame update
    private void Awake()
    {
        _menuManager = GetComponent<MenuManager>();
        health = GetComponent<Health>();
        health.OnRevive += () => { Revive(); };
        health.OnDeath += () => { Dies(); };
    }


    void Start()
    {
        if (!isLocalPlayer) return;     

        _movement = GetComponent<PlayerMovement>();
        _movement.Setup(_moveSpeed);       

        hasInitiallySpawned = false;

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    

    [Server]
    public void InitialSpawn() // from server
    {
        Debug.Log(conn.connectionId);
        
        TargetInitialSpawn(startPos);        
        hasInitiallySpawned = true;
    }

    [TargetRpc]
    public void TargetInitialSpawn(Vector3 startPos)    
    {
        Teleport(startPos);      
        _menuManager.EnableSpawnMenu();
        //transform.rotation = startPos.rotation;
    }

    [Client]
    public void Respawn()
    {            
        Teleport(startPos);
        health.CmdReset();
        _menuManager.DisableSpawnMenu();

    }

    [Client]
    public void Teleport(Vector3 point)
    {           
        transform.position = point;
        //transform.rotation = point.rotation;
    }

    private void Dies()
    {
        model.SetActive(false);
        if(isClient)
            _menuManager.EnableSpawnMenu();
    }

    private void Revive()
    {
        model.SetActive(true);
    }



}
