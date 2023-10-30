using System;
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

    [SerializeField] TextMeshProUGUI minTextSoundFx;
    [SerializeField] TextMeshProUGUI minTextMusic;

    [SerializeField] TextMeshProUGUI englishText;
    [SerializeField] TextMeshProUGUI portugueseText;

    [SerializeField] GameObject toggleONText;
    [SerializeField] GameObject toggleOFFText;

    [SerializeField] TextMeshProUGUI followText;
    [SerializeField] TextMeshProUGUI rateText;
    [SerializeField] TextMeshProUGUI creditsText;
    [SerializeField] TextMeshProUGUI otherGamesText;

    [SerializeField] TextMeshProUGUI soundFXText;
    [SerializeField] TextMeshProUGUI musicText;
    [SerializeField] TextMeshProUGUI vibrationText;
    [SerializeField] TextMeshProUGUI languageText;

    [SerializeField] TextMeshProUGUI settingsText;

    [SerializeField] Image soundFXIcon;
    [SerializeField] Image musicIcon;

    [SerializeField] Image englishFlag;
    [SerializeField] Image portugueseFlag;

    [SerializeField] Image toggleBackground;
    [SerializeField] Image toggleHandle;

    [SerializeField] GameObject followGoldIcon;
    [SerializeField] GameObject rateGoldIcon;
    [SerializeField] GameObject downloadGoldIcon;

    [SerializeField] TextMeshProUGUI creditsTitleText;
    [SerializeField] TextMeshProUGUI leftCreditsText;
    [SerializeField] TextMeshProUGUI rightCreditsText;
    [SerializeField] GameObject creditsPanel;

    [SerializeField] GameObject otherGamesPanel;
    [SerializeField] TextMeshProUGUI otherGamesTitleText;

    [HideInInspector] public UIController uiController;

    private bool isPortuguese = false;

    private bool hasFollowInstagram = false;
    private bool hasDownloadFocusedMonsters = false;
    private bool hasRatedGame = false;

    private Inventory inventory;

    public bool IsPortuguese() => isPortuguese;

    private void Start()
    {
        uiController = UIController.instance;
        inventory = Inventory.instance;
        OnLanguageChanged += LanguageChanged;

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
        if (!isPortuguese) return;

        uiController.PlayDefaultAudioClip();

        isPortuguese = false;
        englishFlag.color = flagColors[0];
        portugueseFlag.color = flagColors[1];

        englishText.color = flagTextColors[0];
        portugueseText.color = flagTextColors[1];

        OnLanguageChanged?.Invoke(this, new OnLanguageChangeEventHandler { _isPortuguese = isPortuguese });
    }

    public void OnPortuguesePressed()
    {
        if (isPortuguese) return;

        uiController.PlayDefaultAudioClip();

        isPortuguese = true;
        portugueseFlag.color = flagColors[0];
        englishFlag.color = flagColors[1];

        portugueseText.color = flagTextColors[0];
        englishText.color = flagTextColors[1];

        OnLanguageChanged?.Invoke(this, new OnLanguageChangeEventHandler { _isPortuguese = isPortuguese });
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

    public event EventHandler<OnLanguageChangeEventHandler> OnLanguageChanged;

    public class OnLanguageChangeEventHandler : EventArgs
    {
        public bool _isPortuguese;
    }

    private void LanguageChanged(object sender, OnLanguageChangeEventHandler handler)
    {
        if (handler._isPortuguese == true)
        {
            englishText.text = "Inglês";
            portugueseText.text = "Português";

            languageText.text = "Idioma";
            musicText.text = "Música";
            soundFXText.text = "Efeitos Sonoros";
            vibrationText.text = "Vibrar";


            followText.text = "Seguir";
            rateText.text = "Avaliar";
            creditsText.text = "Créditos";
            otherGamesText.text = "Outros Jogos";

            settingsText.text = "Configurações";

            creditsTitleText.text = "Creditos";
            leftCreditsText.text = "<uppercase><size=40><color=#F6E19C>Gráficos</color></uppercase></size><color=white>\r\n\r\nArt by Kandles (artbykandles.com)\r\nBorodar (borodar.com)\r\nBroken Vector (brokenvector.com)\r\nDungeon Mason (alexkim0415.wixsite.com/dungeonmason)\r\nEmacEArt (www.emaceart.com)\r\nCreativetrio (creativetrio.art)\r\nInfinity PBR (infinitypbr.com)\r\nJean Moreno (jeanmoreno.com)\r\nLayer Lab (layerlabgames.com)\r\nMalbers Animations (malbersanimations.artstation.com)\r\nMeshtint Studio (meshtint.com)\r\nOMABUARTS STUDIO (www.omabuarts.com)\r\nRetroVistas (artstation.com/ramonavladut)\r\nRunemark Studio (runemarkstudio.com)\r\nSynty Studios (syntystudios.com)\r\n";
            rightCreditsText.text = "<uppercase><size=40><color=#F6E19C>Efeitos Sonoros e Áudio</color></uppercase></size><color=white>\r\n\r\nAmeAngelofSin (twitter.com/ameangelofsin)\r\nCafofo (cafofomusic.com)\r\nFreesound (freesound.org)\r\nSkyRaeVoicing (skyraevoicing.com)\r\nZapslat (zapsplat.com)</color>\r\n\r\n<uppercase><size=40>\r\n<color=#F6E19C>Ferramentas</color></uppercase></size><color=white>\r\n\r\nDented Pixel (dentedpixel.com)\r\nM Studio Hub (mstudiohub.com)</color>";

            otherGamesTitleText.text = "Outros Jogos";
        }

        else
        {
            englishText.text = "English";
            portugueseText.text = "Portuguese";

            languageText.text = "Language";
            musicText.text = "Music";
            soundFXText.text = "Sound Fx";
            vibrationText.text = "Vibration";

            followText.text = "Follow";
            rateText.text = "Rate";
            creditsText.text = "Credits";
            otherGamesText.text = "Other Games";

            settingsText.text = "Settings";

            creditsTitleText.text = "Credits";
            leftCreditsText.text = "<uppercase><size=40><color=#F6E19C>Graphics</color></uppercase></size><color=white>\r\n\r\nArt by Kandles (artbykandles.com)\r\nBorodar (borodar.com)\r\nBroken Vector (brokenvector.com)\r\nDungeon Mason (alexkim0415.wixsite.com/dungeonmason)\r\nEmacEArt (www.emaceart.com)\r\nCreativetrio (creativetrio.art)\r\nInfinity PBR (infinitypbr.com)\r\nJean Moreno (jeanmoreno.com)\r\nLayer Lab (layerlabgames.com)\r\nMalbers Animations (malbersanimations.artstation.com)\r\nMeshtint Studio (meshtint.com)\r\nOMABUARTS STUDIO (www.omabuarts.com)\r\nRetroVistas (artstation.com/ramonavladut)\r\nRunemark Studio (runemarkstudio.com)\r\nSynty Studios (syntystudios.com)\r\n";
            rightCreditsText.text = "<uppercase><size=40><color=#F6E19C>Sound Effects and Audio</color></uppercase></size><color=white>\r\n\r\nAmeAngelofSin (twitter.com/ameangelofsin)\r\nCafofo (cafofomusic.com)\r\nFreesound (freesound.org)\r\nSkyRaeVoicing (skyraevoicing.com)\r\nZapslat (zapsplat.com)</color>\r\n\r\n<uppercase><size=40>\r\n<color=#F6E19C>Tools</color></uppercase></size><color=white>\r\n\r\nDented Pixel (dentedpixel.com)\r\nM Studio Hub (mstudiohub.com)</color>";

            otherGamesTitleText.text = "Other Games";

        }
    }

    private void CheckInteraction(bool interaction, GameObject coinIcon)
    {
        if (interaction == true)
            coinIcon.SetActive(false);
        else
            coinIcon.SetActive(true);
    }

}
