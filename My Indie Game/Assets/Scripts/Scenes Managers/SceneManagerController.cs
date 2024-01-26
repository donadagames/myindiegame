using System.Collections.Generic;
using UnityEngine;

public class SceneManagerController : MonoBehaviour
{
    Status status;
    public SceneData sceneData;
    public Interactable[] iSavables;
    public Dictionary<string, Interactable> savablesID = new Dictionary<string, Interactable>();
    public Interactable[] iSavablesDialogueActivators;
    public Dictionary<string, Interactable> savablesDialogueActivatorsID = new Dictionary<string, Interactable>();
    public SavableEntity[] savableEntities;
    public Dictionary<string, SavableEntity> savableEntitiesID = new Dictionary<string, SavableEntity>();
    public EnemySpawner[] enemySpawners;
    public Dictionary<string, EnemySpawner> enemySpawnersID = new Dictionary<string, EnemySpawner>();

    private void Awake()
    {
        PopulateInteractablesIDDict();
    }

    private void Start()
    {
        status = Status.instance;
        status.sceneManager = this;
    }

    private void PopulateInteractablesIDDict()
    {
        foreach (Interactable savable in iSavables)
        {
            if (!savablesID.ContainsValue(savable))
            {
                savablesID.Add(savable.saveableEntityId, savable);
            }
        }

        foreach (Interactable savable in iSavablesDialogueActivators)
        {
            if (!savablesDialogueActivatorsID.ContainsValue(savable))
            {
                savablesDialogueActivatorsID.Add(savable.saveableEntityId, savable);
            }
        }

        foreach (SavableEntity savable in savableEntities)
        {
            if (!savableEntitiesID.ContainsValue(savable))
            {
                savableEntitiesID.Add(savable.saveableEntityId, savable);
            }
        }

        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            if (!enemySpawnersID.ContainsValue(enemySpawner))
            {
                enemySpawnersID.Add(enemySpawner.saveableEntityId, enemySpawner);
            }
        }
    }
}
