using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using NaughtyAttributes;

public enum CustomizationType
{
    Hat,
    BackPack,
    ClothColor,
    SkinColor
}
public class CustomizerDropDown : MonoBehaviour, ISettingDataPersistence
{
    public CustomizationType customizationType; 
    [SerializeField, HideIf("IsCustomizingColor")] List<GameObject> clothingOptions;
    [SerializeField, ShowIf("IsCustomizingColor")] List<ColorOption> colorOptions;

    [SerializeField] TextMeshProUGUI activeText;

    const int Custom_Color_Index = 0;
    TMP_Dropdown dropdown;
    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        Initialize();
    }

    void Initialize()
    {
        List<string> options = new List<string>();
        if (!IsCustomizingColor())
        {
            clothingOptions.Insert(0, new GameObject(""));
            foreach (GameObject clothing in clothingOptions)
            {
                options.Add(clothing.name);
            }
        }
        else
        {   
            colorOptions[Custom_Color_Index] = new ColorOption("Custom", Color.white);
            foreach (ColorOption color in colorOptions)
            {
                options.Add(color.name);
            }
        }

        dropdown.AddOptions(options);
    }

    bool IsCustomizingColor()
    {
        if (customizationType == CustomizationType.ClothColor || customizationType == CustomizationType.SkinColor)
            return true;
        else
            return false;

    }
    public ColorOption GetCustomColorOption()
    {
        return colorOptions[Custom_Color_Index];
    }
    public int GetCurrentDropDownValue()
    {
        return dropdown.value;
    }
    public int GetCustomColorIndex()
    {
        return Custom_Color_Index;
    }
    public void Select()
    {
        if (!IsCustomizingColor())
            activeText.text = clothingOptions[dropdown.value].name;
        else
            activeText.text = colorOptions[dropdown.value].name;


        if (customizationType == CustomizationType.Hat)
        {
            MinionManager.instance.SetNewDefaultHat(clothingOptions[dropdown.value]);
        }
        if (customizationType == CustomizationType.BackPack)
        {
            MinionManager.instance.SetNewDefaultBackPack(clothingOptions[dropdown.value]);
        }
        if (customizationType == CustomizationType.ClothColor)
        {
            MinionManager.instance.SetNewClothingColor(colorOptions[dropdown.value].color);
        }
        if (customizationType == CustomizationType.SkinColor)
        {
            MinionManager.instance.SetNewSkinColor(colorOptions[dropdown.value].color);
        }
    }

    public void SaveData(SettingsData data)
    {
        data.customizationPicks[customizationType] = dropdown.value;
    }

    public void LoadData(SettingsData data)
    {
        dropdown.value = data.customizationPicks[customizationType];
    }
}
[System.Serializable]
public class ColorOption
{
    public string name;
    public Color color;

    public ColorOption(string name, Color color)
    {
        this.name = name;
        this.color = color;
    }
    public ColorOption()
    {

    }
}