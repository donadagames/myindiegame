using System.Collections;
using TMPro;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelUpText;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] CanvasGroup panelCanvasGroup;
    [SerializeField] GameObject levelUpPanel;
    Status status;

    private void Start()
    {
        status = Status.instance;
        status.OnLevelUp += OnPlayerLevelUp;
    }

    private void OnPlayerLevelUp(object sender, Status.OnLevelUpEventHandler handler)
    {
        StartCoroutine(PlayerLevelUp());
        panelCanvasGroup.alpha = 0f;
        SetLevelUpText(handler._currentLevel);
        levelUpPanel.SetActive(true);
    }

    private IEnumerator PlayerLevelUp()
    {
        yield return new WaitForSeconds(status.player.levelUpClipDuration - .5f);
        panelCanvasGroup.LeanAlpha(1, .5f);
        yield return new WaitForSeconds(4f);
        panelCanvasGroup.LeanAlpha(0, .5f).setOnComplete(() => levelUpPanel.SetActive(false));
    }

    public void SetLevelUpText(float level)
    {

        if (status.dialogueUI.settingsController.languageController.GetGlobalLanguage() == Language.Portuguese)
        {
            status.dialogueUI.settingsController.languageController.SetRegularFont(rewardText);
            rewardText.text = $"+100 PONTOS DE VITALIDADE\n+100 PONTOS DE ENERGIA\n+25 PONTOS DE POTÊNCIA DE ATAQUE";

        }
        else if (status.dialogueUI.settingsController.languageController.GetGlobalLanguage() == Language.English)
        {
            status.dialogueUI.settingsController.languageController.SetRegularFont(rewardText);
            rewardText.text = $"+100 HEALTH POINTS\n+100 ENERGY POITS\n+25 POWER ATTACK POINTS";

        }
        else if (status.dialogueUI.settingsController.languageController.GetGlobalLanguage() == Language.Chinese)
        {
            status.dialogueUI.settingsController.languageController.SetChineseFont(rewardText);
            rewardText.text = $"+100 健康点\n+100 能量点\n+25 强力攻击点";
        }

        levelUpText.text = $"{level}";
    }
}
