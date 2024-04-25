using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour, ISettingDataPersistence
{
    public static MinionManager instance;
    [SerializeField] List<GameObject> minions = new List<GameObject>();
    [SerializeField] ClothingSet defaultClothSet;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("More Then one minion manager exists please remove one");
    }
    void UpdateAllMinionClothes()
    {
        foreach (GameObject minion in minions)
        {
            minion.GetComponent<MinionCustomizer>().UpdateClothing(defaultClothSet);
        }
    }
    public void SetNewDefaultHat(GameObject hat)
    {
        defaultClothSet.hat = hat;
        UpdateAllMinionClothes();

    }
    public void SetNewDefaultBackPack(GameObject backpack)
    {
        defaultClothSet.backpack = backpack;
        UpdateAllMinionClothes();
    }
    public void SetNewClothingColor(Color color)
    {
        defaultClothSet.clothColor = color;
        UpdateAllMinionClothes();
    }
    public void SetNewSkinColor(Color color)
    {
        defaultClothSet.skinColor = color;
        UpdateAllMinionClothes();
    }
    public ClothingSet GetDefaultClothing()
    {
        return defaultClothSet;
    }
    public void Add(GameObject minion)
    {
        minions.Add(minion);
    }
    public void Remove(GameObject minion)
    {
        minions.Remove(minion);
        CheckWinState();
    }
    void CheckWinState()
    {
        if (minions.Count != 0 && LevelEndPoint.instance.CheckForVictory())
        {
            UIManager.instance.victoryButton.SetActive(true);
        }
        if (minions.Count == 0 && LevelEndPoint.instance.GetPassedMinionCount() == 0)
        {
            VictoryManager.instance.TriggerLoss();
        }
        else if (minions.Count == 0 && LevelEndPoint.instance.CheckForVictory())
        {
            VictoryManager.instance.TriggerWin();
        }
    }

    public void SaveData(SettingsData data)
    {
        DefaultClothing clothing = defaultClothSet;
        data.defaultClothing = clothing;
    }

    public void LoadData(SettingsData data)
    {
        if (data.defaultClothing.hatPrefabName != null)
            defaultClothSet.hat = Resources.Load<GameObject>(data.defaultClothing.hatPrefabName);
        if (data.defaultClothing.backpackPrefabName != null)
            defaultClothSet.backpack = Resources.Load<GameObject>(data.defaultClothing.backpackPrefabName);
        defaultClothSet.clothColor = VectorUtility.ToColor(data.defaultClothing.clothingColor);
        defaultClothSet.skinColor = VectorUtility.ToColor(data.defaultClothing.skinColor);
        UpdateAllMinionClothes();
        //defaultClothSet = data.defaultClothing; not working
    }
}
