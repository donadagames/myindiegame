using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;
    public float fallingDuration = .4f;
    //public Skill cureSkill;

    [SerializeField] GameObject puff;
    [SerializeField] GameObject splash;

    [SerializeField] float gravityMultiplier = 1;
    [SerializeField] float smoothTime = .05f;
    [SerializeField] float searchRadius = 40;

    [SerializeField] LayerMask waterLayer;
    [SerializeField] LayerMask interactablesLayer;
    [SerializeField] LayerMask enemySpawnerLayer;

    public Status status;

    private Vector2 input = new Vector2();

    private const float GRAVITY = -9.8f;
    private float velocity;
    private float currentVelocity;

    [HideInInspector] public Interactable interactable;

    [HideInInspector] public bool IsGrounded() => status.player.characterController.isGrounded;
    [HideInInspector] public bool isFalling = false;
    [HideInInspector] public bool canMeleeAttack = true;
    [HideInInspector] public bool canMagicAttack = true;
    [HideInInspector] public bool headIsOnWater = false;
    [HideInInspector] public bool footIsOnWater = false;
    [HideInInspector] public bool isInteracting = false;
    [HideInInspector] public bool isChatting;
    [HideInInspector] public bool hasPressedJumpButton = false;
    [HideInInspector] public bool hasPressedMeleeAttackButton = false;
    [HideInInspector] public bool hasPressedMagicAttackButton = false;
    [HideInInspector] public bool hasEndedLanding = false;
    public bool hasCompletedMagicTimer = true;

    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public PlayerInputActions playerInputActions;
    [HideInInspector] public Vector3 direction = new Vector3();
    [HideInInspector] public int jumpCount = 0;

    public Skill selectedSkill;
    //public Enemy targetEnemy;

    public void SearchForEnemySpawner()
    {
        var spawners = Physics.OverlapSphere(status.player.transform.position, searchRadius, enemySpawnerLayer);

        foreach (var spawner in spawners)
        {
            var enemySpawner = spawner.GetComponent<EnemySpawner>();
            enemySpawner.SpawEnemy();
        }
    }

    public void SearchForInteractables()
    {
        if (interactable == null)
        {
            if (HasInteractable())
            {
                interactable = CheckForInteractable()[0].GetComponentInParent<Interactable>();
                interactable.OnEnter();
            }
        }

        else
        {
            if (!HasInteractable())
            {
                interactable.OnExit();
                interactable = null;
            }
        }
    }

    public Collider[] CheckForInteractable() => Physics.OverlapSphere(status.player.headPos.position, .3f, interactablesLayer);

    public bool HasInteractable() => Physics.CheckSphere(status.player.headPos.position, .3f, interactablesLayer);

    public bool CheckHeadWater() => Physics.CheckSphere(status.player.headPos.position, .2f, waterLayer);
    public bool CheckFootdWater() => Physics.CheckSphere(status.player.footPos.position, .1f, waterLayer);
    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;

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
            new LandOnWater(this, status);
        var meleeAttack =
            new MeleeAttack(status, this);
        var magicAttack =
            new MagicAttack(status, this);
        var getHit =
            new GetHit(status);
        var die = new Die(status, this);
        var levelUp = new LevelUp(status);

        var interacting = new Interacting(status, this);

        var chatting = new Chat(status, this);

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
            (idle, magicAttack, ShouldMagicAttack());


        AddTransition
            (moving, idle, PlayerHasNoMovementInput());
        AddTransition
            (moving, jumpMoving, ShouldJump());
        AddTransition
            (moving, falling, () => jumpCount == 0 && !IsGrounded() && isFalling);
        AddTransition
            (moving, meleeAttack, ShouldMeleeAttack());
        AddTransition
            (moving, magicAttack, ShouldMagicAttack());
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
             (jumpMoving, falling, () => jumpCount == 0 && !IsGrounded() && !headIsOnWater && !footIsOnWater);

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
            (swimming, idle, () => !headIsOnWater);
        AddTransition
            (swimming, floating, () => input.sqrMagnitude <= 0);

        AddTransition
            (floating, to: swimming, () => headIsOnWater && input.sqrMagnitude > 0);
        AddTransition
          (floating, to: idle, () => !headIsOnWater && input.sqrMagnitude <= 0);

        AddTransition
            (landOnWater, floating, () => headIsOnWater && input.sqrMagnitude <= 0);
        AddTransition
           (landOnWater, swimming, () => headIsOnWater && input.sqrMagnitude > 0);
        AddTransition
           (landOnWater, moving, () => !headIsOnWater && footIsOnWater && input.sqrMagnitude > 0);
        AddTransition
           (landOnWater, idle, () => !headIsOnWater && footIsOnWater && input.sqrMagnitude <= 0);

        AddTransition(idle, interacting, () => status.isAlive && isInteracting);
        AddTransition(moving, interacting, () => status.isAlive && isInteracting);
        AddTransition(interacting, idle, () => status.isAlive && !isInteracting);

        AddTransition(meleeAttack, idle,
            () => IsGrounded() && input.sqrMagnitude <= 0 && canMeleeAttack == true);
        AddTransition(meleeAttack, moving,
           () => IsGrounded() && input.sqrMagnitude > 0 && canMeleeAttack == true);

        stateMachine.AddAnyTransition(die, () => !status.isAlive);
        stateMachine.AddAnyTransition(getHit, () => status.isAlive && status.isDamaged);

        AddTransition(getHit, idle, () => status.isAlive && !status.isDamaged);

        stateMachine.AddAnyTransition(levelUp, () => status.isAlive && status.isLevelUp);
        AddTransition(levelUp, idle, () => status.isAlive && !status.isLevelUp);


        AddTransition(idle, chatting, () => status.isAlive && isChatting);
        AddTransition(moving, chatting, () => status.isAlive && isChatting);
        AddTransition(chatting, idle, () => status.isAlive && !isChatting);

        AddTransition(magicAttack, idle,
            () => IsGrounded() && input.sqrMagnitude <= 0 && canMagicAttack == true);
        AddTransition(magicAttack, moving,
           () => IsGrounded() && input.sqrMagnitude > 0 && canMagicAttack == true);

        Func<bool> PlayerHasMovementInput() => () =>
        input.sqrMagnitude > 0;
        Func<bool> PlayerHasNoMovementInput() => () =>
        input.sqrMagnitude <= 0;
        Func<bool> ShouldJump() => () =>
        IsGrounded() && hasPressedJumpButton && jumpCount == 0 && !status.dialogueUI.isOpen;

        Func<bool> ShouldLandIdle() => () =>
        IsGrounded() && jumpCount == 0 && input.sqrMagnitude <= 0
        || IsGrounded() && input.sqrMagnitude <= 0;
        Func<bool> ShouldDoubleJump() => () =>
        hasPressedJumpButton && jumpCount == 1 && !status.dialogueUI.isOpen;
        Func<bool> ShouldLandInMovement() => () => input.sqrMagnitude > 0 &&
        IsGrounded() && jumpCount == 0 || input.sqrMagnitude > 0 && IsGrounded();
        Func<bool> ShouldLand() => () => IsGrounded() && jumpCount == 0 && !headIsOnWater;
        Func<bool> ShouldMeleeAttack() =>
            () => canMeleeAttack == true && hasPressedMeleeAttackButton && !status.dialogueUI.isOpen;

        Func<bool> ShouldMagicAttack() =>
            () => canMagicAttack == true && hasPressedMagicAttackButton && !status.dialogueUI.isOpen && hasCompletedMagicTimer == true;

        stateMachine.SetState(idle);
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

    public virtual void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - status.player.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x + 0.0001f, 0f, direction.z + 0.0001f));
        status.player.transform.rotation = Quaternion.Slerp(status.player.transform.rotation, lookRotation, Time.deltaTime * 5f);
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
        hasPressedMagicAttackButton = false;
        canMeleeAttack = true;
        canMagicAttack = true;
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

    public void MagicAttackButton()
    {
        if (canMagicAttack == false || status.currentEnergy < selectedSkill.energyCost || hasCompletedMagicTimer == false) return;
        /*
        if (targetEnemy == null && selectedSkill != cureSkill)
        {
            // mandar mensagem que está sem target enemy
            Debug.Log("No Target Selected");
        }
        */
        else
        {
            //FaceTarget(targetEnemy.transform);
            canMagicAttack = true;
            hasPressedMagicAttackButton = true;
        }
    }

    public float MagicAttack()
    {
        status.player.animations.animator.Play(selectedSkill.animationClip);
        status.uiController.DealMagicTimer(selectedSkill);
        return selectedSkill.animationDuration;
    }

    public void InteractButton()
    {
        if (interactable == null) return;
        interactable.Interact();
    }

    /*
    public void TargetEnemy(Enemy enemy)
    {
        if (targetEnemy != null)
        {
            targetEnemy.ClearTargetForMagicAttack();
        }
        targetEnemy = enemy;
    }
    */
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
