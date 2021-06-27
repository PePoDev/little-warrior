using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float hp;
	public float attack;
	public float speed;
	public float dps;
		
	private float dpsCount;
	private bool isAttack;

	private PlayerController _player;
	private GameManager _gameManager;
	private Animator _animator;

	private void Start()
    {
	    _animator = GetComponent<Animator>();
	    _gameManager = FindObjectOfType<GameManager>();
	    _player = FindObjectOfType<PlayerController>();
    }

	private void Update()
	{
		dpsCount += Time.deltaTime;
		if (isAttack) {
			if (dpsCount > dps) {
				dpsCount = 0f;
				_player.Damage(attack);
				_animator.SetTrigger("attack");
			}
			return;	
		}
		
		transform.Translate(new Vector3(speed * Time.deltaTime, 0f, 0f));
	}
	
	public void TakeDamage(float damage) {
		hp -= damage;
		if (hp <= 0) {
			_animator.SetTrigger("die");
			_gameManager.EnemyDied();
			enabled = false;
		}
	}
    
	protected void OnCollisionEnter2D(Collision2D collisionInfo)
	{
		if (collisionInfo.collider.CompareTag("Player")) {
			isAttack = true;
			_animator.SetBool("attacking", isAttack);
		}
	}
	
	protected void OnCollisionExit2D(Collision2D collisionInfo)
	{
		if (collisionInfo.collider.CompareTag("Player")) {
			isAttack = false;
			_animator.SetBool("attacking", isAttack);
		}
	}
}
