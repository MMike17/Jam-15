using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class WS_Object : WorldSpecific
{   
    public bool IsWorldA;

MeshRenderer rend;

    void Awake ()
    {
        rend = GetComponent<MeshRenderer>();
    }
    public override void OnSwitchWorlds(World.WorldState state)
    {
        switch (state)
        {
            case World.WorldState.WorldA :
            rend.enabled = IsWorldA;
            break;

            case World.WorldState.WorldB :
            rend.enabled = !IsWorldA;
            break;
        }

        Debug.Log("State : "+state+ " / world A : "+ IsWorldA);
    }
}
