using System.Collections.Generic;
using UnityEngine;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class CameraHandler : NetworkBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] float _camMoveSpeed;

    private void Start()
    {
        DisableNonLocalPlayerCameras();
    }
    void Update()
    {
        if (isLocalPlayer)
        {
            HandleCamera();
        }
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

    public void DisableNonLocalPlayerCameras()
    {
        if (!isLocalPlayer)
        {
            _camera.gameObject.SetActive(false);
            return;
        }
    }
}
