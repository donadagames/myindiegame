using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class CoinBasicInventorySlot : BasicInventorySlots
{
    public override void OnUpdateInventory(object sender, Inventory.OnUpdateInventoryEventHandler handler)
    {
        if (handler.item == item)
            foreach (TextMeshProUGUI text in quantity)
                text.text = $"{handler.item.quantity.ToString()},00";
    }
}
