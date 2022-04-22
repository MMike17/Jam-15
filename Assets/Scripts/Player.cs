using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance != null) return instance;
            // not assigned yet
            instance = FindObjectOfType<Player>();
            if (instance == null) instance = new GameObject("Player", typeof(Player))
                .GetComponent<Player>();
            instance.Init();
            return instance;
        }
    }

    private Rigidbody rb;
    private Collider coll;
    [SerializeField] private float accelerationFactor = 20.0f;
    [SerializeField] private float jumpForce = 50.0f;
    
    private void Init()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    public void Move(Vector3 movementVector, Vector2 rotationVector)
    {
        var jump = movementVector.y;
        movementVector.y = 0;
        rb.AddForce(movementVector * (accelerationFactor * Time.deltaTime), ForceMode.Impulse); // horizontal movement
        rb.AddForce(Vector3.up * (jump * jumpForce * Time.deltaTime), ForceMode.Impulse); // jump
        transform.Rotate(Vector3.up, rotationVector.x);
    }
}