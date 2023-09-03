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
        status = GetComponent<Status>();
    }

    private void Update()
    {
        direction = GetDirectionInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

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

        return new Vector3(input.x, 0, input.y);
    }
}
