using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>Represents an enemy that patrolls a zone and pursues the player if they see them</summary>
[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Enemy : MonoBehaviour
{
	[Header("Settings")]
	public float maxSpeed;
	public float patrollSpeed;
	[Space]
	public float idleDuration;
	public float attackRange;
	public float searchDuration;
	[Space]
	public float maxDetecDistance;
	public float maxDetecAngle;
	[Range(0, 1)]
	public float boneDetecThreshold;

	[Header("Scene references")]
	public Transform visionHub;
	public Transform catchPoint;
	public Path patrolPath;

	enum AIState
	{
		Patrol,
		Idle,
		Attack,
		Search
	}

	AIState currentState;
	NavMeshAgent agent;
	Animator anim;
	Transform player;
	Vector3 lastSeenPlayerPos;
	float idleTimer;
	int boneDetecCount;
	int currentPathWaypointIndex;
	bool isInCatchAnim;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();

		player = Player.Instance.transform;

		isInCatchAnim = false;
		idleTimer = 0;

		boneDetecCount = Mathf.FloorToInt(Player.Instance.raycastBones.Length * boneDetecThreshold);

		currentPathWaypointIndex = patrolPath.GetClosestWaypointIndex(transform.position);
		currentState = AIState.Patrol;
	}

	void CheckDetectPlayer()
	{
		bool isInRange = Vector3.Distance(player.position, visionHub.position) <= maxDetecDistance;

		bool isInView = Vector3.Angle(transform.forward, player.position - visionHub.position) <= maxDetecAngle;

		if (isInRange && isInView)
		{
			int detectedBones = 0;

			for (int i = 0; i < Player.Instance.raycastBones.Length; i++)
			{
				if (Physics.Raycast(visionHub.position, Player.Instance.raycastBones[i].position - visionHub.forward))
					detectedBones++;
			}

			if (detectedBones >= boneDetecCount)
			{
				if (currentState == AIState.Idle)
					StopCoroutine(IdleAnimation());

				currentState = AIState.Attack;
				lastSeenPlayerPos = player.position;
			}
		}
	}

	void Update()
	{
		CheckDetectPlayer();

		agent.speed = currentState == AIState.Attack || currentState == AIState.Search ? maxSpeed : patrollSpeed;

		switch (currentState)
		{
			case AIState.Patrol:
				PatrollBehaviour();
				break;

			case AIState.Idle:
				StartCoroutine(IdleAnimation());
				break;

			case AIState.Attack:
				AttackBehaviour();
				break;

			case AIState.Search:
				SearchBehaviour();
				break;
		}
	}

	void PatrollBehaviour()
	{
		anim.Play("Move", 0);
		anim.Play("Move", 1);

		agent.SetDestination(patrolPath.GetWaypoint(currentPathWaypointIndex));

		if (agent.pathStatus == NavMeshPathStatus.PathComplete)
			currentState = AIState.Idle;
	}

	IEnumerator IdleAnimation()
	{
		idleTimer = 0;

		while (idleTimer < idleDuration)
		{
			idleTimer += Time.deltaTime;
			float percent = idleTimer / idleDuration;

			anim.Play("Idle", 0, percent);
			anim.Play("Idle", 1, percent);
			yield return null;
		}

		currentState = AIState.Patrol;
		currentPathWaypointIndex = patrolPath.WrapIndex(currentPathWaypointIndex++);
	}

	void AttackBehaviour()
	{
		if (isInCatchAnim)
			return;

		anim.Play("Sprint", 0);
		anim.Play("Sprint", 1);

		agent.SetDestination(player.position);

		if (Vector3.Distance(player.position, transform.position) <= attackRange)
		{
			isInCatchAnim = true;
			agent.isStopped = true;

			StartCoroutine(CatchAnimation());
		}
	}

	IEnumerator CatchAnimation()
	{
		isInCatchAnim = true;

		anim.Play("Catch", 0);
		anim.Play("Catch", 1);

		yield return new WaitForEndOfFrame();
		float animDuration = anim.GetCurrentAnimatorStateInfo(0).length;

		Player.Instance.Catch(animDuration, transform.position);

		Player.Instance.transform.position = catchPoint.position;
		Player.Instance.transform.SetParent(catchPoint);

		yield return new WaitForSeconds(animDuration);

		Player.Instance.SwitchWorld(true);
		Player.Instance.Release();

		isInCatchAnim = false;
		agent.isStopped = false;

		currentState = AIState.Patrol;
		currentPathWaypointIndex = patrolPath.GetClosestWaypointIndex(transform.position);
	}

	void SearchBehaviour()
	{
		anim.Play("Sprint", 0);
		anim.Play("Sprint", 1);

		agent.SetDestination(lastSeenPlayerPos);
	}
}