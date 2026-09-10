using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestMove : MonoBehaviour
{
    public Camera cam;
    public float moveSpeed;
    private CharacterController characterController;
    public InputSystem_Actions inputActions;
    private float gravity = -9.41f;
    private float gravityMultiplier = 1.75f;
    private bool grounded;
    private Vector3 velocity;
    private Vector2 move; //x, y

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Gravity();
    }

    void Movement()
    {
        move = inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 cameraForward = cam.transform.forward;
        cameraForward.y = 0; //move horizontal
        float forwardInput = move.y;

        Vector3 forwardDirection = cameraForward.normalized * forwardInput;
        Vector3 direction = (forwardDirection * moveSpeed) + (velocity.y * Vector3.up);

        characterController.Move(direction * Time.deltaTime);

        if (move.sqrMagnitude > 0.1)
        {
            this.transform.rotation = Quaternion.LookRotation(forwardDirection);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        try
        {
            //Debug.Log("Hit: " + hit.point);

            if (hit.point.y >= 1f)
            {
                Destroy(this.gameObject);
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log($"Object was destroyed, this was the error: {ex}"); // not functional because object is destroyed
        }  
    }

    void Gravity()
    {
        grounded = characterController.isGrounded;

        if (grounded)
        {
            velocity.y += 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;

            /*
            if (characterController.velocity.y < 0)
            {
                velocity.y += gravity * gravityMultiplier * Time.deltaTime;
            }
            */
        }
    }

}
