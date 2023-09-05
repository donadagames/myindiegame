using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 3f;

    public float jumpHight = 5f;

    public CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }


}
