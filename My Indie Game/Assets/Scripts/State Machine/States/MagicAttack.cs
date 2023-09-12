using UnityEngine;

public class MagicAttack : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;
    private float magicAttackTime;

    public MagicAttack(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        magicAttackTime = Time.time;
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}
