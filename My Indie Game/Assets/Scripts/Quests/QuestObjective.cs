using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Quest/Quest Objective")]
public class QuestObjective : ScriptableObject
{
    public int completeQuantity = 0;
    public int currentQuantity;
    public string questObjectiveID = string.Empty;
    public string namePT = string.Empty;
    public string nameENG = string.Empty;
    public bool isCompleted = false;
    public Quest quest;

    [ContextMenu("Generate ID")]
    private void GenereteID() => questObjectiveID = Guid.NewGuid().ToString();
}
