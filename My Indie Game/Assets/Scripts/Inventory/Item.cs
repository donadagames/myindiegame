using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    [ContextMenu("Generate ID")]
    private void GenereteID() => saveableEntityId = Guid.NewGuid().ToString();
    public string saveableEntityId;
    public string[] itemNames;
    [TextArea(5, 10)]
    public string[] descriptions;
    public int quantity;
    public Sprite icon;
    public AudioClip audioClip;
    public QuestObjective questObjective;
    public Status status;
    public bool deletable = true;
    public bool canBeUsedOnActionBar = false;
    public bool isOnlyQuestObjective = false;

    public virtual void Use()
    {
        status = Status.instance;
    }

    public void ResetItem()
    { 
        quantity = 0;
    }
}
