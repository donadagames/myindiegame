using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Potions/Health Potion")]
public class RedPotion : Item
{
    public int healthRecoverAmount;

    public override void Use()
    {
        base.Use();
        status.RecoverHealth(healthRecoverAmount);
        status.player.soundController.PlayClip(audioClip);
    }
}
