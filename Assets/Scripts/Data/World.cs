using System;

/// <summary>World manager</summary>
public static class World
{
	public enum WorldState
	{
		Normal,
		Other
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
		currentWorld = currentWorld == WorldState.Normal ? WorldState.Other : WorldState.Normal;

		OnWorldSwitchedEvent?.Invoke(currentWorld);
	}
}