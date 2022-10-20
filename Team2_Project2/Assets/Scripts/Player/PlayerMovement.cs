using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;
using System;

public class PlayerMovement : MonoBehaviour
{
    public bool isControllable = true;

    [Header("Movement values")]
    [SerializeField] private float speed = 7f;

    //Input Holder
    public enum InputDevice { Keyboard, Gamepad }
    private InputDevice inputDevice = InputDevice.Keyboard;
    [HideInInspector] public Vector3 moveInput;
    [HideInInspector] public Vector3 aimInput;

    [Header("Components")]
    [SerializeField] private CharacterController controller = null;
    [SerializeField] private Transform model = null;
    [SerializeField] private Transform attackParent = null;
    [SerializeField] private Camera cam = null;

    Vector3 velocity;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float fallMultiplier = 1.5f;
    private bool isGrounded;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform feetPos = null;
    [SerializeField] float groundCheckRadius = .4f;

    public event EventHandler<PlayerMoveEventArgs> OnPlayerMove;

    public TMPro.TMP_Text debugText;

    public enum MoveDirection { Idle, Foward, Backward, Right, Left }
    public MoveDirection curDirection;

    public class PlayerMoveEventArgs : EventArgs
    {
        public Vector2 moveDirection;
        public MoveDirection curDirection;

        public PlayerMoveEventArgs(Vector2 input, MoveDirection dir)
        {
            moveDirection = input;

            curDirection = dir;
        }
    }

    #region Input Handlers
    public void Move(CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector3(input.x, 0f, input.y);
    }

    public void Aim(CallbackContext context)
    {
        if (inputDevice == InputDevice.Keyboard)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if(Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                Vector3 input = raycastHit.point;
                aimInput = new Vector3(input.x, 0f, input.z);
            }
        }
        else
        {
            Vector2 input = context.ReadValue<Vector2>();
            aimInput = new Vector3(input.x, 0f, input.y);
        }
    }
    #endregion
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    private void Update()
    {
        #region Visual
        if (isControllable)
        {
            if (aimInput.magnitude > 0f)
            {
                if (inputDevice == InputDevice.Gamepad)
                {

                    Vector3 aim = new Vector3(transform.position.x + aimInput.x, 0 , transform.position.y + aimInput.y);
                    Vector3 vectorToTarget = aim - model.position;
                    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                    Quaternion qt = Quaternion.AngleAxis(angle - 90, Vector3.up);

                    model.rotation = qt;
                    attackParent.rotation = qt;
                }
                else if (inputDevice == InputDevice.Keyboard)
                {
                    Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                    if (Physics.Raycast(ray, out RaycastHit raycastHit))
                    {
                        Vector3 input = raycastHit.point;
                        aimInput = new Vector3(input.x, 0f, input.z);
                    }

                    Vector3 lookDir = aimInput - transform.position;
                    float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
                    Quaternion qt = Quaternion.AngleAxis(angle, Vector3.up);

                    model.rotation = qt;
                    attackParent.rotation = qt;
                }
            }
        }
        #endregion

        isGrounded = Physics.CheckSphere(feetPos.position, groundCheckRadius, whatIsGround);

        if (!isGrounded)
        {
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (isControllable)
        {
            Vector3 lastPostion = transform.position;

            controller.Move(moveInput * speed * Time.deltaTime + new Vector3(0.0f, velocity.y, 0.0f));

            Vector2 faceDirection = new Vector2(model.forward.z, model.forward.x);

            Vector3 dir = (transform.position - lastPostion)*10;

            Vector2 moveDirection = Vector2.zero;

            float temp = faceDirection.x + faceDirection.y;

            if(temp > 0)
            {
                if (faceDirection.x > faceDirection.y)
                {
                    //North
                    moveDirection = new Vector2(dir.z, dir.x);
                }
                else if (faceDirection.x < faceDirection.y)
                {
                    //East
                    moveDirection = new Vector2(dir.x, dir.z);
                }
            }
            else if(temp < 0)
            {
                if (faceDirection.x > faceDirection.y)
                {
                    //South
                    moveDirection = new Vector2(dir.x, dir.z) * -1;
                }
                else if (faceDirection.x < faceDirection.y)
                {
                    //West
                    moveDirection = new Vector2(dir.z, dir.x) * -1;
                }
            }

            #region temp
            if (moveDirection.magnitude <= 0)
            {
                curDirection = MoveDirection.Idle;
                
            }
            if (moveDirection.x > 0 && moveDirection.y == 0)
            {
                curDirection = MoveDirection.Foward;
            }
            if (moveDirection.x < 0 && moveDirection.y == 0)
            {
                curDirection = MoveDirection.Backward;
            }
            if (moveDirection.x == 0 && moveDirection.y < 0)
            {
                curDirection = MoveDirection.Left;
            }
            if(moveDirection.x == 0 && moveDirection.y > 0)
            {
                curDirection = MoveDirection.Right;
            }
            #endregion

            DebugText(curDirection + "\nMove Direction:" + moveDirection + "\nFacing Direction:" + faceDirection + "\nTest: " + (faceDirection.x + faceDirection.y));

            if (OnPlayerMove != null) OnPlayerMove(this, new PlayerMoveEventArgs(moveDirection, curDirection));
        }
    }

    public void DebugText(string text)
    {
        if (debugText != null)
        {
            debugText.text = text;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(model.position, aimInput);
        //Gizmos.DrawWireSphere(aimInput, .5f);
    }
}
