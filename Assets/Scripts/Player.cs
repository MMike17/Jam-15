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
	public float decelerationFactor;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float maxVelocity;

	float currentSpeed;
	bool isGrounded;
    
    private void Init()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

	void Awake()
	{
		Init();
	}

    public void Move(Vector3 movementVector, Vector2 rotationVector)
    {
		// movement
        float jump = movementVector.y;
        movementVector.y = 0;

		float targetSpeed = movementVector.magnitude > 0.1f ? maxVelocity : 0;
		float step = movementVector.magnitude > 0.1f ? accelerationFactor : decelerationFactor;

		currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, step * Time.deltaTime);

		if(!isGrounded)
			jump = 0;

		// works very well for controller but is shit with keyboard
		Vector3 finalSpeed = (transform.forward * movementVector.z + transform.right * movementVector.x) * currentSpeed * Time.deltaTime;
		finalSpeed.y = rb.velocity.y;

		rb.velocity = finalSpeed;

		if(jump > 0)
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // jump

		// rotation
		transform.Rotate(Vector3.up, rotationVector.x * rotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
		isGrounded = Physics.Raycast(transform.position, -Vector3.up, 2);
    }

    private Vector3 ClampV3(Vector3 vector, float max)
    {
        vector.x = Mathf.Clamp(vector.x, -max, max);
        vector.y = Mathf.Clamp(vector.y, -max, max);
        vector.z = Mathf.Clamp(vector.z, -max, max);

        return vector;
    }
}