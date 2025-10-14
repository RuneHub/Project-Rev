using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

class BehaviourTreeSettings : ScriptableObject
{

    public VisualTreeAsset behaviourTreeXml;
    public StyleSheet behaviourTreeStyle;
    public VisualTreeAsset nodeXml;
    public TextAsset TemplateActionNode;
    public TextAsset TemplateCompositeNode;
    public TextAsset TemplateDecoratorNode;
    public TextAsset TemplateUtilityActionNode;
    public TextAsset TemplateUtilityCompositeNode;
    public TextAsset TemplateUtilityDecoratorNode;
    public string newNodeBasePath = "Assets/";

    static BehaviourTreeSettings FindSettings()
    {
        var guids = AssetDatabase.FindAssets("t:BehaviourTreeSettings");
        if (guids.Length > 1)
        {
            Debug.LogWarning($"Multiple Setting files found, Inserting first found file.");
        }

        switch (guids.Length)
        {
            case 0:
                return null;
            default:
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<BehaviourTreeSettings>(path);
        }
    }

    internal static BehaviourTreeSettings GetOrCreateSettings()
    {
        var settings = FindSettings();
        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<BehaviourTreeSettings>();
            AssetDatabase.CreateAsset(settings, $"Assets");
            AssetDatabase.SaveAssets();
        }
        return settings;
    }

    internal static SerializedObject GetSerializedSettings()
    {
        return new SerializedObject(GetOrCreateSettings());
    }

}


static class CustomSettingsRegisterUIElements 
{
    [SettingsProvider]
    public static SettingsProvider CreateCustomSettingsRegisterUIElements()
    {
        var provider = new SettingsProvider("Project/CustomSettingsIUElements", SettingsScope.Project)
        {
            label = "BehaviourTree",
            activateHandler = (searchContext, rootElement) =>
            {
                var settings = BehaviourTreeSettings.GetSerializedSettings();

                var title = new Label()
                {
                    text = "Behaviour Tree Settings"
                };
                title.AddToClassList("title");
                rootElement.Add(title);

                var properties = new VisualElement()
                {
                    style = { flexDirection = FlexDirection.Column }
                };
                properties.AddToClassList("poperty-list");
                rootElement.Add(properties);

                properties.Add(new InspectorElement(settings));

                rootElement.Bind(settings);
            },
        };

        return provider;
    }
}