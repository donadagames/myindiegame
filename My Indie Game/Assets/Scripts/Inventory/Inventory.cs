using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }
    #endregion

    public Status status;
    public List<Item> itens = new List<Item>();
    public List<Item> basicItens = new List<Item>(4);
    public List<InventorySlot> slots = new List<InventorySlot>();
    public Sprite fullInventoryIcon;
    public int inventorySpace;

    public bool InventoryIsFull(Item item) => GetFirstEmptySlot() == null && !itens.Contains(item);

    public AudioSource _audio;

    public InventoryUI inventoryUI;

    [SerializeField] Transform inventorySlotsParent;
    [SerializeField] GameObject infoText; //Prefab Infotext

    private void Start()
    {
        slots = inventorySlotsParent.GetComponentsInChildren<InventorySlot>().ToList();
        inventoryUI = InventoryUI.instance;
        status = Status.instance;
    }

    public void AddItem(Item item, int quantity)
    {
        if (item == null || quantity <= 0) return;

        if (basicItens.Contains(item))
        {
            #region ADD Basic Itens (Coin[0] and Gems(Red[1], Blue[2], Green[3]))
            item.quantity += quantity;

            if (item.questObjective != null)
            {
                item.questObjective.currentQuantity += quantity;
                OnUpdateQuestObjective?.Invoke
                    (this, new OnUpdateQuestObjectiveEventHandler { questObjective = item.questObjective });

                if (item.questObjective.currentQuantity >= item.questObjective.completeQuantity &&
                    !item.questObjective.isCompleted)
                {
                    item.questObjective.isCompleted = true;
                }
            }

            OnUpdateInventory?.Invoke(this, new OnUpdateInventoryEventHandler { item = item });
            #endregion
        }

        else
        {
            var slot = GetFirstEmptySlot();
            #region Inventory is full
            if (slot == null)
            {
                var text = string.Empty;

                if (inventoryUI.languageControll.isPortuguese)
                {
                    text = "Sem espaço na mochila!";
                }
                else
                {
                    text = "No space in the bag!";
                }

                //ShowInfoText(text);
            }
            #endregion

            #region Inventory is NOT full
            else
            {
                var usedSlot = GetSlotWithItem(item);
                _audio.PlayOneShot(item.audioClip, .5f);
                item.quantity += quantity;

                if (item.questObjective != null)
                {
                    item.questObjective.currentQuantity += quantity;
                    OnUpdateQuestObjective?.Invoke
                    (this, new OnUpdateQuestObjectiveEventHandler { questObjective = item.questObjective });
                    if (item.questObjective.currentQuantity >= item.questObjective.completeQuantity &&
                    !item.questObjective.isCompleted)
                    {
                        item.questObjective.isCompleted = true;
                    }
                }

                #region Inventory don't have this specific item
                if (usedSlot == null)
                {
                    slot.AddItem(item, quantity); // Add to inventory
                    //ShowItemInfo(item, quantity.ToString());
                    OnUpdateInventory?.Invoke(this, new OnUpdateInventoryEventHandler { item = item });
                    return;
                }
                #endregion

                #region Inventory already have this specific item
                else
                {
                    //ShowItemInfo(item, quantity.ToString());
                    OnUpdateInventory?.Invoke(this, new OnUpdateInventoryEventHandler { item = item });
                }
                #endregion
            }
            #endregion
        }
    }

    private InventorySlot GetFirstEmptySlot()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null) return slot;
        }
        return null;
    }

    private InventorySlot GetSlotWithItem(Item item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item) return slot;
        }
        return null;
    }

    public void RemoveItem(Item item, int quantity)
    {
        item.quantity -= quantity;

        if (item.quantity <= 0 && !basicItens.Contains(item))
        {
            itens.Remove(item);
        }

        OnUpdateInventory?.Invoke(this, new OnUpdateInventoryEventHandler { item = item });
    }

    public event EventHandler<OnUpdateInventoryEventHandler> OnUpdateInventory;

    public class OnUpdateInventoryEventHandler : EventArgs
    {
        public Item item;
    }

    public event EventHandler<OnUpdateQuestObjectiveEventHandler> OnUpdateQuestObjective;

    public class OnUpdateQuestObjectiveEventHandler : EventArgs
    {
        public QuestObjective questObjective;
    }
}
