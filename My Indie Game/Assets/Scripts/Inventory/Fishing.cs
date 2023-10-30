public class Fishing : Interactable
{
    public override void Interact()
    {
        if (hasInteract) return;


        if (inventory.InventoryIsFull(item) == true)
        {
            DisplayAddItem display = Instantiate(displayItem, transform).GetComponent<DisplayAddItem>();

            display.quantity.text = string.Empty;
            display.cam = inventory.status.mainCamera.transform;
            display.icon.sprite = inventory.fullInventoryIcon;
            return;
        }

        else
        {
            hasInteract = true;
            uiController.SetDefaultInteractionSprite();

            status.input.isInteracting = true;
            status.player.SetFishingConfiguration();
        }
    }
}
