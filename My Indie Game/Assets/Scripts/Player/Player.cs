using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimationController))]
public class Player : MonoBehaviour
{
    public CharacterController characterController;
    public AnimationController animations;

    public GameObject handWeapon;
    public GameObject handShield;
    public GameObject backWeapon;
    public GameObject backShield;

    public float moveSpeed = 3f;
    public float jumpInPlaceHight = 5f;
    public float jumpMovingHight = 5f;


    public Transform footPos;
    public Transform headPos;

    public float jumpClipDuration;
    public float doubleJumpClipDuration;
    public float landClipDuration;

    private void Awake()
    {
        animations = GetComponent<AnimationController>();
        characterController = GetComponent<CharacterController>();
    }

    
}
