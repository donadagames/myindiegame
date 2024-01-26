using UnityEngine;
using UnityEngine.InputSystem;

public class QuestGiver : DialogueActivator
{
    public Quest quest;
    Animator animator;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public virtual void GiveQuest()
    {
        status.quests.AddQuest(quest);
        UpdateDialogueData(quest.waitingCompletionDialogue);
    }

    public void CheckIfQuestIsComplete()
    {
        if (quest.isCompleted) return;

        if (status.quests.QuestIsCompleted(quest))
        {
            status.quests.CompleteQuest(quest);
            UpdateDialogueData(quest.onCompleteDialogue);
        }
    }

    public void GiveReward()
    {
        inventory.AddItem(quest.rewardItem, quest.rewardQuantity);

        UpdateDialogueData(quest.afterCompletedDialogue);
    }

    public override void Interact()
    {
        CheckIfQuestIsComplete();
        base.Interact();
    }

    public void RemoveQuestObjectiveItem()
    {
        inventory.RemoveItem(quest.objective.item, quest.objective.completeQuantity);
    }

    public void IncreaseParameter(string parameter)
    {
        animator.SetInteger(parameter, (animator.GetInteger(parameter) + 1));
    }

    public void SetParameterToZero(string parameter)
    {
        animator.SetInteger(parameter, 0);
    }
}
