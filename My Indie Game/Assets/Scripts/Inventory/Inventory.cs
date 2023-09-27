using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    public List<Item> itens = new List<Item>();
    public int inventorySpace;

    public void AddItem(Item item, int quantity)
    {
        if (itens.Count == inventorySpace)
        { 
        
        }

        if (itens.Contains(item))
        { 
            
        }

        OnItemChange?.Invoke(this, new OnItemChangeEventHandler { _item = item });
    }

    public void RemoveItem(Item item, int quantity)
    {

        OnItemChange?.Invoke(this, new OnItemChangeEventHandler { _item = item });
    }

    public event EventHandler<OnItemChangeEventHandler> OnItemChange;

    public class OnItemChangeEventHandler : EventArgs
    {
        public Item _item;
    }
}
