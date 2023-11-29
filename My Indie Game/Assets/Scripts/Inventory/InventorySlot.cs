using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    public Item item;
    public Image itemIcon;
    [HideInInspector] public Inventory inventory;
    [SerializeField] GameObject selectedEdge;
    public Image placeHolderIcon;
    public DragableItem dragableItem;

    private void Start()
    {
        if (inventory == null) inventory = Inventory.instance;
    }

    public void AddItem(Item _item, int _quantity)
    {
        item = _item;
        itemIcon.sprite = item.icon;
        itemIcon.enabled = true;
        inventory = Inventory.instance;
        placeHolderIcon.sprite = item.icon;

        if (item.canBeUsedOnActionBar)
            dragableItem.enabled = true;
        else
            dragableItem.enabled = false;

        inventory.OnUpdateInventory += OnUpdateInventory;
    }

    private void OnUpdateInventory(object sender, Inventory.OnUpdateInventoryEventHandler handler)
    {
        if (handler.item != item) return;
        else
        {
            inventory.inventoryUI.itemQuantityTPM.text = item.quantity.ToString();

            if (item.quantity <= 0)
            {
                item.quantity = 0;
                ClearInventorySlot();
            }
        }
    }

    public void ClearInventorySlot()
    {
        item = null;
        itemIcon.enabled = false;
        placeHolderIcon.enabled = false;
        inventory.OnUpdateInventory -= OnUpdateInventory;

        if (dragableItem.actionbarSlot != null)
        {
            dragableItem.actionbarSlot.CleanSlot();

            dragableItem.actionbarSlot = null;
            dragableItem.changebleParentTransform = dragableItem.parentTransform;
            dragableItem.transform.SetParent(dragableItem.changebleParentTransform);
            dragableItem.transform.localPosition = new Vector3(0, 0, 0);
            dragableItem.image.raycastTarget = true;
        }
    }

    public void OnSlotPressed()
    {
        if (item == null) return;

        if (inventory.inventoryUI.selectedInventorySlot != null)
            inventory.inventoryUI.selectedInventorySlot.DeselectSlot();

        selectedEdge.SetActive(true);
        inventory.inventoryUI.selectedInventorySlot = this;
        inventory.inventoryUI.DisplayItemInformation(item);
    }

    public void DeselectSlot()
    {
        selectedEdge.SetActive(false);
    }
}
