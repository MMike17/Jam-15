using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

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
			instance.Init();
			return instance;
		}
	}

	[Header("Settings")]
	[SerializeField] private float accelerationFactor = 20.0f;
	[SerializeField] private float jumpForce = 5.0f;
	[SerializeField] private float rotationSpeed = 10f;
	[SerializeField] private float maxVelocity;
	public float decelerationFactor;
	public float interactMaxDistance;
	[Space]
	public float normalSwitchCooldown;
	public float damageSwitchCooldown;
	[Space]
	public Color interactOnColor;
	public Color interactOffColor;

	[Header("Scene references")]
	public Transform[] raycastBones;
	public Image interactionCursor;
	public Animator anim;
	public CameraFollowSystem cameraFollowSystem;

	Rigidbody rb;
	Collider coll;
	Switch currentSwitch;
	float currentSpeed;
	float switchTimer;
	bool isGrounded;
	bool inputBlocked;

	void OnDrawGizmosSelected()
	{
		if (cameraFollowSystem != null && cameraFollowSystem.mainCamera != null)
			Debug.DrawLine(cameraFollowSystem.mainCamera.transform.position,
				cameraFollowSystem.mainCamera.transform.position + cameraFollowSystem.mainCamera.transform.forward * interactMaxDistance, Color.red);
	}

	private void Init()
	{
		rb = GetComponent<Rigidbody>();
		coll = GetComponent<Collider>();
		inputBlocked = false;
	}

	void Awake()
	{
		Init();
	}

	void Update()
	{
		// update switch timer
		if (switchTimer > 0)
			switchTimer -= Time.deltaTime;

		currentSwitch = null;
		interactionCursor.color = interactOffColor;

		if (Physics.Raycast(cameraFollowSystem.mainCamera.transform.position, cameraFollowSystem.mainCamera.transform.forward, out RaycastHit hit,
				interactMaxDistance))
		{
			currentSwitch = hit.collider.GetComponent<Switch>();

			if (currentSwitch != null)
				interactionCursor.color = interactOnColor;
		}

		cameraFollowSystem.UpdateCamera(currentSpeed / maxVelocity);
	}

	public void Interact()
	{
		if (currentSwitch != null)
			currentSwitch.Pull();
	}

	public void SwitchWorld(bool takeDamage = false)
	{
		if (!takeDamage && (switchTimer > 0 || inputBlocked))
			return;

		if (takeDamage)
			switchTimer = takeDamage ? damageSwitchCooldown : normalSwitchCooldown;

		World.SwitchWorld();
	}

	public void Move(Vector3 movementVector, Vector2 rotationVector)
	{
		if (inputBlocked)
			return;

		bool canMouvement = true;
		anim.SetFloat("RightLeft", movementVector.x);
		anim.SetFloat("FrontBack", movementVector.z);

		// movement
		float jump = movementVector.y;
		movementVector.y = 0;

		float targetSpeed = movementVector.magnitude > 0.1f ? maxVelocity : 0;
		float step = movementVector.magnitude > 0.1f ? accelerationFactor : decelerationFactor;

		currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, step * Time.deltaTime);

		if (!isGrounded)
		{
			jump = 0;
			canMouvement = false;
		}

		// works very well for controller but is shit with keyboard
		Vector3 finalSpeed = (transform.forward * movementVector.z + transform.right * movementVector.x) * currentSpeed * Time.deltaTime;
		finalSpeed.y = rb.velocity.y;

		rb.velocity = finalSpeed;

		if (jump > 0)
		{
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // jump	
			canMouvement = false;

			anim.Play("Jump");
		}

		if (!isGrounded && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
			anim.Play("Jump");

		// rotation
		transform.Rotate(Vector3.up, rotationVector.x * rotationSpeed * Time.deltaTime);
		cameraFollowSystem.SetXAngle(rotationVector.y * rotationSpeed);

		if (canMouvement)
			anim.Play("Movement");
	}

	private void FixedUpdate()
	{
		isGrounded = Physics.Raycast(transform.position, -Vector3.up, 0.001f);
		Debug.DrawLine(transform.position, transform.position - Vector3.up * 0.001f, Color.black);
	}

	public void Catch(float animDuration, Vector3 enemyPos)
	{
		inputBlocked = true;
		rb.isKinematic = true;
		rb.useGravity = false;
		currentSpeed = 0;

		StartCoroutine(CatchTurn(animDuration, enemyPos));
	}

	public void Release()
	{
		inputBlocked = false;
		rb.isKinematic = false;
		rb.useGravity = true;

		transform.SetParent(null);
	}

	IEnumerator CatchTurn(float animDuration, Vector3 enemyPos)
	{
		float animTurnPercent = 0.3f;
		float turnDuration = animDuration * animTurnPercent;
		Quaternion initialRotation = transform.rotation;

		float turnTimer = 0;

		while (turnTimer < animDuration)
		{
			turnTimer += Time.deltaTime;

			Quaternion targetRotation = Quaternion.LookRotation(enemyPos - transform.position);
			targetRotation.z = 0; // this is a test

			if (turnTimer < turnDuration)
			{
				float percent = turnTimer / turnDuration;
				targetRotation = Quaternion.Lerp(initialRotation, targetRotation, percent);
			}

			transform.rotation = targetRotation;
			yield return null;
		}
	}
}