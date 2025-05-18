using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Singelton<CameraController>
{
    private Transform playerTransform;
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = FindObjectOfType<PlayerController>().transform;
        }
    }

    public void SetPlayerCameraFollow()
    {
        if (cinemachineVirtualCamera == null)
        {
            cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }
        cinemachineVirtualCamera.Follow = playerTransform;
    }
    
}

