using System;
using System.Collections.Generic;
using UnityEngine;

public class MinionCustomizer : MonoBehaviour
{
    //public ClothingSet defaultClothing;
    //make all minionCustomizer's reference 1 default clothing set on the minion manager class and have methods to change the accessories  
    //also add colors to the clothingSet Class and have methods to change that as well 
    //finally use the dataPersistenceManager to save and load between scenes and when closing and opening the game  

    BoneContainer bones;
    [SerializeField] Material skinMatRef;
    [SerializeField] Material clothingMatRef;

    Material skinMat;
    Material clothingMat;
    List<GameObject> oldClothes = new List<GameObject>();
    void Awake()
    {
        bones = GetComponent<BoneContainer>();
        skinMat = skinMatRef;
        clothingMat = clothingMatRef;
    }
    void Start()
    {
        UpdateClothing(MinionManager.instance.GetDefaultClothing());
    }

    public void UpdateClothing(ClothingSet clothingSet)
    {
        if (oldClothes.Count != 0) // delete old clothings
        {
            foreach (GameObject cloth in oldClothes)
            {
                Destroy(cloth);
            }
        }
        SetHat(clothingSet);
        SetBackpack(clothingSet);
        SetHandItem(clothingSet);
        SetClothingColor(clothingSet);
        SetSkinColor(clothingSet);
    }

    void SetClothingColor(ClothingSet clothingSet)
    {
        clothingMat.color = clothingSet.clothColor;
    }
    void SetSkinColor(ClothingSet clothingSet)
    {
        skinMat.color = clothingSet.skinColor;
    }

    void SetHandItem(ClothingSet clothingSet)
    {
        if (clothingSet.handItem != null)
        {
            GameObject handItem = Instantiate(clothingSet.handItem, bones.hand.position, Quaternion.identity, bones.hand);
            handItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
            oldClothes.Add(handItem);
        }
    }

    void SetBackpack(ClothingSet clothingSet)
    {
        if (clothingSet.backpack != null)
        {
            GameObject backpack = Instantiate(clothingSet.backpack, bones.back.position, Quaternion.identity, bones.back);
            backpack.transform.localRotation = Quaternion.Euler(0, 0, 0);
            oldClothes.Add(backpack);
        }
    }

    void SetHat(ClothingSet clothingSet)
    {
        if (clothingSet.hat != null)
        {
            GameObject hat = Instantiate(clothingSet.hat, bones.head.position, Quaternion.identity, bones.head);
            hat.transform.localRotation = Quaternion.Euler(0, 0, 0);
            oldClothes.Add(hat);
        }
    }
}
