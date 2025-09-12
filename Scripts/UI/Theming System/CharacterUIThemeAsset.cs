using KS.SHADING;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "CharacterAsset/CharacterUIThemeAsset")]
public class CharacterUIThemeAsset : ScriptableObject
{
    public string ThemeName;

    public CharacterThemeAsset characterTheme;

    public Sprite UICharacterPreview;
    public Sprite UICharacterThemeImage;
    public Sprite LeftWeaponPreview;
    public Sprite RightWeaponPreview;

}
