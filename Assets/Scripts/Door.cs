using UnityEngine;

/// <summary>Door that can be opened by an actuator</summary>
[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour
{
	[Header("Settings")]
	public string openAnimName;
	public string closeAnimName;

	Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void TryOpen()
	{
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName(openAnimName))
			anim.Play(openAnimName);
	}

	public void TryClose()
	{
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName(closeAnimName))
			anim.Play(closeAnimName);
	}
}