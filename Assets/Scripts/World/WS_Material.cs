using UnityEngine;
using static World;

/// <summary>Object changing materials based on the world</summary>
public class WS_Material : WorldSpecific
{
	[Header("Settings")]
	public Material[] normalWorldMaterials;
	public Material[] otherWorldMaterials;

	Renderer _renderer;
	new Renderer renderer
	{
		get
		{
			if (_renderer == null)
				_renderer = GetComponent<Renderer>();

			if (_renderer == null)
			{
				Debug.LogError("Didn't find any renderer, please attach a renderer to this object", gameObject);
			}

			return _renderer;
		}
	}

	public override void OnSwitchWorlds(WorldState state)
	{
		switch (state)
		{
			case WorldState.Normal:
				renderer.materials = normalWorldMaterials;
				break;

			case WorldState.Other:
				renderer.materials = otherWorldMaterials;
				break;
		}
	}
}