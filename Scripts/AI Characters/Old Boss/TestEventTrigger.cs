using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestEventTrigger : MonoBehaviour
{

    public event EventHandler OnEventTriggered;

    public void TriggerEvent()
    {
        OnEventTriggered?.Invoke(this, EventArgs.Empty);
    }
}
