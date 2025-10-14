using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    [SerializeReference] 
    public Node rootNode;
    [SerializeReference] 
    public List<Node> nodes = new List<Node>();

    public Node.State treeState = Node.State.Running;

    public Blackboard blackboard = new Blackboard();

    public Vector3 viewPosition = new Vector3(600, 300);
    public Vector3 viewScale = Vector3.one;

    public BehaviourTree()
    {
        rootNode = new RootNode();
        nodes.Add(rootNode);
    }

    public Node.State Update()
    {
        if (rootNode.state == Node.State.Running)
        {
            treeState = rootNode.Update();
        }

        return treeState;
    }

    public static List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();

        if(parent is RootNode rootNode && rootNode.child != null)
        {
            children.Add(rootNode.child);
        }

        if (parent is DecoratorNode decorator && decorator.child != null)
        {
            children.Add(decorator.child);
        }

        if (parent is CompositeNode composite)
        {
            return composite.children;
        }

        if (parent is UtilityCompositeNode uComposite)
        {
            return uComposite.children;
        }

        if (parent is UtilityDecoratorNode udecorator && udecorator.child != null)
        {
            children.Add(udecorator.child);
        }

        return children;
    }

    public static void Traverse(Node node, System.Action<Node> visiter)
    {
        if (node != null)
        {
            visiter.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visiter));
        }
    }

    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        return tree;
    }

    public void Bind(BehaviourContext context)
    {
        Traverse(rootNode, node =>
        {
            node.context = context;
            node.blackboard = blackboard;
        });
    }

}
