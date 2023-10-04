using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Cinemachine;
using System;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public bool isPortuguese = false;


    [SerializeField] private TextMeshProUGUI goldQuantity;
    [SerializeField] private TextMeshProUGUI redDiamondQuantity;
    [SerializeField] private TextMeshProUGUI greenDiamondQuantity;
    [SerializeField] private TextMeshProUGUI blueDiamondQuantity;

    [SerializeField] private TextMeshProUGUI redPotionQuantity;
    [SerializeField] private TextMeshProUGUI bluePotionQuantity;
    [SerializeField] private TextMeshProUGUI purplePotionQuantity;

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
    [SerializeField] Image interactIcon;
    [SerializeField] Sprite handSprite;
    private int leanIndex;
    private int leandDefaultIndex;
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

        body = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        healthSlider.minValue = 0;
        healthSlider.maxValue = status.health;
        healthSlider.value = status.currentHealth;

        energySlider.minValue = 0;
        energySlider.maxValue = status.energy;
        energySlider.value = status.currentEnergy;

        experienceSlider.minValue = 0;
        experienceSlider.maxValue = status.nextLevelExperienceNeeded;
    }

    [Header("Left Stick")]
    #region LEFT STICK 

    [SerializeField] Slider zoomSlider;
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

    //public event EventHandler<OnLanguageChangeEventHandler> OnLanguageChange;

    public class OnLanguageChangeEventHandler : EventArgs
    {
        public bool isPortuguese;
    }
}

public enum Direction
{
    UpLeft, DownLeft, UpRight, DownRight, Default
}
