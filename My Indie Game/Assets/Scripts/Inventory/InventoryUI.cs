using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventorySlot selectedInventorySlot;

    [SerializeField] TextMeshProUGUI itemNameTPM;
    [SerializeField] TextMeshProUGUI itemDescriptionTPM;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject itemInfoPanel;

    [Header("Intem Info Panel")]
    public TextMeshProUGUI itemQuantityTPM;

    [SerializeField] TextMeshProUGUI deleteQuantityTPM;
    [SerializeField] GameObject deletePanel;
    [SerializeField] Image itemIcon;

    [HideInInspector] public SettingsController settingsController;

    public Sprite[] selectedSlot;
    public Sprite[] deselectedSlot;
    public TextMeshProUGUI displayItem;

    private Item selectedItem;
    private int quantityToDelete;
    private Inventory inventory;

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
        settingsController = SettingsController.instance;
        inventory = Inventory.instance;
    }

    public void DisplayItemInformation(Item item)
    {
        if (item == null) return;
        else
        {
            itemIcon.sprite = item.icon;
            selectedItem = item;
            settingsController.uiController.PlayDefaultAudioClip();

            if (settingsController.languageController.GetGlobalLanguage() == Language.Portuguese)
            {
                settingsController.languageController.SetRegularFont(itemNameTPM);
                settingsController.languageController.SetRegularFont(itemDescriptionTPM);
                settingsController.languageController.SetRegularFont(itemQuantityTPM);

                itemNameTPM.text = item.itemNames[0];
                itemDescriptionTPM.text = item.descriptions[0];
                itemQuantityTPM.text = item.quantity.ToString();

            }

            else if(settingsController.languageController.GetGlobalLanguage() == Language.English)
            {
                settingsController.languageController.SetRegularFont(itemNameTPM);
                settingsController.languageController.SetRegularFont(itemDescriptionTPM);
                settingsController.languageController.SetRegularFont(itemQuantityTPM);

                itemNameTPM.text = item.itemNames[1];
                itemDescriptionTPM.text = item.descriptions[1];
                itemQuantityTPM.text = item.quantity.ToString();
            }

            else if (settingsController.languageController.GetGlobalLanguage() == Language.Chinese)
            {
                settingsController.languageController.SetChineseFont(itemNameTPM);
                settingsController.languageController.SetChineseFont(itemDescriptionTPM);
                settingsController.languageController.SetChineseFont(itemQuantityTPM);

                itemNameTPM.text = item.itemNames[2];
                itemDescriptionTPM.text = item.descriptions[2];
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

        settingsController.uiController.PlayDefaultAudioClip();

        selectedItem = null;
        quantityToDelete = 0;
        if (inventoryPanel.activeSelf == true)
        {
            inventoryPanel.SetActive(false);
            inventory.status.saveSystem.SaveInventory();
        }

        else
        {
            inventoryPanel.SetActive(true);
        }

        itemInfoPanel.SetActive(false);
    }

    public void OnPlusButtonPressed()
    {
        if (quantityToDelete < selectedItem.quantity)
        {
            settingsController.uiController.PlayDefaultAudioClip();

            quantityToDelete++;
            deleteQuantityTPM.text = $"{quantityToDelete} / {selectedItem.quantity}";
        }
    }

    public void OnMinusButtonPressed()
    {
        if (quantityToDelete > 0)
        {
            settingsController.uiController.PlayDefaultAudioClip();

            quantityToDelete--;
            deleteQuantityTPM.text = $"{quantityToDelete} / {selectedItem.quantity}";
        }
    }

    public void OnDeleteButtonPressed()
    {
        if (quantityToDelete <= 0) return;

        if (quantityToDelete == selectedItem.quantity)
        {
            selectedInventorySlot.DeselectSlot();
            itemInfoPanel.SetActive(false);
            selectedInventorySlot = null;

        }

        settingsController.uiController.PlayDefaultAudioClip();

        inventory.RemoveItem(selectedItem, quantityToDelete);
        quantityToDelete = 0;
        deleteQuantityTPM.text = $"{quantityToDelete} / {selectedItem.quantity}";
    }

    

}
