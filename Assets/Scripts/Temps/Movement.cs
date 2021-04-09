using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 3;
    public float interactRange;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, interactRange);
            foreach (RaycastHit hit in hits)
            {
                hit.transform.gameObject.GetComponent<Interactable>()?.Interact(transform);
            }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = transform.position + (transform.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = transform.position + (-transform.forward * Time.deltaTime * speed);
        }
    }
}
