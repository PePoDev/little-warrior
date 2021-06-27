using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D _rigid;
	private Animator _animator;
	
	public float damage;
	public float moveSpeed;
	public float hp;
	public float towerHp;
	
	public float dps;
	
	public RectTransform playerHP, towerHP;
	public GameObject animHelp;
	public RuntimeAnimatorController melee, range;
	// public AttackMelee attackMeleeRight, attackMeleeLeft;
	
	public GameObject vfxSwitch;
	public AudioClip audioSwitch;
	
	public AudioClip audioAttack;
	public AudioClip audioHit;
	public AudioClip audioDie;
	
	private float max_hp;
	private float max_hpTown;
	
	private float hpBarSizeX;
	
	private float dpsCount;
	
    private void Start()
	{
		_rigid = GetComponent<Rigidbody2D>();
	    _animator = GetComponent<Animator>();
		_animator.runtimeAnimatorController  = melee;
	    
		moveSpeed += PlayerPrefs.GetFloat("speed") * 0.5f;
		damage += PlayerPrefs.GetFloat("damage") * 10f;
		hp += PlayerPrefs.GetFloat("hp") * 20f;
		towerHp += PlayerPrefs.GetFloat("hp") * 20f;
		
		hpBarSizeX = playerHP.sizeDelta.x;
    }

    private void Update()
	{
		if (hp <= 0 ){
			_animator.SetTrigger("die");
			return;
		}
		
	    var horizontal = Input.GetAxis("Horizontal");
	    if (horizontal > 0) {
	    	GetComponent<SpriteRenderer>().flipX = false;
	    } else if (horizontal < 0) {
	    	GetComponent<SpriteRenderer>().flipX = true;
		}
	    
	    var posX = transform.position.x + (horizontal * moveSpeed * Time.deltaTime);
	    var newPos = new Vector3(Mathf.Lerp(transform.position.x,posX,1f), transform.position.y, transform.position.z);
	   
		_animator.SetFloat("speed", Mathf.Abs(horizontal));
		_rigid.MovePosition(newPos);
	    
	    // Attack
		dpsCount += Time.deltaTime;
		if (Input.GetButtonDown("Fire1") && dpsCount > dps){
			dpsCount = 0f;
	    	_animator.SetTrigger("attack");
	    	if (_animator == melee){
	    		
	    	} else {
	    		
	    	}
	    }
	    
	    // Switch
	    if (Input.GetButtonDown("Fire2")){
	    	_animator.runtimeAnimatorController = _animator.runtimeAnimatorController == melee? range : melee;
	    	animHelp.SetActive(_animator.runtimeAnimatorController == range);
	    	AudioSource.PlayClipAtPoint(audioSwitch, Camera.main.transform.position);
	    	GameObject.Instantiate(vfxSwitch, transform.position, transform.rotation);
	    } 
	}
    
	public void Damage(float raw) {
		hp -= raw;
		_animator.SetTrigger("hit");
		var newHP = playerHP.sizeDelta.x - raw;
		playerHP.sizeDelta = new Vector2(newHP, playerHP.sizeDelta.y);
		
	}
}
