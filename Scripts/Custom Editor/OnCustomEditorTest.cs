using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCustomEditorTest : MonoBehaviour
{
    /*
     * link to the page this is from:
     * https://forum.unity.com/threads/draw-a-field-only-if-a-condition-is-met.448855/
     * */


    public bool BoolTest = false;

    [DrawIf("BoolTest", true)]
    public Vector2 v2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
