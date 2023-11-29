using System.Collections;
using UnityEngine;

public class CarryObject : Interactable
{
    private Vector3 defaultPosition = new Vector3();

    public override void Start()
    {
        base.Start();
        defaultPosition = transform.position;
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

    public void SetDefaultPosition()
    {
        transform.SetParent(null);
        transform.position = defaultPosition;
        status.input.isCarrying = false;
        status.input.carryObject = null;

        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }

        hasInteract = false;
    }
}
