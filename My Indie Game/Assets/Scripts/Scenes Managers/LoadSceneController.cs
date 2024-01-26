using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class LoadSceneController : MonoBehaviour
{
    public static LoadSceneController instance;
    Status status;
    public AudioSource audioSource;
    SceneData sceneToLoad;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        status = Status.instance;
    }

    public void LoadSceneWithSpecificPositionAndRotation(SceneData scene, Vector3 playerPosition, Vector3 playerRotation)
    {
        sceneToLoad = scene;
        status.uiController.fadeBackgroundCanvasGroup.alpha = 0;
        status.uiController.fadeBackgroundPanel.SetActive(true);
        status.uiController.fadeBackgroundCanvasGroup.LeanAlpha(1, 2).setOnComplete(() => StartCoroutine(EnterSceneWithSpecificPositionAndRotation(scene, playerPosition, playerRotation)));
        LeanTween.value(this.gameObject, audioSource.volume, 0, 2).setOnUpdate(OnUpdateAudio);
    }

    public void LoadScene(SceneData scene)
    {
        sceneToLoad = scene;
        status.uiController.fadeBackgroundCanvasGroup.alpha = 0;
        status.uiController.fadeBackgroundPanel.SetActive(true);
        status.uiController.fadeBackgroundCanvasGroup.LeanAlpha(1, 2).setOnComplete(() => StartCoroutine(EnterScene(scene)));
        LeanTween.value(this.gameObject, audioSource.volume, 0, 2).setOnUpdate(OnUpdateAudio);
    }

    private IEnumerator EnterScene(SceneData scene)
    {
        status.uiController.mainCanvasCanvasGroup.interactable = false;
        status.uiController.mainCanvasCanvasGroup.blocksRaycasts = false;

        status.input.canMove = false;

        status.uiController.virtualCameras[0].gameObject.SetActive(false);
        status.uiController.virtualCameras[scene.virtualCameraIndex].gameObject.SetActive(true);

        yield return new WaitForSeconds(.5f);

        status.uiController.mainMenu.DesablePanels();
        status.uiController.mainCanvasCanvasGroup.alpha = 1;
        status.uiController.mainCanvasCanvasGroup.interactable = true;
        status.uiController.mainCanvasCanvasGroup.blocksRaycasts = true;
        status.uiController.SetPositionFocusUI(Direction.Default);
        SceneManager.LoadScene(sceneToLoad.builtName, LoadSceneMode.Single);
        yield return new WaitForSeconds(1f);

        status.player.transform.position = scene.playerPosition;
        status.player.transform.eulerAngles = scene.playerRotation;

        status.mainCamera.gameObject.SetActive(true);

        status.input.SearchForEnemySpawner();
        status.saveSystem.RestoreAllStates();

        status.isSafeZone = scene.isSafeZone;
        status.player.animations.PlayAnimation(status.player.animations.IDLE, scene.isSafeZone);

        if (status.inventory.swordAndShield[0].quantity >= 1  && status.inventory.swordAndShield[1].quantity >= 1)
            status.input.SetGroundConfiguration(scene.isSafeZone);
        else

        {
            status.player.SetUnequipedConfiguration();
        }

        yield return new WaitForSeconds(1f);
        audioSource.clip = scene.sceneAudioClip;
        audioSource.Play();
        LeanTween.value(this.gameObject, audioSource.volume, 1, 2).setOnUpdate(OnUpdateAudio);
        status.uiController.fadeBackgroundCanvasGroup.LeanAlpha(0, 3).setOnComplete(() => status.uiController.fadeBackgroundPanel.SetActive(false));
        yield return new WaitForSeconds(.5f);
        status.input.canMove = true;

    }

    private IEnumerator EnterSceneWithSpecificPositionAndRotation(SceneData scene, Vector3 playerPosition, Vector3 playerRotation)
    {
        status.uiController.mainCanvasCanvasGroup.interactable = false;
        status.uiController.mainCanvasCanvasGroup.blocksRaycasts = false;
        status.input.canMove = false;

        yield return new WaitForSeconds(.5f);

        status.uiController.mainMenu.DesablePanels();
        status.uiController.mainCanvasCanvasGroup.alpha = 1;
        status.uiController.mainCanvasCanvasGroup.interactable = true;
        status.uiController.mainCanvasCanvasGroup.blocksRaycasts = true;
        status.uiController.SetPositionFocusUI(Direction.Default);
        SceneManager.LoadScene(sceneToLoad.builtName, LoadSceneMode.Single);
        yield return new WaitForSeconds(1f);

        status.player.transform.position = playerPosition;
        status.player.transform.eulerAngles = playerRotation;

        status.mainCamera.gameObject.SetActive(true);

        status.input.SearchForEnemySpawner();
        status.saveSystem.RestoreAllStates();

        status.isSafeZone = scene.isSafeZone;
        status.player.animations.PlayAnimation(status.player.animations.IDLE, scene.isSafeZone);

        status.input.SetGroundConfiguration(scene.isSafeZone);

        yield return new WaitForSeconds(1f);
        audioSource.clip = scene.sceneAudioClip;
        audioSource.Play();
        LeanTween.value(this.gameObject, audioSource.volume, 1, 2).setOnUpdate(OnUpdateAudio);
        status.uiController.fadeBackgroundCanvasGroup.LeanAlpha(0, 3).setOnComplete(() => status.uiController.fadeBackgroundPanel.SetActive(false));
        yield return new WaitForSeconds(.5f);
        status.input.canMove = true;

    }

    public void OnUpdateAudio(float value)
    {
        audioSource.volume = value;
    }
}
