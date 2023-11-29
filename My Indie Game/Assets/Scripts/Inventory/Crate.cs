using UnityEngine;

public class Crate : Interactable
{
    [SerializeField] Transform tampa;


    public override void Interact()
    {
        if (hasInteract) return;

        if (inventory.InventoryIsFull(item) == true)
        {
            inventory.DisplayFullInventoyText();
            return;
        }

        hasInteract = true;
        uiController.SetDefaultInteractionSprite();

        status.player.soundController.OpenCrateSound();

        tampa.LeanRotateAroundLocal(Vector3.right, -45, .5f).
            setOnComplete(GiveItem);
    }
}
