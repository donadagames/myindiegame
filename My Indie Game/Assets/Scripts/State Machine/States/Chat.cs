using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;
    public Chat(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }
    public void OnEnter()
    {
        status.player.sword.shouldCheck = false;
        inputHandler.isFalling = false;
        inputHandler.jumpCount = 0;

        inputHandler.stateMachine.shouldChange = false;
    }

    public void OnExit()
    {
        inputHandler.hasPressedJumpButton = false;
        inputHandler.isFalling = false;
        inputHandler.jumpCount = 0;
        inputHandler.hasPressedMeleeAttackButton = false;
    }

    public void Tick()
    {
        return;
    }
}
