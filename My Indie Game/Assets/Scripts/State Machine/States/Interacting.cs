using UnityEngine;

public class Interacting : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;
    private float clipDuration;
    private float time;

    public Interacting(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        time = Time.time;
        status.player.sword.shouldCheck = false;
        inputHandler.isFalling = false;
        inputHandler.jumpCount = 0;
        inputHandler.canMount = false;
        inputHandler.canMeleeAttack = false;
        inputHandler.canMagicAttack = false;
        var index = inputHandler.interactable.interactionIndex;
        clipDuration = status.player.animations.interactionsClipsDurations[index];
        status.player.animations.animator.Play(status.player.animations.INTERACT[index]);
        inputHandler.stateMachine.shouldChange = false;
    }

    public void OnExit()
    {
        inputHandler.hasPressedJumpButton = false;
        inputHandler.isFalling = false;
        inputHandler.jumpCount = 0;
        inputHandler.hasPressedMeleeAttackButton = false;
        inputHandler.hasPressedMagicAttackButton = false;

        inputHandler.isInteracting = false;

        inputHandler.canMount = true;
        inputHandler.canMeleeAttack = true;
        inputHandler.canMagicAttack = true;

        if (inputHandler.interactable.item != null)
            inputHandler.interactable.GiveItem();

        inputHandler.interactable = null;

        if (status.inventory.swordAndShield[0].quantity < 1 || status.inventory.swordAndShield[1].quantity < 1)
        {
            if (status.inventory.swordAndShield[0].quantity < 1 && status.inventory.swordAndShield[1].quantity < 1)
                status.player.SetUnequipedConfiguration();
            else if (status.inventory.swordAndShield[1].quantity >= 1 && status.inventory.swordAndShield[0].quantity < 1)
                status.player.SetOnlyShieldConfiguration();
            else
                status.player.SetOnlySwordConfiguration();

            return;
        }

        if (status.isSafeZone || inputHandler.isCarrying)
        {
            status.player.SetUnarmedConfiguration();
        }
        else
            status.player.SetSwordAndShieldConfiguration();
    }

    public void Tick()
    {
        if (!inputHandler.isCarrying)
            inputHandler.FaceTarget(inputHandler.interactable.transform);

        if (Time.time > time + (clipDuration * inputHandler.interactable.repetitions))
            inputHandler.isInteracting = false;
    }
}
