
using UnityEngine;

public class MeleeAttack : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    private float meleeAttackTime;
    private float nextAttackTime;

    public MeleeAttack(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        

        inputHandler.canMeleeAttack = false;
        status.player.sword.shouldCheck = true;
        inputHandler.hasPressedMeleeAttackButton = false;
        meleeAttackTime = Time.time;

        if (status.uiController.aim != null)
            status.uiController.aim.m_Damping = status.dampingTime;

        nextAttackTime = inputHandler.MeleeAttack();
    }

    public void OnExit()
    {
        inputHandler.SetDefaultConfigurations();
        status.player.sword.shouldCheck = false;
        status.uiController.aim.m_Damping = 999999999f;
    }

    public void Tick()
    {
        inputHandler.GetDirection();
        inputHandler.ApplyGravity();

        if (Time.time > meleeAttackTime + nextAttackTime)
        {
            inputHandler.canMeleeAttack = true;
        }
    }
}
