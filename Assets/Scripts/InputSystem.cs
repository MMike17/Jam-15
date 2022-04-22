using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    private Vector3 movementVector; 
    private void Update()
    {
        movementVector = Vector3.zero;
        movementVector.x = Input.GetAxis("Horizontal");
        movementVector.z = Input.GetAxis("Vertical");
        movementVector.y = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;
        Player.Instance.Move(movementVector);
    }
}