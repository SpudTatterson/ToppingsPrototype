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

        List<string> clothingNames = new List<string>();
        clothingNames.Add("");
        foreach (GameObject clothing in clothingOptions)
        {
            clothingNames.Add(clothing.name);
        }
        clothingOptions.Insert(0, new GameObject(""));
        dropdown.AddOptions(clothingNames);
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
        activeText.text = clothingOptions[dropdown.value].name;
        if(customizationType == CustomizationType.Hat)
        {
            MinionManager.instance.SetNewDefaultHat(clothingOptions[dropdown.value]);
        }
        if(customizationType == CustomizationType.BackPack)
        {
            MinionManager.instance.SetNewDefaultBackPack(clothingOptions[dropdown.value]);
        }
    }
}
