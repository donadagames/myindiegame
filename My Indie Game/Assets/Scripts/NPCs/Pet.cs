using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;

public class Pet : Interactable
{
    public Transform target;
    public Player player;
    public Player mount;
    public GameObject mountPrefab;
    public GameObject mount_VFX;

    [SerializeField] float speed = 1f;
    [SerializeField] float mindDistance = 1f;

    public CharacterController controller;

    private Animator animator;
    private string currentAnimation;
    float distance;
    float velocity;
    public Vector3 direction = new Vector3();
    private Vector3 _direction;
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        if (hasInteract || status.input.isCarrying) return;

        uiController.SetInteractionSprite(icon);
    }

    private void FaceTarget()
    {
        _direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(_direction.x + 0.0001f, 0f, _direction.z + 0.0001f));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void MoveToTarget()
    {
        if (player.input.isOnPlatform)
        {
            SetAnimation("Idle");
            return;
        }

        if (distance > mindDistance && !player.input.isPushing  && distance < 14 || player.input.input.magnitude > 0 && !player.input.isPushing && distance < 14 || player.input.isInteracting)
        {
            direction = new Vector3(_direction.x, -10, _direction.z);
            controller.Move(direction * speed * Time.deltaTime);
            SetAnimation("Run Forward In Place");
        }

        else
        {
            SetAnimation("Idle");
        }
    }

    private void SetAnimation(string animation)
    {
        if (currentAnimation == animation) return;
        else
        {
            animator.Play(animation);
            currentAnimation = animation;
        }
    }

    private void GetDistance()
    {
        distance = Vector3.Distance(transform.position, target.position);

        if (distance > 14)
        {
            transform.position = target.position + new Vector3(.5f, 0, .34f);
            Instantiate(mount_VFX, transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        GetDistance();
        FaceTarget();
        MoveToTarget();
    }

    public override void Interact()
    {
        if (hasInteract) return;

        mount = Instantiate(mountPrefab, transform.position, transform.rotation).GetComponent<Player>();
        status.mount = mount;
        mount.pet = this;
        status.mountTransform = mount.mounsTransform;

        status.player.soundController.FireballSound();

        Instantiate(mount_VFX, transform.position, Quaternion.identity);
        hasInteract = true;
        uiController.SetDefaultInteractionSprite();
        status.input.isMounting = true;
        status.player.SetSwordAndShieldConfiguration();
        gameObject.SetActive(false);
    }
}
