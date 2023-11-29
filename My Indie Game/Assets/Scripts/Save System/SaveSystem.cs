using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SaveSystem : MonoBehaviour
{
    #region INVENTORY AND ITENS
    public Item[] allItens;
    [SerializeField] private List<ActionBarSlot> actionBarSlots = new List<ActionBarSlot>();
    public Dictionary<string, Item> itensID = new Dictionary<string, Item>();
    public Dictionary<string, Quest> questsID = new Dictionary<string, Quest>();

    #endregion

    #region QUESTS AND QUESTOBJECTIVES
    [SerializeField] private Quest[] allQuests;
    [SerializeField] private QuestObjective[] allQuestsObjectives;
    #endregion

    #region PLAYER STATUS
    private Status status;
    #endregion

    private UIController uiController;
    public string path => $"{Application.persistentDataPath}/data.txt";

    #region SINGLETON
    public static SaveSystem instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        PopulateItensIDDict();
        PopulateQuestsIDDict();
    }
    #endregion

    private void Start()
    {
        status = Status.instance;
        uiController = UIController.instance;
        ResetAllItens();
        ResetAllQuests();
        ResetAllQuestsObjetives();
    }

    #region INVENTORY METHODS
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

    private void CaptureQuestsData(Dictionary<string, object> state)

    {
        state[status.quests.saveableEntityId] = status.quests.CaptureState();
    }

    private void RestoreQuestsData(Dictionary<string, object> state)
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
        state[status.saveableEntityId] = status.CaptureState();
    }

    private void RestorePlayerStatusData(Dictionary<string, object> state)
    {
        if (state.TryGetValue(status.saveableEntityId, out object value))
        {
            status.RestoreState(value);
        }
    }
    #endregion

    #region CAPTURE AND RESTORE METHODS
    private void CaptureState(Dictionary<string, object> state)
    {
        CaptureInventoryData(state);
        CapturePlayerStatusData(state);
        CaptureQuestsData(state);
    }


    private void RestoreState(Dictionary<string, object> state)
    {
        RestoreQuestsData(state);
        RestoreInventoryData(state);
        RestorePlayerStatusData(state);
    }
    #endregion

    #region SAVE METHODS
    private void SaveFile(object state)
    {
        using (var stream = File.Open(path, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }
    private Dictionary<string, object> LoadFile()
    {
        if (!File.Exists(path))
        {
            ResetAllItens();
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(path, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    [ContextMenu("Save")]
    public void Save()
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

        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
    }
    [ContextMenu("Load")]
    public void Load()
    {
        var state = LoadFile();
        RestoreState(state);
    }

    [ContextMenu("Delete")]
    public void DeleteSavedeFile()
    {
        if (File.Exists(path))
        {
            ResetAllQuestsObjetives();
            ResetAllItens();
            ResetAllQuests();
            File.Delete(path);
        }
    }
    #endregion

}

public interface ISaveable
{
    //SAVE GAME
    object CaptureState();

    //LOAD GAME
    void RestoreState(object state);
}
