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

        var index = inputHandler.interactable.interactionIndex;
        clipDuration = status.player.animations.interactionsClipsDurations[index];
        status.player.animations.animator.Play
            (status.player.animations.INTERACT[index]);
        inputHandler.stateMachine.shouldChange = false;
    }

    public void OnExit()
    {
        inputHandler.hasPressedJumpButton = false;
        inputHandler.isFalling = false;
        inputHandler.jumpCount = 0;
        inputHandler.hasPressedMeleeAttackButton = false;

        inputHandler.interactable.GiveItem();

        inputHandler.interactable = null;

        if (status.isSafeZone)
        {
            status.player.SetUnarmedConfiguration();
        }

        else
        {
            status.player.SetSwordAndShieldConfiguration();
        }
    }

    public void Tick()
    {

        inputHandler.FaceTarget(inputHandler.interactable.transform);

        if (Time.time > time + (clipDuration * inputHandler.interactable.repetitions))
        {
            inputHandler.isInteracting = false;
        }
    }

}
