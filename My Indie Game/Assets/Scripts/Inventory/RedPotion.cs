using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPotion : Item
{
    public int healthRecoverAmount;

    public override void Use()
    {
        base.Use();
        status.RecoverHealth(healthRecoverAmount);
    }
}
