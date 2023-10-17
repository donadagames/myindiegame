using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventorySlot selectedInventorySlot;

    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject itemInfoPanel;

    [Header("Top Panel")]
    [SerializeField] TextMeshProUGUI actionBarTPM;

    [Header("Intem Info Panel")]
    [SerializeField] TextMeshProUGUI itemNameTPM;
    [SerializeField] TextMeshProUGUI itemDescriptionTPM;
    [SerializeField] TextMeshProUGUI deleteQuantityTPM;
    [SerializeField] TextMeshProUGUI quantityTPM;
    public TextMeshProUGUI itemQuantityTPM;
    [SerializeField] GameObject deletePanel;
    [SerializeField] Image itemIcon;

    [HideInInspector] public LanguageControll languageControll;

    public Sprite[] selectedSlot;
    public Sprite[] deselectedSlot;

    public TextMeshProUGUI displayItem;

    Item selectedItem;
    int quantityToDelete;
    Inventory inventory;

    #region Singleton
    public static InventoryUI instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
    #endregion

    private void Start()
    {
        languageControll = LanguageControll.instance;
        inventory = Inventory.instance;
        languageControll.OnLanguageUpdate += OnLanguageUpdate;
    }

    public void OnLanguageUpdate(object sender, LanguageControll.OnLanguageUpdateEventHandler handler)
    {
        if (handler._isPortuguese == true)
        {
            actionBarTPM.text = "Barra de ação";
            quantityTPM.text = "Quantidade";
        }

        else
        {
            actionBarTPM.text = "Action bar";
            quantityTPM.text = "Quantity:";
        }
    }

    public void DisplayItemInformation(Item item)
    {
        if (item == null) return;
        else
        {
            itemIcon.sprite = item.icon;
            selectedItem = item;
            if (languageControll.isPortuguese == true)
            {
                itemNameTPM.text = item.itemNames[0];
                itemDescriptionTPM.text = item.descriptions[0];
                itemQuantityTPM.text = item.quantity.ToString();

            }

            else
            {
                itemNameTPM.text = item.itemNames[1];
                itemDescriptionTPM.text = item.descriptions[1];
                itemQuantityTPM.text = item.quantity.ToString();
            }

            if (item.deletable == true)
            {
                deletePanel.SetActive(true);
                deleteQuantityTPM.text = $"{quantityToDelete} / {item.quantity}";
            }
            else
            {
                deletePanel.SetActive(false);
            }

            itemInfoPanel.SetActive(true);
        }
    }

    public void CloseItemInformation()
    {
        itemInfoPanel.SetActive(false);
    }

    public void OnInventoryButtonPressed()
    {
        if (selectedInventorySlot != null)
        {
            selectedInventorySlot.DeselectSlot();
            selectedInventorySlot = null;
        }

        selectedItem = null;
        quantityToDelete = 0;
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        itemInfoPanel.SetActive(false);
    }

    public void OnPlusButtonPressed()
    {
        if (quantityToDelete < selectedItem.quantity)
        {
            quantityToDelete++;
            deleteQuantityTPM.text = $"{quantityToDelete} / {selectedItem.quantity}";
        }
    }

    public void OnMinusButtonPressed()
    {
        if (quantityToDelete > 0)
        {
            quantityToDelete--;
            deleteQuantityTPM.text = $"{quantityToDelete} / {selectedItem.quantity}";
        }
    }

    public void OnDeleteButtonPressed()
    {
        if (quantityToDelete == selectedItem.quantity)
        {
            selectedInventorySlot.DeselectSlot();
            itemInfoPanel.SetActive(false);
            selectedInventorySlot = null;

        }

        inventory.RemoveItem(selectedItem, quantityToDelete);
        quantityToDelete = 0;
        deleteQuantityTPM.text = $"{quantityToDelete} / {selectedItem.quantity}";
    }

    

}
