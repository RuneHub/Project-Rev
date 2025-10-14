using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;
using System.Linq;
using UnityEngine;
using UnityEditor.Playables;

public class BehaviourTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

    SerializedBehaviourTree serializer;
    BehaviourTreeSettings settings;

    public struct ScriptTemplate
    {
        public TextAsset templateFile;
        public string defaultFileName;
        public string subFolder;
    }

    public ScriptTemplate[] scriptFileAsset =
    {
        new ScriptTemplate{ templateFile=BehaviourTreeSettings.GetOrCreateSettings().TemplateActionNode, defaultFileName="newActionNode.cs", subFolder="Action Node"},
        new ScriptTemplate{ templateFile=BehaviourTreeSettings.GetOrCreateSettings().TemplateCompositeNode, defaultFileName="newCompositeNode.cs", subFolder="Composite Node"},
        new ScriptTemplate{ templateFile=BehaviourTreeSettings.GetOrCreateSettings().TemplateDecoratorNode, defaultFileName="newDecoratorNode.cs", subFolder="Decorator Node"},
        new ScriptTemplate{ templateFile=BehaviourTreeSettings.GetOrCreateSettings().TemplateUtilityActionNode, defaultFileName="newUtilityActionNode.cs", subFolder="Utility Action Node" },
        new ScriptTemplate{ templateFile=BehaviourTreeSettings.GetOrCreateSettings().TemplateUtilityCompositeNode, defaultFileName="newUtilityCompositeNode.cs", subFolder="Utility Composite Node"},
        new ScriptTemplate{ templateFile=BehaviourTreeSettings.GetOrCreateSettings().TemplateUtilityDecoratorNode, defaultFileName="newUtilityDecoratorNode.cs", subFolder="Utility Decorator Node"},
    };

    public BehaviourTreeView()
    {
        settings = BehaviourTreeSettings.GetOrCreateSettings();
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourTree/UIBuilder/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    internal void PopulateView(SerializedBehaviourTree tree)
    {
        serializer = tree;

        ClearView();

        Debug.Assert(serializer.tree.rootNode != null);

        serializer.tree.nodes.ForEach(n => CreateNodeView(n));

        serializer.tree.nodes.ForEach(n =>
        {
            var children = BehaviourTree.GetChildren(n);
            children.ForEach(c =>
            {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);

                Edge edge = parentView.output.ConnectTo(childView.input);
                AddElement(edge);
            });
        });

        contentViewContainer.transform.position = serializer.tree.viewPosition;
        contentViewContainer.transform.scale = serializer.tree.viewScale;

    }

    public void ClearView()
    {
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {

        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem => 
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    serializer.DeleteNode(nodeView.node);
                    OnNodeSelected(null);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    serializer.RemoveChild(parentView.node, childView.node);
                }
            });
        }

        if(graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                serializer.AddChild(parentView.node, childView.node);

            });
        }

        if (graphViewChange.movedElements != null)
        {
            nodes.ForEach((n) => {
                NodeView view = n as NodeView;
                view.SortChildren();
            });
        }

        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);

        evt.menu.AppendAction($"Create Node Script/New Action Node", (a) => CreateNewScript(scriptFileAsset[0]));
        evt.menu.AppendAction($"Create Node Script/New Composite Node", (a) => CreateNewScript(scriptFileAsset[1]));
        evt.menu.AppendAction($"Create Node Script/New Decorator Node", (a) => CreateNewScript(scriptFileAsset[2]));
        evt.menu.AppendAction($"Create Node Script/New Utility Action Node", (a) => CreateNewScript(scriptFileAsset[3]));
        evt.menu.AppendAction($"Create Node Script/New Utility Composite Node", (a) => CreateNewScript(scriptFileAsset[4]));
        evt.menu.AppendAction($"Create Node Script/New Utility Decorator Node", (a) => CreateNewScript(scriptFileAsset[5]));

        Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);

        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Action]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Composite]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Decorator]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<UtilityActionNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Utility Action]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<UtilityCompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Utility Composite]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<UtilityDecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[Utility Decorator]/{type.Name}", (a) => CreateNode(type, nodePosition));
            }
        }

        evt.menu.AppendAction($"Create Group", (a) => CreateGroup("Node Group", nodePosition));
    }

    private void CreateNewScript(ScriptTemplate scriptTemplate)
    {
        SelectFolder($"{settings.newNodeBasePath}/{scriptTemplate.subFolder}");
        var templatePath = AssetDatabase.GetAssetPath(scriptTemplate.templateFile);
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, scriptTemplate.defaultFileName);
    }

    private void CreateGroup(string _title, Vector2 mousePosition)
    {
        Group group = new Group()
        {
            title = _title
        };

        group.SetPosition(new Rect(mousePosition, Vector2.zero));
        AddElement(group);
    }

    private void SelectFolder(string path)
    {
        if (path[path.Length - 1] == '/')
            path = path.Substring(0, path.Length - 1);

        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
        Selection.activeObject = obj;
        EditorGUIUtility.PingObject(obj);

    }

    void CreateNode(System.Type type, Vector2 position)
    {
        Node node = serializer.CreateNode(type, position);
        CreateNodeView(node);
    }

    void CreateNodeView(Node node)
    {
        if (serializer == null)
            Debug.Log("null serializer");

        NodeView nodeView = new NodeView(serializer, node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    public void UpdateNodeState()
    {
        nodes.ForEach(n => {
            NodeView view = n as NodeView;
            view.UpdateState();
        });
    }

}
