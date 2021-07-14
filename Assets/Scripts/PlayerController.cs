using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D _rigid;
	private Animator _animator;
	private Camera _camera;
	private GameManager _gameManager;
	
	public float bulletDamage;
	public float damage;
	public float moveSpeed;
	public float hp;
	public float towerHp;
	
	public float dps;
	public float dpsRange;
	
	public RectTransform playerHP, towerHP;
	public GameObject animHelp;
	public RuntimeAnimatorController melee, range;
	public AttackMelee attackMeleeRight; //, attackMeleeLeft;
	
	public Transform animCenter;
	public Transform animPoint;
	public GameObject bullet;
		
	public GameObject vfxSwitch;
	public AudioSource audioSwitch;
	
	public AudioSource audioHit;
	public AudioSource audioDie;
	public AudioSource audioArrow;
	public AudioSource audioSword;
	
	private float max_playerHP;
	private float max_towerHP;

	private float maxPlayerHP_UI;
	private float maxTowerHP_UI;
	
	private float dpsCount;
	
    private void Start()
	{
		_rigid = GetComponent<Rigidbody2D>();
	    _animator = GetComponent<Animator>();
		_animator.runtimeAnimatorController  = melee;
		_camera = Camera.main;
		_gameManager = FindObjectOfType<GameManager>();
		
		hp += PlayerPrefs.GetInt("hp-point") * 15f;
		max_playerHP = hp;
		towerHp += PlayerPrefs.GetInt("hp-point") * 5f;
		max_towerHP = towerHp;
		bulletDamage += PlayerPrefs.GetInt("damage-point") * 1f;
		damage += PlayerPrefs.GetInt("damage-point") * 1f;
		moveSpeed += PlayerPrefs.GetInt("speed-point") * 0.5f;
		
		maxPlayerHP_UI = playerHP.sizeDelta.x;
		maxTowerHP_UI = towerHP.sizeDelta.x;
    }

    private void Update()
	{
		var horizontal = Input.GetAxis("Horizontal");
	    //if (horizontal > 0) {
	    	//GetComponent<SpriteRenderer>().flipX = false;
	    //} else if (horizontal < 0) {
	    	//GetComponent<SpriteRenderer>().flipX = true;
		//}
	    
		if (_animator.runtimeAnimatorController == range) {
			Vector3 dir = Input.mousePosition - _camera.WorldToScreenPoint(transform.position);
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			animCenter.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	    
	    var posX = transform.position.x + (horizontal * moveSpeed * Time.deltaTime);
	    var newPos = new Vector3(Mathf.Lerp(transform.position.x,posX,1f), transform.position.y, transform.position.z);
	   
		_animator.SetFloat("speed", Mathf.Abs(horizontal));
		_rigid.MovePosition(newPos);
	    
	    // Attack
		dpsCount += Time.deltaTime;
		var isMelee = _animator.runtimeAnimatorController == melee;
		if (Input.GetButtonDown("Fire1") && (isMelee ? dpsCount > dps : dpsCount > dpsRange)){
			dpsCount = 0f;
	    	_animator.SetTrigger("attack");
			if (_animator.runtimeAnimatorController == melee){
				audioSword.Play();
				attackMeleeRight.Attack(damage);
			} else {
				audioArrow.Play();
	    		GameObject.Instantiate(bullet, animPoint.position, animCenter.rotation);
	    	}
	    }
	    
	    // Switch
	    if (Input.GetButtonDown("Fire2")){
	    	_animator.runtimeAnimatorController = _animator.runtimeAnimatorController == melee? range : melee;
	    	animHelp.SetActive(_animator.runtimeAnimatorController == range);
	    	audioSwitch.Play();
	    	GameObject.Instantiate(vfxSwitch, transform.position, transform.rotation);
	    } 
	}
    
	private float hpPercent = 100;
	public void Damage(float raw) {
		hp -= raw;
		hpPercent = (hp * 100) / max_playerHP;
		audioHit.Play();
		
		_animator.SetTrigger("hit");
		var newHP = (hpPercent * playerHP.sizeDelta.x) / 100;
		playerHP.sizeDelta = new Vector2(newHP, playerHP.sizeDelta.y);
		
		if (hp <= 0 ){
			_animator.SetTrigger("die");
			_gameManager.GameOver();
			enabled = false;
		}
	}
	
	private float towerHpPercent = 100;
	public void DamageTower(float raw) {
		towerHp -= raw;
		towerHpPercent = (towerHp * 100) / max_towerHP;
		audioHit.Play();
		
		_animator.SetTrigger("hit");
		var newHP = (towerHpPercent * towerHP.sizeDelta.x) / 100;
		towerHP.sizeDelta = new Vector2(newHP, towerHP.sizeDelta.y);
		
		if (towerHp <= 0 ){
			_animator.SetTrigger("die");
			_gameManager.GameOver();
			enabled = false;
		}
	}
}
