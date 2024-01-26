using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private UIController uiController;

    #region INVENTORY AND ITENS
    public Item[] allItens;
    public Dialogue[] allDialogues;
    [SerializeField] private List<ActionBarSlot> actionBarSlots = new List<ActionBarSlot>();
    public Dictionary<string, Item> itensID = new Dictionary<string, Item>();
    public Dictionary<string, Quest> questsID = new Dictionary<string, Quest>();
    public Dictionary<string, Dialogue> dialoguesID = new Dictionary<string, Dialogue>();
    #endregion

    #region QUESTS AND QUESTOBJECTIVES
    [SerializeField] private Quest[] allQuests;
    [SerializeField] private QuestObjective[] allQuestsObjectives;
    #endregion

    #region PLAYER STATUS
    private Status status;
    #endregion

    #region PATHS
    public string statusPath => $"{Application.persistentDataPath}/statusData.txt";
    public string inventoryPath => $"{Application.persistentDataPath}/inventoryData.txt";
    public string interactablesPath => $"{Application.persistentDataPath}/interactablesData.txt";
    public string questsPath => $"{Application.persistentDataPath}/questsData.txt";
    public string dialoguesPath => $"{Application.persistentDataPath}/dialogueData.txt";
    public string entitiesPath => $"{Application.persistentDataPath}/entitiesData.txt";
    public string enemiesPath => $"{Application.persistentDataPath}/enemiesData.txt";
    #endregion

    #region SINGLETON
    public static SaveSystem instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        PopulateItensIDDict();
        PopulateQuestsIDDict();
        PopulateDialoguesIDDict();
        PopulateScenesIDDict();
    }
    #endregion

    #region SCENES
    public SceneData[] allScenes;
    public Dictionary<string, SceneData> scenesBuildNames = new Dictionary<string, SceneData>();
    #endregion
    private void Start()
    {
        status = Status.instance;
        uiController = UIController.instance;
        ResetAllItens();
        ResetAllQuests();
        ResetAllQuestsObjetives();
        //StartCoroutine(OnStartCoroutine());

    }

    private IEnumerator OnStartCoroutine()
    {
        yield return new WaitForSeconds(.25f);

        RestoreAllStates();
    }

    #region INVENTORY METHODS

    #region SAVE INVENTORY METHODS
    private void SaveInventoryFile(object state)
    {
        using (var stream = File.Open(inventoryPath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }
    private Dictionary<string, object> LoadInventoryFile()
    {
        if (!File.Exists(inventoryPath))
        {
            //ResetAllItens();
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(inventoryPath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    [ContextMenu("SaveInventory")]
    public void SaveInventory()
    {
        var state = LoadInventoryFile();
        CaptureInventoryData(state);
        SaveInventoryFile(state);
    }
    [ContextMenu("LoadInventory")]
    public void LoadInventory()
    {
        var state = LoadInventoryFile();
        if (state.Count <= 0) return;
        RestoreInventoryData(state);
    }

    [ContextMenu("Delete Inventory Saved File")]
    public void DeleteSavedInventoryFile()
    {
        if (File.Exists(inventoryPath))
        {
            ResetAllItens();
            File.Delete(inventoryPath);
        }
    }
    #endregion

    private void PopulateItensIDDict()
    {
        foreach (Item item in allItens)
        {
            if (!itensID.ContainsValue(item))
            {
                itensID.Add(item.saveableEntityId, item);
            }
        }
    }

    private void ResetAllItens()
    {
        for (int i = 0; i < allItens.Length; i++)
        {
            allItens[i].ResetItem();
        }
    }

    public Item GetItemFromID(string id)
    {
        if (itensID.TryGetValue(id, out Item item))
            return item;
        return null;
    }

    private void CaptureInventoryData(Dictionary<string, object> state)
    {
        foreach (var slot in actionBarSlots)
        {
            state[slot.saveableEntityId] = slot.CaptureState();
        }

        state[status.inventory.saveableEntityId] = status.inventory.CaptureState();
    }

    private void RestoreInventoryData(Dictionary<string, object> state)
    {
        if (state.TryGetValue(status.inventory.saveableEntityId, out object _value))
        {
            status.inventory.RestoreState(_value);
        }

        foreach (var slot in actionBarSlots)
        {
            if (state.TryGetValue(slot.saveableEntityId, out object value))
            {
                slot.RestoreState(value, status.inventory);
            }
        }

    }
    #endregion

    #region QUESTS METHODS
    private void PopulateQuestsIDDict()
    {
        foreach (Quest quest in allQuests)
        {
            if (!questsID.ContainsValue(quest))
            {
                questsID.Add(quest.saveableEntityId, quest);
            }
        }
    }
    private void ResetAllQuests()
    {
        for (int i = 0; i < allQuests.Length; i++)
        {
            allQuests[i].ResetQuest();
        }
    }
    private void ResetAllQuestsObjetives()
    {
        for (int i = 0; i < allQuestsObjectives.Length; i++)
        {
            allQuestsObjectives[i].ResetQuestObjective();
        }
    }

    #region SAVE QUESTS METHODS
    private void SaveQuestsFile(object state)
    {
        using (var stream = File.Open(questsPath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }
    private Dictionary<string, object> LoadQuestsFile()
    {
        if (!File.Exists(questsPath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(questsPath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    [ContextMenu("Save Quests")]
    public void SaveQuests()
    {
        var state = LoadQuestsFile();
        CaptureQuestsState(state);
        SaveQuestsFile(state);
    }
    [ContextMenu("Load Quests")]
    public void LoadQuests()
    {
        var state = LoadQuestsFile();
        if (state.Count <= 0) return;
        RestoreQuestsState(state);
    }

    [ContextMenu("Delete Quests")]
    public void DeleteQuestsFile()
    {
        if (File.Exists(questsPath))
        {
            File.Delete(questsPath);
        }
    }
    #endregion

    private void CaptureQuestsState(Dictionary<string, object> state)
    {
        state[status.quests.saveableEntityId] = status.quests.CaptureState();
    }

    private void RestoreQuestsState(Dictionary<string, object> state)
    {
        if (state.TryGetValue(status.quests.saveableEntityId, out object value))
        {
            status.quests.RestoreState(value);
        }
    }
    #endregion

    #region STATUS METHODS
    private void CapturePlayerStatusData(Dictionary<string, object> state)
    {
        state[status.saveableEntityId] = status.CapturePlayerStatusState();
    }

    private void RestorePlayerStatusData(Dictionary<string, object> state)
    {
        if (state.TryGetValue(status.saveableEntityId, out object value))
        {
            status.RestorePlayerStatusState(value);
        }
    }

    #region SAVE STATUS METHODS

    public bool HasSavedStatus() => File.Exists(statusPath);

    private void SaveStatusFile(object state)
    {
        using (var stream = File.Open(statusPath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }
    private Dictionary<string, object> LoadStatusFile()
    {
        if (!File.Exists(statusPath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(statusPath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    [ContextMenu("Save Status")]
    public void SaveStatus()
    {
        var state = LoadStatusFile();
        CapturePlayerStatusData(state);
        SaveStatusFile(state);
    }
    [ContextMenu("Load Status")]
    public void LoadStatus()
    {
        var state = LoadStatusFile();
        RestorePlayerStatusData(state);
    }

    [ContextMenu("Delete Status")]
    public void DeleteStatusFile()
    {
        if (File.Exists(statusPath))
        {
            ResetAllQuestsObjetives();
            ResetAllItens();
            ResetAllQuests();
            File.Delete(statusPath);
        }
    }
    #endregion

    #endregion

    #region DIALOGUE METHODS
    private void PopulateDialoguesIDDict()
    {
        foreach (Dialogue dialogue in allDialogues)
        {
            if (!dialoguesID.ContainsValue(dialogue))
            {
                dialoguesID.Add(dialogue.dialogueID, dialogue);
            }
        }
    }

    public Dialogue GetDialogueFromID(string id)
    {
        if (dialoguesID.TryGetValue(id, out Dialogue dialogue))
            return dialogue;
        return null;
    }

    #region SAVE DIALOGUES METHODS
    private void SaveDialoguesFile(object state)
    {
        using (var stream = File.Open(dialoguesPath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }
    private Dictionary<string, object> LoadDialoguesFile()
    {
        if (!File.Exists(dialoguesPath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(dialoguesPath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    [ContextMenu("Save Dialogues")]
    public void SaveDialogues()
    {
        var state = LoadDialoguesFile();
        CaptureDialoguesState(state);
        SaveDialoguesFile(state);
    }
    [ContextMenu("Load Dialogues")]
    public void LoadDialogues()
    {
        var state = LoadDialoguesFile();
        if (state.Count <= 0) return;
        RestoreDialoguesState(state);
    }

    [ContextMenu("Delete Dialogues")]
    public void DeleteSavedDialoguesFile()
    {
        if (File.Exists(dialoguesPath))
        {
            File.Delete(dialoguesPath);
        }
    }
    #endregion

    private void CaptureDialoguesState(Dictionary<string, object> state)
    {
        state[status.saveableEntityId] = status.CaptureDialogueActivatorsState();
    }

    private void RestoreDialoguesState(Dictionary<string, object> state)
    {
        if (state.TryGetValue(status.saveableEntityId, out object value))
        {
            status.RestoreDialogueActivatorsState(value);
        }
    }

    #endregion

    #region INTERACTABLES METHODS

    #region SAVE INTERACTABLES METHODS
    private void SaveInteractablesFile(object state)
    {
        using (var stream = File.Open(interactablesPath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }
    private Dictionary<string, object> LoadInteractablesFile()
    {
        if (!File.Exists(interactablesPath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(interactablesPath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    [ContextMenu("Save Interactables")]
    public void SaveInteractables()
    {
        var state = LoadInteractablesFile();
        CaptureInteractablesState(state);
        SaveInteractablesFile(state);
    }
    [ContextMenu("Load Interactables")]
    public void LoadInteractables()
    {
        var state = LoadInteractablesFile();
        if (state.Count <= 0) return;
        RestoreInteractablesState(state);
    }

    [ContextMenu("Delete Interactables")]
    public void DeleteSavedInteractablesFile()
    {
        if (File.Exists(interactablesPath))
        {
            File.Delete(interactablesPath);
        }
    }
    #endregion

    private void CaptureInteractablesState(Dictionary<string, object> state)
    {
        state[status.saveableEntityId] = status.CaptureInteractablesState();
    }

    private void RestoreInteractablesState(Dictionary<string, object> state)
    {
        if (state.TryGetValue(status.saveableEntityId, out object value))
        {
            status.RestoreInteractablesState(value);
        }
    }
    #endregion

    #region SAVABLES ENTITIES
    #region SAVE SAVABLES ENTITIES METHODS
    private void SaveEntitiesFile(object state)
    {
        using (var stream = File.Open(entitiesPath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }
    private Dictionary<string, object> LoadEntitiesFile()
    {
        if (!File.Exists(entitiesPath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(entitiesPath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    [ContextMenu("Save Entities")]
    public void SaveEntities()
    {
        var state = LoadEntitiesFile();
        CaptureEntitiesState(state);
        SaveEntitiesFile(state);
    }
    [ContextMenu("Load Entities")]
    public void LoadEntities()
    {
        var state = LoadEntitiesFile();
        if (state.Count <= 0) return;
        RestoreEntitiesState(state);
    }

    [ContextMenu("Delete Entities")]
    public void DeleteEntitiesFile()
    {
        if (File.Exists(entitiesPath))
        {
            File.Delete(entitiesPath);
        }
    }
    #endregion

    private void CaptureEntitiesState(Dictionary<string, object> state)
    {
        state[status.saveableEntityId] = status.CaptureSavableEntitiesState();
    }

    private void RestoreEntitiesState(Dictionary<string, object> state)
    {
        if (state.TryGetValue(status.saveableEntityId, out object value))
        {
            status.RestoreSavableEntitiesState(value);
        }
    }
    #endregion

    #region SCENES

    private void PopulateScenesIDDict()
    {
        foreach (SceneData scene in allScenes)
        {
            if (!scenesBuildNames.ContainsValue(scene))
            {
                scenesBuildNames.Add(scene.builtName, scene);
            }
        }
    }

    public SceneData GetSceneDataFromBuildName(string buildName)
    {
        if (scenesBuildNames.TryGetValue(buildName, out SceneData scenedata))
            return scenedata;
        return null;
    }
    #endregion

    #region ENEMIES SPAWNERS METHODS
    #region SAVE ENEMIES SPAWNERS METHODS
    private void SaveEnemiesFile(object state)
    {
        using (var stream = File.Open(enemiesPath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }
    private Dictionary<string, object> LoadEnemiesFile()
    {
        if (!File.Exists(enemiesPath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(enemiesPath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    [ContextMenu("Save Enemies")]
    public void SaveEnemies()
    {
        var state = LoadEnemiesFile();
        CaptureEnemiesState(state);
        SaveEnemiesFile(state);
    }
    [ContextMenu("Load Enemies")]
    public void LoadEnemies()
    {
        var state = LoadEnemiesFile();
        if (state.Count <= 0) return;
        RestoreEnemiesState(state);
    }

    [ContextMenu("Delete Enemies")]
    public void DeleteEnemiesFile()
    {
        if (File.Exists(enemiesPath))
        {
            File.Delete(enemiesPath);
        }
    }
    #endregion

    private void CaptureEnemiesState(Dictionary<string, object> state)
    {
        state[status.saveableEntityId] = status.CaptureEnemiesState();
    }

    private void RestoreEnemiesState(Dictionary<string, object> state)
    {
        if (state.TryGetValue(status.saveableEntityId, out object value))
        {
            status.RestoreEnemiesState(value);
        }
    }
    #endregion

    public void DisplaySaveText()
    {
        var text = string.Empty;

        if (status.inventory.inventoryUI.settingsController.languageController.GetGlobalLanguage() == Language.Portuguese)
        {
            text = "O jogo foi salvo!";
        }
        else if (status.inventory.inventoryUI.settingsController.languageController.GetGlobalLanguage() == Language.English)
        {
            text = "Game was saved!";
        }
        else if (status.inventory.inventoryUI.settingsController.languageController.GetGlobalLanguage() == Language.Chinese)
        {
            text = "游戏已保存！";
        }

        uiController.DisplayInfoText(text, status.inventory.inventoryUI.settingsController.languageController);
    }

    #region CAPTURE ALL AND RESTORE ALL METHODS
    [ContextMenu("Save All")]
    public void CaptureAllStates()
    {
        SaveInventory();
        SaveQuests();
        SaveStatus();
        SaveInteractables();
        SaveEntities();
        SaveDialogues();
        SaveEnemies();
        DisplaySaveText();
    }

    [ContextMenu("Load All")]
    public void RestoreAllStates()
    {
        LoadInventory();
        LoadInteractables();
        LoadDialogues();
        LoadEntities();
        LoadQuests();
        LoadStatus();
        LoadEnemies();
    }

    [ContextMenu("Delete All")]
    public void DeleteAllFiles()
    {
        DeleteEntitiesFile();
        DeleteQuestsFile();
        DeleteSavedDialoguesFile();
        DeleteSavedInteractablesFile();
        DeleteStatusFile();
        DeleteSavedInventoryFile();
        DeleteEnemiesFile();
    }

    #endregion

}

public class SavableEntity : MonoBehaviour
{
    public string saveableEntityId;
    [ContextMenu("Generate ID")]
    private void GenereteID() => saveableEntityId = Guid.NewGuid().ToString();

    public bool shouldSave = false;


    public virtual void CaptureState()
    {
        return;
    }

    public virtual void RestoreState(SerializableSavableEntity savable)
    {
        return;
    }
}