using UnityEngine;

public class MagicAttack : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;
    private float magicAttackTime;
    private float nextAttackTime;

    public MagicAttack(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        inputHandler.canMagicAttack = false;
        status.player.sword.shouldCheck = false;
        inputHandler.hasPressedMagicAttackButton = false;
        nextAttackTime = inputHandler.MagicAttack();
        magicAttackTime = Time.time;
        status.ConsumeEnergy(inputHandler.selectedSkill.energyCost);
    }

    public void OnExit()
    {
        status.player.sword.shouldCheck = true;
        inputHandler.SetDefaultConfigurations();
    }

    public void Tick()
    {
        if (Time.time > magicAttackTime + nextAttackTime)
        {
            inputHandler.canMagicAttack = true;
        }
    }
}
