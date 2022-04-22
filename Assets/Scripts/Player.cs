using System;
using UnityEngine;

[SelectionBase]
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
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float maxVelocity, maxTorque;
    
    private void Init()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    public void Move(Vector3 movementVector, Vector2 rotationVector)
    {
        var jump = movementVector.y;
        movementVector.y = 0;
        rb.AddRelativeForce(movementVector * (accelerationFactor * Time.deltaTime), ForceMode.Impulse); // horizontal movement
        rb.AddForce(Vector3.up * (jump * jumpForce * Time.deltaTime), ForceMode.Impulse); // jump
        rb.AddRelativeTorque(Vector3.up * rotationVector.x * rotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rb.velocity = ClampV3(rb.velocity, maxVelocity);
        rb.angularVelocity = ClampV3(rb.angularVelocity, maxTorque);
    }

    private Vector3 ClampV3(Vector3 vector, float max)
    {
        vector.x = Mathf.Clamp(vector.x, -max, max);
        vector.y = Mathf.Clamp(vector.y, -max, max);
        vector.z = Mathf.Clamp(vector.z, -max, max);

        return vector;
    }
}