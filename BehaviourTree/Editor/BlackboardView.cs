using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class BlackboardView : VisualElement
{
    public class UXMLFactory : UxmlFactory<BlackboardView, VisualElement.UxmlTraits> { }

    public BlackboardView()
    {

    }

    internal void Bind(SerializedBehaviourTree serializer)
    {
        Clear();

        var blackboardProperty = serializer.Blackboard;
        blackboardProperty.isExpanded = true;

        PropertyField field = new PropertyField();
        field.BindProperty(blackboardProperty);
        Add(field);

    }

}
