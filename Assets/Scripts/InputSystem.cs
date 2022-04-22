using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    private void Update()
    {
        var movementVector = Vector3.zero;
        movementVector.x = Input.GetAxis("Horizontal");
        movementVector.z = Input.GetAxis("Vertical");
        movementVector.y = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;

        var rotation = Vector2.zero;
        var qPressed = Input.GetKey(KeyCode.Q);
        var ePressed = Input.GetKey(KeyCode.E);
        if (qPressed != ePressed) // only one of them is pressed
        {
            rotation.x = qPressed ? -1 : 1;
        }
        Player.Instance.Move(movementVector, rotation);
    }
}