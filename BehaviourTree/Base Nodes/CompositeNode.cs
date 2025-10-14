using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public abstract class CompositeNode : Node
{
    [HideInInspector]
    [SerializeReference]
    public List<Node> children = new List<Node>();
    
}
