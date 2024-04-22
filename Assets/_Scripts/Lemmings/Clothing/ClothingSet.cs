using UnityEngine;

[CreateAssetMenu(menuName = "Minion/ClothingSet")]
public class ClothingSet : ScriptableObject
{
    public GameObject hat;
    public GameObject backpack;
    public GameObject handItem;

    public Color clothColor;
    public Color skinColor;

    
    public static implicit operator ClothingSet(DefaultClothing defaultClothing)
    {
        ClothingSet clothingSet = CreateInstance<ClothingSet>();
        

        if (defaultClothing.hatPrefabName != null)
            clothingSet.hat = Resources.Load<GameObject>(defaultClothing.hatPrefabName);
        if (defaultClothing.backpackPrefabName != null)
            clothingSet.backpack = Resources.Load<GameObject>(defaultClothing.backpackPrefabName);
        clothingSet.clothColor = new Color(defaultClothing.clothingColor.x, defaultClothing.clothingColor.y,
         defaultClothing.clothingColor.z);
        clothingSet.skinColor = new Color(defaultClothing.skinColor.x, defaultClothing.skinColor.y,
         defaultClothing.skinColor.z);

         return clothingSet;
    }
}
