using UnityEngine;

public class PushInteractable : Interactable
{
    public Sprite downUpSprite;
    public Sprite sidesSprite;

    private bool PlayerIsFacingDown() =>
        status.player.transform.position.z > transform.position.z &&
        status.player.transform.position.x >= transform.position.x - 1 &&
        status.player.transform.position.x <= transform.position.x + 1;

    private bool PlayerIsFacingUp() =>
        status.player.transform.position.z < transform.position.z &&
        status.player.transform.position.x >= transform.position.x - 1 &&
        status.player.transform.position.x <= transform.position.x + 1;

    private bool PlayerIsFacingLeft() =>
        status.player.transform.position.x > transform.position.x &&
        status.player.transform.position.z >= transform.position.z - 1 &&
        status.player.transform.position.z <= transform.position.z + 1;

    private bool PlayerIsFacingRight() =>
        status.player.transform.position.x < transform.position.x &&
        status.player.transform.position.z >= transform.position.z - 1 &&
        status.player.transform.position.z <= transform.position.z + 1;

    private Vector3 GetObjetcSide()
    {
        if (PlayerIsFacingDown())
        {
            icon = downUpSprite;
            return new Vector3(0, 0, -1);
        }
        else if (PlayerIsFacingUp())
        {
            icon = downUpSprite;
            return new Vector3(0, 0, 1);
        }
        else if (PlayerIsFacingLeft())
        {
            icon = sidesSprite;
            return new Vector3(-1, 0, 0);
        }
        else if (PlayerIsFacingRight())
        {
            icon = sidesSprite;
            return new Vector3(1, 0, 0);
        }

        return Vector3.zero;
    }

    public override void OnEnter()
    {
        if (hasInteract) return;
        side = GetObjetcSide();
        uiController.SetInteractionSprite(icon);

    }

    public override void OnExit()
    {
        uiController.SetDefaultInteractionSprite();
    }

    public override void Interact()
    {
        if (hasInteract) return;

        hasInteract = true;
        transform.SetParent(status.player.transform);
        status.input.isPushing = true;
        uiController.SetDefaultInteractionSprite();
        status.player.soundController.PlayClip(clip);
    }
}

public enum InteractableSide
{
    right, left, up, down
}
