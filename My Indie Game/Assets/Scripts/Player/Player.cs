using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpInPlaceHight = 5f;
    public float jumpMovingHight = 5f;
    public CharacterController characterController;
    public Animator animator;
    public WaterDetector waterDetector;

    public float jumpClipDuration;
    public float doubleJumpClipDuration;
    public float landClipDuration;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        waterDetector = GetComponentInChildren<WaterDetector>();
    }


}
