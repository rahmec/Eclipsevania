using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Speed of horizontaly Movement")]
    [Range(5, 15)]
    [SerializeField] float speed = 1;
    [SerializeField] GameObject canvasShop;
    private Vector2 movementInput=Vector2.zero;
    
    [Header("Jumping")]
    [Tooltip("Jumping height")]
    [Range(0, 100)]
    [SerializeField] float thrust = 300;
    bool grounded = true;
    bool jumped;
    bool knocked = false;
    public bool dead = false;
    bool attacking = false;
    public bool hit = false;
    public bool blocking = false;

    bool sync = true;

    Rigidbody2D rb;
    public Animator playerAnimator;
    CameraController camera;
    SpriteRenderer SR;

    //OnMove checks if controlls for the movement are pressed
    public void OnMove(InputAction.CallbackContext context){
    	movementInput = context.ReadValue<Vector2>();
    }

    
    //OnJump checks if controlls for the jumping are pressed
    public void OnJump(InputAction.CallbackContext context){
    	jumped = context.action.triggered;
    }

    public void GetKnocked(float KBForce, float KBDirection){
	    rb.velocity = new Vector2(KBForce*KBDirection, KBForce);
	    knocked = true;
    }

    public void Die(){
	    playerAnimator.SetBool("Death", true);
	    StartCoroutine(SoundCoroutine("Death"));
	    playerAnimator.SetTrigger("Die");
	    dead = true;
	    disableCollision("Enemy");
    }
    //Jump is the class that includes jumping mechanics
    void Jump()
    {
	    //If jump is pressed and the player is on the ground, execute jumping
	    if(jumped && grounded){
		//This will add a force to the players rigidbody, so the player will jump
		Debug.Log("Jumpato");
		rb.AddForce(transform.up*thrust*100);
		//Player is now in the air, therefore he cant jump anymore. Set this true again, if player hits the floor.
		grounded=false;
		playerAnimator.SetTrigger("Jump");
	    }
    }
	
    // Start is called before the first frame update
    void Start()
    {
        //Get the rigidbody attached to the player
        rb = GetComponent<Rigidbody2D> ();
        playerAnimator = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
    }

    public void OnBlock(InputAction.CallbackContext context){
	if (context.performed){
	    playerAnimator.SetTrigger("Block");
	    StartCoroutine(SoundCoroutine("Block"));
	    StartCoroutine(BlockCoroutine());
	}
    }

    private IEnumerator BlockCoroutine()
    {
	blocking = true;
        playerAnimator.SetBool("IdleBlock", true);
	while(Input.GetMouseButton(2)){
	    Debug.Log("Blocking");
	    yield return null;
	}
	blocking = false;
        playerAnimator.SetBool("IdleBlock", false);
    }

    public void OnRoll(InputAction.CallbackContext context){
	if(context.performed){
	    playerAnimator.SetTrigger("Roll");
	    StartCoroutine(SoundCoroutine("Roll"));
	    StartCoroutine(RollCoroutine());
	    Debug.Log("Rolling");
	}
    }

    private IEnumerator RollCoroutine()
    {
	disableCollision("Enemy");
	disableCollision("Bullet");
        yield return new WaitForSeconds(0.8f);
	enableCollision("Enemy");
	enableCollision("Bullet");
    }
    
    //Move is the class that includes running mechanics
	void Move()
    {
		//Add force to the players rigidbody in the direction, of the input in movementInput.x
		rb.velocity = new Vector2 ( movementInput.x*speed, rb.velocity.y );
        if(Mathf.Abs(movementInput.x)>0.01f){ 
            playerAnimator.SetBool("Run", true);
			Debug.Log("passato");
        }
        else {
            playerAnimator.SetBool("Run", false);
        }
        if(movementInput.x>0.01f) SR.flipX = false;
        if(movementInput.x<-0.01f) SR.flipX = true;
    }
	

    // Update is called once per frame
    void FixedUpdate()
    {
	if (!dead){
		playerAnimator.SetBool("Grounded", grounded);
		playerAnimator.SetFloat("Velocity", rb.velocity.y);
		if (!hit && !blocking && !knocked && !attacking && sync){
			Jump();
			Move();
		}
	}
    }
    public void OnShop(InputAction.CallbackContext context){
	Debug.Log("cliccato");
	Vector2 attackPosition = transform.position; // Center of the attack
	Vector2 attackSize = new Vector2(10f, 10f); // Size of the rectangle (width x height)
	float attackAngle = 0f; // Angle of the rectangle, if needed
	// Perform the overlap box check
	Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, attackAngle);
	foreach (Collider2D enemy in hitEnemies)
	{
	    if (enemy.CompareTag("Shop") && context.performed)
	    {
				if(canvasShop.activeSelf){
					canvasShop.SetActive(false);
			Time.timeScale = 1f;
				}
				else{
					canvasShop.SetActive(true);
			Time.timeScale = 0f;
				}
		
	    }
	}
    }

    public void setAttacking(bool flag){
	    attacking = flag;
    }
    
    // OnCollisionEnter2D is called when the object collides with an collider of another object
    void OnCollisionEnter2D(Collision2D col){
    	grounded=true;
		knocked=false;
    	//checks wethere the collision is made with the floor
    	//if(col.gameObject.tag=="Floor"){
    		
    		//This bool will make jumping possible if true
    		//grounded=true;    	}
    }

	public void disableCollision(string tag){
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(tag), true);
	}
	public void enableCollision(string tag){
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(tag), false);
	}

	public void takeHit(){
        StartCoroutine(HitCoroutine());
    }

    private IEnumerator SoundCoroutine(string name)
    {
	FindObjectOfType<AudioManager>().Play(name);
        yield return new WaitForSeconds(0.1f);
    }

     private IEnumerator HitCoroutine()
    {
        hit = true;
        playerAnimator.SetTrigger("Hit");
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(0.5f);
        hit = false;
    }

	public void SetSync(bool var){
		sync = var;
	}
}
