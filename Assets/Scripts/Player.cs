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
    [SerializeField] private float movementSpeed = 5.0f;
    
    private void Init()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    public void Move(Vector3 movementVector)
    {
        rb.AddForce(movementVector * movementSpeed * Time.deltaTime, ForceMode.Acceleration);
    }
}