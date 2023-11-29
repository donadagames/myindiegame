using System;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionBarSlot : MonoBehaviour, IDropHandler
{
    [ContextMenu("Generate ID")]
    private void GenereteID() => saveableEntityId = Guid.NewGuid().ToString();
    public string saveableEntityId;
    public Item item;
    public ActionBarUsableSlot usableActionBarSlot;
    public TextMeshProUGUI quantity;
    public DragableItem dragableItem;
    Inventory inventory;

    private void Start()
    {
        inventory = Inventory.instance;
        inventory.OnUpdateInventory += OnUpdateInventory;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        var dragable = dropped.GetComponent<DragableItem>();

        if (dragableItem == null) // estou vazio
        {
            if (dragable.actionbarSlot != null) // veio de outro Action Bar Slot
            {
                dragable.actionbarSlot.CleanSlot();
            }
            UpdateActionbarSlot(this, dragable);
        }

        else // já tenho um dragableItem dentro de mim
        {
            if (dragable.actionbarSlot != null) // veio de outro Action Bar Slot
            {
                var slot = dragable.actionbarSlot;
                UpdateActionbarSlot(slot, dragableItem);
                UpdateActionbarSlot(this, dragable);
            }

            else // veio do inventário
            {
                dragableItem.actionbarSlot = null;
                dragableItem.changebleParentTransform = dragableItem.parentTransform;
                dragableItem.transform.SetParent(dragableItem.changebleParentTransform);
                dragableItem.transform.localPosition = new Vector3(0, 0, 0);

                dragableItem.transform.localScale = new Vector3(0, 0, 0);
                dragableItem.transform.LeanScale(new Vector3(.75f, .75f, .1f), .15f);
                dragableItem.image.raycastTarget = true;

                UpdateActionbarSlot(this, dragable);
            }
        }
    }

    private void OnUpdateInventory(object sender, Inventory.OnUpdateInventoryEventHandler handler)
    {
        if (handler.item == item)
        {
            quantity.text = $"{handler.item.quantity}";
        }
    }

    public void CleanSlot()
    {
        quantity.text = "";
        item = null;

        usableActionBarSlot.item = null;
        usableActionBarSlot.quantity.text = string.Empty;
        usableActionBarSlot.icon.sprite = null;
        usableActionBarSlot.icon.enabled = false;

        dragableItem = null;
    }

    public void UpdateActionbarSlot(ActionBarSlot slot, DragableItem dragable)
    {
        slot.item = dragable.parentSlot.item;
        slot.quantity.text = $"{item.quantity}";
        slot.dragableItem = dragable;

        slot.usableActionBarSlot.item = item;
        slot.usableActionBarSlot.quantity.text = $"{item.quantity}";
        slot.usableActionBarSlot.icon.sprite = item.icon;
        slot.usableActionBarSlot.icon.enabled = true;

        dragable.actionbarSlot = slot;
        dragable.changebleParentTransform = slot.transform;
        dragable.transform.SetParent(dragable.changebleParentTransform);
        dragable.transform.localPosition = new Vector3(0, 0, 0);

        dragable.transform.localScale = new Vector3(0, 0, 0);
        dragable.transform.LeanScale(new Vector3(1, 1, 1), .15f);

        dragable.image.raycastTarget = true;
    }

    public object CaptureState()
    {
        return new ActionBarSlotData(this);
    }

    public void RestoreState(object state, Inventory _inventory)
    {
        var data = (ActionBarSlotData)state;

        if (data.itemID != string.Empty)
        {
            var _dragableItem = _inventory.GetDragableFromItem(_inventory.status.saveSystem.GetItemFromID(data.itemID));
            _dragableItem.RestoreOnLoad();

            UpdateActionbarSlot(this, _dragableItem);
        }
    }
}

[System.Serializable]
public class ActionBarSlotData
{
    public string itemID;

    public ActionBarSlotData(ActionBarSlot slot)
    {
        if (slot.item != null)
            itemID = slot.item.saveableEntityId;
        else
            itemID = string.Empty;
    }
}
