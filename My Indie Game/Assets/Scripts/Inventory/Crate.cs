using UnityEngine;
using UnityEngine.InputSystem.DualShock;

public class Crate : Interactable
{
    [SerializeField] Transform tampa;


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

        hasInteract = true;
        uiController.SetDefaultInteractionSprite();
        tampa.LeanRotateAroundLocal(Vector3.right, -45, .5f).
            setOnComplete(GiveItem);
    }
}
