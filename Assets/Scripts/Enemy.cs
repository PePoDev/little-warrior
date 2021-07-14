using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
	public float hp;
	public float attack;
	public float speed;
	public float dps;
	public float resetDamage;
	public float stepBack = 0.6f;
	public float rangeArea;
	public float waitAfterAttack;
	
	public bool isFly;
	public bool isRange;
	public bool isBoss;
		
	public GameObject dieEffect;
	public AudioSource DieSound;
	public AudioSource HitSound;
	public Magic rangeAttackEffect;
		
	private float dpsCount;
	private bool isAttack;
	private bool idleAfterAttack = false;

	private PlayerController _player;
	private GameManager _gameManager;
	private Animator _animator;

	private void Start()
	{
		var modeFactor = PlayerPrefs.GetFloat("mode-factor");
		hp *= modeFactor;
		attack *= modeFactor;
		
		DieSound = GameObject.Find("SFX Death").GetComponent<AudioSource>();
		HitSound = GameObject.Find("SFX Hit").GetComponent<AudioSource>();
	    _animator = GetComponent<Animator>();
	    _gameManager = FindObjectOfType<GameManager>();
	    _player = FindObjectOfType<PlayerController>();
    }

	private void Update()
	{
		dpsCount += Time.deltaTime;
		if (isAttack && !isRange) {
			if (dpsCount > dps) {
				dpsCount = 0f;
				_animator.SetTrigger("attack");
					
				if (isFly) {
					_player.DamageTower(attack);	
				} else {
					_player.Damage(attack);
				}
			}
			return;	
		} else if (isRange && rangeArea > Mathf.Abs(transform.position.x - _player.transform.position.x)) {
			if (dpsCount > dps) {
				dpsCount = 0f;
				
				Magic magic = GameObject.Instantiate<Magic>(rangeAttackEffect, _player.transform.position, _player.transform.rotation);
				magic.damage = attack;
				_animator.SetTrigger("attack");
				_animator.SetBool("attacking", false);
				
				idleAfterAttack = true;
				StartCoroutine(WaitAfterAttack());
			}
			return;
		}
		
		if (idleAfterAttack) {
			return;
		}
				
		transform.Translate(new Vector3(speed * Time.deltaTime, 0f, 0f));
	}
	
	private IEnumerator WaitAfterAttack(){
		yield return new WaitForSeconds(waitAfterAttack);
		idleAfterAttack = false;
	}
	
	public void TakeDamage(float damage) {
		if (hp <= 0) {
			GetComponent<BoxCollider2D>().enabled = false;
			return;
		}
		
		hp -= damage;
		resetDamage -= damage;
		HitSound.Play();
		
		var newPos = transform.position.x + stepBack;
		var timeBack = 0.2f;
		
		if (isBoss && resetDamage <= 0) {
			newPos = 6.61f;
			timeBack = 0.6f;
			stepBack = 1f;
			dps = 1f;
			speed = -3f;
			resetDamage = 200;
		}
		
		transform.DOMoveX(newPos, timeBack);
		if (hp <= 0) {
			_animator.SetTrigger("die");
			_gameManager.EnemyDied();
			StartCoroutine(WaitDie());
			enabled = false;
		} else {
			_animator.SetTrigger("hit");
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
	
	private IEnumerator WaitDie(){
		yield return new WaitForSeconds(1f);
		GameObject.Instantiate(dieEffect, transform.position, transform.rotation);
		DieSound.Play();
		Destroy(gameObject);
	}
}
