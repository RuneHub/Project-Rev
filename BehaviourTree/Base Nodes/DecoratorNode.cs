using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DecoratorNode : Node
{
    [HideInInspector]
    [SerializeReference]
    public Node child;

}
