using UnityEngine;
using static UnityEngine.ParticleSystem;
using static World;

/// <summary>Particle system changing emission based on the world</summary>
[RequireComponent(typeof(ParticleSystem))]
public class WS_Particles : WorldSpecific
{
	[Header("Settings")]
	public bool normalWorldEmits;

	MinMaxCurve emissionRate;
	MinMaxCurve emptyRate;

	EmissionModule emission
	{
		get
		{
			if (emissionRate.Equals(default(MinMaxCurve)))
				emissionRate = GetComponent<ParticleSystem>().emission.rateOverTime;

			if (emptyRate.Equals(default(MinMaxCurve)))
				emptyRate = new MinMaxCurve(0, 0);

			return GetComponent<ParticleSystem>().emission;
		}
	}

	public override void OnSwitchWorlds(WorldState state)
	{
		EmissionModule module = emission;

		switch (state)
		{
			case WorldState.WorldA:
				module.rateOverTime = normalWorldEmits ? emissionRate : emptyRate;
				break;

			case WorldState.WorldB:
				module.rateOverTime = normalWorldEmits ? emptyRate : emissionRate;
				break;
		}
	}
}