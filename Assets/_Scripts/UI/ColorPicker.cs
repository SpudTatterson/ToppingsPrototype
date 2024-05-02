using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorPicker : MonoBehaviour, ISettingDataPersistence
{
    [SerializeField] CustomizerDropDown customizerDropDown;
    public CustomizationType customizationType;
    ColorOption colorToCustomize;
    [SerializeField] Image colorImage;
    [SerializeField] TMP_InputField hexInput;
    float r = 1, g = 1, b = 1;
    bool toggle = false;
    void Awake()
    {
        colorToCustomize = customizerDropDown.GetCustomColorOption();
        gameObject.SetActive(false);
    }
    public void UpdateColor()
    {
        if (colorToCustomize == null)
            colorToCustomize = customizerDropDown.GetCustomColorOption();

        colorToCustomize.color = VectorUtility.ToColor(new Vector3(r, g, b));
        hexInput.text = ToHex(colorToCustomize.color);

        if (customizerDropDown.GetCurrentDropDownValue() == customizerDropDown.GetCustomColorIndex())
        {
            if (customizationType == CustomizationType.ClothColor)
                MinionManager.instance.SetNewClothingColor(colorToCustomize.color);
            else if (customizationType == CustomizationType.SkinColor)
                MinionManager.instance.SetNewSkinColor(colorToCustomize.color);
            else
                Debug.LogWarning("ColorPicker Not Set to color Customization Type " + gameObject.name);
        }

        UpdateColorImage();
    }
    void UpdateColorImage()
    {
        colorImage.color = colorToCustomize.color;
    }
    public void SetR(float r)
    {
        this.r = r;
        UpdateColor();
    }
    public void SetG(float g)
    {
        this.g = g;
        UpdateColor();
    }
    public void SetB(float b)
    {
        this.b = b;
        UpdateColor();
    }
    void SetRGB(Vector3 rgb)
    {
        r = rgb.x;
        g = rgb.y;
        b = rgb.z;
        if (customizationType == CustomizationType.SkinColor)
        {
            UIManager.instance.rSkinColor.value = r;
            UIManager.instance.gSkinColor.value = g;
            UIManager.instance.bSkinColor.value = b;
        }
        else if (customizationType == CustomizationType.ClothColor)
        {
            UIManager.instance.rClothingColor.value = r;
            UIManager.instance.gClothingColor.value = g;
            UIManager.instance.bClothingColor.value = b;
        }

    }
    public void FromHex(string hex)
    {
        hex = "#" + hex;
        Debug.Log(hex);
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
            SetRGB(VectorUtility.FromColor(color));

    }
    string ToHex(Color color)
    {
        return ColorUtility.ToHtmlStringRGB(color);
    }
    public void Toggle()
    {
        toggle = !toggle;
        gameObject.SetActive(toggle);
    }

    public void SaveData(SettingsData data)
    {
        data.customColors[customizationType] = colorToCustomize;
    }

    public void LoadData(SettingsData data)
    {
        SetRGB(VectorUtility.FromColor(data.customColors[customizationType].color));
    }
}
