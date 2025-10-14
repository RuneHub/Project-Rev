using KS;
using KS.AI;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BehaviourContext
{

    public GameObject gameObject;

    public AIBossManager boss;
    public PlayerManager target;
    public UtilityAI utility;

    public static BehaviourContext CreateFromObject(GameObject gameObject)  
    {
        BehaviourContext context = new BehaviourContext();
        context.gameObject = gameObject;
        context.boss = gameObject.GetComponent<AIBossManager>();
        context.target = context.boss.GetTarget();
        context.utility = context.boss.GetComponent<UtilityAI>();

        return context;
    }
}
