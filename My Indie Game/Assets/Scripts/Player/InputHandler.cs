using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] float gravityMultiplier = 1;
    [SerializeField] float smoothTime = .05f;
    private Status status;
    private Vector3 direction = new Vector3();
    private Vector2 input = new Vector2();
    private float velocity;
    private const float GRAVITY = -9.8f;
    private float currentVelocity;

    private bool IsGrounded() => status.player.characterController.isGrounded;

    private void Start()
    {
        status = Status.instance;
    }

    private void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
    }

    #region Left Stick
    private void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        status.uiController.
            SetPositionFocusUI(GetJoystickDirection(input));

        direction = new Vector3(input.x, 0, input.y);
    }

    private void ApplyMovement()
    {
        status.player.characterController.Move(direction *
            status.player.moveSpeed *
            Time.deltaTime);
    }

    private void ApplyRotation()
    {
        if (input.sqrMagnitude == 0) return;
        var tangetAngle = Mathf.Atan2(direction.x,
            direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(status.player.transform.
            eulerAngles.y, tangetAngle, ref currentVelocity,
            smoothTime);
        status.player.transform.rotation =
            Quaternion.Euler(0, angle, 0);
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && velocity < 0.0f)
        {
            velocity = -1;
        }

        else
        {
            velocity += GRAVITY * gravityMultiplier * Time.deltaTime;
        }

        direction.y = velocity;
    }

    private Direction GetJoystickDirection(Vector2 input)
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
        if (!IsGrounded()) return;
        velocity = Mathf.Sqrt(status.player.jumpHight * -2 * GRAVITY);
    }

    #endregion
}


