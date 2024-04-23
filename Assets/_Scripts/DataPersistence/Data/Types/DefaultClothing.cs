using UnityEngine;

[System.Serializable]
public class DefaultClothing
{
    public string hatPrefabName;
    public string backpackPrefabName;
    public Vector3 clothingColor;
    public Vector3 skinColor;

    public static implicit operator DefaultClothing(ClothingSet clothingSet)
    {
        DefaultClothing clothing = new DefaultClothing();
        if (clothingSet.hat != null)
            clothing.hatPrefabName = clothingSet.hat.name;
        if (clothingSet.backpack != null)
            clothing.backpackPrefabName = clothingSet.backpack.name;
        clothing.clothingColor = VectorUtility.FromColor(clothingSet.clothColor);
        clothing.skinColor = VectorUtility.FromColor(clothingSet.skinColor);
        return clothing;
    }
}
