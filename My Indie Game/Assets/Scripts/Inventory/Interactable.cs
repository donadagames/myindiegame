using System;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [Header("----------BASIC INTERACTABLE----------")]
    public string saveableEntityId;
    [ContextMenu("Generate ID")]
    private void GenereteID() => saveableEntityId = Guid.NewGuid().ToString();

    public Status status;
    public UIController uiController;
    public Inventory inventory;
    public bool hasInteract;
    public Sprite icon;
    public GameObject displayItem;
    public AudioClip clip;

    [Header("----------TREE, MINE, FISH INTERACTABLE----------")]
    public int itemQuantity;
    public int interactionIndex;
    public int repetitions;
    public Item item;
    public Vector3 offset = new Vector3(0, 0, 0);
    public Vector3 vfxOffset = new Vector3(0, 0, 0);

    [Header("----------PUSH INTERACTABLE----------")]
    public Vector3 side = new Vector3();

    [Header("----------PET INTERACTABLE----------")]
    public bool isPet = false;

    [Header("----------CARRY INTERACTABLE----------")]

    public Vector3 carryPosition = new Vector3();
    public Vector3 carryRotation = new Vector3();
    public Collider[] colliders;

    [Header("----------DIALOGUE ACTIVATOR INTERACTABLE----------")]
    public Dialogue currentDialogue;

    public virtual void Start()
    {
        status = Status.instance;
        uiController = UIController.instance;
        inventory = Inventory.instance;
    }

    public virtual void OnEnter()
    {
        if (hasInteract) return;

        uiController.SetInteractionSprite(icon);
    }

    public virtual void OnExit()
    {
        uiController.SetDefaultInteractionSprite();
    }


    public virtual void Interact()
    {
        if (hasInteract) return;

        hasInteract = true;
        uiController.SetDefaultInteractionSprite();
    }

    public virtual void GiveItem()
    {
        inventory.AddItem(item, itemQuantity);
        DisplayAddItem display = Instantiate(displayItem, transform.position + offset, transform.rotation, transform).GetComponent<DisplayAddItem>();

        status.saveSystem.SaveInteractables();

        display.quantity.text = $"+{itemQuantity}";
        display.cam = inventory.status.mainCamera.transform;
        display.icon.sprite = item.icon;
    }

    public virtual void CaptureState()
    {
        return;
    }

    public virtual void RestoreState(Savable savable = null, SavableDialogueActivator savableDialogueActivator = null)
    {
        return;
    }
}

public interface IInteractable
{
    public void Interact();
    public void OnEnter();
    public void OnExit();
    public void CaptureState();
    public void RestoreState(Savable savable = null, SavableDialogueActivator savableDialogueActivator = null);
}
