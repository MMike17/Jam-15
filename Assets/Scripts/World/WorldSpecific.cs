using UnityEngine;

/// <summary>Parent of classes that will represent objects affected by world switching</summary>
public abstract class WorldSpecific : MonoBehaviour
{
	void OnEnable() => World.SubscribeOnWorldSwitched(OnSwitchWorlds);

	void OnDisable() => World.UnsubscribeOnWorldSwitched(OnSwitchWorlds);

	public abstract void OnSwitchWorlds(World.WorldState state);
}