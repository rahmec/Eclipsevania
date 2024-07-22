using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
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
    [SerializeField] bool isShooting = false;
    [SerializeField] bool boss = false;
    [SerializeField] GameObject canvasWin;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletPos;
    
    bool isDead = false;
    [SerializeField] float chasingDistance;
    [SerializeField] float attackingDistance;
    [SerializeField] int attackDamage;
    [SerializeField] float attackAnimationDelay;
    [SerializeField] float attackDelay;
    [SerializeField] float deathAnimDelay;
    [SerializeField] float defaultHeight;
    [SerializeField] float KBForce;
    [SerializeField] float horizBullFlip;
    private Vector2 targetPosition;

    Rigidbody2D rb;
    Animator monsterAnimator;
    SpriteRenderer SR;


    

    void Start()
    {
        //Get the rigidbody attached to the player
        rb = GetComponent<Rigidbody2D>();
        monsterAnimator = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update(){
        // Decide target position
        if(isAttacking || isDead || isHit){
	    if(isDead){
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Debug.Log(player.name);
		Physics2D.IgnoreCollision(player.GetComponent<CapsuleCollider2D>(), GetComponent<CapsuleCollider2D>());
	    }
            return;
        }
	targetPosition.y = transform.position.y;
        if(isChasing & (playerHealth.GetHealth()>0)){
            targetPosition.x = playerTransform.position.x;
            if (Vector2.Distance(transform.position, playerTransform.position) < attackingDistance){
                isNear = true;
                monsterAnimator.SetBool("Running", false);
                rb.velocity = new Vector2 ( 0, rb.velocity.y );
                StartCoroutine(AttackCoroutine(attackAnimationDelay));
            }
            if (Vector2.Distance(transform.position, playerTransform.position) > attackingDistance){
                isNear = false;
            }
        }else{
            isChasing=false;
            isNear = false;
            if(Vector2.Distance(transform.position, playerTransform.position) < chasingDistance){
                isChasing = true;
		targetPosition.x = playerTransform.position.x;
            }
            if(patrolDestination == 0){
                targetPosition.x = patrolPoints[0].position.x;
                if(Vector2.Distance(transform.position, targetPosition) < 0.4f){
                    patrolDestination = 1;
                }
            }
            else if (patrolDestination == 1){
                targetPosition.x = patrolPoints[1].position.x;
                if(Vector2.Distance(transform.position, targetPosition) < 0.4f){
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

        // Move towards target position

        if(!isNear){
            monsterAnimator.SetBool("Running", true);
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);    
	    Debug.Log("Daje rivo");
        }
        
        
    }

    private IEnumerator AttackCoroutine(float attackAnimationDelay)
    {
        isAttacking = true;
        monsterAnimator.SetBool("Attacking", true);
        yield return new WaitForSeconds(attackAnimationDelay * 0.8f);
        if(!isHit && !isDead){
	    if(isShooting){
		Shoot();
	    }else{
		DealDamageToEnemies();
	    }
        }
	yield return new WaitForSeconds(attackAnimationDelay * 0.2f);
        monsterAnimator.SetBool("Attacking", false);
	yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    public void Die(){
        isDead = true;
	if(isFlying)
	    rb.gravityScale = 9.8f;
        StartCoroutine(DeathCoroutine());
	disableCollision("Player");
        disableCollision("Enemy");
	}
    private IEnumerator DeathCoroutine()
    {
        isDead = true;
        //monsterAnimator.speed = 0.3;
        monsterAnimator.SetBool("Dead", true);
	GameObject player = GameObject.FindGameObjectWithTag("Player");
	Physics2D.IgnoreCollision(player.GetComponent<CapsuleCollider2D>(), GetComponent<CapsuleCollider2D>());
        yield return new WaitForSeconds(deathAnimDelay);
        //Destroy(gameObject);
	gameObject.SetActive(false);
	if(boss){
	    canvasWin.SetActive(true);
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

    private void DealDamageToEnemies()
    {
        Vector3 hitboxDelta;
        if (SR.flipX) { // Character facing left
            hitboxDelta = new Vector3(-3f,0f,0f);
        }else{// Character facing right
            hitboxDelta = new Vector3(1.3f,0f,0f);
        }
        Vector2 attackPosition = transform.position + hitboxDelta; // Center of the attack
        Vector2 attackSize = new Vector2(attackingDistance, 4.0f); // Size of the rectangle (width x height)
        float attackAngle = 0f; // Angle of the rectangle, if needed
        // Perform the overlap box check
        Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(attackPosition, attackSize, attackAngle);

        foreach (Collider2D player in hitPlayers)
        {
            if (player.CompareTag("Player"))
            {
                player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
		if (KBForce != 0){
		    int KBDirection;
		    if (player.transform.position.x <= transform.position.x){
			KBDirection = -1;
		    }
		    else{
			KBDirection = 1;
		    }
		    player.GetComponent<PlayerMovement>().GetKnocked(KBForce, KBDirection);
		}
            }
        }
    }

    public void disableCollision(string tag){
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
	}

    public void takeHit(){
        StartCoroutine(HitCoroutine());
    }

     private IEnumerator HitCoroutine()
    {
        isHit = true;
        monsterAnimator.SetTrigger("Hit");
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

    void Shoot(){
	Vector2 bullSpawn = bulletPos.position;
	if(SR.flipX)
	    bullSpawn -= new Vector2(horizBullFlip,0f);
	Instantiate(bullet, bullSpawn, Quaternion.identity);
    }

}
