using UnityEngine;
using UnityEngine.InputSystem;

public class TestMove : MonoBehaviour
{
    public Camera cam;
    private CharacterController characterController;
    public InputSystem_Actions inputActions;
    public GameObject explosion;
    public float moveSpeed;
    private float gravity = -9.41f;
    private int health = 2;
    public int damage;
    //private float gravityMultiplier = 1.75f;
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

    void OnDisable()
    {
        inputActions.Disable();
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
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

        if (move.sqrMagnitude > 0.1)
        {
            moveSpeed = 40.0f;

            if (move.y == -1)
            {
                moveSpeed = 5.0f;
            }
        }
        else
        {
            moveSpeed = 15.0f;
        }

        Vector3 direction = (cameraForward * moveSpeed) + (velocity.y * Vector3.up);
        characterController.Move(direction * Time.deltaTime);

        /////////////////////////////////////////////////////////////////////////////////////
        Vector3 Xinput = Vector3.zero;

        if (Keyboard.current.dKey.isPressed)
        {
            Xinput += transform.InverseTransformDirection(Vector3.right);
        }

        if (Keyboard.current.aKey.isPressed)
        {
            Xinput += transform.InverseTransformDirection(Vector3.left);
        }
        
        BikeRotation(cameraForward, Xinput);
        Debug.DrawLine(transform.position, transform.position + Xinput * 1.5f, Color.blue, 0.01f);
    }

    void BikeRotation(Vector3 dir, Vector3 rightDir)
    {
        /*
        Vector3 cameraRight = cam.transform.right;
        cameraRight = cameraRight.normalized;
        cameraRight.y = 0;

        Vector3 rightDirection = cameraRight * rightDir.x;

        float aroundZleft = rightDirection.x * -45.0f;
        */

        Quaternion orignalRotation = Quaternion.identity;
        Quaternion target = Quaternion.LookRotation(dir);
        Quaternion currentRotation = Quaternion.Slerp(this.transform.rotation, target, Time.deltaTime * 15.0f);

        transform.rotation = currentRotation;

        /*
        if (rightDir.sqrMagnitude > 0.1)
        {
            Quaternion localRot = Quaternion.Euler(0, 0, aroundZleft);
            currentRot = Quaternion.Slerp(transform.rotation, currentRot, Time.deltaTime * 5.0f);
        }

        if (rightDir.sqrMagnitude < 0.1)
        {
            Quaternion localRot = orignalRotation;
            currentRot = Quaternion.Slerp(transform.rotation, currentRot, Time.deltaTime * 2.5f);
        }

        //Debug.Log($"{rightDir.x} + {rightDir.sqrMagnitude}");
        */
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.point.y >= 1f)
        {
            this.gameObject.SetActive(false);
            Instantiate(explosion, hit.point, Quaternion.identity);
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
