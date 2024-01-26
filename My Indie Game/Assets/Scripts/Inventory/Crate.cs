using System;
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

        tampa.LeanRotateAroundLocal(Vector3.right, -45, .5f).setOnComplete(GiveItem);
    }

    public override void CaptureState()
    {
        return;
    }

    public override void RestoreState(Savable savable, SavableDialogueActivator savableDialogueActivator = null)
    {
        item = inventory.basicItens[0];
        itemQuantity = UnityEngine.Random.Range(10, 100);
    }


    [Serializable]
    private struct SaveData
    {
        public bool _hasInteracted;
        public int sceneIndex;
    }
}
