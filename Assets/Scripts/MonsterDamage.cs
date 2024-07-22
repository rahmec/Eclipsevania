using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float KBForce;
    [SerializeField] float KBTime;
    [SerializeField] MonsterHealth monsterHealth;

    void Start()
    {
	monsterHealth = GetComponent<MonsterHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Player"){
	    if(monsterHealth.GetHealth() <= 0){
		Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		Debug.Log(collision.gameObject.name);
		return;
	    }
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            playerHealth.TakeDamage(damage);

            int KBDirection;

            if (collision.transform.position.x <= transform.position.x){
                KBDirection = -1;
            }
            else{
                KBDirection = 1;
            }
            
            playerMovement.GetKnocked(KBForce, KBDirection);           
        }
    }
}
