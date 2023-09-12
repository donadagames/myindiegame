public class GetHit : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    public GetHit(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}
