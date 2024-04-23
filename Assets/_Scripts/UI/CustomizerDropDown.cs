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
public class CustomizerDropDown : MonoBehaviour
{
    public CustomizationType customizationType; //[ShowIf("enumFlag", EMyEnum.Case0)]
    [SerializeField, HideIf("IsCustomizingColor")] List<GameObject> clothingOptions;
    [SerializeField, ShowIf("IsCustomizingColor")] List<ColorOption> colorOptions;

    [SerializeField] TextMeshProUGUI activeText;
    TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();

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
}
[System.Serializable]
class ColorOption
{
    public string name;
    public Color color;
}