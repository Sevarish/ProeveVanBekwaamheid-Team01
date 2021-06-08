using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    PlayerInput reloadCheck;
    Vector3 oldRotation;

    private void Start()
    {
        reloadCheck = GameObject.FindObjectOfType<PlayerInput>();
        oldRotation = transform.eulerAngles;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 7;
       transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        
        if (reloadCheck.isReloading)
        {
            transform.eulerAngles += new Vector3(0,0,1);
        }
        else
        {
            Vector3 lookAt = new Vector3(transform.position.x, reloadCheck.transform.position.y, transform.position.z);
            transform.LookAt(lookAt);
        }
    }
}
