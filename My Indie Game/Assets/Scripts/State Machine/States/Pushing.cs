using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushing : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;
    private float clipDuration;
    private float time;
    public Pushing(Status _status, InputHandler _inputHandler)
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
        inputHandler.GetDirectionInputForPush(inputHandler.interactable.side);
        status.player.SetUnarmedConfiguration();
        inputHandler.canMount = false;
        clipDuration = (1.667f*2);
        status.player.animations.animator.Play("Push");
        inputHandler.stateMachine.shouldChange = false;
    }

    public void OnExit()
    {
        inputHandler.interactable.hasInteract = false;
        inputHandler.interactable.transform.SetParent(null);
        inputHandler.hasPressedJumpButton = false;
        inputHandler.isFalling = false;
        inputHandler.jumpCount = 0;
        inputHandler.hasPressedMeleeAttackButton = false;
        inputHandler.hasPressedMagicAttackButton = false;
        inputHandler.interactable = null;
        inputHandler.canMount = true;



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
        inputHandler.ApplyAllMovementForPushing();
        inputHandler.SearchForEnemySpawner();

        if (Time.time > time + (clipDuration * inputHandler.interactable.repetitions))
        {
            inputHandler.isPushing = false;
        }
    }
}
