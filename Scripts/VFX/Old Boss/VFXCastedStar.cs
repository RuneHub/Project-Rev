using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXCastedStar : VFXBase
{

    [ColorUsage(true, true)] public Color color;
    [ColorUsage(true, true)] public Color rColor;

    public float rDuration;
    public float rSize;

    public Vector2 particleSize;
    public float particleRadius;

    public void SetColor(Color _color)
    {
        color = _color;
    }

    public void SetRColor(Color _color)
    {
        rColor = _color;
    }

    public void SetRDuration(float _duration)
    {
        rDuration = _duration;
    }

    public void SetRSize(float _size)
    {
        rSize = _size;
    }

    public void SetParticleSize(Vector2 _size)
    {
        particleSize = _size;    
    }

    public void SetSubRadius(float radius)
    {
        particleRadius = radius;
    }

    protected override void Start()
    {
        base.Start();

        vfx.SetFloat("R_Duration", rDuration);
        vfx.SetFloat("R_Size", rSize);
        vfx.SetVector2("BuildUpParticles", particleSize);
        vfx.SetFloat("SubRadius", particleRadius);

        if (applyChanges)
        {
            vfx.SetVector4("Color", color);
            vfx.SetVector4("R_Color", rColor);
        }
    }
}
