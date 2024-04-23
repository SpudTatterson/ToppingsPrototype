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
        clothingSet.clothColor = VectorUtility.ToColor(defaultClothing.clothingColor);
        clothingSet.skinColor = VectorUtility.ToColor(defaultClothing.skinColor);

         return clothingSet;
    }
}
