using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public string[] names;
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
}
