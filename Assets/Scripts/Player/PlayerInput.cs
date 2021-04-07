using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]Player player; //The parent object of the entire player.
    [SerializeField]GameObject playerVisual; //The visual part of the player which rotates.

    [SerializeField]GameObject targetPoint; //The object which the player will always rotate towards. AKA targetting point.

    [SerializeField]private bool controller = false; //While true, the input will go via a Controller (PS4 Controller).
    public float sensitivity = 20; //Sensitivity of aiming with the controller

    [SerializeField]
    private int groundLayer = 7;

    [SerializeField]
    private float interactRange = 4;

    [SerializeField]
    private KeyCode interactButton = KeyCode.Space;

    //Keyboard/Mouse
    private readonly KeyCode moveLeft = KeyCode.A,
    moveRight = KeyCode.D,
    moveUp = KeyCode.W,
    moveDown = KeyCode.S;
    //Controller
    private readonly string conMoveSide = "LR", //Left Right for walking
    conMoveFrontBack = "FB", //Front Back for walking
    conAimLR = "AimLR", //Left Right for aiming
    conAimFB = "AimFB"; //Front Back for aming

    void Update()
    {
        if (!controller) {
            if (Input.GetKey(moveLeft)) { MoveLeft(); }
            if (Input.GetKey(moveRight)) { MoveRight(); }
            if (Input.GetKey(moveUp)) { MoveUp(); }
            if (Input.GetKey(moveDown)) { MoveDown(); }
            Aim();
        } else
        {
            MoveController();
            AimController();
        }

        if (Input.GetKeyDown(interactButton))
        {
            Interact();
        }
    }
    //MOUSE AND KEYBOARD
    //Movement for Mouse and Keyboard input. Will run when controller is false.
    private void MoveLeft() { player.transform.Translate(-player.speed * Time.deltaTime, 0, 0); }
    private void MoveRight() { player.transform.Translate(player.speed * Time.deltaTime, 0, 0); }
    private void MoveUp() { player.transform.Translate(0, 0, player.speed * Time.deltaTime); }
    private void MoveDown() { player.transform.Translate(0, 0, -player.speed * Time.deltaTime); }

    //Moves the aim (crosshair) to the mouse's world position. Also rotates the player's visual object towards the crosshair.
    private void Aim()
    {
        //Create and invert the layermask.
        int layerMask = 1 << groundLayer;
        //layerMask = ~layerMask;

        //Casts a raycast from the camera to the mouse position and moves the targetting point to the mouse's location.
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
        {
            targetPoint.transform.position = new Vector3(hit.point.x,
                                                 hit.point.y + 1,
                                                 hit.point.z);
        }

        //Rotate the visual object of the player towards the targetting point.
        Vector3 tarDir = targetPoint.transform.position - playerVisual.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(playerVisual.transform.forward, tarDir, 1, 0.0f);
        playerVisual.transform.rotation = Quaternion.LookRotation(newDirection);
        playerVisual.transform.localEulerAngles = new Vector3(0, playerVisual.transform.localEulerAngles.y, 0);
    }

    //CONTROLLER
    //Movement for Controller input. Will run when controller is true.
    private void MoveController() {
        player.transform.Translate(Input.GetAxis(conMoveSide) * player.speed * Time.deltaTime,
                                   0,
                                   -Input.GetAxis(conMoveFrontBack) * player.speed * Time.deltaTime);
    }

    //Moves the aim (crosshair) across the screen. Also rotates the player's visual object towards the crosshair.
    private void AimController()
    {
        targetPoint.transform.Translate(Input.GetAxis(conAimLR) * sensitivity * Time.deltaTime,
                                        0,
                                       -Input.GetAxis(conAimFB) * sensitivity * Time.deltaTime);

        //Rotate the visual object of the player towards the targetting point.
        Vector3 tarDir = targetPoint.transform.position - playerVisual.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(playerVisual.transform.forward, tarDir, 1, 0.0f);
        playerVisual.transform.rotation = Quaternion.LookRotation(newDirection);
        playerVisual.transform.localEulerAngles = new Vector3(0, playerVisual.transform.localEulerAngles.y, 0);
    }

    private void Interact()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, interactRange);
        
        foreach (RaycastHit hit in hits)
        {
            hit.transform.gameObject.GetComponent<Interactable>()?.Interact(transform);
        }
    }
}
