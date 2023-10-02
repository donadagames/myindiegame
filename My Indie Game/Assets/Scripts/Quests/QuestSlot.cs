using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class QuestSlot : MonoBehaviour
{
    public Quest quest;
    public TextMeshProUGUI questTitle = null;
    public TextMeshProUGUI questInfo = null;
    public Transform questObjectivesParent;
    public List<QuestObjectiveSlot> objectiveSlots;
    UIController uiController;

    private void Start()
    {
        uiController = UIController.instance;
        uiController.OnLanguageChange += UpdateLanguage;

        if (uiController.isPortuguese == true)
        {
            questTitle.text = quest.questTitlePT;
            questInfo.text = quest.questInfoPT;
        }

        else
        {
            questTitle.text = quest.questTitle;
            questInfo.text = quest.questInfo;
        }
    }

    public void UpdateLanguage(object sender, UIController.OnLanguageChangeEventHandler args)
    {
        if (args.isPortuguese == true)
        {
            questTitle.text = quest.questTitlePT;
            questInfo.text = quest.questInfoPT;
        }

        else
        {
            questTitle.text = quest.questTitle;
            questInfo.text = quest.questInfo;
        }
    }
}
