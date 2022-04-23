using UnityEngine;

public class InputSystem : MonoBehaviour
{
    private void Update()
    {
        var movementVector = Vector3.zero;
        movementVector.x = Input.GetAxis("Horizontal");
        movementVector.z = Input.GetAxis("Vertical");
        movementVector.y = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;

		var rotation = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
		if(Input.GetMouseButtonDown(0))
			Player.Instance.Interract();

        Player.Instance.Move(movementVector, rotation);
    }
}