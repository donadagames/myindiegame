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

        status.player.animations.PlayAnimation(status.player.animations.IDLE,
    status.isSafeZone);

        inputHandler.input = new UnityEngine.Vector2(0, 0);
        inputHandler.stateMachine.shouldChange = false;
    }

    public void OnExit()
    {
        inputHandler.hasPressedJumpButton = false;
        inputHandler.isFalling = false;
        inputHandler.jumpCount = 0;
        inputHandler.hasPressedMeleeAttackButton = false;
        inputHandler.hasPressedMagicAttackButton = false;
    }

    public void Tick()
    {
        return;
    }
}
