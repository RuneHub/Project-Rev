using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaseSO : ScriptableObject
{
    public AnimationClip attackAnim;
    public AudioClip SFX;

    public float baseDamage = 100;
    public float cooldown;

}
