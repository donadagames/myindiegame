using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public Status status;
    public UIController uiController;
    public Inventory inventory;
    public bool hasInteract;
    public Sprite icon;
    public int interactionIndex;
    public Item item;
    public GameObject displayItem;
    public int itemQuantity;
    public Vector3 offset = new Vector3 (0, 0, 0);
    public int repetitions;
    public Vector3 vfxOffset = new Vector3(0, 0, 0);
    
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

        display.quantity.text = $"+{itemQuantity}";
        display.cam = inventory.status.mainCamera.transform;
        display.icon.sprite = item.icon;
    }
}

public interface IInteractable
{
    public void Interact();
    public void OnEnter();
    public void OnExit();

}
