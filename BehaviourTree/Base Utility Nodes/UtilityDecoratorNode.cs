using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class UtilityDecoratorNode : Node
{
    protected float _score = 0;
    public float score { get { return _score; } set { _score = Mathf.Clamp01(value); } }

    [HideInInspector]
    [SerializeReference]
    public Node child;
    public Consideration[] considerations;
}
