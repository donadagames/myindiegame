using TMPro;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    public static LanguageController instance;

    [SerializeField] private TMP_FontAsset chineseFont;
    [SerializeField] private TMP_FontAsset regularFont;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        SetGlobalLanguage(Language.English);
    }

    private Language GlobalLanguage;

    #region SETTINGS PANEL
    [SerializeField] TextMeshProUGUI followText;
    [SerializeField] TextMeshProUGUI rateText;
    [SerializeField] TextMeshProUGUI creditsText;
    [SerializeField] TextMeshProUGUI otherGamesText;
    [SerializeField] TextMeshProUGUI soundFXText;
    [SerializeField] TextMeshProUGUI musicText;
    [SerializeField] TextMeshProUGUI vibrationText;
    [SerializeField] TextMeshProUGUI languageText;
    [SerializeField] TextMeshProUGUI settingsText;
    [SerializeField] TextMeshProUGUI creditsTitleText;
    [SerializeField] TextMeshProUGUI leftCreditsText;
    [SerializeField] TextMeshProUGUI rightCreditsText;
    [SerializeField] TextMeshProUGUI leftText;
    [SerializeField] TextMeshProUGUI rightText;
    [SerializeField] TextMeshProUGUI controllsText;
    [SerializeField] TextMeshProUGUI otherGamesTitleText;
    #endregion

    #region INVENTORY PANEL
    [SerializeField] TextMeshProUGUI inventoryTMP;
    [SerializeField] TextMeshProUGUI quantityTPM;
    [SerializeField] TextMeshProUGUI actionBarTPM;
    [SerializeField] TextMeshProUGUI itemTPM;
    #endregion

    #region QUEST PANEL
    [SerializeField] TextMeshProUGUI inProgressText;
    [SerializeField] TextMeshProUGUI completedText;
    [SerializeField] TextMeshProUGUI questTitle;
    #endregion

    #region DEATH PANEL
    [SerializeField] TextMeshProUGUI tryAgainText;
    [SerializeField] TextMeshProUGUI quitGameText;
    [SerializeField] TextMeshProUGUI deathText;
    #endregion

    #region DIALOGUE PANEL
    [SerializeField] TextMeshProUGUI dialogueText;
    #endregion

    #region MAIN MENU
    [SerializeField] TextMeshProUGUI newGameText;
    [SerializeField] TextMeshProUGUI continueGameText;
    [SerializeField] TextMeshProUGUI newGameQuestionText;
    [SerializeField] TextMeshProUGUI newGameYesText;
    [SerializeField] TextMeshProUGUI newGameNoText;
    [SerializeField] TextMeshProUGUI newGameTitleText;
    [SerializeField] TextMeshProUGUI exitText;
    [SerializeField] TextMeshProUGUI menuSettingsText;
    #endregion

    public Language GetGlobalLanguage() => GlobalLanguage;

    public void SetGlobalLanguage(Language language)
    {
        GlobalLanguage = language;
        OnLanguageChanged();
    }

    public void SetChineseFont(TextMeshProUGUI text) => text.font = chineseFont;
    public void SetRegularFont(TextMeshProUGUI text) => text.font = regularFont;

    public void OnLanguageChanged()
    {
        if (GlobalLanguage == Language.Portuguese)
        {
            #region SETTINGS PANEL
            SetRegularFont(languageText);
            SetRegularFont(musicText);
            SetRegularFont(soundFXText);
            SetRegularFont(vibrationText);
            SetRegularFont(followText);
            SetRegularFont(rateText);
            SetRegularFont(creditsText);
            SetRegularFont(otherGamesText);
            SetRegularFont(settingsText);
            SetRegularFont(creditsTitleText);
            SetRegularFont(leftCreditsText);
            SetRegularFont(rightCreditsText);
            SetRegularFont(otherGamesTitleText);
            SetRegularFont(leftText);
            SetRegularFont(rightText);
            SetRegularFont(controllsText);

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
            rightCreditsText.text = "<uppercase><size=40><color=#F6E19C>Efeitos Sonoros e Áudio</color></uppercase></size><color=white>\r\n\r\nAmeAngelofSin (twitter.com/ameangelofsin)\r\nCafofo (cafofomusic.com)\r\nCicifyre (cicifyre.carrd.co)\r\nFreesound (freesound.org)\r\nSkyRaeVoicing (skyraevoicing.com)\r\nZapslat (zapsplat.com)</color>\r\n\r\n<uppercase><size=40>\r\n<color=#F6E19C>Ferramentas</color></uppercase></size><color=white>\r\n\r\nDented Pixel (dentedpixel.com)\r\nM Studio Hub (mstudiohub.com)</color>";
            otherGamesTitleText.text = "Outros Jogos";
            leftText.text = "Esquerda";
            rightText.text = "Direita";
            controllsText.text = "Controles";
            #endregion

            #region INVENTORY PANEL
            SetRegularFont(actionBarTPM);
            SetRegularFont(quantityTPM);
            SetRegularFont(inventoryTMP);
            SetRegularFont(itemTPM);

            actionBarTPM.text = "Barra de ação";
            quantityTPM.text = "Quantidade";
            inventoryTMP.text = "Mochila";
            itemTPM.text = "Itens";
            #endregion

            #region QUEST PANEL
            SetRegularFont(questTitle);
            SetRegularFont(inProgressText);
            SetRegularFont(completedText);

            questTitle.text = "Missões";
            inProgressText.text = "Em andamento";
            completedText.text = "Finalizadas";
            #endregion

            #region DEATH PANEL
            SetRegularFont(deathText);
            SetRegularFont(tryAgainText);
            SetRegularFont(quitGameText);

            deathText.text = "Você foi derrotad@!";
            tryAgainText.text = "Tente de novo";
            quitGameText.text = "Sair do Jogo";
            #endregion

            #region DIALOGUE PANEL
            SetRegularFont(dialogueText);
            #endregion

            #region MAIN MENU PANEL
            SetRegularFont(newGameText);
            SetRegularFont(continueGameText);
            SetRegularFont(newGameQuestionText);
            SetRegularFont(newGameYesText);
            SetRegularFont(newGameNoText);
            SetRegularFont(newGameTitleText);
            SetRegularFont(exitText);
            SetRegularFont(menuSettingsText);
            menuSettingsText.text = "Configurações";
            exitText.text = "Sair";
            newGameText.text = "Novo Jogo";
            newGameTitleText.text = "Novo Jogo";
            continueGameText.text = "Continuar";
            newGameQuestionText.text = "Você tem certeza que quer começar um novo jogo? Irá perder todo o progresso já salvo!";
            newGameYesText.text = "Sim";
            newGameNoText.text = "Não";

            #endregion
        }

        else if (GlobalLanguage == Language.English)
        {
            #region SETTINGS PANEL
            SetRegularFont(languageText);
            SetRegularFont(musicText);
            SetRegularFont(soundFXText);
            SetRegularFont(vibrationText);
            SetRegularFont(followText);
            SetRegularFont(rateText);
            SetRegularFont(creditsText);
            SetRegularFont(otherGamesText);
            SetRegularFont(settingsText);
            SetRegularFont(creditsTitleText);
            SetRegularFont(leftCreditsText);
            SetRegularFont(rightCreditsText);
            SetRegularFont(otherGamesTitleText);
            SetRegularFont(leftText);
            SetRegularFont(rightText);
            SetRegularFont(controllsText);

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
            rightCreditsText.text = "<uppercase><size=40><color=#F6E19C>Sound Effects and Audio</color></uppercase></size><color=white>\r\n\r\nAmeAngelofSin (twitter.com/ameangelofsin)\r\nCafofo (cafofomusic.com)\r\nCicifyre (cicifyre.carrd.co)\r\nFreesound (freesound.org)\r\nSkyRaeVoicing (skyraevoicing.com)\r\nZapslat (zapsplat.com)</color>\r\n\r\n<uppercase><size=40>\r\n<color=#F6E19C>Tools</color></uppercase></size><color=white>\r\n\r\nDented Pixel (dentedpixel.com)\r\nM Studio Hub (mstudiohub.com)</color>";
            otherGamesTitleText.text = "Other Games";
            leftText.text = "Left";
            rightText.text = "Right";
            controllsText.text = "Controls";
            #endregion

            #region INVENTORY PANEL
            SetRegularFont(actionBarTPM);
            SetRegularFont(quantityTPM);
            SetRegularFont(inventoryTMP);
            SetRegularFont(itemTPM);

            actionBarTPM.text = "Action bar";
            quantityTPM.text = "Quantity";
            inventoryTMP.text = "Inventory";
            itemTPM.text = "Itens";
            #endregion

            #region QUEST PANEL
            SetRegularFont(questTitle);
            SetRegularFont(inProgressText);
            SetRegularFont(completedText);

            questTitle.text = "Quests";
            inProgressText.text = "In Progress";
            completedText.text = "Completed";
            #endregion

            #region DEATH PANEL
            SetRegularFont(deathText);
            SetRegularFont(tryAgainText);
            SetRegularFont(quitGameText);

            deathText.text = "You have been defeated!";
            tryAgainText.text = "Try again";
            quitGameText.text = "Quit Game";
            #endregion

            #region DIALOGUE PANEL
            SetRegularFont(dialogueText);
            #endregion

            #region MAIN MENU PANEL
            SetRegularFont(newGameText);
            SetRegularFont(continueGameText);
            SetRegularFont(newGameQuestionText);
            SetRegularFont(newGameYesText);
            SetRegularFont(newGameNoText);
            SetRegularFont(newGameTitleText);
            SetRegularFont(exitText);
            SetRegularFont(menuSettingsText);
            menuSettingsText.text = "Settings";
            exitText.text = "Exit";
            newGameText.text = "New Game";
            newGameTitleText.text = "New Game";
            continueGameText.text = "Continue";
            newGameQuestionText.text = "Are you sure you want to start a new game? You will lose all saved progress!";
            newGameYesText.text = "Yes";
            newGameNoText.text = "No";
            #endregion
        }

        else if (GlobalLanguage == Language.Chinese)
        {
            #region SETTINGS PANEL

            SetChineseFont(languageText);
            SetChineseFont(musicText);
            SetChineseFont(soundFXText);
            SetChineseFont(vibrationText);
            SetChineseFont(followText);
            SetChineseFont(rateText);
            SetChineseFont(creditsText);
            SetChineseFont(otherGamesText);
            SetChineseFont(settingsText);
            SetChineseFont(creditsTitleText);
            SetChineseFont(leftCreditsText);
            SetChineseFont(rightCreditsText);
            SetChineseFont(otherGamesTitleText);
            SetChineseFont(leftText);
            SetChineseFont(rightText);
            SetChineseFont(controllsText);


            languageText.text = "语言";
            musicText.text = "音乐";
            soundFXText.text = "声音特效";
            vibrationText.text = "振动";
            followText.text = "<跟随";
            rateText.text = "<速度";
            creditsText.text = "<制作人员";
            otherGamesText.text = "其他游戏";
            settingsText.text = "设置";
            creditsTitleText.text = "<制作人员";
            leftCreditsText.text = "<uppercase><size=40><color=#F6E19C>图形</color></uppercase></size><color=white>\r\n\r\nArt by Kandles (artbykandles.com)\r\nBorodar (borodar.com)\r\nBroken Vector (brokenvector.com)\r\nDungeon Mason (alexkim0415.wixsite.com/dungeonmason)\r\nEmacEArt (www.emaceart.com)\r\nCreativetrio (creativetrio.art)\r\nInfinity PBR (infinitypbr.com)\r\nJean Moreno (jeanmoreno.com)\r\nLayer Lab (layerlabgames.com)\r\nMalbers Animations (malbersanimations.artstation.com)\r\nMeshtint Studio (meshtint.com)\r\nOMABUARTS STUDIO (www.omabuarts.com)\r\nRetroVistas (artstation.com/ramonavladut)\r\nRunemark Studio (runemarkstudio.com)\r\nSynty Studios (syntystudios.com)\r\n";
            rightCreditsText.text = "<<uppercase><size=40><color=#F6E19C>音效和音频</color></uppercase></size><color=white>\r\n\r\nAmeAngelofSin (twitter.com/ameangelofsin)\r\nCafofo (cafofomusic.com)\r\nCicifyre (cicifyre.carrd.co)\r\nFreesound (freesound.org)\r\nSkyRaeVoicing (skyraevoicing.com)\r\nZapslat (zapsplat.com)</color>\r\n\r\n<uppercase><size=40>\r\n<color=#F6E19C>工具</color></uppercase></size><color=white>\r\n\r\nDented Pixel (dentedpixel.com)\r\nM Studio Hub (mstudiohub.com)</color>";
            otherGamesTitleText.text = "其他游戏";
            leftText.text = "左边";
            rightText.text = "正确的";
            controllsText.text = "控制";
            #endregion

            #region INVENTORY PANEL
            SetChineseFont(actionBarTPM);
            SetChineseFont(quantityTPM);
            SetChineseFont(inventoryTMP);
            SetChineseFont(itemTPM);

            actionBarTPM.text = "操作栏";
            quantityTPM.text = "数量";
            inventoryTMP.text = "存货";
            itemTPM.text = "伊滕斯";
            #endregion

            #region QUESTS PANEL
            SetChineseFont(questTitle);
            SetChineseFont(inProgressText);
            SetChineseFont(completedText);

            questTitle.text = "任务";
            inProgressText.text = "进行中";
            completedText.text = "完全的";
            #endregion

            #region DEATH PANEL
            SetChineseFont(deathText);
            SetChineseFont(tryAgainText);
            SetChineseFont(quitGameText);

            deathText.text = "你被打败了！";
            tryAgainText.text = "再试一次";
            quitGameText.text = "退出游戏";
            #endregion

            #region DIALOGUE PANEL
            SetChineseFont(dialogueText);
            #endregion

            #region MAIN MENU PANEL
            SetChineseFont(newGameText);
            SetChineseFont(continueGameText);
            SetChineseFont(newGameQuestionText);
            SetChineseFont(newGameYesText);
            SetChineseFont(newGameNoText);
            SetChineseFont(newGameTitleText);
            SetChineseFont(exitText);
            SetChineseFont(menuSettingsText);
            menuSettingsText.text = "设置";
            exitText.text = "出口";
            newGameText.text = "新游戏";
            newGameTitleText.text = "新游戏";
            continueGameText.text = "继续游戏";
            newGameQuestionText.text = "您确定要开始新游戏吗？ 您将失去所有已保存的进度！";
            newGameYesText.text = "是的";
            newGameNoText.text = "在";
            #endregion
        }
    }
}

public enum Language
{
    Portuguese, English, Chinese
}
