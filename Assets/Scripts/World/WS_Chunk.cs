using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WS_Chunk : WorldSpecific
{
    public static float transitionLength = 0.5f;
    public override void OnSwitchWorlds(World.WorldState newWorld)
    {
        var isNewWorldB = newWorld == World.WorldState.WorldB;
        var prevWorldName = isNewWorldB ? "World A" : "World B";
        var currWorldName = isNewWorldB ? "World B" : "World A";

        var prevWorld = transform.Find(prevWorldName);
        var currWorld = transform.Find(currWorldName);

        StartCoroutine(Transition(prevWorld, false));
        StartCoroutine(Transition(currWorld, true));
    }

    private IEnumerator Transition(Transform world, bool transitionToShow)
    {
        var timeElapsed = 0.0f;

        var colliders = world.GetComponentsInChildren<Collider>();
        var renderers = world.GetComponentsInChildren<Renderer>();
        foreach (var coll in colliders)
        {
            coll.enabled = transitionToShow;
        }
        
        while (timeElapsed < transitionLength)
        {
            timeElapsed += Time.deltaTime;
            var percent = timeElapsed / transitionLength;
            if (transitionToShow) percent = 1.0f - percent; 

            foreach (var rend in renderers)
            {
                var mat = rend.material;
                if (mat.HasProperty("_DissolveAmount")) mat.SetFloat("_DissolveAmount", percent);
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
}
