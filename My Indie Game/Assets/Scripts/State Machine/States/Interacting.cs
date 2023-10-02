using System.Collections;
using System.Collections.Generic;
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
        if (Time.time > time + (clipDuration * 3))
        {
            inputHandler.isInteracting = false;
        }
    }
}
