using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Gradient gradient;
    public Slider _slider;
    public Image fill;
    Enemy _enemy = null;
    public TextMeshProUGUI _nameText;
    Transform cam;
    public GameObject healthBar;


    private void Start()
    {
        cam = _enemy.spawner.status.mainCamera.transform;
    }

    private void OnEnable()
    {
        _enemy = GetComponentInParent<Enemy>();

        /*
        if (gameData.isPortuguese)
        {
            _nameText.text = _enemy.names[0];
        }
        else
        {
            _nameText.text = _enemy.names[1];
        }
        */
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
