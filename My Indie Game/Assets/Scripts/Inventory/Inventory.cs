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

    public string saveableEntityId;
    [ContextMenu("Generate ID")]
    private void GenereteID() => saveableEntityId = Guid.NewGuid().ToString();
    public Transform draggingParent;
    public Status status;
    public List<Item> itens = new List<Item>();
    public List<Item> basicItens = new List<Item>(4);
    public List<InventorySlot> slots = new List<InventorySlot>();
    public Sprite fullInventoryIcon;
    public int inventorySpace;
    public Item[] swordAndShield = new Item[2];
    public bool InventoryIsFull(Item item) => GetFirstEmptySlot() == null && !itens.Contains(item);
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
            status.player.soundController.PlayClip(item.audioClip);
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

            if (item.isOnlyQuestObjective)
            {
                status.player.soundController.PlayClip(item.audioClip);
                itens.Add(item);
                item.quantity += quantity;

                if (item.questObjective != null)
                {
                    item.questObjective.currentQuantity += quantity;
                    OnUpdateQuestObjective?.Invoke(this, new OnUpdateQuestObjectiveEventHandler { questObjective = item.questObjective });
                    if (item.questObjective.currentQuantity >= item.questObjective.completeQuantity &&
                    !item.questObjective.isCompleted)
                    {
                        item.questObjective.isCompleted = true;
                    }
                }

                OnUpdateInventory?.Invoke(this, new OnUpdateInventoryEventHandler { item = item });
                return;
            }

            var slot = GetFirstEmptySlot();
            #region Inventory is full
            if (slot == null)
            {
                DisplayFullInventoyText();
            }
            #endregion

            #region Inventory is NOT full
            else
            {
                var usedSlot = GetSlotWithItem(item);
                status.player.soundController.PlayClip(item.audioClip);
                itens.Add(item);
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
                    OnUpdateInventory?.Invoke(this, new OnUpdateInventoryEventHandler { item = item });
                    return;
                }
                #endregion

                #region Inventory already have this specific item
                else
                {
                    OnUpdateInventory?.Invoke(this, new OnUpdateInventoryEventHandler { item = item });
                }
                #endregion
            }
            #endregion
        }

        status.saveSystem.SaveInventory();
    }

    public void DiplayMensage(string text)
    {
        status.uiController.DisplayInfoText(text, inventoryUI.settingsController.languageController);
    }

    public void DisplayFullInventoyText()
    {
        var text = string.Empty;

        if (inventoryUI.settingsController.languageController.GetGlobalLanguage() == Language.Portuguese)
        {
            text = "Sem espaço na mochila!";
        }
        else if (inventoryUI.settingsController.languageController.GetGlobalLanguage() == Language.English)
        {
            text = "No space in the bag!";
        }
        else if (inventoryUI.settingsController.languageController.GetGlobalLanguage() == Language.Chinese)
        {
            text = "包里没空间了！";
        }

        DiplayMensage(text);
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

        if (item.questObjective != null)
        {
            item.questObjective.currentQuantity -= quantity;
            OnUpdateQuestObjective?.Invoke
            (this, new OnUpdateQuestObjectiveEventHandler { questObjective = item.questObjective });
            if (item.questObjective.currentQuantity < item.questObjective.completeQuantity &&
            item.questObjective.isCompleted)
            {
                item.questObjective.isCompleted = false;
            }
        }

        if (item.quantity <= 0 && !basicItens.Contains(item))
        {
            itens.Remove(item);
            var _slot = GetSlotWithItem(item);
            _slot.ClearInventorySlot();
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

    #region SAVE SYSTEM
    public object CaptureState()
    {
        var listOfItens = new List<SaveGameItem>();
        var listOfBasicItens = new List<SaveGameItem>();
        var listOfQuestOnlyItens = new List<SaveGameItem>();

        foreach (InventorySlot slot in slots)
        {
            if (slot.item != null)
            {
                listOfItens.Add(new SaveGameItem(slot.item.saveableEntityId, slot.item.quantity));
            }
        }

        foreach (Item item in basicItens)
        {
            listOfBasicItens.Add(new SaveGameItem(item.saveableEntityId, item.quantity));
        }

        foreach (Item item in status.saveSystem.allItens)
        {
            if (item.isOnlyQuestObjective && item.quantity > 0)
            {
                listOfQuestOnlyItens.Add(new SaveGameItem(item.saveableEntityId, item.quantity));
            }
        }

        return new SaveData { itens = listOfItens, questOnlyItens = listOfQuestOnlyItens, basicItens = listOfBasicItens };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;
        itens = new List<Item>();
        

        foreach (InventorySlot slot in slots)
        {
            if (slot.item != null)
            {
                slot.ClearInventorySlot();
            }
        }

        for (int i = 0; i < saveData.itens.Count; i++)
        {
            if (status.saveSystem.itensID.TryGetValue(saveData.itens[i].itemID, out Item item))
            {
                AddItemOnLoad(item, saveData.itens[i].quantity);
            }
        }

        for (int i = 0; i < saveData.basicItens.Count; i++)
        {
            if (status.saveSystem.itensID.TryGetValue(saveData.basicItens[i].itemID, out Item item))
            {
                AddItemOnLoad(item, saveData.basicItens[i].quantity);
            }
        }

        for (int i = 0; i < saveData.questOnlyItens.Count; i++)
        {
            if (status.saveSystem.itensID.TryGetValue(saveData.questOnlyItens[i].itemID, out Item item))
            {
                AddItemOnLoad(item, saveData.questOnlyItens[i].quantity);
            }
        }
    }

    private void AddItemOnLoad(Item item, int quantity)
    {
        if (item == null || quantity <= 0) return;

        if (basicItens.Contains(item))
        {
            #region ADD Basic Itens (Coin[0] and Gems(Red[1], Blue[2], Green[3]))
            item.quantity = quantity;
            if (item.questObjective != null)
            {
                item.questObjective.currentQuantity = quantity;

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
            if (item.isOnlyQuestObjective)
            {
                itens.Add(item);
                item.quantity += quantity;

                if (item.questObjective != null)
                {
                    item.questObjective.currentQuantity += quantity;
                    OnUpdateQuestObjective?.Invoke(this, new OnUpdateQuestObjectiveEventHandler { questObjective = item.questObjective });
                    if (item.questObjective.currentQuantity >= item.questObjective.completeQuantity &&
                    !item.questObjective.isCompleted)
                    {
                        item.questObjective.isCompleted = true;
                    }
                }

                OnUpdateInventory?.Invoke(this, new OnUpdateInventoryEventHandler { item = item });
                return;
            }

            var slot = GetFirstEmptySlot();
            itens.Add(item);
            item.quantity = quantity;

            if (item.questObjective != null)
            {
                item.questObjective.currentQuantity += quantity;
                OnUpdateQuestObjective?.Invoke(this, new OnUpdateQuestObjectiveEventHandler { questObjective = item.questObjective });
                if (item.questObjective.currentQuantity >= item.questObjective.completeQuantity &&
                !item.questObjective.isCompleted)
                {
                    item.questObjective.isCompleted = true;
                }
            }

            slot.AddItem(item, quantity);
            OnUpdateInventory?.Invoke(this, new OnUpdateInventoryEventHandler { item = item });
        }
    }

    public DragableItem GetDragableFromItem(Item item)
    {
        var slot = GetSlotWithItem(item);
        DragableItem dragable = null;
        if (slot != null)
            dragable = slot.dragableItem;
        else
            Debug.Log("slot está empty");

        if (dragable == null)
            return null;
        else
        {

            return dragable;
        }
    }

    [Serializable]
    private struct SaveData
    {
        public List<SaveGameItem> itens;
        public List<SaveGameItem> questOnlyItens;
        public List<SaveGameItem> basicItens;
    }
    #endregion
}

[System.Serializable]
public class SaveGameItem
{
    public string itemID;
    public int quantity;
    public SaveGameItem(string _itemID, int _quantity)
    {
        itemID = _itemID;
        quantity = _quantity;
    }
}
