using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{

    public BehaviourTree tree;

    private BehaviourContext context;

    // Start is called before the first frame update
    void Start()
    {
        context = CreateBehaviourTreeContext();
        tree = tree.Clone();
        tree.Bind(context);
    }

    // Update is called once per frame
    void Update()
    {
        if (tree)
        {
            tree.Update(); 
        }
    }

    BehaviourContext CreateBehaviourTreeContext()
    {
        return BehaviourContext.CreateFromObject(gameObject);
    }

    public BehaviourContext GetContext()
    {
        return context;
    }

}
