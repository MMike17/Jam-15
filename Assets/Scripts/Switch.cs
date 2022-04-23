using UnityEngine;

/// <summary>Switch that can be activated by the player</summary>
public class Switch : MonoBehaviour
{
	[Header("Settings")]
	public float openDuration;

	[Header("Scene references")]
	public Door door;

	float timer;

	void Update()
	{
		if (timer > 0)
			timer -= Time.deltaTime;

		if (timer <= 0)
			door.TryClose();
	}

	public void Pull()
	{
		door.TryOpen();
		timer = openDuration;
	}
}