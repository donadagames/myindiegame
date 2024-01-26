using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : SavableEntity
{
    public override void RestoreState(SerializableSavableEntity savable)
    {
        transform.localEulerAngles = new Vector3(-90, 90, 0);
    }
}
