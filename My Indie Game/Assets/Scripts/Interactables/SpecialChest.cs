using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.InputSystem.XR;
using JetBrains.Annotations;

public class SpecialChest : Interactable
{
    [SerializeField] Transform tampa;
    [SerializeField] Item key;
    [SerializeField] Item specialItem;
    [SerializeField] bool isSword = true;

    public override void Interact()
    {
        if (hasInteract)
        { 
            return;
        } 

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
            status.input.isInteracting = true;
            uiController.SetDefaultInteractionSprite();
            inventory.RemoveItem(key, 1);
            status.player.soundController.PlayClip(clip);
            tampa.LeanRotateAroundLocal(Vector3.right, -45, .5f).setOnComplete(DealInteraction);
        }
    }

    private void DealInteraction()
    {

        inventory.AddItem(specialItem, 1);

        DisplayAddItem display = Instantiate(displayItem, transform.position + offset, transform.rotation, transform).GetComponent<DisplayAddItem>();
        status.saveSystem.SaveInteractables();
        display.quantity.text = "";
        display.cam = inventory.status.mainCamera.transform;
        display.icon.sprite = specialItem.icon;

        LeanTween.value(0, 1, status.player.animations.interactionsClipsDurations[interactionIndex]).setOnComplete(CompleteInteraction);
    }

    private void CompleteInteraction()
    {
        if (isSword)
        {
            status.player.backWeapon.SetActive(true);
        }
        else
        {
            status.player.backShield.SetActive(true);
        }
    }


    public override void CaptureState()
    {
        return;
    }

    public override void RestoreState(Savable savable, SavableDialogueActivator savableDialogueActivator = null)
    {
        tampa.localEulerAngles = new Vector3(-135, 0, 0);
        hasInteract = true;
    }
}
