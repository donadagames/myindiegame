using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestsUI : MonoBehaviour
{
    public GameObject questPanel;
    public Transform inProgressQuestsParent;
    public Transform completedQuestParent;
    Quests quests;
    [SerializeField] GameObject questSlot;
    [HideInInspector] public LanguageControll languageControll;
    public TextMeshProUGUI inProgressText;
    public TextMeshProUGUI completedText;
    private List<QuestSlot> inProgressQuestSlots = new List<QuestSlot>();
    [Header("Sprite[0] = default")]
    public Sprite[] inProgressSprites = new Sprite[2];
    public Sprite[] completedSprites = new Sprite[2];
    public Image inProgressIcon;
    public Image completedIcon;
    public Color defaultColor;
    public Color selectedColor;
    Inventory inventory;
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
        languageControll = LanguageControll.instance;
        inventory = Inventory.instance;
    }

    public void OnQuestButtonPressed()
    {
        if (questPanel.activeSelf == false)
            OnInProgressButtonPressed();

        questPanel.SetActive(!questPanel.activeSelf);
    }

    public void AddNewQuest(Quest quest)
    {
        QuestSlot slot = Instantiate(questSlot, inProgressQuestsParent).GetComponent<QuestSlot>();

        slot.OnCreateQuestSlot(inventory);
        slot.quest = quest;
        slot.questObjective = quest.objective;
        slot.slider.maxValue = quest.objective.completeQuantity;

        inProgressQuestSlots.Add(slot);

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

        if (languageControll.isPortuguese)
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
    }

    public void OnCompleteButtonPressed()
    {
        completedIcon.sprite = completedSprites[1];
        inProgressIcon.sprite = inProgressSprites[0];

        inProgressText.color = defaultColor;
        completedText.color = selectedColor;

        inProgressQuestsParent.gameObject.SetActive(false);
        completedQuestParent.gameObject.SetActive(true);
    }
}
