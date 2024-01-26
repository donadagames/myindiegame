using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class SCRIPTFORINSTAGRAMVIDEOS : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private Vector3 direction = new Vector3();
    public Transform _player;
    public float moveSpeed = 3;
    public float rotationAngle = 45;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
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
        _player.Translate(direction * Time.deltaTime * moveSpeed);

        _player.Rotate(0, Time.deltaTime * rotationAngle * direction.x, 0);

    }

    private Vector3 GetDirectionInput()
    {
        Vector2 input = playerInputActions.Player.
            Move.ReadValue<Vector2>();

        return new Vector3(input.x, 0, input.y);
    }
}
