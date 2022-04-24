using UnityEngine;

/// <summary>Switch that can be activated by the player</summary>
[RequireComponent(typeof(Animator))]
public class Switch : MonoBehaviour
{
	[Header("Settings")]
	public float openDuration;

	[Header("Scene references")]
	public Door door;

	Animator anim;
	float timer;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		if (timer > 0)
			timer -= Time.deltaTime;

		if (timer <= 0)
		{
			door.TryClose();
			anim.Play("Close");
		}
	}

	public void Pull()
	{
		anim.Play("Open");
		door.TryOpen();
		timer = openDuration;
	}
}