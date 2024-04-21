using System.Collections.Generic;
using UnityEngine;

public class MinionCustomizer : MonoBehaviour
{
    //public ClothingSet defaultClothing;
    //make all minionCustomizer's reference 1 default clothing set on the minion manager class and have methods to change the accessories  
    //also add colors to the clothingSet Class and have methods to change that as well 
    //finally use the dataPersistenceManager to save and load between scenes and when closing and opening the game  

    BoneContainer bones;

    List<GameObject> oldClothes = new List<GameObject>();
    void Awake()
    {
        bones = GetComponent<BoneContainer>();
    }
    void Start()
    {
        UpdateClothing(MinionManager.instance.GetDefaultClothing());
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
        if (clothingSet.backpack != null)
        {
            GameObject backpack = Instantiate(clothingSet.backpack, bones.back.position, Quaternion.identity, bones.back);
            backpack.transform.localRotation = Quaternion.Euler(0, 0, 0);
            oldClothes.Add(backpack);
        }
        if (clothingSet.handItem != null)
        {
            GameObject handItem = Instantiate(clothingSet.handItem, bones.hand.position, Quaternion.identity, bones.hand);
            handItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
            oldClothes.Add(handItem);
        }
    }
}
