using System;
using UnityEngine;

using Random = UnityEngine.Random;

/// <summary>Manages camera movement</summary>
public class CameraFollowSystem : MonoBehaviour
{
	[Header("Settings")]
	public CameraSettings idleSettings;
	public CameraSettings runSettings;
	[Space]
	public float minXAngle;
	public float maxXAngle;

	[Header("Scene references")]
	public Camera mainCamera;
	public Transform target;

	float currentXAngle;

	void OnDrawGizmosSelected()
	{
		if (target != null)
		{
			idleSettings.OnDrawGizmos(target);
			runSettings.OnDrawGizmos(target);
		}

		if (mainCamera != null)
		{
			// currentXAngle = 0;

			if (idleSettings.test)
				ApplyCameraSettings(idleSettings);
			else if (runSettings.test)
				ApplyCameraSettings(runSettings);
		}
	}

	public void UpdateCamera(float percent)
	{
		CameraSettings currentSettings = CameraSettings.Lerp(idleSettings, runSettings, percent);
		ApplyCameraSettings(currentSettings);
	}

	public void SetXAngle(float delta)
	{
		currentXAngle = Mathf.Clamp(currentXAngle + delta * Time.deltaTime, minXAngle, maxXAngle);
	}

	void ApplyCameraSettings(CameraSettings settings)
	{
		Vector3 cameraTargetPos = ApplyRelativeOffset(target, settings.positionOffset);
		Vector3 start = target.position + Vector3.up * 2;

		if (Physics.Raycast(start, cameraTargetPos, out RaycastHit hit, Vector3.Distance(start, cameraTargetPos)))
			cameraTargetPos = hit.point;

		mainCamera.transform.position = cameraTargetPos;
		mainCamera.transform.LookAt(ApplyRelativeOffset(target, settings.targetOffset));
		mainCamera.transform.RotateAround(ApplyRelativeOffset(target, settings.positionOffset), target.right, currentXAngle);

		mainCamera.fieldOfView = settings.fov;
	}

	public Vector3 ApplyRelativeOffset(Transform target, Vector3 offset)
	{
		return target.position + target.forward * offset.z + target.up * offset.y + target.right * offset.x;
	}

	/// <summary>Camera presets</summary>
	[Serializable]
	public class CameraSettings
	{
		public Vector3 positionOffset;
		public Vector3 targetOffset;
		public float fov;
		[Space]
		public bool test;

		Color gizmosColor;

		public CameraSettings(Vector3 positionOffset, Vector3 targetOffset, float fov)
		{
			this.positionOffset = positionOffset;
			this.targetOffset = targetOffset;
			this.fov = fov;
		}

		public void OnDrawGizmos(Transform parent)
		{
			if (gizmosColor == default(Color))
				gizmosColor = new Color(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2), 0.7f);

			Gizmos.color = gizmosColor;

			Vector3 pos = parent.forward * positionOffset.z + parent.up * positionOffset.y + parent.right * positionOffset.x;
			Vector3 target = parent.forward * targetOffset.z + parent.up * targetOffset.y + parent.right * targetOffset.x;

			Gizmos.DrawSphere(pos, 0.5f);
			Gizmos.DrawSphere(target, 0.3f);

			Gizmos.DrawLine(pos, target);
		}

		public static CameraSettings Lerp(CameraSettings start, CameraSettings end, float percent)
		{
			Vector3 posOffset = Vector3.Lerp(start.positionOffset, end.positionOffset, percent);
			Vector3 targOffset = Vector3.Lerp(start.targetOffset, end.targetOffset, percent);
			float fov = Mathf.Lerp(start.fov, end.fov, percent);

			return new CameraSettings(posOffset, targOffset, fov);
		}
	}
}