using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CustomizationType
{
    Hat,
    BackPack,
    ClothColor,
    SkinColor
}
public class CustomizerDropDown : MonoBehaviour
{
    public CustomizationType customizationType;
    [SerializeField] List<GameObject> clothingOptions;
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

    // Update is called once per frame
    void Update()
    {
        
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
