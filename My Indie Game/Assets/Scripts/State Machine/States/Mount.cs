using UnityEngine;

public class Mount : IState
{
    private readonly Status status;
    private readonly InputHandler inputHandler;

    float time;

    public Mount(Status _status, InputHandler _inputHandler)
    {
        status = _status;
        inputHandler = _inputHandler;
    }

    public void OnEnter()
    {
        time = Time.time;
        status.uiController.SetCameraTarget(status.mount.transform);
        status.player.animations.animator.Play("Mounting");
        status.player.transform.SetParent(status.mountTransform);

        status.player.transform.localEulerAngles = new Vector3(0,0,0);
        status.player.transform.LeanMoveLocal(new Vector3(0, 0, 0), .75f);
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
        if (Time.time >= time + 1.25f)
        {
            status.player = status.mount;
            inputHandler.isMounting = false;
        }
    }
}
