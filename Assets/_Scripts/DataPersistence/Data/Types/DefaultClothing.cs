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
        if(clothingSet.hat != null)
            clothing.hatPrefabName = clothingSet.hat.name;
        if(clothingSet.backpack != null)
            clothing.backpackPrefabName = clothingSet.backpack.name;
        clothing.clothingColor =new Vector3(clothingSet.clothColor.r, clothingSet.clothColor.g,
         clothingSet.clothColor.b);
         clothing.skinColor =new Vector3(clothingSet.skinColor.r, clothingSet.skinColor.g,
         clothingSet.skinColor.b);
        return clothing;
    }
}
