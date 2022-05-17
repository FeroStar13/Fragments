using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyBrain : MonoBehaviour
{

	[Header("Movement")]
	[SerializeField] protected float maxDistance;
	[SerializeField] float _coneAngle;
	[SerializeField] protected float _movementSpeed;
	

	[Header("Life")]
	[SerializeField] protected float _hp;
	[SerializeField] protected private Image enemyHpImage;

	protected Transform target;
	protected virtual void Update()
	{
		Search();
	}

	void Search()
	{

		Collider[] hitColliders = new Collider[100];

		hitColliders = Physics.OverlapSphere(transform.position, maxDistance);

		for (var i = 0; i < hitColliders.Length; i++)
		{
			Transform tempTarget = hitColliders[i].transform;

			Vector3 dir = tempTarget.position - transform.position; // find target direction
			if (Vector3.Angle(dir, transform.forward) <= _coneAngle / 2)
			{
				PlayerCharacter player = tempTarget.GetComponentInChildren<PlayerCharacter>();
				if (player != null)
				{
					target = player.transform;
				}
			}
		}
	}
}

