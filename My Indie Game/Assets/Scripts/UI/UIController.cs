using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    private Status status;

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

    public Gradient healthGradient;
    public Gradient energyGradient;

    public Image healthFill;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        status = Status.instance;
        status.OnHealthChange += OnHealthChange;

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

}

public enum Direction
{
    UpLeft, DownLeft, UpRight, DownRight, Default
}
