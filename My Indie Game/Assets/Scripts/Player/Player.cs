using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

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
}
