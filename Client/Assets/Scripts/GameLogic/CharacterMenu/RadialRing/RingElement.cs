using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RingElement", menuName = "RingMenu/Element", order = 1)]
public class RingElement : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Ring NextRing;
}
