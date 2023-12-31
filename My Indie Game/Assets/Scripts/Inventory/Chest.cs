using UnityEngine;

public class Chest : Interactable
{
    [SerializeField] Transform tampa;
    [SerializeField] Item key;

    public override void Interact()
    {
        if (hasInteract) return;

        hasInteract = true;
        uiController.SetDefaultInteractionSprite();
        tampa.LeanRotateAroundLocal(Vector3.right, -45, .5f);
    }
}
