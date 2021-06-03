using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float force = 2;
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += -transform.right * Time.deltaTime * force;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * Time.deltaTime * force;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.forward * Time.deltaTime * force;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += -transform.forward * Time.deltaTime * force;
        }
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.LookAt(new Vector3(mousepos.x, mousepos.y, 0));
    }
}
