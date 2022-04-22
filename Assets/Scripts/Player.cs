using System;
using UnityEngine;

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
            if (instance == null) instance = new GameObject("Player", typeof(Player), typeof(Rigidbody), typeof(CapsuleCollider))
                .GetComponent<Player>();
            instance.Init();
            return instance;
        }
    }

    private Rigidbody rb;
    private CapsuleCollider coll;
    [SerializeField] private float movementSpeed = 5.0f;
    
    private void Init()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
    }

    public void Move(Vector3 movementVector)
    {
        rb.AddForce(movementVector * movementSpeed * Time.deltaTime, ForceMode.Acceleration);
    }
}