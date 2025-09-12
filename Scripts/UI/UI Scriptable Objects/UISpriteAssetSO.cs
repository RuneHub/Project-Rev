using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(menuName = "UI/Sprite Asset")]
    public class UISpriteAssetSO : ScriptableObject
    {
        public List<TMP_SpriteAsset> spriteAssets;
    }
}