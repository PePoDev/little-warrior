using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
	public GameObject attackEffect;
	
	public float damage;
	public float DelayAttack;
	
	private PlayerController _player;
	private bool isPlayInArea;
	
	private IEnumerator Start()
    {
	    _player = FindObjectOfType<PlayerController>();
	    
	    yield return new WaitForSeconds(DelayAttack);
	    
	    GameObject.Instantiate(attackEffect, _player.transform.position, _player.transform.rotation);
	    
	    if (isPlayInArea) {
		    _player.Damage(damage);
	    }
	    
	    Destroy(gameObject);
    }

	protected void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player")) {
			isPlayInArea = true;
		}
	}
	
	protected void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player")) {
			isPlayInArea = false;
		}
	}
}
