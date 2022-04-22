using UnityEngine;
using static World;

/// <summary>Object changing collider's state based on the world</summary>
public class WS_Collider : WorldSpecific
{
	[Header("Settings")]
	public WorldState activeState;

	Collider _collider;
	new Collider collider
	{
		get
		{
			if (_collider == null)
				_collider = GetComponentInChildren<Collider>();

			if (_collider == null)
				Debug.LogError("Didn't find any colliders, please attach a collider to this object", gameObject);

			return _collider;
		}
	}

	public override void OnSwitchWorlds(WorldState state) => collider.enabled = state == activeState;
}
