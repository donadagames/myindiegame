using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public Status status;
    public UIController uiController;
    public Inventory inventory;

    public bool hasInteract;
    public Sprite icon;

    public int interactionIndex;
    private void Start()
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
}

public interface IInteractable
{
    public void Interact();
    public void OnEnter();
    public void OnExit();

}
