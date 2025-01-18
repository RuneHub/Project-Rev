using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.AI
{
    [CreateAssetMenu(fileName = "ArmorConsideration", menuName = "AI Behaviour/Test Considerations/Armor Consideration")]
    public class AIC_Armor : Consideration
    {
        [SerializeField] private AnimationCurve responseCurve;
        public override float ScoreConsideration(AIBossManager ai, Node node)
        {
            return score = responseCurve.Evaluate(Mathf.Clamp01(ai.charStatManager.currentArmor / ai.charStatManager.maxArmor));
        }
    }
}