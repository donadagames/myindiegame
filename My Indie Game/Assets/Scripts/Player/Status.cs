using System;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public static Status instance;
    public bool isSafeZone = true;

    public List<Enemy> enemies = new List<Enemy>();

    public float currentHealth;
    public float currentEnergy;
    public float currentExperience;

    public float health;
    public float energy;
    public float nextLevelExperienceNeeded;

    public Camera mainCamera;

    public bool isAlive = true;
    public bool isDamaged = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        mainCamera = Camera.main;
        currentHealth = health;
        currentEnergy = energy;
    }

    public Player player;
    public UIController uiController;

    private void Start()
    {
        uiController = UIController.instance;

    }

    public event EventHandler<OnSafeZoneChangeEventHandler> OnSafeZoneChange;

    public class OnSafeZoneChangeEventHandler : EventArgs
    {
        public bool _isSafeZone;
    }

    public void ChangeSafeZone(bool _isSafeZone)
    {
        isSafeZone = _isSafeZone;
        OnSafeZoneChange?.Invoke(this, new OnSafeZoneChangeEventHandler { _isSafeZone = isSafeZone });
    }

    public void TakeDamage(float damage, bool isCritical)
    {
        currentHealth -= damage;
        isDamaged = isCritical;
        OnHealthChange?.Invoke(this, new OnHealthEventHandler { _currentHealth = currentHealth });
        CheckDeath();
    }

    public void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            isAlive = false;

            foreach (Enemy enemy in enemies)
            {
                if (enemy.isAlive)
                { 
                    enemy.isVictory = true;
                }
            }

            OnDie?.Invoke(this, new OnDieEventHandler { _isAlive = isAlive });
        }
    }

    public event EventHandler<OnHealthEventHandler> OnHealthChange;

    public class OnHealthEventHandler : EventArgs
    {
        public float _currentHealth;
    }

    public event EventHandler<OnDieEventHandler> OnDie;

    public class OnDieEventHandler : EventArgs
    {
        public bool _isAlive;
    }
}
