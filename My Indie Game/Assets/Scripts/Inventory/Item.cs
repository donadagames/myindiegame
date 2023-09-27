using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public string[] _name;
    [TextArea(5, 10)]
    public string[] _description;
    public int _quantity;
    public Sprite _icon;

    public virtual void Use()
    {

    }
}
