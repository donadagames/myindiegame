using System;
using System.Collections.Generic;
using UnityEngine;

public class Quests : MonoBehaviour
{
    #region SINGLETON
    public static Quests instance;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }
    #endregion

    public List<Quest> quests = new List<Quest>();
    public QuestsUI questsUI;

    private void Start()
    {
        questsUI = QuestsUI.instance;
    }

    public void AddQuest(Quest quest)
    {
        if (quests.Contains(quest))
            return;
        else
        {
            quests.Add(quest);
            questsUI.AddNewQuest(quest);
        }
    }

    public bool QuestIsCompleted(Quest quest) => quests.Contains(quest) 
        && quest.objective.currentQuantity >= quest.objective.completeQuantity;

    public void CompleteQuest(Quest quest)
    {
        OnQuestCompleted?.Invoke(this, new OnQuestCompletedEventHandler { quest = quest });
        quest.isCompleted = true;
    }

    public event EventHandler<OnQuestCompletedEventHandler> OnQuestCompleted;

    public class OnQuestCompletedEventHandler : EventArgs
    {
        public Quest quest;
    }
}
