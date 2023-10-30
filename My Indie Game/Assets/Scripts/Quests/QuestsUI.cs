using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestsUI : MonoBehaviour
{
    public GameObject questPanel;
    public Transform inProgressQuestsParent;
    public Transform completedQuestParent;
    public TextMeshProUGUI inProgressText;
    public TextMeshProUGUI completedText;
    [Header("Sprite[0] = default")]
    public Sprite[] inProgressSprites = new Sprite[2];
    public Sprite[] completedSprites = new Sprite[2];
    public Image inProgressIcon;
    public Image completedIcon;
    public Color defaultColor;
    public Color selectedColor;

    [HideInInspector] public SettingsController settingsController;

    [SerializeField] GameObject questSlot;
    [SerializeField] TextMeshProUGUI questTitle;

    private Inventory inventory;
    private Quests quests;
   private List<QuestSlot> questSlots = new List<QuestSlot>();
    #region Singleton
    public static QuestsUI instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
    #endregion

    private void Start()
    {
        quests = Quests.instance;
        settingsController = SettingsController.instance;
        inventory = Inventory.instance;
        settingsController.OnLanguageChanged += OnLanguageUpdate;
    }

    public void OnLanguageUpdate(object sender, SettingsController.OnLanguageChangeEventHandler handler)
    {
        if (handler._isPortuguese == true)
        {
            questTitle.text = "Missões";
            inProgressText.text = "Em andamento";
            completedText.text = "Finalizadas";
        }

        else
        {
            questTitle.text = "Quests";
            inProgressText.text = "In Progress";
            completedText.text = "Completed";
        }

        foreach (QuestSlot slot in questSlots)
        {
            slot.UpdateLanguage(handler._isPortuguese);
        }
    }

    public void OnQuestButtonPressed()
    {
        if (questPanel.activeSelf == false)
            OnInProgressButtonPressed();

        questPanel.SetActive(!questPanel.activeSelf);
        settingsController.uiController.PlayDefaultAudioClip();
    }

    public void AddNewQuest(Quest quest)
    {
        QuestSlot slot = Instantiate(questSlot, inProgressQuestsParent).GetComponent<QuestSlot>();

        slot.OnCreateQuestSlot(inventory);
        slot.quest = quest;
        slot.questObjective = quest.objective;
        slot.slider.maxValue = quest.objective.completeQuantity;

        questSlots.Add(slot);

        if (quest.objective.currentQuantity < quest.objective.completeQuantity)

        {
            slot.sliderText.text = $"{quest.objective.currentQuantity} / {quest.objective.completeQuantity}";
            slot.slider.value = quest.objective.currentQuantity;
        }

        else
        {
            slot.sliderText.text = $"{quest.objective.completeQuantity} / {quest.objective.completeQuantity}";
            slot.slider.value = quest.objective.completeQuantity;
        }

        slot.questIcon.sprite = quest.icon;
        slot.rewardIcon.sprite = quest.rewardItem.icon;
        slot.rewardText.text = $"{quest.rewardQuantity}";

        if (settingsController.IsPortuguese() == true)
        {
            slot.questText.text = quest.questTexts[0];
        }

        else
        {
            slot.questText.text = quest.questTexts[1];
        }
    }

    public void OnCompleteQuest(Quest quest)
    {

    }

    public void OnInProgressButtonPressed()
    {
        completedIcon.sprite = completedSprites[0];
        inProgressIcon.sprite = inProgressSprites[1];

        inProgressText.color = selectedColor;
        completedText.color = defaultColor;

        completedQuestParent.gameObject.SetActive(false);
        inProgressQuestsParent.gameObject.SetActive(true);

        settingsController.uiController.PlayDefaultAudioClip();
    }

    public void OnCompleteButtonPressed()
    {
        completedIcon.sprite = completedSprites[1];
        inProgressIcon.sprite = inProgressSprites[0];

        inProgressText.color = defaultColor;
        completedText.color = selectedColor;

        inProgressQuestsParent.gameObject.SetActive(false);
        completedQuestParent.gameObject.SetActive(true);

        settingsController.uiController.PlayDefaultAudioClip();
    }
}
