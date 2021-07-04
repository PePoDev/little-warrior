using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
	public GameObject effect;
	public float speed;
	
	private PlayerController _player;
	private Rigidbody2D _rigid;
	
	private void Start()
	{
		_rigid = GetComponent<Rigidbody2D>();
		_player = FindObjectOfType<PlayerController>();
		
		var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
		_rigid.AddForce(dir.normalized * speed);
    }
	
	protected void OnCollisionEnter2D(Collision2D collisionInfo)
	{
		if (collisionInfo.collider.CompareTag("Enemy")) {
			collisionInfo.gameObject.GetComponent<Enemy>().TakeDamage(_player.bulletDamage);
			GameObject.Instantiate(effect, transform.position, transform.rotation);
		}		
		Destroy(gameObject);
	}
}
