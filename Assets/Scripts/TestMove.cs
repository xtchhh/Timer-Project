using UnityEngine;
using UnityEngine.InputSystem;

public class TestMove : MonoBehaviour
{
    public Camera cam;
    private CharacterController characterController;
    public InputSystem_Actions inputActions;
    public GameObject explosion;
    public AudioSource engine;
    public AudioSource gearShift;
    public AudioSource howl;
    public AudioSource explosionSound;
    private float moveSpeed;
    private float gravity = -9.41f;
    private bool grounded;
    private bool canPress = true;
    public static bool canRide = false;
    private Vector3 velocity;
    private Vector2 move;

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

        if (Keyboard.current.eKey.wasPressedThisFrame && canPress)
        {
            canRide = true;
            canPress = false;
            
            engine.volume = 1f;
            howl.volume = 1f;
            gearShift.volume = 1f;
            gearShift.Play();
            engine.Play();
        }

        if (canRide == true)
        {
            moveSpeed = 25f;
            engine.pitch = 1f;

            if (move.y >= 1)
            {
                engine.pitch = 1.25f;
                moveSpeed = 50f;
                howl.Play();
                gearShift.Play();
            }

            if (move.y == -1)
            {
                moveSpeed = 15f;
                engine.pitch = 0.75f;
                gearShift.Play();
            }
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
        if (hit.point.y > 0.8f)
        {
            canRide = false;
            this.gameObject.SetActive(false);

            explosionSound.Play();
            engine.volume = 0f;
            howl.volume = 0f;
            gearShift.volume = 0f;

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
        }
    }
}
