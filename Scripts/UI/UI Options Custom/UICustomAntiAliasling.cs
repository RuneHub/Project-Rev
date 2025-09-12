using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

namespace KS { 
public class UICustomAntiAliasling : BaseUIOptions
{
    [SerializeField] private UniversalRenderPipelineAsset customRenderAsset;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void NextOption()
    {
        base.NextOption();
    }

    protected override void PrevOption()
    {
        base.PrevOption();
    }

    protected override void SetOption()
    {
        base.SetOption();
        switch (currentIndex)
        {
            case 0:
                customRenderAsset.msaaSampleCount = 0;
                break;
            case 1:
                customRenderAsset.msaaSampleCount = 2;
                break;
            case 2:
                customRenderAsset.msaaSampleCount = 4;
                break;
            case 3:
                customRenderAsset.msaaSampleCount = 8;
                break;
            }

    }

    public override void OnMove(AxisEventData eventData)
    {
        base.OnMove(eventData);
    }
}
}