using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AnimationController : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        MELEE_ATTACKS.Add(MELEE_ATTACK_01);
        MELEE_ATTACKS.Add(MELEE_ATTACK_02);
        MELEE_ATTACKS.Add(MELEE_ATTACK_03);
        MELEE_ATTACKS.Add(MELEE_ATTACK_04);
        MAGIC_ATTACKS.Add(MAGIC_ATTACK_01);
        MAGIC_ATTACKS.Add(MAGIC_ATTACK_02);
        MAGIC_ATTACKS.Add(MAGIC_ATTACK_03);
        MAGIC_ATTACKS.Add(MAGIC_ATTACK_04);
    }

    #region ANIMATION NAMES

    public string[] IDLE = new string[2];
    public string[] MOVE = new string[2];
    public string[] LAND = new string[2];
    public string[] FALL = new string[2];
    public string[] JUMP_MOVING = new string[2];
    public string[] JUMP_IN_PLACE = new string[2];
    public string[] DOUBLE_JUMP = new string[2];
    public string[] SWIM = new string[2];
    public string[] FLOAT = new string[2];
    public string[] DIE = new string[2];
    public string[] GETHIT = new string[2];
    public string[] MELEE_ATTACK_01 = new string[2];
    public string[] MELEE_ATTACK_02 = new string[2];
    public string[] MELEE_ATTACK_03 = new string[2];
    public string[] MELEE_ATTACK_04 = new string[2];
    public string[] MAGIC_ATTACK_01 = new string[2];
    public string[] MAGIC_ATTACK_02 = new string[2];
    public string[] MAGIC_ATTACK_03 = new string[2];
    public string[] MAGIC_ATTACK_04 = new string[2];
    public string[] LEVELUP = new string[2];
    public string[] REBIRTH = new string[2];
    //public string[] WALKING = new string[2];
    public string[] MOUNT = new string[2];

    public List<string[]> MELEE_ATTACKS = new List<string[]>(4);
    public List<string[]> MAGIC_ATTACKS = new List<string[]>(4);

    public float[] meleeAttackDuration;
    public float[] swordAndShieldAttackDuration;
    public float[] magicAttackDuration;
    public string[] INTERACT;
    public float[] interactionsClipsDurations;
    public string CHAT;

    #endregion

    public void IncreaseParameter(string parameter)
    {
        var currentValue = animator.GetInteger(parameter);
        var randomValue = Random.Range(0, 2);
        animator.SetInteger(parameter, currentValue + randomValue);
    }

    public void SetParameterToZero(string parameter)
    {
        animator.SetInteger(parameter, 0);
    }

    public void PlayAnimation(string[] animation, bool isSafeZone)
    {
        if (isSafeZone)
        {
            animator.Play(animation[0]);
        }
        else
        {
            animator.Play(animation[1]);
        }
    }

    public int GetMeleeAttack()
    {
        var index = Random.Range(0, MELEE_ATTACKS.Count);
        return index;
    }
    public string[] GetMagicAttack()
    {
        var index = Random.Range(0, MAGIC_ATTACKS.Count);
        return MAGIC_ATTACKS[index];
    }
}
