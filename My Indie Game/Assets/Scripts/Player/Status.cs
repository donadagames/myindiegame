using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Status : MonoBehaviour
{
    public string saveableEntityId;
    [ContextMenu("Generate ID")]
    private void GenereteID() => saveableEntityId = Guid.NewGuid().ToString();


    public static Status instance;
    public Camera mainCamera;
    public Player player;
    public Inventory inventory;
    public List<Enemy> enemies = new List<Enemy>();
    [HideInInspector] public DialogueUI dialogueUI;
    [HideInInspector] public Quests quests;
    [HideInInspector] public UIController uiController;
    [HideInInspector] public InputHandler input;
    public Transform mountTransform;
    public Player mount;
    public Pet pet;
    public Player lili;
    public GameObject liliGhost;
    public SaveSystem saveSystem;
    public Ghost ghost;
    public GameObject rebirth_VFX;

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
        inventory = Inventory.instance;
        saveSystem = SaveSystem.instance;
    }

    #region EVENTS
    #region SAFE ZONE EVENTS
    public bool isSafeZone = true;

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

    public void DealGhost()
    {
        ghost = Instantiate(liliGhost, player.transform.position, player.transform.rotation, player.transform).GetComponent<Ghost>();
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
            OnLevelUp?.Invoke(this, new OnLevelUpEventHandler
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

    public event EventHandler<OnLevelUpEventHandler> OnLevelUp;
    #endregion

    #region SAVE EVENTS
    public object CaptureState()
    {
        return new StatusData(this);
    }

    public void RestoreState(object state)
    {
        var data = (StatusData)state;

        Vector3 playerPos = new Vector3(data._playerXPosition, data._playerYPosition, data._playerZPosition);
        player.transform.position = playerPos;
    }

    #endregion
    #endregion
}

[System.Serializable]
public class StatusData
{
    public float _playerXPosition;
    public float _playerYPosition;
    public float _playerZPosition;

    public StatusData(Status status)
    {
        _playerXPosition = status.player.transform.position.x;
        _playerYPosition = status.player.transform.position.y;
        _playerZPosition = status.player.transform.position.z;
    }
}
