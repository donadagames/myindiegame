using UnityEngine;

public class Chest : Interactable
{
    [SerializeField] Transform tampa;
    [SerializeField] Item key;

    public override void Interact()
    {
        if (hasInteract) return;

        if (inventory.InventoryIsFull(item) == true)
        {
            inventory.DisplayFullInventoyText();
            return;
        }

        if (!inventory.itens.Contains(key))
        {
            status.input.PlayWrongAudioClip();
            return;
        }

        else
        {
            hasInteract = true;
            uiController.SetDefaultInteractionSprite();
            tampa.LeanRotateAroundLocal(Vector3.right, -45, .5f).setOnComplete(DealInteraction);
        }
    }

    private void DealInteraction()
    {
        //GiveItem();
        status.input.isInteracting = true;
        status.player.SetUnarmedConfiguration();
    }
}
