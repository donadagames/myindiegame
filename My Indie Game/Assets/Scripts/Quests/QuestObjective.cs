using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Quest/Quest Objective")]
public class QuestObjective : ScriptableObject
{
    public int completeQuantity = 0;
    public int currentQuantity;
    public string questObjectiveID = string.Empty;
    public bool isCompleted = false;
    public Quest quest;
    public Item item;

    [ContextMenu("Generate ID")]
    private void GenereteID() => questObjectiveID = Guid.NewGuid().ToString();

    public void ResetQuestObjective()
    {
        isCompleted = false;
        currentQuantity = 0;
    }
}
