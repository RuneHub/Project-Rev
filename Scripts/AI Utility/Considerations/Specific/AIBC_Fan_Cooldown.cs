using KS;
using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "Fan Cooldown Consideration", menuName = "AI Behaviour/Boss Considerations/Fan cooldown Consideration")]
public class AIBC_Fan_Cooldown : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    [SerializeField] private float maxCooldownTime = 60;

    public override float ScoreConsideration(AIBossManager a, Node _node)
    {
        float cd = 0;
        node = _node;

        if (node is UFanAttackNode)
        {
            UFanAttackNode uan = (UFanAttackNode)node;
            cd = uan.FanAttack.cooldown;
        }

        return score = responseCurve.Evaluate(cd / maxCooldownTime);
    }
}