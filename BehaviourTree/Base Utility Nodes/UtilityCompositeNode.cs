using KS.AI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public abstract class UtilityCompositeNode : Node
{
    protected float _score = 0;
    public float score { get { return _score; } set { _score = Mathf.Clamp01(value); } }

    [HideInInspector]
    [SerializeReference]
    public List<Node> children = new List<Node>();
    public Consideration[] considerations;
}
