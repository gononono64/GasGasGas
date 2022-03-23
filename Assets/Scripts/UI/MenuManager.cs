using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : NetworkBehaviour
{
    public Canvas canvas;
    public GameObject SpawnMenu;
    private void Start()
    {        
        if (!isLocalPlayer)
        {
            DisableCanvas();
        }        
    }

    public void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetButtonDown("Toggle Mouse Lock"))
        {
            ToggleMouseLock();
        }
    }

    public void ToggleMouseLock()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.None;
        else if(Cursor.lockState == CursorLockMode.None)
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisableCanvas()
    {
        canvas.gameObject.SetActive(false);
    }

    public void DisableSpawnMenu()
    {
        if (SpawnMenu.activeSelf)        
            SpawnMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Disabled");
    }
    public void EnableSpawnMenu()
    {
        if (!SpawnMenu.activeSelf)        
            SpawnMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Enabled");
    }
}
