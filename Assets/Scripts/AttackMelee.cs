using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMelee : MonoBehaviour
{
	public GameObject hitEffect;
	
	private List<Enemy> avaliableEnemys = new List<Enemy>();
	
	public void Attack(float damage) {
		var target = GetClosestEnemy(avaliableEnemys);
		if (target != null) {
			target.TakeDamage(damage);
			GameObject.Instantiate(hitEffect, transform.position, transform.rotation);
		}
	}
	
	private Enemy GetClosestEnemy (List<Enemy> enemies)
	{
		Enemy bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		foreach(var potentialTarget in enemies)
		{
			Vector3 directionToTarget = potentialTarget.gameObject.transform.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget;
			}
		}
     
		return bestTarget;
	}
	
	protected void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy")) {
			avaliableEnemys.Add(other.gameObject.GetComponent<Enemy>());
		}
	}
	
	protected void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Enemy")) {
			avaliableEnemys.Remove(other.gameObject.GetComponent<Enemy>());
		}
	}
}
