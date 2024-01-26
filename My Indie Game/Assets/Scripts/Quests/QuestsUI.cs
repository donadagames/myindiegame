using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestsUI : MonoBehaviour
{
    public GameObject questPanel;
    public Transform inProgressQuestsParent;
    public Transform completedQuestParent;
    [SerializeField] TextMeshProUGUI inProgressText;
    [SerializeField] TextMeshProUGUI completedText;
    [Header("Sprite[0] = default")]
    public Sprite[] inProgressSprites = new Sprite[2];
    public Sprite[] completedSprites = new Sprite[2];
    public Image inProgressIcon;
    public Image completedIcon;
    public Color defaultColor;
    public Color selectedColor;

    [HideInInspector] public SettingsController settingsController;

    [SerializeField] GameObject questSlot;

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
        foreach (QuestSlot slot in questSlots)
        {
            slot.UpdateLanguage(handler.language, settingsController.languageController);
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

        if (settingsController.languageController.GetGlobalLanguage() == Language.Portuguese)
        {
            settingsController.languageController.SetRegularFont(slot.questText);
            slot.questText.text = quest.questTexts[0];
        }
        else if (settingsController.languageController.GetGlobalLanguage() == Language.English)
        {
            settingsController.languageController.SetRegularFont(slot.questText);
            slot.questText.text = quest.questTexts[1];
        }
        else if (settingsController.languageController.GetGlobalLanguage() == Language.Chinese)
        {
            settingsController.languageController.SetChineseFont(slot.questText);
            slot.questText.text = quest.questTexts[2];
        }
    }


    //TEM QUE FAZER ON COMPLETE QUEST

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

    public void ResetAllQuestSlots()
    { 
        foreach(QuestSlot slot in questSlots)
        {
            Destroy(slot.gameObject);
        }
    }
}
