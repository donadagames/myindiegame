using System;
using UnityEngine;


[Serializable]
[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public string[] itemNames;
    [TextArea(5, 10)]
    public string[] descriptions;
    public int quantity;
    public Sprite icon;
    public AudioClip audioClip;
    public bool deletable = true;
    public QuestObjective questObjective;
    public Status status;
    public bool canBeUsedOnActionBar = false;

    public virtual void Use()
    {
        status = Status.instance;
    }

    public void ResetItem()
    { 
        quantity = 0;
    }
}
