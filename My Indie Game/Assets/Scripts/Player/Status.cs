using System;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public static Status instance;
    public Camera mainCamera;
    public Player player;

    [HideInInspector] public List<Enemy> enemies = new List<Enemy>();
    [HideInInspector] public DialogueUI dialogueUI;
    [HideInInspector] public Quests quests;
    [HideInInspector] public UIController uiController;
    [HideInInspector] public InputHandler input;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        mainCamera = Camera.main;
        currentHealth = health;
        currentEnergy = energy;
    }

    private void Start()
    {
        uiController = UIController.instance;
        input = InputHandler.instance;
        dialogueUI = DialogueUI.instance;
        quests = Quests.instance;
    }

    #region EVENTS
    #region SAFE ZONE EVENTS
    [HideInInspector] public bool isSafeZone = true;

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
    #endregion

    #region HEALTH EVENTS
    public float health;
    public float currentHealth;
    [HideInInspector] public bool isDamaged = false;
    [HideInInspector] public bool isAlive = true;

    public void TakeDamage(float damage, bool isCritical)
    {
        if (currentHealth - damage > 0)
            currentHealth -= damage;
        else
            currentHealth = 0;

        isDamaged = isCritical;
        OnHealthChange?.Invoke(this, new OnHealthEventHandler { _currentHealth = currentHealth });
        CheckDeath();
    }

    public void RecoverHealth(float recoverAmount)
    {
        if (currentHealth + recoverAmount <= health)
            currentHealth += recoverAmount;
        else
            currentHealth = health;

        OnHealthChange?.Invoke(this, new OnHealthEventHandler { _currentHealth = currentHealth });
    }

    public event EventHandler<OnHealthEventHandler> OnHealthChange;

    public class OnHealthEventHandler : EventArgs
    {
        public float _currentHealth;
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

    public event EventHandler<OnDieEventHandler> OnDie;

    public class OnDieEventHandler : EventArgs
    {
        public bool _isAlive;
    }
    #endregion

    #region ENERGY EVENTS
    public float currentEnergy;
    public float energy;

    public void ConsumeEnergy(float amount)
    {
        if (currentEnergy - amount > 0)
            currentEnergy -= amount;
        else
            currentEnergy = 0;

        OnEnergyChange?.Invoke(this, new OnEnergyEventHandler { _currentEnergy = currentEnergy });
    }

    public void RecoverEnergy(float recoverAmount)
    {
        if (currentEnergy + recoverAmount <= energy)
            currentEnergy += recoverAmount;
        else
            currentEnergy = energy;

        OnEnergyChange?.Invoke(this, new OnEnergyEventHandler { _currentEnergy = currentEnergy });
    }

    public event EventHandler<OnEnergyEventHandler> OnEnergyChange;

    public class OnEnergyEventHandler : EventArgs
    {
        public float _currentEnergy;
    }
    #endregion

    #region EXPERIENCE EVENTS
    [HideInInspector] public bool isLevelUp = false;
    public int currentLevel;
    public GameObject levelUp_VFX;
    public float currentExperience;
    public float nextLevelExperienceNeeded;
    public float baseExperience;
    public void ReciveExperience(float _xp)
    {
        currentExperience += _xp;
        OnExperienceChange?.Invoke(this, new OnExperienceEventHandler { _currentExperience = currentExperience });
        CheckLevelUp();
    }

    public void CheckLevelUp()
    {
        if (currentExperience >= nextLevelExperienceNeeded)
        {
            Instantiate(levelUp_VFX, player.transform.position, Quaternion.AngleAxis(-90, Vector3.left), player.transform);

            isLevelUp = true;
            currentLevel++;
            currentExperience = 0;
            nextLevelExperienceNeeded = currentLevel * baseExperience;
            player.soundController.LevelUpSound();
            OnLEvelUp?.Invoke(this, new OnLevelUpEventHandler
            { _isLevelUp = isLevelUp, _currentLevel = currentLevel, _nextLevelExperienceNeeded = nextLevelExperienceNeeded });
        }
    }

    public event EventHandler<OnExperienceEventHandler> OnExperienceChange;

    public class OnExperienceEventHandler : EventArgs
    {
        public float _currentExperience;
    }

    public class OnLevelUpEventHandler : EventArgs
    {
        public bool _isLevelUp;
        public float _currentLevel;
        public float _nextLevelExperienceNeeded;
    }

    public event EventHandler<OnLevelUpEventHandler> OnLEvelUp;
    #endregion

    #region SAVE EVENTS
    #endregion
    #endregion
}
