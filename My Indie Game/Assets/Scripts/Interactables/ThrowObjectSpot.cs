using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjectSpot : Interactable
{
    [SerializeField] private Transform button;
    [SerializeField] private Transform platform;
    [SerializeField] private Vector3 destination = new Vector3();
    [SerializeField] private float time;

    public override void Interact()
    {
        if (hasInteract || !status.input.isCarrying) return;

        hasInteract = true;
        uiController.SetDefaultInteractionSprite();
        status.input.isInteracting = true;
        StartCoroutine(ThrowObjectNow());
    }

    private IEnumerator ThrowObjectNow()
    {
        yield return new WaitForSeconds(.5f);
        status.input.carryObject.transform.SetParent(button);
        Instantiate(displayItem, transform.position + new Vector3(0, .4f, 0), Quaternion.identity, transform);
        status.player.soundController.PlayClip(clip);
        status.input.carryObject.colliders[1].enabled = true;
        status.input.carryObject.transform.localPosition = carryPosition;
        status.input.carryObject.transform.localEulerAngles = carryRotation;
        status.input.carryObject = null;
        status.input.isCarrying = false;
        button.LeanMoveLocalZ(-.25f, 1f);

        yield return new WaitForSeconds(1f);
        platform.LeanMoveLocal(destination, time).setLoopPingPong();

    }

    public override void OnEnter()
    {
        if (hasInteract || !status.input.isCarrying) return;
        uiController.SetInteractionSprite(icon);
    }




}
