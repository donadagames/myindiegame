using System;
using System.Collections;
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

    public List<QuestObjective> questObjectives = new List<QuestObjective>();
    public List<Quest> quests = new List<Quest>();
    public GameObject questObjectiveSlot;

    [SerializeField] AudioClip completedQuestAudioClip = null;
    [SerializeField] AudioClip recivedQuestAudioClip = null;
    [SerializeField] GameObject questSlot;
    [SerializeField] Transform questsContainerParent = null;

    private List<QuestSlot> questSlots = new List<QuestSlot>();
    private Inventory inventory;

    private void Start()
    {
        inventory = Inventory.instance;

        OnUpdateQuestObjetiveData += UpdateQuestObjectiveData;
        OnUpdateQuestObjetiveDataOnLoad += UpdateQuestObjectiveDataOnLoad;
    }


    #region QUEST OBJECTIVES DATA

    public void AddQuestObjectiveData(QuestObjective _questObjective, int _quantidade)
    {
        if (_questObjective == null || !questObjectives.Contains(_questObjective)) return;

        OnUpdateQuestObjetiveData?.Invoke(this, new OnUpdateQuestObjetiveDataEventHandler { questObjetive = _questObjective, quantity = _quantidade });
    }

    public void AddQuestObjectiveDataOnLoad(QuestObjective _questObjective, int _quantidade)
    {
        if (_questObjective == null || !questObjectives.Contains(_questObjective)) return;

        OnUpdateQuestObjetiveDataOnLoad?.Invoke(this, new OnUpdateQuestObjetiveDataEventHandler { questObjetive = _questObjective, quantity = _quantidade });
    }

    public void RemoveQuestObjectiveData(QuestObjective _questObjective, int _quantidade)
    {
        if (_questObjective == null || !questObjectives.Contains(_questObjective)) return;

        OnUpdateQuestObjetiveData?.Invoke(this, new OnUpdateQuestObjetiveDataEventHandler { questObjetive = _questObjective, quantity = _quantidade });
    }

    public void UpdateQuestObjectiveDataOnLoad(object sender, OnUpdateQuestObjetiveDataEventHandler handler)
    {
        handler.questObjetive.currentQuantity = handler.quantity;
    }

    public void UpdateQuestObjectiveData(object sender, OnUpdateQuestObjetiveDataEventHandler handler)
    {
        var text = string.Empty;

        /*
        if (inventory.inventoryUI.    isPortuguese)
        {
            if (handler.quantity > 0)
                text = $"+{handler.quantity} {handler.questObjetive.namePT}\nAtualizando missão: {handler.questObjetive.quest.questTitlePT}";
            else
                text = $"Atualizando missão: {handler.questObjetive.quest.questTitlePT}";
        }

        else
        {
            if (handler.quantity > 0)
                text = $"+{handler.quantity} {handler.questObjetive.nameENG}\nQuest update: {handler.questObjetive.quest.questTitle}";
            else
                text = $"Quest update: {handler.questObjetive.quest.questTitle}";
        }
        */
        handler.questObjetive.currentQuantity += handler.quantity;
        CheckObjectiveComplete(handler.questObjetive);
        //inventory.ShowInfoText(text);

        foreach (Quest quest in quests)
        {
            if (quest.objectives.Contains(handler.questObjetive))
            {
                CheckQuestCompleted(quest);
            }
        }
    }

    private void AddObjective(Quest quest, QuestSlot slot, QuestObjective objective)
    {
        var _objective = Instantiate(questObjectiveSlot, slot.questObjectivesParent);
        var _objectiveSlot = _objective.GetComponent<QuestObjectiveSlot>();
        _objectiveSlot.objective = objective;
        slot.objectiveSlots.Add(_objectiveSlot);
    }

    public void CheckQuestCompleted(Quest _quest)
    {
        foreach (QuestObjective objective in _quest.objectives)
        {
            if (objective.isCompleted == false) return;
        }

        _quest.isCompleted = true;

        OnQuestCompleted?.Invoke(this, new OnQuestCompletedEventHandler { quest = _quest });
    }

    public void CheckObjectiveComplete(QuestObjective objective)
    {
        if (objective.currentQuantity >= objective.completeQuantity)
        {
            objective.isCompleted = true;
        }
    }
    #endregion

    #region EVENTS
    public event EventHandler<OnUpdateQuestObjetiveDataEventHandler> OnUpdateQuestObjetiveData;

    public event EventHandler<OnUpdateQuestObjetiveDataEventHandler> OnUpdateQuestObjetiveDataOnLoad;

    public class OnUpdateQuestObjetiveDataEventHandler : EventArgs
    {
        public QuestObjective questObjetive;
        public int quantity;
        public bool isCompleted;
    }

    public event EventHandler<OnQuestCompletedEventHandler> OnQuestCompleted;

    public class OnQuestCompletedEventHandler : EventArgs
    {
        public Quest quest;
    }
    #endregion
}
