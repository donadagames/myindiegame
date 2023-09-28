using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Interactable
{
    public override void Interact()
    {
        if (hasInteract) return;

        hasInteract = true;
        uiController.SetDefaultInteractionSprite();

        status.input.isInteracting = true;

        status.player.SetCutWoodConfiguration();
    }
}
