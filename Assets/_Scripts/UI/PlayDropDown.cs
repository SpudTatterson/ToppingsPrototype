using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayDropDown : MonoBehaviour
{
    TMP_Dropdown playDropdown;
    [SerializeField] int firstLevelBuildIndex;
    [SerializeField] int levelSelectorBuildIndex;

    void Start()
    {
        playDropdown = GetComponent<TMP_Dropdown>();
    }
    public void StartGame()
    {
        if (playDropdown.value == 0)
            SceneManager.LoadScene(firstLevelBuildIndex);
        else if (playDropdown.value == 1)
            SceneManager.LoadScene(levelSelectorBuildIndex);
    }
}
