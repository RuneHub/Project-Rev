using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using System;
using Codice.Client.Selector;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class BehaviourTreeEditor : EditorWindow
{

    SerializedBehaviourTree serializer;
    BehaviourTreeSettings settings;

    BehaviourTreeView treeView;
    InspectorView inspectorView;
    Label titleLabel;
    BlackboardView blackboardView;

    [MenuItem("BehaviourTreeEditor/Editor ...")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    public static void OpenWindow(BehaviourTree tree)
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
        wnd.SelectTree(tree);
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is BehaviourTree)
        {
            OpenWindow(Selection.activeObject as BehaviourTree);
            return true;
        }
        return false;
    }

    public void CreateGUI()
    {
        settings = BehaviourTreeSettings.GetOrCreateSettings();

        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/BehaviourTree/UIBuilder/BehaviourTreeEditor.uxml");

        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourTree/UIBuilder/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<BehaviourTreeView>();
        inspectorView = root.Q<InspectorView>();
        blackboardView = root.Q<BlackboardView>();
        titleLabel = root.Q<Label>("TitleLabel");

        treeView.OnNodeSelected = OnNodeSelectionChanged;
        Undo.undoRedoPerformed += OnUndoRedo;

        //OnSelectionChange();
        if (serializer != null)
        {
            SelectTree(serializer.tree);
        }
    }

    void OnUndoRedo()
    {
        if (serializer != null)
        {
            treeView.PopulateView(serializer);
        }
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                EditorApplication.delayCall += OnSelectionChange;
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                EditorApplication.delayCall += OnSelectionChange;
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }

    private void OnSelectionChange()
    {
        if (Selection.activeGameObject)
        {
            BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
            if (runner)
            {
                SelectTree(runner.tree);
            }
        }
    }

    void SelectTree(BehaviourTree newTree)
    {
        if (!newTree)
        {
            ClearSelection();
            return;
        }

        serializer = new SerializedBehaviourTree(newTree);

        if (titleLabel != null)
        {
            string path = AssetDatabase.GetAssetPath(serializer.tree);
            if (path == "")
            {
                path = serializer.tree.name;
            }
            titleLabel.text = $"TreeView ({path})";
        }

        treeView.PopulateView(serializer);
        blackboardView.Bind(serializer);
    }

    void ClearSelection()
    {
        serializer = null;
        treeView.ClearView();
    }

    void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateSelection(serializer, node); 
    }

    public void OnInspectorUpdate()
    {
        treeView?.UpdateNodeState();
    }

}