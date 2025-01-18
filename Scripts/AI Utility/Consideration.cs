using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.AI
{
    public abstract class Consideration : ScriptableObject
    {
        protected Node node;

        protected BehaviourContext _context;
        public BehaviourContext context { get { return _context; } set { _context = value; } }

        protected float _score = 0;
        public float score { get { return _score; } set { _score = Mathf.Clamp01(value); } }

        public abstract float ScoreConsideration(AIBossManager a, Node _node);
    }
}