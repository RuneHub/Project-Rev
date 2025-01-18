using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXBase : MonoBehaviour
{
    [SerializeField] protected VisualEffect vfx;
    [SerializeField] protected float duration;
    [SerializeField] protected float size;

    public bool applyChanges;
    public void SetDuration(float _duration)
    {
        duration = _duration;
    }

    public void SetSize(float _size)
    {
        size = _size;
    }

    protected virtual void Start()
    {
        vfx.SetFloat("Duration", duration);
        vfx.SetFloat("Size", size);

    }

    protected virtual void Update()
    { }
}
