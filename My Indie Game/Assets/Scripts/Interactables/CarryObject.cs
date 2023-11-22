using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.InputSystem.XR;
using static UnityEngine.TouchScreenKeyboard;

public class CarryObject : Interactable
{
    public override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        if (hasInteract) return;

        hasInteract = true;
        uiController.SetDefaultInteractionSprite();

       

        foreach (Collider collider in colliders)
        { 
            collider.enabled = false;
        }

        status.input.isInteracting = true;
        status.player.SetUnarmedConfiguration();

        StartCoroutine(GrabObjectNow());

    }

    private IEnumerator GrabObjectNow()
    {
        yield return new WaitForSeconds(.5f);

        transform.SetParent(status.player.carryTransform);
        transform.localPosition = carryPosition;
        transform.localEulerAngles = carryRotation;
        status.input.isCarrying = true;
        status.input.carryObject = this;
    }

}
