using KS;
using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Random Consideration", menuName = "AI Behaviour/Boss Considerations/Random Consideration")]
public class AIBC_RandomScore : Consideration
{
    public float minNum;
    public float maxNum;

    public override float ScoreConsideration(AIBossManager a, Node _node)
    {
        return score = Random.Range(minNum, maxNum);
    }
}
