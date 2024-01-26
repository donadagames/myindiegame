using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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

    public string saveableEntityId;
    [ContextMenu("Generate ID")]
    private void GenereteID() => saveableEntityId = Guid.NewGuid().ToString();

    public List<Quest> quests = new List<Quest>();
    public QuestsUI questsUI;
    private Status status;


    private void Start()
    {
        status = Status.instance;

        quests = new List<Quest>();
        questsUI = QuestsUI.instance;
    }

    public void AddQuest(Quest quest)
    {
        if (quests.Contains(quest))
            return;
        else
        {
            quests.Add(quest);

            if (!quest.isOnlyForDialogues)
                questsUI.AddNewQuest(quest);

            status.saveSystem.SaveQuests();
        }
    }

    public bool QuestIsCompleted(Quest quest) => quests.Contains(quest)
        && quest.objective.currentQuantity >= quest.objective.completeQuantity;

    public void CompleteQuest(Quest quest)
    {
        OnQuestCompleted?.Invoke(this, new OnQuestCompletedEventHandler { quest = quest });
        quest.isCompleted = true;

        status.saveSystem.SaveQuests();
    }

    public event EventHandler<OnQuestCompletedEventHandler> OnQuestCompleted;

    public class OnQuestCompletedEventHandler : EventArgs
    {
        public Quest quest;
    }

    public object CaptureState()
    {
        var listOfSavedQuests = new List<SaveGameQuest>();

        foreach (Quest quest in quests)
        {
            listOfSavedQuests.Add(new SaveGameQuest(quest.saveableEntityId, quest.isCompleted));
        }

        return new SaveData { savedQuests = listOfSavedQuests };
    }
    public void RestoreState(object state)
    {

        questsUI.ResetAllQuestSlots();

        var saveData = (SaveData)state;

        for (int i = 0; i < saveData.savedQuests.Count; i++)
        {
            if (status.saveSystem.questsID.TryGetValue(saveData.savedQuests[i].questID, out Quest quest))
            {
                AddQuest(quest);
                if (saveData.savedQuests[i].isCompleted)
                {
                    CompleteQuest(quest);
                    quest.objective.currentQuantity = quest.objective.completeQuantity;
                    quest.objective.isCompleted = true;
                }
            }
        }
    }

    [Serializable]
    private struct SaveData
    {
        public List<SaveGameQuest> savedQuests;
    }
}

[System.Serializable]
public class SaveGameQuest
{
    public string questID;
    public bool isCompleted;

    public SaveGameQuest(string _questID, bool _isCompleted)
    {
        questID = _questID;
        isCompleted = _isCompleted;
    }
}
