using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionBarUsableSlot : MonoBehaviour
{
    public Item item;
    Inventory inventory;
    public Image icon;
    public TextMeshProUGUI quantity;

    private void Start()
    {
        inventory = Inventory.instance;
        inventory.OnUpdateInventory += OnUpdateInventory;

        icon.sprite = null;
        icon.enabled = false;
        quantity.text = string.Empty;

    }

    private void OnUpdateInventory(object sender, Inventory.OnUpdateInventoryEventHandler handler)
    {
        if (handler.item == item)
        {
            quantity.text = $"{item.quantity}";
        }
    }

    public void OnButtonPressed()
    {
        if (item == null) return;
        else
        {
            item.Use();
            inventory.RemoveItem(item, 1);
        }
    }
}
