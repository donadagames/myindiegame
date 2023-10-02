using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BasicInventorySlots : MonoBehaviour
{
    public Item item;
    public TextMeshProUGUI[] quantity;
    private Inventory inventory;

    private void Start()
    {
        inventory = Inventory.instance;
        inventory.OnUpdateInventory += OnUpdateInventory;
    }


    public virtual void OnUpdateInventory(object sender, Inventory.OnUpdateInventoryEventHandler handler)
    {
        if (handler.item == item)
            foreach(TextMeshProUGUI text in quantity)
                text.text = handler.item.quantity.ToString();
    }
}
