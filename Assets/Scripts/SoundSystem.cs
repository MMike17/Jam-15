using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
	static SoundSystem instance;

	[Header("Settigs")]
	public List<GameSound> soundPool;

	List<SpawnedSource> spawnedSources;

	void Awake()
	{
		instance = this;

		spawnedSources = new List<SpawnedSource>();
	}

	void Update()
	{
		List<SpawnedSource> toDelete = new List<SpawnedSource>();

		spawnedSources.ForEach(item =>
		{
			if (item.source.time >= item.source.clip.length)
				toDelete.Add(item);
		});

		toDelete.ForEach(item =>
		{
			if (item.shouldDestroy)
				Destroy(item.source.gameObject);

			spawnedSources.Remove(item);
		});
	}

	public static void PlaySound(string id, Transform parent = null)
	{
		List<GameSound> sounds = instance.soundPool.FindAll(item => item.id == id);

		if (sounds == null)
		{
			Debug.LogWarning("Could not find sound for id " + id);
			return;
		}

		GameSound sound = sounds[UnityEngine.Random.Range(0, sounds.Count)];

		if (sound.spawnable)
		{
			AudioSource source = Instantiate(sound.source);

			if (parent != null)
			{
				source.transform.position = parent.position;
				source.transform.SetParent(parent);
			}

			source.Play();
			instance.spawnedSources.Add(new SpawnedSource(sound.id, source, sound.spawnable));
		}
		else
		{
			sound.source.Play();
			instance.spawnedSources.Add(new SpawnedSource(sound.id, sound.source, sound.spawnable));
		}
	}

	public static void StopSound(string id)
	{
		GameSound sound = instance.soundPool.Find(item => item.id == id);

		if (sound == null)
		{
			Debug.LogWarning("Could not find sound for id " + id);
			return;
		}

		SpawnedSource spawned = instance.spawnedSources.Find(item => item.id == id);

		if (spawned == null)
		{
			Debug.LogWarning("Could not find spawned source for id " + id);
			return;
		}

		instance.spawnedSources.Remove(spawned);

		if (sound.spawnable)
			Destroy(spawned.source.gameObject);
		else
			spawned.source.Stop();
	}

	[Serializable]
	public class GameSound
	{
		public string id;
		public AudioSource source;
		[Space]
		public bool spawnable;
	}

	class SpawnedSource
	{
		public string id;
		public AudioSource source;
		public bool shouldDestroy;

		public SpawnedSource(string id, AudioSource source, bool shouldDestroy)
		{
			this.id = id;
			this.source = source;
			this.shouldDestroy = shouldDestroy;
		}
	}
}