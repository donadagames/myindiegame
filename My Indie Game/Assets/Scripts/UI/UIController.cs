using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] AudioClip defaultClickAudioClip;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider energySlider;
    [SerializeField] private Slider experienceSlider;

    [SerializeField] private TextMeshProUGUI level;

    [SerializeField] CinemachineVirtualCamera virtualCamera;

    private CinemachineFramingTransposer body;
    private Status status;

    public Gradient healthGradient;
    public Gradient energyGradient;
    bool isClosing = false;
    public Image healthFill;
    public Image energyFill;

    [SerializeField] Image interactIcon;
    private int leanIndex;
    private int leandDefaultIndex;
    public Image fillMagicTimer;

    [SerializeField] Transform fireballBtn;
    [SerializeField] Transform cureBtn;
    [SerializeField] Transform blastBtn;
    [SerializeField] Transform iceBtn;
    [SerializeField] Transform starfallBtn;
    [SerializeField] Image skillIcon;
    [SerializeField] Transform skillArrow;
    [SerializeField] GameObject skillsToHideAndShow;
    public bool canInteract = true;

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
        status.OnLEvelUp += OnLevelUp;
        status.OnEnergyChange += OnEnergyChange;

        body = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        healthSlider.minValue = 0;
        healthSlider.maxValue = status.health;
        healthSlider.value = status.currentHealth;

        energySlider.minValue = 0;
        energySlider.maxValue = status.energy;
        energySlider.value = status.currentEnergy;

        experienceSlider.minValue = 0;
        experienceSlider.maxValue = status.nextLevelExperienceNeeded;

        zoomSlider.value = zoomSlider.maxValue;
        body.m_CameraDistance = zoomSlider.value;
    }
    public void PlayDefaultAudioClip() => status.player.soundController.PlayClip(defaultClickAudioClip);

    [Header("Left Stick")]
    #region LEFT STICK 

    public Slider zoomSlider;
    [SerializeField] GameObject[] positionFocus;
    private Direction currentDirection;

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

    public void ZoomSlider(float value)
    {
        body.m_CameraDistance = value;
    }

    public void SetDialogueCamera()
    {
        var currentDistance = body.m_CameraDistance;

        var zoom = LeanTween.value(zoomSlider.gameObject, currentDistance, zoomSlider.maxValue, .5f).setOnUpdate(OnZoomUpdate);
    }

    private void OnZoomUpdate(float value)
    {
        zoomSlider.value = value;
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

        if (skillsToHideAndShow.activeSelf == true)
        {
            CloseSkillsOptions();
        }

        else
        {
            OpenSkillsOptions();
        }
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
}


public enum Direction
{
    UpLeft, DownLeft, UpRight, DownRight, Default
}
