using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public Quest[] allQuests;
    public QuestObjective[] allQuestsObjectives;
    public Item[] allItens;

    private void Start()
    {
        ResetAllItens();
        ResetAllQuests();
        ResetAllQuestsObjetives();
    }

    private void ResetAllQuests()
    { 
        for(int i = 0; i<allQuests.Length; i++)
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

    private void ResetAllItens()
    {
        for (int i = 0; i < allItens.Length; i++)
        {
            allItens[i].ResetItem();
        }
    }
}
