using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class QuestSlot : MonoBehaviour
{
    public Quest quest;
    public TextMeshProUGUI questText = null;
    public QuestObjective questObjective;
    public Slider slider;
    public TextMeshProUGUI sliderText;
    public Image questIcon;
    public Image rewardIcon;
    public TextMeshProUGUI rewardText;
    Inventory inventory;

    public Image background;
    public Sprite completedBackground;

    public void OnCreateQuestSlot(Inventory _inventory)
    {
        inventory = _inventory;

        inventory.OnUpdateQuestObjective += UpdateQuest;
        inventory.status.quests.OnQuestCompleted += CompleteQuest;
    }

    public void UpdateLanguage(object sender)
    {
    }

    public void UpdateQuest(object sender, Inventory.OnUpdateQuestObjectiveEventHandler handler)
    {
        if (!handler.questObjective.isCompleted)
        {
            if (handler.questObjective.currentQuantity >= handler.questObjective.completeQuantity)
            {
                slider.value = handler.questObjective.completeQuantity;
                sliderText.text = $"{handler.questObjective.completeQuantity} / {handler.questObjective.completeQuantity}";
            }

            else
            {
                slider.value = handler.questObjective.currentQuantity;
                sliderText.text = $"{handler.questObjective.currentQuantity} / {handler.questObjective.completeQuantity}";
            }
        }
    }

    public void CompleteQuest(object sender, Quests.OnQuestCompletedEventHandler handler)
    {
        if (handler.quest == quest)
        {
            inventory.OnUpdateQuestObjective -= UpdateQuest;
            transform.SetParent(inventory.status.quests.questsUI.completedQuestParent);
            background.sprite = completedBackground;
            slider.gameObject.SetActive(false);
        }
    }
}
