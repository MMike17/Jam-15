using UnityEngine;

public class Light : MonoBehaviour
{
	[Header("Settings")]
	[Range(0.1f, 20)]
	public float size;

	[Header("Scene references")]
	public Transform lightHolderLeft;
	public Transform lightHolderRight;
	public Transform tube;

	void OnDrawGizmosSelected()
	{
		if (tube != null)
		{
			tube.position = transform.position;

			Vector3 scale = tube.localScale;
			scale.y = size / transform.localScale.x;
			tube.localScale = scale;
		}

		if (lightHolderLeft != null)
			lightHolderLeft.position = transform.position - transform.right * size;

		if (lightHolderRight != null)
			lightHolderRight.position = transform.position + transform.right * size;
	}
}