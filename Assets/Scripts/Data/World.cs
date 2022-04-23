using System;
using UnityEngine;

/// <summary>World manager</summary>
public static class World
{
	public enum WorldState
	{
		WorldA,
		WorldB
	}

	static Action<WorldState> OnWorldSwitchedEvent;
	static WorldState currentWorld;

	public static void SubscribeOnWorldSwitched(Action<WorldState> callback)
	{
		if (callback != null)
			OnWorldSwitchedEvent += callback;
	}

	public static void UnsubscribeOnWorldSwitched(Action<WorldState> callback)
	{
		if (OnWorldSwitchedEvent != null)
			OnWorldSwitchedEvent -= callback;
	}

	public static void SwitchWorld()
	{
		currentWorld = currentWorld == WorldState.WorldA ? WorldState.WorldB : WorldState.WorldA;

		OnWorldSwitchedEvent?.Invoke(currentWorld);
	}

	[RuntimeInitializeOnLoadMethod]
	public static void InitializeWorld()
	{
		currentWorld = WorldState.WorldA;
		OnWorldSwitchedEvent(currentWorld);
	}
}