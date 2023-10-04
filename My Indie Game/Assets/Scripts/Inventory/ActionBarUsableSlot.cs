using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

}
