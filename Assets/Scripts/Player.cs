using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController characterController;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float pushPower = 5f;
    private float yDirection = 0;

    [Header("Mouse Rotation")]
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private GameObject FPSCamera;
    private float xRotation = 0f;

    [Header("Items")]
    [SerializeField] private int ballCount = 0;
    [SerializeField] private int greenCubeCount = 0;
    [SerializeField] private int yellowCubeCount = 0;
    [SerializeField] UIManager uIManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        //Lock Cursor in game view
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        MouseRotation();
        FireRayCast();
    }

    private void FireRayCast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(FPSCamera.transform.position, FPSCamera.transform.forward);
            Debug.DrawRay(FPSCamera.transform.position, FPSCamera.transform.forward * 10, Color.red);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("Ball"))
                {
                    ballCount++;
                    uIManager.UpdateBallScoreUI(ballCount);
                    Destroy(hit.transform.gameObject);
                    // Debug.Log(hit.distance);
                } else if(hit.transform.CompareTag("Green Cube") && hit.distance < 3)
                {
                    greenCubeCount++;
                    uIManager.UpdateGreenCubeScoreUI(greenCubeCount);
                    Destroy(hit.transform.gameObject);
                } else if(hit.transform.CompareTag("Yellow Cube") && hit.distance < 3)
                {
                    yellowCubeCount++;
                    uIManager.UpdateYellowCubeScoreUI(yellowCubeCount);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    private void Movement()
    {
        // Input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Direction
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);

        if (characterController.isGrounded)
        {
            // Reset yDirection
            yDirection = -0.5f;

            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yDirection = jumpPower;
            }
        }

        // Gravity
        yDirection += gravity * Time.deltaTime;

        // Velocity
        Vector3 velocity = direction * moveSpeed;
        velocity.y = yDirection;

        // Change direction to face the rotation of the object
        velocity = transform.TransformDirection(velocity);

        //Movement
        characterController.Move(velocity * Time.deltaTime);
    }

    private void MouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Up and Down Rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        FPSCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Left and Right Rotation
        transform.Rotate(Vector3.up, mouseX);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ball"))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.linearVelocity = pushDirection * pushPower;
        }
    }
}
