using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("Movement")]
    CharacterController Player;
    private float moveSpeed = 10f;
    private float runSpeed = 15f;
    private float gravity = -9.81f;
    public float Speed;
    bool readyToJump;
    public float jumpCooldown = 0.75f;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask Conveyor;
    bool isGrounded;
    bool isConveyored;
    private float velocity = 0;
    public float jumpHeight = 3f;

    void Start()
    {
        Application.targetFrameRate = 240;
        Player = GetComponent<CharacterController>();
        readyToJump = true;

    }
    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        isConveyored = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, Conveyor);
    }
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (Player.isGrounded)
        {
            velocity = 0f;
            if (Input.GetKey(jumpKey))
            {
                if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
                {
                    readyToJump = false;
                    velocity = jumpHeight;
                    Invoke(nameof(ResetJump), jumpCooldown);
                }
            }
        }
        else
        {
            velocity += gravity * Time.deltaTime;
        }
        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1) move.Normalize();
        Player.Move(move * Speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = runSpeed;
        }
        else
        {
            Speed = moveSpeed;
        }
        if (isConveyored) Player.Move(new Vector3(2f, 0, 0) * Time.deltaTime);
        Player.Move(new Vector3(0, velocity, 0));
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}