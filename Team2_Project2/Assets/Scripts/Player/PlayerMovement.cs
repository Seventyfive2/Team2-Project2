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

    public class PlayerMoveEventArgs : EventArgs
    {
        public enum moveDirection { Foward, Backward, Sideways }
        public moveDirection curDirection;

        public PlayerMoveEventArgs(Vector2 direction)
        {
            
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
            controller.Move(moveInput * speed * Time.deltaTime + new Vector3(0.0f, velocity.y, 0.0f));

            Vector2 moveDirection = Vector2.zero;
            if (OnPlayerMove != null) OnPlayerMove(this, new PlayerMoveEventArgs(moveDirection));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(model.position, aimInput);
        //Gizmos.DrawWireSphere(aimInput, .5f);
    }
}
