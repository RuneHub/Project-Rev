using KS.AI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[System.Serializable]
public abstract class UtilityActionNode : Node
{
    public float range;
    public float cooldown;
    public bool useCooldown;

    protected float _score = 0;
    public float score { get { return _score; } set { _score = Mathf.Clamp01(value); } }

    public Consideration[] considerations;
}
