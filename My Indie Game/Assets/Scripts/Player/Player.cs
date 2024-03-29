using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(PlayerSoundController))]
public class Player : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public AnimationController animations;
    [HideInInspector] public PlayerSoundController soundController;

    public Mesh[] mouthNormalAndDeath;
    public Mesh[] eyeNormalAndDeath;

    public MeshFilter eyeRenderer;
    public MeshFilter mouthRenderer;

    public const float mass = 10;

    public InputHandler input;

    public GameObject handAxe;
    public GameObject handWeapon;
    public GameObject handShield;
    public GameObject backWeapon;
    public GameObject backShield;
    public GameObject fishingRod;
    public GameObject handPickAxe;

    public GameObject impactWood_VFX;
    public GameObject impactStone_VFX;

    public Transform impactTransform;

    public float moveSpeed = 3f;
    public float jumpInPlaceHight = 5f;
    public float jumpMovingHight = 5f;

    public EnemyAttackPosition[] enemyPosition;

    public Transform footPos;
    public Transform headPos;
    public Transform carryTransform;
    public Transform mounsTransform;
    public Pet pet;

    public float jumpClipDuration;
    public float doubleJumpClipDuration;
    public float landClipDuration;
    public float getHitClipDuration;
    public float dieClipDuration;
    public float levelUpClipDuration;
    public float rebirthClipDuration;

    public int minDamage;
    public int maxDamage;

    public Sword sword;
    public GameObject rope;

    private void Awake()
    {
        animations = GetComponent<AnimationController>();
        characterController = GetComponent<CharacterController>();
        soundController = GetComponent<PlayerSoundController>();
    }
    private void Start()
    {
        input = InputHandler.instance;
    }

    public void JumpInPlace()
    {
        input.Jump(jumpInPlaceHight);
    }

    public void JumpMoving()
    {
        input.Jump(jumpMovingHight);
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

    private IEnumerator ScaleRope()
    {
        yield return new WaitForSeconds(.2f);
        rope.LeanScaleZ(4, 1f);
        yield return new WaitForSeconds(7.8f);
        rope.LeanScaleZ(0, 1f);
    }

    public void SetCutWoodConfiguration()
    {
        handWeapon.SetActive(false);
        handShield.SetActive(false);
        handAxe.SetActive(true);
        backWeapon.SetActive(true);
        backShield.SetActive(true);
        fishingRod.SetActive(false);
        handPickAxe.SetActive(false);
    }

    public void SetFishingConfiguration()
    {
        handWeapon.SetActive(false);
        handShield.SetActive(false);
        handAxe.SetActive(false);
        backWeapon.SetActive(true);
        backShield.SetActive(true);
        fishingRod.SetActive(true);
        handPickAxe.SetActive(false);

        StartCoroutine(ScaleRope());

    }

    public void SetMinningConfiguration()
    {
        handWeapon.SetActive(false);
        handShield.SetActive(false);
        handAxe.SetActive(false);
        backWeapon.SetActive(true);
        backShield.SetActive(true);
        fishingRod.SetActive(false);
        handPickAxe.SetActive(true);
    }

    public void SetUnarmedConfiguration()
    {
        handWeapon.SetActive(false);
        handShield.SetActive(false);
        handAxe.SetActive(false);
        backWeapon.SetActive(true);
        backShield.SetActive(true);
        fishingRod.SetActive(false);
        handPickAxe.SetActive(false);
    }

    public void SetOnlyShieldConfiguration()
    {
        handWeapon.SetActive(false);
        handShield.SetActive(false);
        handAxe.SetActive(false);
        backWeapon.SetActive(false);
        backShield.SetActive(true);
        fishingRod.SetActive(false);
        handPickAxe.SetActive(false);
    }

    public void SetOnlySwordConfiguration()
    {
        handWeapon.SetActive(false);
        handShield.SetActive(false);
        handAxe.SetActive(false);
        backWeapon.SetActive(true);
        backShield.SetActive(false);
        fishingRod.SetActive(false);
        handPickAxe.SetActive(false);
    }

    public void SetUnequipedConfiguration()
    {
        handWeapon.SetActive(false);
        handShield.SetActive(false);
        handAxe.SetActive(false);
        backWeapon.SetActive(false);
        backShield.SetActive(false);
        fishingRod.SetActive(false);
        handPickAxe.SetActive(false);

    }

    public void SetSwordAndShieldConfiguration()
    {
        handWeapon.SetActive(true);
        handShield.SetActive(true);
        handAxe.SetActive(false);
        backWeapon.SetActive(false);
        backShield.SetActive(false);
        fishingRod.SetActive(false);
        handPickAxe.SetActive(false);
    }

    #region SKILLS
    public void Fireball()
    {
        soundController.FireballSound();
        Instantiate(input.selectedSkill.skill_VFX, handWeapon.transform.position, transform.rotation);
    }

    public void Cure()
    {
        soundController.CureSound();
        input.status.RecoverHealth(input.status.energy * input.selectedSkill.healthRegenaration);
        Instantiate(input.selectedSkill.skill_VFX, transform.position, transform.rotation, transform);
    }

    public void Ice()
    {
        soundController.IceSound();
        Instantiate(input.selectedSkill.skill_VFX, transform.position + new Vector3(0, 0f, .5f), transform.rotation);
    }

    public void Blast()
    {
        soundController.BlastSound();
        Instantiate(input.selectedSkill.skill_VFX, transform.position, transform.rotation);
    }

    public void Starfall()
    {
        soundController.StarfallSound();
        Instantiate(input.selectedSkill.skill_VFX, transform.position, Quaternion.identity);
    }

    public void OnParticleCollision(GameObject other)
    {
        var diamond = other.GetComponent<Diamond>();

        if (diamond != null)
        {
            diamond.Interact(input.status.inventory, this);
        }
    }

    #endregion
}
