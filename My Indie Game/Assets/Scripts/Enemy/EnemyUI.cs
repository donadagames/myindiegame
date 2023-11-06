using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Gradient gradient;
    public Slider _slider;
    public Image fill;
    Enemy _enemy = null;
    Transform cam;
    public GameObject healthBar;
    public GameObject displayDamage;
    public TextMeshProUGUI damageText;
    public CanvasGroup damageCanvasGroup;

    private void Start()
    {
        cam = _enemy.spawner.status.mainCamera.transform;
    }

    public void DisplayDamageText(float damage, Color _color)
    {
        int _dmg = (int)damage;

        damageText.text = _dmg.ToString();
        damageText.color = _color;
        _enemy.canGetHit = false;
        displayDamage.SetActive(true);

        damageCanvasGroup.LeanAlpha(0, .5f);

        displayDamage.transform.LeanMoveLocalY
            (displayDamage.transform.position.y + 80, .5f).
            setEase(LeanTweenType.easeOutBack).setOnComplete(OnCompleteDisplayDamageText);
    }

    private void OnCompleteDisplayDamageText()
    {
        displayDamage.SetActive(false);
        _enemy.shouldCheckParticleHit = true;
        damageCanvasGroup.alpha = 1;
        displayDamage.transform.localPosition = new Vector3(0, displayDamage.transform.localPosition.y - 80, 0);
        _enemy.canGetHit = true;
    }

    private void OnEnable()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    public void SetMaxHealth(float maxHealth)
    {
        _slider.maxValue = maxHealth;
        SetValue(maxHealth);
    }

    public void SetValue(float value)
    {
        _slider.value = value;
        fill.color = gradient.Evaluate(_slider.normalizedValue);
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }

}
