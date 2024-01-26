using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    //[0] = Main Menu Virtual Camera
    //[1] = Default Virtual Camera
    //[2] = Dialogue Virtual Camera
    //[3] = Short Cut Virtual Camera
    public CinemachineVirtualCamera[] virtualCameras;
    public AudioClip shortCutAudioClip;
    public Transform shortCutTarget;
    public AudioClip defaultClickAudioClip;
    public GameObject uiPanels;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider energySlider;
    [SerializeField] private Slider experienceSlider;
    [SerializeField] GameObject deathPanel;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] CanvasGroup deathPanelBackgroundCanvasGroup;
    public CanvasGroup mainCanvasCanvasGroup;
    [SerializeField] CanvasGroup deathPanelTextsCanvasGroup;
    [SerializeField] CanvasGroup deathPanelButtonsCanvasGroup;
    [SerializeField] GameObject deathButtonsPanel;
    public GameObject fadeBackgroundPanel;
    public CanvasGroup fadeBackgroundCanvasGroup;

    [SerializeField] GameObject displayInfoTextPrefab;
    [SerializeField] Transform infoPanel;
    public CinemachineVirtualCamera virtualCamera;
    public bool canDisplayMessage = true;
    public CinemachineFramingTransposer body;
    public CinemachineSameAsFollowTarget aim;
    private Status status;
    private bool canRebirth = true;

    public Gradient healthGradient;
    public Gradient energyGradient;
    bool isClosing = false;
    public Image healthFill;
    public Image energyFill;

    [SerializeField] Image interactIcon;
    private int leanIndex;
    private int leandDefaultIndex;
    public Image fillMagicTimer;
    public Image fillMountTimer;
    [SerializeField] Transform fireballBtn;
    [SerializeField] Transform cureBtn;
    [SerializeField] Transform blastBtn;
    [SerializeField] Transform iceBtn;
    [SerializeField] Transform starfallBtn;
    [SerializeField] Image skillIcon;
    [SerializeField] Transform skillArrow;
    [SerializeField] GameObject skillsToHideAndShow;
    public bool canInteract = true;

    [SerializeField] HorizontalLayoutGroup downLayoutGroup;
    [SerializeField] HorizontalLayoutGroup upLayoutGroup;

    [SerializeField] VerticalLayoutGroup uiBarsLayoutGroup;
    [SerializeField] VerticalLayoutGroup itensPanelLayoutGroup;

    [SerializeField] VerticalLayoutGroup rightJoystickLayoutGroup;
    [SerializeField] HorizontalLayoutGroup leftJoystickLayoutGroup;

    [SerializeField] RectTransform meleeAttackBtn;
    [SerializeField] Transform magicAttackBtn;
    [SerializeField] Transform animalInteractionBtn;
    [SerializeField] Transform interactBtn;
    [SerializeField] Transform skillArrowBtn;
    private int mountLeanIndex;
    public MainMenu mainMenu;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        status = Status.instance;
        status.OnHealthChange += OnHealthChange;
        status.OnExperienceChange += OnExperienceChange;
        status.OnLevelUp += OnLevelUp;
        status.OnEnergyChange += OnEnergyChange;
        status.OnDie += OnPlayerDies;
        body = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        aim = virtualCamera.GetCinemachineComponent<CinemachineSameAsFollowTarget>();
        healthSlider.minValue = 0;
        healthSlider.maxValue = status.health;
        healthSlider.value = status.currentHealth;

        energySlider.minValue = 0;
        energySlider.maxValue = status.energy;
        energySlider.value = status.currentEnergy;

        experienceSlider.minValue = 0;
        experienceSlider.maxValue = status.nextLevelExperienceNeeded;

        //inicia em zoom in
        zoomSlider.value = zoomSlider.minValue;
        zoomSlider.interactable = false;

        //inicia em zoom out
        //zoomSlider.value = zoomSlider.maxValue;
        //zoomSlider.interactable = true;

        virtualCamera.transform.eulerAngles = new Vector3(zoomSlider.value * 5, 0, 0);
        body.m_CameraDistance = zoomSlider.value;
    }
    public void PlayDefaultAudioClip() => status.player.soundController.PlayClip(defaultClickAudioClip);

    [Header("Left Stick")]
    #region LEFT STICK 

    public Slider zoomSlider;
    [SerializeField] GameObject[] positionFocus;
    private Direction currentDirection;

    public void SetMainMenuCamera()
    {
        virtualCameras[0].gameObject.SetActive(true);
        virtualCameras[1].gameObject.SetActive(false);
        virtualCameras[2].gameObject.SetActive(false);
        virtualCameras[3].gameObject.SetActive(false);
    }
    public void SetDefaultCamera()
    {
        virtualCameras[0].gameObject.SetActive(false);
        virtualCameras[1].gameObject.SetActive(true);
        virtualCameras[2].gameObject.SetActive(false);
        virtualCameras[3].gameObject.SetActive(false);
    }

    public void SetDialogueCamera()
    {
        virtualCameras[0].gameObject.SetActive(false);
        virtualCameras[1].gameObject.SetActive(false);
        virtualCameras[2].gameObject.SetActive(true);
        virtualCameras[3].gameObject.SetActive(false);
    }

    public void SetShortCutCamera()
    {
        virtualCameras[0].gameObject.SetActive(false);
        virtualCameras[1].gameObject.SetActive(false);
        virtualCameras[2].gameObject.SetActive(false);
        virtualCameras[3].gameObject.SetActive(true);
    }

    private void SetPositionFocus(int focusIndex)
    {
        for (int i = 0; i < positionFocus.Length; i++)
        {
            positionFocus[i].SetActive(false);
        }
        if (focusIndex < positionFocus.Length)
        {
            positionFocus[focusIndex].SetActive(true);
        }
    }

    public void SetPositionFocusUI(Direction direction)
    {
        if (direction == currentDirection) return;
        else
        {
            switch (direction)
            {
                case Direction.UpLeft:
                    currentDirection = Direction.UpLeft;
                    SetPositionFocus(0);
                    break;
                case Direction.UpRight:
                    currentDirection = Direction.UpRight;
                    SetPositionFocus(1);
                    break;
                case Direction.DownLeft:
                    currentDirection = Direction.DownLeft;
                    SetPositionFocus(2);
                    break;
                case Direction.DownRight:
                    currentDirection = Direction.DownRight;
                    SetPositionFocus(3);
                    break;
                case Direction.Default:
                    currentDirection = Direction.Default;
                    SetPositionFocus(10);
                    break;
                default:
                    currentDirection = Direction.Default;
                    SetPositionFocus(10);
                    break;
            }
        }
    }
    #endregion

    public void OnHealthChange(object sender, Status.OnHealthEventHandler handler)
    {
        healthSlider.value = handler._currentHealth;
        healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    public void OnEnergyChange(object sender, Status.OnEnergyEventHandler handler)
    {
        energySlider.value = handler._currentEnergy;
        energyFill.color = energyGradient.Evaluate(energySlider.normalizedValue);
    }

    public void OnExperienceChange(object sender, Status.OnExperienceEventHandler handler)
    {
        experienceSlider.value = handler._currentExperience;
    }

    public void OnLevelUp(object sender, Status.OnLevelUpEventHandler handler)
    {
        level.text = handler._currentLevel.ToString();
        experienceSlider.minValue = 0;
        experienceSlider.maxValue = handler._nextLevelExperienceNeeded;
        experienceSlider.value = 0;
    }

    public void ZoomSlider(float value) // 3 a 9 
    {
        body.m_CameraDistance = value;
        virtualCamera.transform.eulerAngles = new Vector3(value * 5, 0, 0);
    }

    public void SetInsideHouseCamera()
    {
        var currentDistance = body.m_CameraDistance;
        zoomSlider.interactable = false;
        var zoom = LeanTween.value(zoomSlider.gameObject, currentDistance, zoomSlider.minValue, .5f).setOnUpdate(OnZoomUpdate);
    }

    public void SetOutsideHouseCamera()
    {
        // var currentDistance = body.m_CameraDistance;
        zoomSlider.interactable = true;
        //var zoom = LeanTween.value(zoomSlider.gameObject, currentDistance, zoomSlider.maxValue, .5f).setOnUpdate(OnZoomUpdate);
    }

    public void OnZoomUpdate(float value)
    {
        zoomSlider.value = value;
    }

    public void SetCameraTarget(Transform target)
    {
        virtualCamera.Follow = target;
    }

    public void SetLeftControlsConfigurations()
    {
        downLayoutGroup.reverseArrangement = false;
        downLayoutGroup.childAlignment = TextAnchor.LowerLeft;

        upLayoutGroup.reverseArrangement = false;
        upLayoutGroup.childAlignment = TextAnchor.UpperLeft;

        uiBarsLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
        uiBarsLayoutGroup.padding.right = 200;
        uiBarsLayoutGroup.transform.localEulerAngles = new Vector3(0, 0, 0);
        itensPanelLayoutGroup.childAlignment = TextAnchor.MiddleRight;

        leftJoystickLayoutGroup.childAlignment = TextAnchor.LowerLeft;
        rightJoystickLayoutGroup.childAlignment = TextAnchor.LowerRight;
        rightJoystickLayoutGroup.padding = new RectOffset(390, 75, 0, 20);
        rightJoystickLayoutGroup.transform.eulerAngles = new Vector3(0, 0, 0);

        meleeAttackBtn.localEulerAngles = new Vector3(0, 0f, 0);
        magicAttackBtn.localEulerAngles = new Vector3(0, 0, 0);
        animalInteractionBtn.localEulerAngles = new Vector3(0, 0, 0);
        interactBtn.localEulerAngles = new Vector3(0, 0, 0);

        skillArrowBtn.localEulerAngles = new Vector3(0, 0, 0);
        fireballBtn.localEulerAngles = new Vector3(0, 0, 0);
        cureBtn.localEulerAngles = new Vector3(0, 0, 0);
        blastBtn.localEulerAngles = new Vector3(0, 0, 0);
        iceBtn.localEulerAngles = new Vector3(0, 0, 0);
        starfallBtn.localEulerAngles = new Vector3(0, 0, 0);

        skillArrowBtn.localPosition = new Vector3(-35, 0, 0);
        skillsToHideAndShow.transform.localEulerAngles = new Vector3(0, 0, 0);
        skillsToHideAndShow.transform.localPosition = new Vector3(-220, 40, 0);

        level.transform.localEulerAngles = new Vector3(0, 0, 0);

    }

    public void SetRightControlsConfigurations()
    {
        downLayoutGroup.reverseArrangement = true;
        downLayoutGroup.childAlignment = TextAnchor.LowerLeft;

        upLayoutGroup.reverseArrangement = true;
        upLayoutGroup.childAlignment = TextAnchor.UpperLeft;

        uiBarsLayoutGroup.childAlignment = TextAnchor.MiddleRight;
        uiBarsLayoutGroup.padding.right = 1540;
        uiBarsLayoutGroup.transform.localEulerAngles = new Vector3(0, 180, 0);
        itensPanelLayoutGroup.childAlignment = TextAnchor.MiddleLeft;

        leftJoystickLayoutGroup.childAlignment = TextAnchor.LowerRight;
        rightJoystickLayoutGroup.childAlignment = TextAnchor.LowerRight;
        rightJoystickLayoutGroup.padding = new RectOffset(1258, -836, 0, 20);
        rightJoystickLayoutGroup.transform.eulerAngles = new Vector3(0, 180, 0);

        meleeAttackBtn.localEulerAngles = new Vector3(0, 180f, 0);
        magicAttackBtn.localEulerAngles = new Vector3(0, 180, 0);
        animalInteractionBtn.localEulerAngles = new Vector3(0, 180, 0);
        interactBtn.localEulerAngles = new Vector3(0, 180, 0);

        skillArrowBtn.localEulerAngles = new Vector3(0, 180, 0);
        fireballBtn.localEulerAngles = new Vector3(0, 180, 0);
        cureBtn.localEulerAngles = new Vector3(0, 180, 0);
        blastBtn.localEulerAngles = new Vector3(0, 180, 0);
        iceBtn.localEulerAngles = new Vector3(0, 180, 0);
        starfallBtn.localEulerAngles = new Vector3(0, 180, 0);
        skillArrowBtn.localPosition = new Vector3(40, 0, 0);
        skillsToHideAndShow.transform.localEulerAngles = new Vector3(0, 180, 0);
        skillsToHideAndShow.transform.localPosition = new Vector3(365, 40, 0);

        level.transform.localEulerAngles = new Vector3(0, 180, 0);

    }

    public void SetInteractionSprite(Sprite sprite)
    {
        interactIcon.sprite = sprite;
        interactIcon.enabled = true;

        if (!isClosing)
        {
            leanIndex = interactIcon.gameObject.LeanScale(new Vector3(1, 1, 1), .25f).setOnComplete(() =>
           interactIcon.gameObject.LeanScale(new Vector3(.85f, .85f, .85f), .25f)).
           setLoopPingPong().id;
        }
    }

    public void SetDefaultInteractionSprite()
    {
        isClosing = true;
        LeanTween.cancel(leanIndex);

        leandDefaultIndex = interactIcon.gameObject.LeanScale(new Vector3(0, 0, 0), .1f).
            setOnComplete(OnDefaultComplete).id;
    }

    private void OnDefaultComplete()
    {
        interactIcon.enabled = false;
        isClosing = false;
        LeanTween.cancel(leandDefaultIndex);
        interactIcon.gameObject.transform.localScale = new Vector3(.85f, .85f, .85f);
    }

    public void DealMagicTimer(Skill skill)
    {
        status.input.hasCompletedMagicTimer = false;

        var value = LeanTween.value(gameObject, 1, 0, .25f).setOnUpdate(UpdateFillImage).setOnComplete(() =>
        LeanTween.value(gameObject, 0, 1, skill.time).setOnUpdate(UpdateFillImage).setOnComplete(() =>
        status.input.hasCompletedMagicTimer = true));
    }

    private void UpdateFillImage(float value)
    {
        fillMagicTimer.fillAmount = value;
    }

    public void OnSkillsOptionsButtonPressed()
    {
        if (canInteract == false) return;

        canInteract = false;

        status.player.soundController.PlayClip(defaultClickAudioClip);

        if (skillsToHideAndShow.activeSelf == true)
        {
            CloseSkillsOptions();
        }

        else
        {
            OpenSkillsOptions();
        }
    }

    public void DealMountTimer()
    {
        status.input.hasCompletedMountTimer = false;

        var value = LeanTween.value(1, 0, .25f).setOnUpdate(UpdateMountFillImage).setOnComplete(DealMount);
    }

    private void DealMount()
    {

        var call = LeanTween.value(0, 1, 10).setOnUpdate(UpdateMountFillImage).setOnComplete(OnCompleteMountTimer);
        status.input.isMounted = true;
    }

    private void UpdateMountFillImage(float value)
    {
        fillMountTimer.fillAmount = value;
    }

    public void Dismount()
    {
        LeanTween.cancelAll(false);
        OnCompleteMountTimer();
    }

    private void OnCompleteMountTimer()
    {
        status.player = status.lili;
        Instantiate(status.pet.mount_VFX, status.player.transform.position, Quaternion.identity);

        status.player.soundController.FireballSound();

        status.pet.transform.position = status.player.transform.position + new Vector3(Random.Range(.1f, .3f), 0, Random.Range(-.1f, -.3f));
        status.pet.gameObject.SetActive(true);
        status.pet.hasInteract = false;

        Destroy(status.mount.gameObject);
        status.mount = null;
        status.mountTransform = null;
        status.input.isMounting = false;
        status.input.isMounted = false;

        if (status.isSafeZone == false)
        {
            status.player.SetSwordAndShieldConfiguration();
        }
        else
        {
            status.player.SetUnarmedConfiguration();

        }

        status.uiController.SetCameraTarget(status.player.transform);

        if (status.isSafeZone == true)
        {
            status.player.animations.animator.Play("NoWeapon_Dismount");
        }

        else
        {
            status.player.animations.animator.Play("SwordAndShield_Dismount");
        }

        if (status.input.input.magnitude > 0)
        {
            status.player.animations.animator.SetBool("IsMoving", true);
        }
        else
        {
            status.player.animations.animator.SetBool("IsMoving", false);
        }

        status.player.transform.SetParent(status.playerParentTransform);
        DealMountWaitingTime();

    }

    private void DealMountWaitingTime()
    {
        var value = LeanTween.value(gameObject, 1, 0, .25f).setOnUpdate(UpdateMountFillImage).setOnComplete(() =>
            LeanTween.value(gameObject, 0, 1, 120).setOnUpdate(UpdateMountFillImage).setOnComplete(() => status.input.hasCompletedMountTimer = true));
    }

    private void OpenSkillsOptions()
    {
        skillsToHideAndShow.SetActive(true);

        skillArrow.LeanRotateY(180, .25f);

        fireballBtn.LeanMoveLocalY(0, .25f).setEase(LeanTweenType.easeOutBack).setOnComplete
            (() => cureBtn.LeanMoveLocalY(0, .25f).setEase(LeanTweenType.easeOutBack).setOnComplete
            (() => blastBtn.LeanMoveLocalY(0, .25f).setEase(LeanTweenType.easeOutBack).setOnComplete
            (() => iceBtn.LeanMoveLocalY(0, .25f).setEase(LeanTweenType.easeOutBack).setOnComplete
            (() => starfallBtn.LeanMoveLocalY(0, .25f).setEase(LeanTweenType.easeOutBack).setOnComplete
            (() => canInteract = true)))));
    }

    private void CloseSkillsOptions()
    {
        skillArrow.LeanRotateY(0, .25f);

        starfallBtn.LeanMoveLocalY(-200, .25f).setEase(LeanTweenType.easeInBack).setOnComplete
            (() => iceBtn.LeanMoveLocalY(-200, .25f).setEase(LeanTweenType.easeInBack).setOnComplete
            (() => blastBtn.LeanMoveLocalY(-200, .25f).setEase(LeanTweenType.easeInBack).setOnComplete
            (() => cureBtn.LeanMoveLocalY(-200, .25f).setEase(LeanTweenType.easeInBack).setOnComplete
            (() => fireballBtn.LeanMoveLocalY(-200, .25f).setEase(LeanTweenType.easeInBack).setOnComplete
            (DealCloseSkillsOptions)))));
    }
    private void DealCloseSkillsOptions()
    {
        skillsToHideAndShow.SetActive(false);
        canInteract = true;
    }

    public void UpdateSkillImage(Skill skill)
    {
        skillIcon.sprite = skill.icon;
        status.input.selectedSkill = skill;
    }

    public void OnPlayerDies(object sender, Status.OnDieEventHandler handler)
    {
        deathPanelBackgroundCanvasGroup.alpha = 0;
        deathPanelTextsCanvasGroup.alpha = 0;
        deathPanelButtonsCanvasGroup.alpha = 0;
        deathPanel.SetActive(true);
        deathButtonsPanel.SetActive(true);

        var currentDistance = body.m_CameraDistance;

        var zoom = LeanTween.value(zoomSlider.gameObject, currentDistance, zoomSlider.minValue, 1f).setOnUpdate(OnZoomUpdate);

        deathPanelBackgroundCanvasGroup.LeanAlpha(1, 1f).setOnComplete(() => deathPanelTextsCanvasGroup.LeanAlpha(1, 3f).setOnComplete(() => deathPanelButtonsCanvasGroup.LeanAlpha(1, .5f)));

    }

    public void OnQuitButtonPressed()
    {
        PlayDefaultAudioClip();
        Application.Quit();
    }

    public void OnTryAgainButtonPressed()
    {
        if (!canRebirth) return;

        PlayDefaultAudioClip();
        canRebirth = false;
        fadeBackgroundCanvasGroup.alpha = 0;
        fadeBackgroundPanel.SetActive(true);
        deathPanelButtonsCanvasGroup.LeanAlpha(0, .5f);
        deathPanelBackgroundCanvasGroup.LeanAlpha(0, 1.6f).setOnComplete(() => deathPanel.SetActive(false));
        deathPanelTextsCanvasGroup.LeanAlpha(0, 1.5f).setOnComplete(() => deathButtonsPanel.SetActive(false));
        fadeBackgroundCanvasGroup.LeanAlpha(1, 1.6f).setOnComplete(() => StartCoroutine(DealDeath()));
    }

    private IEnumerator DealDeath()
    {
        yield return new WaitForSeconds(1f);
        status.saveSystem.LoadStatus();

        foreach (Enemy enemy in status.enemies)
        {
            enemy.spawner.shouldSpawn = true;
            enemy.spawner.ResetEnemyPositionIndex();
            Destroy(enemy.gameObject);
        }

        status.enemies = new List<Enemy>();

        yield return new WaitForSeconds(3f);

        fadeBackgroundCanvasGroup.LeanAlpha(0, 3f).setOnComplete(() => fadeBackgroundPanel.SetActive(false));

        yield return new WaitForSeconds(.5f);
        var currentDistance = body.m_CameraDistance;
        var zoom = LeanTween.value(zoomSlider.gameObject, currentDistance, zoomSlider.maxValue, 1f).setOnUpdate(OnZoomUpdate);
        canRebirth = true;
        status.ghost.animator.Play("GoDown");

        yield return new WaitForSeconds(.833f);
        Instantiate(status.rebirth_VFX, status.player.transform.position, Quaternion.identity);
        status.player.soundController.FireballSound();
        Destroy(status.ghost.gameObject);
        status.ghost = null;
        status.input.isRebirth = true;
    }

    public void DisplayInfoText(string text, LanguageController languageController)
    {
        if (!canDisplayMessage) return;

        canDisplayMessage = false;
        DisplayMessage message = Instantiate(displayInfoTextPrefab, infoPanel).GetComponent<DisplayMessage>();
        message.controller = this;
        message.text.text = text;

        if (languageController.GetGlobalLanguage() == Language.Portuguese || languageController.GetGlobalLanguage() == Language.English)
            languageController.SetRegularFont(message.text);
        else
            languageController.SetChineseFont(message.text);
    }

    public void SetShortCutTargetInicialTransform(Vector3 position, Vector3 rotation)
    {
        shortCutTarget.localPosition = position;
        shortCutTarget.localEulerAngles = rotation;
    }
}


public enum Direction
{
    UpLeft, DownLeft, UpRight, DownRight, Default
}
