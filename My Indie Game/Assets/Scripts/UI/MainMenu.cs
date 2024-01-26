using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject newGameQuestionPanel;
    [SerializeField] GameObject buttonsPanel;
    [SerializeField] GameObject continueButton;

    Status status;

    [SerializeField] SceneData introScene;

    private void Start()
    {
        status = Status.instance;
        status.uiController.mainCanvasCanvasGroup.alpha = 0;
        status.uiController.mainCanvasCanvasGroup.interactable = false;
        status.uiController.mainCanvasCanvasGroup.blocksRaycasts = false;
        if (status.saveSystem.HasSavedStatus())
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
            status.saveSystem.DeleteAllFiles();
        }
    }

    public void OnNewGameButtonPressed()
    {
        if (continueButton.activeSelf == true)
        {
            status.loadSceneController.audioSource.PlayOneShot(status.uiController.defaultClickAudioClip);
            newGameQuestionPanel.SetActive(true);
        }
        else
        {
            status.loadSceneController.audioSource.PlayOneShot(status.uiController.defaultClickAudioClip);
            status.loadSceneController.LoadScene(introScene);
        }
    }

    public void QuitButton()
    {
        status.loadSceneController.audioSource.PlayOneShot(status.uiController.defaultClickAudioClip);
        Application.Quit();
    }

    public void OnContinueGameButtonPressed()
    {
        status.loadSceneController.audioSource.PlayOneShot(status.uiController.defaultClickAudioClip);
        status.loadSceneController.LoadScene(status.sceneData);
    }

    public void OnNewGameQuestionButtonPressed(bool accepted)
    {
        status.loadSceneController.audioSource.PlayOneShot(status.uiController.defaultClickAudioClip);

        if (accepted == false)
        {
            status.saveSystem.DeleteAllFiles();
            newGameQuestionPanel.SetActive(false);
        }

        else
        {
            status.saveSystem.DeleteAllFiles();
            status.loadSceneController.LoadScene(introScene);
        }
    }

    public void DesablePanels()
    { 
        mainMenuPanel.SetActive(false);
        newGameQuestionPanel.SetActive(false);
    }
}
