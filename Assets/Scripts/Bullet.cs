using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet: MonoBehaviour
{
    private GameObject player;
    [SerializeField] GameObject player2;
    [SerializeField] float timer;
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] float bullRot;
    [SerializeField] bool isDirectioned=true;
    
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
	
	//Get Player Object
	player = GameObject.FindGameObjectWithTag("Player");
	Debug.Log(player.name);
	// Direct the bullet to the player
	Vector3 playerPosition = player.transform.position;

	if(isDirectioned)
	    playerPosition.y += 2;
	else
	    playerPosition.y = transform.position.y;

	Vector3 direction = playerPosition - transform.position;
	// Add speed to the bullet
	rb.velocity = new Vector2(direction.x, direction.y).normalized*speed;
	// Rotate the bullet in the right direction (for estethics)
	float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
	transform.rotation = Quaternion.Euler(0,0,rot+bullRot);
	// Ignore collision with enemies
	Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Enemy"), true);
	// Destroy the bullet after a while
	StartCoroutine(BulletCoroutine());
    }

    private IEnumerator BulletCoroutine()
    {
        yield return new WaitForSeconds(timer);
	Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
	if (other.gameObject.CompareTag("Player")){
	    other.GetComponent<PlayerHealth>().TakeDamage(damage);
	}
	if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Floor")){
	    Destroy(gameObject);
	}
    }

}
