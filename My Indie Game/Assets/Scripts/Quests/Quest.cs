using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Quest/Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    [TextArea(5, 10)]
    [Header("questTexts[0] = Portuguese")]
    public string[] questTexts = new string[2];
    public bool isCompleted = false;
    public QuestObjective objective;
    public Sprite icon;
    public Item rewardItem;
    public int rewardQuantity;
    public Dialogue waitingCompletionDialogue;
    public Dialogue onCompleteDialogue;
    public Dialogue afterCompletedDialogue;

    [ContextMenu("Generate ID")]
    private void GenereteID() => questID = Guid.NewGuid().ToString();

    public void ResetQuest()
    {
        isCompleted = false;
    }
}

