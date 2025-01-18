using KS;
using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "Cast Cooldown Consideration", menuName = "AI Behaviour/Boss Considerations/Cast Cooldown Consideration")]
public class AIBC_Cast_Cooldown : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    [SerializeField] private float maxCooldownTime = 60;

    public override float ScoreConsideration(AIBossManager a, Node _node)
    {
        float cd = 0;
        node = _node;

        if (node is ULongCastNode)
        {
            ULongCastNode uan = (ULongCastNode)node;
            cd = uan.MagicCastSO.cooldown;
        }

        return score = responseCurve.Evaluate(cd / maxCooldownTime);

    }
}
