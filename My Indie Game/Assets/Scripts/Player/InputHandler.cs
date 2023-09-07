using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float fallingDuration = 1f;
    [SerializeField] float gravityMultiplier = 1;
    [SerializeField] float smoothTime = .05f;
    private Status status;
    private Vector2 input = new Vector2();
    private float velocity;
    private const float GRAVITY = -9.8f;
    private float currentVelocity;
    private StateMachine stateMachine;
    private PlayerInputActions playerInputActions;
    public bool IsGrounded() => status.player.characterController.isGrounded;

    public bool isFalling = false;

    [HideInInspector] public Vector3 direction = new Vector3();
    [HideInInspector] public bool hasPressedJumpButton = false;
    [HideInInspector] public int jumpCount = 0;
    [HideInInspector] public bool hasEndedLanding = false;

    private void Awake()
    {
        status = Status.instance;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        var idle =
            new IdleGrounded(status, this);
        var moving =
            new Moving(status, this);
        var jumpInPlace =
            new JumpingInPlace(status, this);
        var jumpMoving =
            new JumpingMoving(status, this);
        var doubleJumpig =
            new DoubleJumpig(status, this);
        var landing =
            new Land(status, this);
        var falling
            = new Falling(status, this);

        stateMachine = new StateMachine();

        void AddTransition(IState from, IState to,
            Func<bool> condition) =>
            stateMachine.AddTransition
            ((IState)from, (IState)to, condition);

        AddTransition
            (idle, moving, PlayerHasMovementInput());
        AddTransition
            (idle, jumpInPlace, ShouldJump());

        AddTransition
            (moving, idle, PlayerHasNoMovementInput());
        AddTransition
            (moving, jumpMoving, ShouldJump());

        AddTransition
            (jumpInPlace, idle, () => IsGrounded() && jumpCount == 0);
        AddTransition
            (jumpInPlace, doubleJumpig, ShouldDoubleJump());

        AddTransition
            (jumpMoving, idle, ShouldLandIdle());
        AddTransition
            (jumpMoving, doubleJumpig, ShouldDoubleJump());
        AddTransition
             (jumpMoving, moving, ShouldLandInMovement());

        AddTransition
            (doubleJumpig, idle, () => input.sqrMagnitude <= 0 &&
            IsGrounded() && jumpCount == 0);
        AddTransition
            (doubleJumpig, moving, ShouldLandInMovement());

        AddTransition
            (moving, falling, () => jumpCount == 0 && !IsGrounded() && isFalling);
        AddTransition
            (doubleJumpig, falling, () => jumpCount == 0 && !IsGrounded());
        AddTransition
            (idle, falling, () => jumpCount == 0 && !IsGrounded() && isFalling);

        AddTransition
            (falling, landing, ShouldLand());


        AddTransition
            (landing, idle, () => hasEndedLanding && input.sqrMagnitude <= 0);
        AddTransition
            (landing, moving, () => hasEndedLanding && input.sqrMagnitude > 0);

        Func<bool> PlayerHasMovementInput() => () =>
        input.sqrMagnitude > 0;
        Func<bool> PlayerHasNoMovementInput() => () =>
        input.sqrMagnitude <= 0;
        Func<bool> ShouldJump() => () =>
        IsGrounded() && hasPressedJumpButton && jumpCount == 0;
        Func<bool> ShouldLandIdle() => () =>
        IsGrounded() && jumpCount == 0 && input.sqrMagnitude <= 0
        || IsGrounded() && input.sqrMagnitude <= 0;
        Func<bool> ShouldDoubleJump() => () =>
        hasPressedJumpButton && jumpCount == 1;
        stateMachine.SetState(idle);
        Func<bool> ShouldLandInMovement() => () => input.sqrMagnitude > 0 &&
        IsGrounded() && jumpCount == 0 || input.sqrMagnitude > 0 && IsGrounded();
        Func<bool> ShouldLand() => () => IsGrounded() && jumpCount == 0;
    }

    private void Update()
    {
        stateMachine.Tick();
    }

    public void GetInput()
    {
        input = playerInputActions.Player.Move.ReadValue<Vector2>();
        status.uiController.SetPositionFocusUI(GetJoystickDirection(input));

    }

    #region Left Stick
    public void GetDirection()
    {
        GetInput();
        direction = new Vector3(input.x, 0, input.y);
    }

    public void ApplyMovement()
    {
        status.player.characterController.Move(direction *
            status.player.moveSpeed *
            Time.deltaTime);
    }

    public void ApplyRotation()
    {
        if (input.sqrMagnitude == 0) return;
        var tangetAngle = Mathf.Atan2(direction.x,
            direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(status.player.transform.
            eulerAngles.y, tangetAngle, ref currentVelocity,
            smoothTime);
        status.player.transform.rotation =
            Quaternion.Euler(0, angle, 0);
    }

    public void ApplyAllMovement()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
    }


    public void ApplyGravity()
    {
        if (IsGrounded() && velocity < 0.0f)
            velocity = -1;

        else
            velocity += GRAVITY * gravityMultiplier * Time.deltaTime;

        direction.y = velocity;
    }

    private Direction GetJoystickDirection(Vector2 input)
    {
        if (input.x > 0)
        {
            if (input.y > 0) return Direction.UpRight;
            else return Direction.DownLeft;
        }
        else if (input.x < 0)
        {
            if (input.y > 0) return Direction.UpLeft;
            else return Direction.DownRight;
        }
        else return Direction.Default;
    }
    #endregion

    #region Buttons

    public void Jump(float hight)
    {
        velocity = Mathf.Sqrt(hight * -2 * GRAVITY);
    }

    public void JumpButton()
    {
        if (jumpCount == 0 || jumpCount == 1)
            hasPressedJumpButton = true;
        else return;
    }
    #endregion
}
