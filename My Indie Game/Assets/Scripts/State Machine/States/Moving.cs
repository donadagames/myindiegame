public class Moving : IState
{
    private readonly InputHandler inputHandler;
    private readonly Status status;
    public Moving(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }
    public void OnEnter()
    {
        status.player.animator.Play(Animation.
         Run_Unarmed.ToString());
    }

    public void OnExit()
    {
        return;
    }

    public void Tick()
    {
        inputHandler.GetInput();
        inputHandler.ApplyAllMovement();
    }
}
