using System.Collections.Generic;
using UnityEngine;

public class MinionCustomizer : MonoBehaviour
{
    public ClothingSet defaultClothing;

    BoneContainer bones;

    List<GameObject> oldClothes = new List<GameObject>();
    void Awake()
    {
        bones = GetComponent<BoneContainer>();
        UpdateClothing(defaultClothing);
    }

    public void UpdateClothing(ClothingSet clothingSet)
    {
        if (oldClothes.Count != 0)
        {
            foreach (GameObject cloth in oldClothes)
            {
                Destroy(cloth);
            }
        }
        if (clothingSet.hat != null)
        {
            GameObject hat = Instantiate(clothingSet.hat, bones.head.position, Quaternion.identity, bones.head);
            hat.transform.localRotation = Quaternion.Euler(0, 0, 0);
            oldClothes.Add(hat);
        }
        if(clothingSet.backpack != null)
        {
            GameObject backpack = Instantiate(clothingSet.backpack, bones.back.position, Quaternion.identity, bones.back);
            backpack.transform.localRotation = Quaternion.Euler(0, 0, 0);
            oldClothes.Add(backpack);
        }
        if(clothingSet.handItem != null)
        {
            GameObject handItem = Instantiate(clothingSet.handItem, bones.hand.position, Quaternion.identity, bones.hand);
            handItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
            oldClothes.Add(handItem);
        }
    }
}
