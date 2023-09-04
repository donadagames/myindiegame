using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationAngle = 60f;
    public Rigidbody rb;
    public float jumpForce = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
