using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] float gravityMultiplier = 1;
    [SerializeField] float smoothTime = .05f;
    private Status status;
    private Vector3 direction = new Vector3();
    private Vector2 input = new Vector2();
    private float velocity;
    private const float GRAVITY = -9.8f;
    private float currentVelocity;
    private StateMachine stateMachine;
    private PlayerInputActions playerInputActions;
    private bool IsGrounded() => status.player.characterController.isGrounded;

    public bool hasPressedJumpButton = false;
    public bool canJump = true;

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

        stateMachine = new StateMachine();

        void AddTransition(IState from, IState to,
            Func<bool> condition) =>
            stateMachine.AddTransition
            ((IState)from, (IState)to, condition);

        AddTransition
            (idle, moving, PlayerHasMovementInput());
        AddTransition
            (moving, idle, PlayerHasNoMovementInput());
        AddTransition
            (idle, jumpInPlace, ShouldJump());
        AddTransition
            (moving, jumpMoving, ShouldJump());
        AddTransition
            (jumpInPlace, idle, () => IsGrounded() && canJump);
        AddTransition
            (jumpMoving, idle, ShouldLandAfterJumping());


        Func<bool> PlayerHasMovementInput() => () =>
        input.sqrMagnitude > 0;
        Func<bool> PlayerHasNoMovementInput() => () =>
        input.sqrMagnitude <= 0;
        Func<bool> ShouldJump() => () =>
        IsGrounded() && hasPressedJumpButton && canJump;
        Func<bool> ShouldLandAfterJumping() => () =>
        IsGrounded() && canJump || IsGrounded();
        stateMachine.SetState(idle);
    }

    private void Update()
    {
        stateMachine.Tick();
    }

    #region Left Stick
    public void GetInput()
    {
        input = playerInputActions.Player.Move.ReadValue<Vector2>();
        status.uiController.SetPositionFocusUI(GetJoystickDirection(input));

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
        if (!canJump) return;
        hasPressedJumpButton = true;
    }
    #endregion
}


