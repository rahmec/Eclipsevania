using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulMovement : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float moveSpeed;
    [SerializeField] int patrolDestination;
    // Start is called before the first frame update

    [SerializeField] Transform playerTransform;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] bool isChasing = false;
    [SerializeField] bool isNear = false;
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isHit = false;
    [SerializeField] bool isFlying = false;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletPos;
    
    bool isDead = false;
    [SerializeField] float chasingDistance;
    [SerializeField] float attackingDistance;
    [SerializeField] int explosionDamage;
    [SerializeField] float animationDelay;
    [SerializeField] float attackDelay;
    [SerializeField] float deathAnimDelay;
    [SerializeField] float defaultHeight;
    [SerializeField] float KBForce;
    private Vector2 targetPosition;
    bool exploded = false;

    Rigidbody2D rb;
    Animator monsterAnimator;
    MonsterHealth monsterHealth;
    SpriteRenderer SR;


    

    void Start()
    {
        //Get the rigidbody attached to the player
        rb = GetComponent<Rigidbody2D>();
        monsterAnimator = GetComponent<Animator>();
        monsterHealth = GetComponent<MonsterHealth>();
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update(){
        // Decide target position
        if(isDead || isHit){
            return;
        }
        if(isChasing & (playerHealth.GetHealth()>0)){
            targetPosition.x = playerTransform.position.x;
        }else{
            isChasing=false;
            isNear = false;
            if(Vector2.Distance(transform.position, playerTransform.position) < chasingDistance){
                isChasing = true;
                targetPosition.x = playerTransform.position.x;
            }
            if(patrolDestination == 0){
                targetPosition.x = patrolPoints[0].position.x;
                if(Vector2.Distance(transform.position, targetPosition) < 0.3f){
                    patrolDestination = 1;
                }
            }
            else if (patrolDestination == 1){
                targetPosition.x = patrolPoints[1].position.x;
                if(Vector2.Distance(transform.position, targetPosition) < 0.3f){
                    patrolDestination = 0;
                }
                
            }
        }
        // Turn towards target position
        
        if(transform.position.x < targetPosition.x){
            //transform.eulerAngles = new Vector3(0, 0, 0);
            SR.flipX = false;
        }else{
            //transform.eulerAngles = new Vector3(0, 180, 0);
            SR.flipX = true;
        }

        targetPosition.y = transform.position.y;
	transform.position = Vector2.MoveTowards(
	    transform.position,
	    targetPosition,
	    moveSpeed * Time.deltaTime);    
    }
    

    public void Die(){
        isDead = true;
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
	if (!exploded){
	    monsterAnimator.SetTrigger("Death");
	    yield return new WaitForSeconds(deathAnimDelay);
	    Destroy(gameObject);
	}
    }

    private IEnumerator KnockedCoroutine(){
        yield return new WaitForSeconds(0.1f);
	rb.velocity = new Vector2(0f,0f);
    }

    public void GetKnocked(float KBForce, float KBDirection){
	rb.velocity = new Vector2(KBForce*KBDirection, KBForce);
	if(isFlying){
	    StartCoroutine(KnockedCoroutine());
	    Debug.Log("Knocked");
	}
	}

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Player"){
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            int KBDirection;

            if (collision.transform.position.x <= transform.position.x){
                KBDirection = -1;
            }
            else{
                KBDirection = 1;
            }
            
	    StartCoroutine(ExplosionCoroutine(playerHealth, playerMovement, KBDirection));
	    monsterAnimator.SetTrigger("Explosion");
	    exploded = true;
	    monsterHealth.TakeDamage(100);
        }
    }

    private IEnumerator ExplosionCoroutine(PlayerHealth playerHealth, PlayerMovement playerMovement, int KBDirection)
    {
        yield return new WaitForSeconds(deathAnimDelay/2);
	playerHealth.TakeDamage(explosionDamage);
	playerMovement.GetKnocked(KBForce, KBDirection);           
        yield return new WaitForSeconds(deathAnimDelay/2);
        //Destroy(gameObject);
	gameObject.SetActive(false);
	    
    }

}
