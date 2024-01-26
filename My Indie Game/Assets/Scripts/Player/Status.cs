using System;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public string saveableEntityId;
    [ContextMenu("Generate ID")]
    private void GenereteID() => saveableEntityId = Guid.NewGuid().ToString();

    public SceneManagerController sceneManager;
    public static Status instance;
    public Camera mainCamera;
    public Player player;
    public Inventory inventory;
    public List<Enemy> enemies = new List<Enemy>();
    [HideInInspector] public DialogueUI dialogueUI;
    [HideInInspector] public Quests quests;
    public UIController uiController;
    [HideInInspector] public InputHandler input;
    public LoadSceneController loadSceneController;
    public Transform mountTransform;
    public Transform playerParentTransform;
    public Player mount;
    public Pet pet;
    public Player lili;
    public GameObject liliGhost;
    public SaveSystem saveSystem;
    public Ghost ghost;
    public GameObject rebirth_VFX;
    public SceneData sceneData;
    public float dampingTime = 12f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        currentHealth = health;
        currentEnergy = energy;
    }

    private void Start()
    {
        input = InputHandler.instance;
        dialogueUI = DialogueUI.instance;
        quests = Quests.instance;
        loadSceneController = LoadSceneController.instance;
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
    public int force;
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
    public object CapturePlayerStatusState()
    {
        return new SaveData { statusData = new StatusData(this) };
    }

    public object CaptureInteractablesState()
    {
        var listOfSavables = new List<Savable>();
        foreach (var savable in sceneManager.iSavables)
        {
            if (savable.hasInteract)
            {
                listOfSavables.Add(new Savable(savable.saveableEntityId));
            }
        }

        return new SaveData { savables = listOfSavables };
    }

    public object CaptureSavableEntitiesState()
    {
        var listOfSavableEntities = new List<SerializableSavableEntity>();

        foreach (var savable in sceneManager.savableEntities)
        {
            if (savable.shouldSave)
            {
                listOfSavableEntities.Add(new SerializableSavableEntity(savable.saveableEntityId));
            }
        }
        return new SaveData { savablesEntities = listOfSavableEntities };
    }

    public object CaptureEnemiesState()
    {
        var listOfEnemies = new List<SerializableEnemySpawner>();

        foreach (var enemySpawner in sceneManager.enemySpawners)
        {
            if (enemySpawner.shouldSave)
            {
                listOfEnemies.Add(new SerializableEnemySpawner(enemySpawner.saveableEntityId));
            }
        }
        return new SaveData { enemySpawners = listOfEnemies };
    }

    public object CaptureDialogueActivatorsState()
    {
        var listOfDialogueActivators = new List<SavableDialogueActivator>();

        foreach (var savable in sceneManager.iSavablesDialogueActivators)
        {
            listOfDialogueActivators.Add(new SavableDialogueActivator(savable.saveableEntityId, savable.currentDialogue.dialogueID));
        }

        return new SaveData { savablesDialogueActivators = listOfDialogueActivators };
    }

    public void RestoreInteractablesState(object state)
    {
        var data = (SaveData)state;

        for (int i = 0; i < data.savables.Count; i++)
        {
            if (sceneManager.savablesID.TryGetValue(data.savables[i].savableID, out Interactable savable))
            {
                savable.RestoreState(data.savables[i]);
            }
        }
    }

    public void RestoreSavableEntitiesState(object state)
    {
        var data = (SaveData)state;

        for (int i = 0; i < data.savablesEntities.Count; i++)
        {
            if (sceneManager.savableEntitiesID.TryGetValue(data.savablesEntities[i].savableID, out SavableEntity savable))
            {
                savable.RestoreState(data.savablesEntities[i]);
            }
        }

    }

    public void RestoreEnemiesState(object state)
    {
        var data = (SaveData)state;

        for (int i = 0; i < data.enemySpawners.Count; i++)
        {
            if (sceneManager.enemySpawnersID.TryGetValue(data.enemySpawners[i].savableID, out EnemySpawner spawner))
            {
                spawner.RestoreState();
            }
        }

    }

    public void RestoreDialogueActivatorsState(object state)
    {
        var data = (SaveData)state;

        for (int i = 0; i < data.savablesDialogueActivators.Count; i++)
        {
            if (sceneManager.savablesDialogueActivatorsID.TryGetValue(data.savablesDialogueActivators[i].savableID, out Interactable savable))
            {
                savable.RestoreState(null, data.savablesDialogueActivators[i]);
            }
        }
    }

    public void RestorePlayerStatusState(object state)
    {
        var data = (SaveData)state;

        Vector3 playerPos = new Vector3(data.statusData.playerPos.x, data.statusData.playerPos.y, data.statusData.playerPos.z);
        player.transform.position = playerPos;

        sceneData = saveSystem.GetSceneDataFromBuildName(data.statusData.sceneBuildName);
    }

    [Serializable]
    private struct SaveData
    {
        public StatusData statusData;
        public List<Savable> savables;
        public List<SavableDialogueActivator> savablesDialogueActivators;
        public List<SerializableSavableEntity> savablesEntities;
        public List<SerializableEnemySpawner> enemySpawners;
    }
    #endregion
    #endregion
}

[System.Serializable]
public class StatusData
{
    public SerializedVector3 playerPos;
    public string sceneBuildName;

    public StatusData(Status status)
    {
        playerPos = new SerializedVector3(status.player.transform.position.x, status.player.transform.position.y, status.player.transform.position.z);
        sceneBuildName = status.sceneManager.sceneData.builtName;
    }
}

[System.Serializable]
public class Savable
{
    public string savableID;

    public Savable(string _savableID)
    {
        savableID = _savableID;
    }
}

[System.Serializable]
public class SavableDialogueActivator
{
    public string savableID;
    public string dialogueID;

    public SavableDialogueActivator(string _savableID, string _dialogueID)
    {
        savableID = _savableID;
        dialogueID = _dialogueID;
    }
}

[System.Serializable]
public class SerializableSavableEntity
{
    public string savableID;

    public SerializableSavableEntity(string _savableID)
    {
        savableID = _savableID;
    }
}

[System.Serializable]
public class SerializableEnemySpawner
{
    public string savableID;

    public SerializableEnemySpawner(string _savableID)
    {
        savableID = _savableID;
    }
}

[System.Serializable]
public class SerializedVector3
{
    public float x;
    public float y;
    public float z;

    public SerializedVector3(float _x, float _y, float _z)
    {
        this.x = _x;
        this.y = _y;
        this.z = _z;
    }
}
