using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Interactable
{
    [SerializeField] Transform tampa;


    public override void Interact()
    {
        if (hasInteract) return;

        hasInteract = true;
        uiController.SetDefaultInteractionSprite();
        tampa.LeanRotateAroundLocal(Vector3.right, -45, .5f);

        Debug.Log("entrei");
    }
}
