using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    [Header("Victory UI")]
    [Tooltip("Container of the button to finish the level")]
    public GameObject victoryButton;

    [Tooltip("Container of victory screen")]
    public GameObject victoryScreen;

    [Tooltip("Container for new high score/best time effects")]
    public GameObject newBestTimeScreen;

    public TextMeshProUGUI aliveNumberText;
    public TextMeshProUGUI alivePercentageText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI bestTimeText;
    public Image starFillUpBar;
    public Image bestStarFillUpBar;
    
    [Header("LoseScreen")]
    [Tooltip("Container of lose screen")]
    public GameObject loseScreen;
    [Header("Pause Menu")]
    public GameObject pauseScreen;
    public GameObject settingsScreen;
    [Header("Sound Settings")]
    public Slider masterVolume;
    public Slider soundFXVolume;
    public Slider musicVolume;
    [Header("Cursors")]
    public Texture2D defaultCursor;
    public Texture2D deleteCursor;
    public Texture2D placeCursor;
    public Texture2D jobCursor;
    [Header("HUD")]
    public TextMeshProUGUI minionCountHUDText;
    public TextMeshProUGUI timeHUDText;
    public Image starFillUpBarHUD;
    public Image bestStarFillUpBarHUD;
    [Header("Customization")]
    public Slider rSkinColor;
    public Slider gSkinColor;
    public Slider bSkinColor;
    public Slider rClothingColor;
    public Slider gClothingColor;
    public Slider bClothingColor;



    void Awake()
    {
        instance = this;
    }
}
