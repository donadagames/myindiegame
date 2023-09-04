using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private Status status;
    private Vector3 direction = new Vector3();

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    private void Start()
    {
        status = Status.instance;
    }

    private void Update()
    {
        direction = GetDirectionInput();
        Move();
    }

    #region Left Stick
    private void Move()
    {
        status.player.transform.Translate(direction *
            Time.deltaTime *
            status.player.moveSpeed);

        status.player.transform.Rotate(0, Time.deltaTime *
            status.player.rotationAngle *
            direction.x, 0);
    }

    private Vector3 GetDirectionInput()
    {
        Vector2 input = playerInputActions.Player.
            Move.ReadValue<Vector2>();

        status.uiController.SetPositionFocusUI(GetDirection(input));

        return new Vector3(input.x, 0, input.y);
    }

    private Direction GetDirection(Vector2 input)
    {
        if (input.x > 0)
        {
            if (input.y > 0) return Direction.UpRight;
            else return Direction.DownLeft;
        }
        else if (input.x < 0)
        {
            if (input.y > 0) return Direction.UpLeft;
            else return Direction.DownRight;
        }
        else return Direction.Default;
    }
    #endregion


    #region Buttons

    public void Jump()
    {
        status.player.rb.AddForce(Vector3.up * 
            status.player.jumpForce, ForceMode.Impulse);
    }

    #endregion
}


