using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Quest/Quest")]
public class Quest : ScriptableObject
{
    public string questID;

    public string questTitle;
    [TextArea(5, 10)]
    public string questInfo;

    public string questTitlePT;
    [TextArea(5, 10)]
    public string questInfoPT;

    public bool isCompleted = false;

    public List<QuestObjective> objectives = new List<QuestObjective>();

    [ContextMenu("Generate ID")]
    private void GenereteID() => questID = Guid.NewGuid().ToString();
}
