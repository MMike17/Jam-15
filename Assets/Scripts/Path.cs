using UnityEngine;

/// <summary>A path an AI can follow</summary>
public class Path : MonoBehaviour
{
	[Header("Scene references")]
	public Transform[] waypoints;

	[Header("Gizmos")]
	public Color gizmosColor;

	void OnDrawGizmosSelected()
	{
		Gizmos.color = gizmosColor;

		for (int i = 0; i < waypoints.Length; i++)
		{
			if (i == 0)
				Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
			else
				Gizmos.DrawLine(waypoints[i].position, waypoints[i - 1].position);
		}
	}

	public int GetClosestWaypointIndex(Vector3 position)
	{
		int index = 0;
		float maxDistance = 0;

		for (int i = 0; i < waypoints.Length; i++)
		{
			float currentDistance = Vector3.Distance(position, waypoints[i].position);

			if (currentDistance > maxDistance)
			{
				maxDistance = currentDistance;
				index = i;
			}
		}

		return index;
	}

	public Vector3 GetWaypoint(int index) => waypoints[index].position;

	public int WrapIndex(int index)
	{
		if (index >= waypoints.Length)
			index = 0;

		return index;
	}
}