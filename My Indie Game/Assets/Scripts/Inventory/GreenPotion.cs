using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Potions/Magic Potion")]
public class GreenPotion : Item
{
    public int recoverAmout;

    public override void Use()
    {
        base.Use();
        status.RecoverHealth(recoverAmout);
        status.RecoverEnergy(recoverAmout);
        status.player.soundController.PlayClip(audioClip);
    }
}
