using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public GameObject puff;
    public GameObject splash;
    public float fallingDuration = 1f;
    [SerializeField] float gravityMultiplier = 1;
    [SerializeField] float smoothTime = .05f;
    [SerializeField] LayerMask waterLayer;
    [SerializeField] Status status;
    private Vector2 input = new Vector2();
    private float velocity;
    private const float GRAVITY = -9.8f;
    private float currentVelocity;
    public StateMachine stateMachine;
    private PlayerInputActions playerInputActions;
    public bool IsGrounded() => status.player.characterController.isGrounded;
    public bool isFalling = false;
    public bool canMeleeAttack = true;
    [HideInInspector] public Vector3 direction = new Vector3();
    [HideInInspector] public bool hasPressedJumpButton = false;
    [HideInInspector] public bool hasPressedMeleeAttackButton = false;
    [HideInInspector] public int jumpCount = 0;
    [HideInInspector] public bool hasEndedLanding = false;

    public bool headIsOnWater = false;
    public bool footIsOnWater = false;

    public bool CheckHeadWater() => Physics.CheckSphere(status.player.headPos.position, .2f, waterLayer);
    public bool CheckFootdWater() => Physics.CheckSphere(status.player.footPos.position, .1f, waterLayer);
    private void Awake()
    {
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
        var falling =
            new Falling(status, this);
        var swimming =
            new Swimming(status, this);
        var floating =
            new Floating(status, this);
        var landOnWater =
            new LandOnWater(status, this);
        var meleeAttack =
            new MeleeAttack(status, this);
        var magicAttack =
            new MagicAttack(status, this);
        var getHit =
            new GetHit(status, this);
        var die = new Die(status, this);

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
            (idle, falling, () => jumpCount == 0 && !IsGrounded() && isFalling);
        AddTransition
            (idle, meleeAttack, ShouldMeleeAttack());


        AddTransition
            (moving, idle, PlayerHasNoMovementInput());
        AddTransition
            (moving, jumpMoving, ShouldJump());
        AddTransition
            (moving, falling, () => jumpCount == 0 && !IsGrounded() && isFalling);
        AddTransition
            (moving, meleeAttack, ShouldMeleeAttack());
        AddTransition
            (moving, swimming, () => headIsOnWater && input.sqrMagnitude > 0);


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
            (jumpMoving, landOnWater, () => headIsOnWater);


        AddTransition
            (doubleJumpig, idle, () => input.sqrMagnitude <= 0 &&
            IsGrounded() && jumpCount == 0);
        AddTransition
            (doubleJumpig, moving, ShouldLandInMovement());
        AddTransition
            (doubleJumpig, falling, () => jumpCount == 0 && !IsGrounded() && !headIsOnWater && !footIsOnWater);
        AddTransition
            (doubleJumpig, landOnWater, () => headIsOnWater);


        AddTransition
            (falling, landing, ShouldLand());
        AddTransition
            (falling, landOnWater, () => headIsOnWater && jumpCount == 0);


        AddTransition
            (landing, idle, () => hasEndedLanding && input.sqrMagnitude <= 0);
        AddTransition
            (landing, moving, () => hasEndedLanding && input.sqrMagnitude > 0);


        AddTransition
            (swimming, moving, () => !headIsOnWater && input.sqrMagnitude > 0 && IsGrounded());
        AddTransition
            (swimming, floating, () => input.sqrMagnitude <= 0);


        AddTransition
            (floating, to: swimming, () => headIsOnWater && input.sqrMagnitude > 0);


        AddTransition
            (landOnWater, floating, () => headIsOnWater && input.sqrMagnitude <= 0);
        AddTransition
           (landOnWater, swimming, () => headIsOnWater && input.sqrMagnitude > 0);
        AddTransition
           (landOnWater, moving, () => !headIsOnWater && footIsOnWater && input.sqrMagnitude > 0);
        AddTransition
           (landOnWater, idle, () => !headIsOnWater && footIsOnWater && input.sqrMagnitude <= 0);


        AddTransition(meleeAttack, idle,
            () => IsGrounded() && input.sqrMagnitude <= 0 && canMeleeAttack == true);
        AddTransition(meleeAttack, moving,
           () => IsGrounded() && input.sqrMagnitude > 0 && canMeleeAttack == true);


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
        Func<bool> ShouldLand() => () => IsGrounded() && jumpCount == 0 && !footIsOnWater && !headIsOnWater;
        Func<bool> ShouldMeleeAttack() =>
            () => canMeleeAttack == true && hasPressedMeleeAttackButton;
    }

    private void Start()
    {
        status.OnSafeZoneChange += OnSafeZoneChanged;
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


    public void DetectWater()
    {
        headIsOnWater = CheckHeadWater();
        footIsOnWater = CheckFootdWater();
    }

    public void ApplyGravity()
    {
        if (IsGrounded() && velocity < 0.0f)
            velocity = -10;

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

    public void SetDefaultConfigurations()
    {
        hasPressedJumpButton = false;
        hasPressedMeleeAttackButton = false;
        canMeleeAttack = true;
        hasEndedLanding = false;
        jumpCount = 0;
    }

    public void SetGroundConfiguration(bool isSafeZone)
    {
        status.player.handShield.SetActive(!isSafeZone);
        status.player.handWeapon.SetActive(!isSafeZone);
        status.player.backWeapon.SetActive(isSafeZone);
    }

    public void SetSwimmingConfiguration()
    {
        status.player.handShield.SetActive(false);
        status.player.handWeapon.SetActive(false);
        status.player.backWeapon.SetActive(true);
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

    public void MeleeAttackButton()
    {
        if (canMeleeAttack == false) return;
        canMeleeAttack = true;
        hasPressedMeleeAttackButton = true;
    }

    public float MeleeAttack()
    {
        var index = status.player.animations.GetMeleeAttack();
        status.player.animations.PlayAnimation
            (status.player.animations.MELEE_ATTACKS[index], status.isSafeZone);

        if (status.isSafeZone == true)
            return status.player.animations.meleeAttackDuration[index];
        else
            return status.player.animations.swordAndShieldAttackDuration[index];
    }

    #endregion

    #region VFX

    public void Splash()
    {
        Instantiate(splash, status.player.headPos.position, Quaternion.AngleAxis(-90, Vector3.left));
    }

    public void LandPuff()
    {
        Instantiate(original: puff, status.player.footPos.position, Quaternion.AngleAxis(-90, Vector3.left));

    }
    #endregion

    public void OnSafeZoneChanged(object sender, Status.OnSafeZoneChangeEventHandler handler)
    {
        status.player.handShield.SetActive(!handler._isSafeZone);
        status.player.handWeapon.SetActive(!handler._isSafeZone);
        status.player.backShield.SetActive(handler._isSafeZone);
        status.player.backWeapon.SetActive(handler._isSafeZone);

        stateMachine.shouldChange = true;
    }
}
