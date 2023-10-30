using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Potions/Energy Potion")]
public class BluePotion : Item
{
    public int energyRecoverAmount;

    public override void Use()
    {
        base.Use();
        status.RecoverEnergy(energyRecoverAmount);
        status.player.soundController.PlayClip(audioClip);
    }
}
