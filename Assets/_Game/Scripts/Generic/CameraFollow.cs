using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    private Camera playerCam;

    [SerializeField]
    private float cameraHeight = 12.55f;
    private void Awake()
    {
        playerCam = Camera.main;
    }

    void FixedUpdate()
    {
        playerCam.transform.position = Vector3.Lerp(playerCam.transform.position, new Vector3(transform.position.x, cameraHeight, transform.position.z), 0.1f);
    }
}
