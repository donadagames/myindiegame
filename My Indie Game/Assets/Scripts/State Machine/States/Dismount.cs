public class Dismount : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    //float time;

    public Dismount(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        //time = Time.time;

        status.player = status.lili;

        status.mount = null;
        status.mountTransform = null;
        status.input.isMounting = false;

        if (status.isSafeZone == false)
        {
            status.player.SetSwordAndShieldConfiguration();
        }
        else 
        {
            status.player.SetUnarmedConfiguration();

        }

        status.uiController.SetCameraTarget(status.player.transform);
        //status.player.animations.animator.Play("Mounting");
        //status.player.transform.SetParent(null);
    }


    public void OnExit()
    {
        inputHandler.hasPressedJumpButton = false;
        inputHandler.jumpCount = 0;
        inputHandler.canMeleeAttack = true;
        inputHandler.hasPressedMeleeAttackButton = false;
        inputHandler.hasPressedMagicAttackButton = false;
        inputHandler.canMagicAttack = true;
    }

    public void Tick()
    {
        inputHandler.ApplyAllMovement();
    }
}
