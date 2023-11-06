using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Skill")]
public class Skill : ScriptableObject
{
    public string animationClip;
    public float animationDuration;
    public Sprite icon;
    public float energyCost;
    public float healthRegenaration;
    public float minDamage;
    public float maxDamage;
    public GameObject skill_VFX;
    public GameObject enemy_Hit_VFX = null;
    public AudioClip[] hit_SFX;
    public int index;
    public float time;
}

