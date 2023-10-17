using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimationController))]
public class Player : MonoBehaviour
{
    public CharacterController characterController;
    public AnimationController animations;

    public GameObject handAxe;
    public GameObject handWeapon;
    public GameObject handShield;
    public GameObject backWeapon;
    public GameObject backShield;

    public float moveSpeed = 3f;
    public float jumpInPlaceHight = 5f;
    public float jumpMovingHight = 5f;

    public EnemyAttackPosition[] enemyPosition;

    public Transform footPos;
    public Transform headPos;

    public float jumpClipDuration;
    public float doubleJumpClipDuration;
    public float landClipDuration;
    public float getHitClipDuration;
    public float dieClipDuration;
    public float levelUpClipDuration;

    public int minDamage;
    public int maxDamage;

    public Sword sword;

    private void Awake()
    {
        animations = GetComponent<AnimationController>();
        characterController = GetComponent<CharacterController>();
    }

    public EnemyAttackPosition GetEnemyPosition()
    {
        foreach (EnemyAttackPosition pos in enemyPosition)
        {
            if (pos.isInUse == false)
            {
                pos.isInUse = true;
                return pos;
            }
        }

        return null;
    }

    public void SetCutWoodConfiguration()
    {
        handWeapon.SetActive(false);
        handShield.SetActive(false);
        handAxe.SetActive(true);
        backWeapon.SetActive(true);
        backShield.SetActive(true);
    }

    public void SetUnarmedConfiguration()
    {
        handWeapon.SetActive(false);
        handShield.SetActive(false);
        handAxe.SetActive(false);
        backWeapon.SetActive(true);
        backShield.SetActive(true);
    }

    public void SetSwordAndShieldConfiguration()
    {
        handWeapon.SetActive(true);
        handShield.SetActive(true);
        handAxe.SetActive(false);
        backWeapon.SetActive(false);
        backShield.SetActive(false);
    }
}
