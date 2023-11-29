using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public bool shouldVibrate = true;

    public static SettingsController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    [SerializeField] TextMeshProUGUI minTextSoundFx;
    [SerializeField] TextMeshProUGUI minTextMusic;
    [SerializeField] TextMeshProUGUI leftText;
    [SerializeField] TextMeshProUGUI rightText;

    [SerializeField] GameObject settingsPanel;

    [SerializeField] AudioMixer soundFxAudioMixer;
    [SerializeField] AudioMixer musicAudioMixer;

    [SerializeField] Sprite[] soundFXSprites;
    [SerializeField] Sprite[] musicSprites;
    [SerializeField] Sprite[] toggleSprites;
    [SerializeField] Sprite[] switchBackgroundSprites;

    [SerializeField] Color[] switchColors;
    [SerializeField] Color[] flagColors;
    [SerializeField] Color[] flagTextColors;

    [SerializeField] Slider soundFXSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] GameObject toggleONText;
    [SerializeField] GameObject toggleOFFText;

    [SerializeField] Image soundFXIcon;
    [SerializeField] Image musicIcon;

    [SerializeField] Image englishFlag;
    [SerializeField] Image portugueseFlag;
    [SerializeField] Image chineseFlag;

    [SerializeField] Image toggleBackground;
    [SerializeField] Image toggleHandle;

    [SerializeField] GameObject followGoldIcon;
    [SerializeField] GameObject rateGoldIcon;
    [SerializeField] GameObject downloadGoldIcon;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject otherGamesPanel;

    [SerializeField] Image leftArrow;
    [SerializeField] Image rightArrow;

    [HideInInspector] public UIController uiController;
    [HideInInspector] public LanguageController languageController;

    private bool hasFollowInstagram = false;
    private bool hasDownloadFocusedMonsters = false;
    private bool hasRatedGame = false;

    private Inventory inventory;

    private void Start()
    {
        languageController = LanguageController.instance;
        uiController = UIController.instance;
        inventory = Inventory.instance;

        CheckInteraction(hasRatedGame, rateGoldIcon);
        CheckInteraction(hasFollowInstagram, followGoldIcon);
        CheckInteraction(hasDownloadFocusedMonsters, downloadGoldIcon);
    }

    public void SoundFXSlider(float value)
    {
        soundFXSlider.value = value;
        minTextSoundFx.text = value.ToString();

        soundFXIcon.sprite = soundFXSprites[0];

        switch (value)
        {
            case 10:
                soundFxAudioMixer.SetFloat("SoundFxVolume", 0);
                break;
            case 9:
                soundFxAudioMixer.SetFloat("SoundFxVolume", -5);
                break;
            case 8:
                soundFxAudioMixer.SetFloat("SoundFxVolume", -10);
                break;
            case 7:
                soundFxAudioMixer.SetFloat("SoundFxVolume", -12);
                break;
            case 6:
                soundFxAudioMixer.SetFloat("SoundFxVolume", -14);
                break;
            case 5:
                soundFxAudioMixer.SetFloat("SoundFxVolume", -16);
                break;
            case 4:
                soundFxAudioMixer.SetFloat("SoundFxVolume", -18);
                break;
            case 3:
                soundFxAudioMixer.SetFloat("SoundFxVolume", -20);
                break;
            case 2:
                soundFxAudioMixer.SetFloat("SoundFxVolume", -25);
                break;
            case 1:
                soundFxAudioMixer.SetFloat("SoundFxVolume", -30);
                break;
            case 0:
                soundFxAudioMixer.SetFloat("SoundFxVolume", -80);
                soundFXIcon.sprite = soundFXSprites[1];
                break;
        }
    }

    public void MusicSlider(float value)
    {
        musicSlider.value = value;
        minTextMusic.text = value.ToString();

        musicIcon.sprite = musicSprites[0];

        switch (value)
        {
            case 10:
                musicAudioMixer.SetFloat("MusicVolume", -10);
                break;
            case 9:
                musicAudioMixer.SetFloat("MusicVolume", -12);
                break;
            case 8:
                musicAudioMixer.SetFloat("MusicVolume", -14);
                break;
            case 7:
                musicAudioMixer.SetFloat("MusicVolume", -16);
                break;
            case 6:
                musicAudioMixer.SetFloat("MusicVolume", -18);
                break;
            case 5:
                musicAudioMixer.SetFloat("MusicVolume", -20);
                break;
            case 4:
                musicAudioMixer.SetFloat("MusicVolume", -22.5f);
                break;
            case 3:
                musicAudioMixer.SetFloat("MusicVolume", -25);
                break;
            case 2:
                musicAudioMixer.SetFloat("MusicVolume", -27.5f);
                break;
            case 1:
                musicAudioMixer.SetFloat("MusicVolume", -30);
                break;
            case 0:
                musicAudioMixer.SetFloat("MusicVolume", -80);
                musicIcon.sprite = musicSprites[1];
                break;
        }
    }

    public void OnVibrationSwitch(float value)
    {
        uiController.PlayDefaultAudioClip();

        switch (value)
        {
            case 0: //OFF
                toggleOFFText.SetActive(true);
                toggleONText.SetActive(false);

                toggleHandle.sprite = toggleSprites[1];
                toggleBackground.sprite = switchBackgroundSprites[1];
                shouldVibrate = false;

                break;
            case 1: //ON
                toggleOFFText.SetActive(false);
                toggleONText.SetActive(true);

                toggleHandle.sprite = toggleSprites[0];
                toggleBackground.sprite = switchBackgroundSprites[0];
                shouldVibrate = true;
                break;
        }
    }

    public void OnEnglishPressed()
    {
        if (languageController.GetGlobalLanguage() == Language.English) return;

        uiController.PlayDefaultAudioClip();

        englishFlag.color = flagColors[0];
        portugueseFlag.color = flagColors[1];
        chineseFlag.color = flagColors[1];
        languageController.SetGlobalLanguage(Language.English);
        OnLanguageChanged?.Invoke(this, new OnLanguageChangeEventHandler { language = Language.English });
    }

    public void OnPortuguesePressed()
    {
        if (languageController.GetGlobalLanguage() == Language.Portuguese) return;

        uiController.PlayDefaultAudioClip();

        portugueseFlag.color = flagColors[0];
        englishFlag.color = flagColors[1];
        chineseFlag.color = flagColors[1];
        languageController.SetGlobalLanguage(Language.Portuguese);
        OnLanguageChanged?.Invoke(this, new OnLanguageChangeEventHandler { language = Language.Portuguese });
    }

    public void OnChinesePressed()
    {
        if (languageController.GetGlobalLanguage() == Language.Chinese) return;

        uiController.PlayDefaultAudioClip();

        portugueseFlag.color = flagColors[1];
        englishFlag.color = flagColors[1];
        chineseFlag.color = flagColors[0];
        languageController.SetGlobalLanguage(Language.Chinese);
        OnLanguageChanged?.Invoke(this, new OnLanguageChangeEventHandler { language = Language.Chinese });
    }

    public void OnLeftControllPressed()
    {
        uiController.PlayDefaultAudioClip();

        leftArrow.color = flagColors[0];
        rightArrow.color = flagColors[1];

        leftText.color = flagTextColors[0];
        rightText.color = flagTextColors[1];

        uiController.SetLeftControlsConfigurations();
    }

    public void OnRightControllPressed()
    {
        uiController.PlayDefaultAudioClip();

        leftArrow.color = flagColors[1];
        rightArrow.color = flagColors[0];

        leftText.color = flagTextColors[1];
        rightText.color = flagTextColors[0];

        uiController.SetRightControlsConfigurations();
    }

    public void OnFollowPressed()
    {
        uiController.PlayDefaultAudioClip();

        if (hasFollowInstagram == false)
        {
            hasFollowInstagram = true;
            followGoldIcon.SetActive(false);
            inventory.AddItem(inventory.basicItens[0], 500);
        }

        string url = "instagram://user?username=donadagames";
        Application.OpenURL(url);
    }

    public void OnRatePressed()
    {
        uiController.PlayDefaultAudioClip();

        if (hasRatedGame == false)
        {
            hasRatedGame = true;
            rateGoldIcon.SetActive(false);
            inventory.AddItem(inventory.basicItens[0], 500);
        }

        string url = $"market://details?id=" + "com.donadagames.focusedmonsters";
        Application.OpenURL(url);
    }

    public void OnCreditsPressed()
    {
        creditsPanel.SetActive(!creditsPanel.activeSelf);
        uiController.PlayDefaultAudioClip();
    }

    public void OnOtherGamesPressed()
    {
        uiController.PlayDefaultAudioClip();
        otherGamesPanel.SetActive(!otherGamesPanel.activeSelf);
    }

    public void OnSettingsButtonPressed()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        uiController.PlayDefaultAudioClip();

        creditsPanel.SetActive(false);
        otherGamesPanel.SetActive(false);
    }

    public void OnDownloadFocusedMonstersButtonPressed()
    {
        uiController.PlayDefaultAudioClip();

        if (hasDownloadFocusedMonsters == false)
        {
            hasDownloadFocusedMonsters = true;
            downloadGoldIcon.SetActive(false);
            inventory.AddItem(inventory.basicItens[0], 500);
        }

        string url = $"market://details?id=" + "com.donadagames.focusedmonsters";
        Application.OpenURL(url);
    }

    public void OnTrailerButtonPressed()
    {
        uiController.PlayDefaultAudioClip();
        string url = "https://www.youtube.com/watch?v=2ORw9uAIv00&t=2s";
        Application.OpenURL(url);
    }

    private void CheckInteraction(bool interaction, GameObject coinIcon)
    {
        if (interaction == true)
            coinIcon.SetActive(false);
        else
            coinIcon.SetActive(true);
    }

    public event EventHandler<OnLanguageChangeEventHandler> OnLanguageChanged;

    public class OnLanguageChangeEventHandler : EventArgs
    {
        public Language language;
    }
}
