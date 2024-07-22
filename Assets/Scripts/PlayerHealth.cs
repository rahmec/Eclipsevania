using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth = 10f;
    public float health;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] int timer;
    [SerializeField] int healthRegenWait = 10;
    Animator playerAnimator;
    public Slider healtslider;

    public GameObject panel;

    void Start()
    {
        health = maxHealth;
        playerAnimator = GetComponent<Animator>();
        healtslider.value = health;
	StartCoroutine(RegenTimerCoroutine());
    }

    void Update() {
        healtslider.value = health;
        
    }

    public float GetHealth(){
        return health;
    }

    private IEnumerator RegenTimerCoroutine()
    {
	while(health > 0){
	    yield return new WaitForSeconds(1f);
	    timer+=1;
	    if (timer >= healthRegenWait){
		if(health < maxHealth)
		    StartCoroutine(HealthRegenCoroutine());
		timer = 0;
	    }
	}
    }

    private IEnumerator HealthRegenCoroutine()
    {
	float totalHealthRegen = maxHealth - health;
	float regenPerSlice = totalHealthRegen/40;
	for(int i = 0; i < 40; i++){
	    if(health <= 0)
		break;
	    health += regenPerSlice;
	    healtslider.value = health;
	    yield return new WaitForSeconds(0.1f);
	}
    }
    
    public void TakeDamage(float damage) {
	if(!playerMovement.blocking){
	    playerMovement.takeHit();
	    health -= damage;
	    StartCoroutine(SoundCoroutine("Hit"));
	    timer = 0;
	    if (health <= 0) {
		playerMovement.Die();
		panel.SetActive(true);
	    }
	}
    }

    private IEnumerator SoundCoroutine(string name)
    {
	FindObjectOfType<AudioManager>().Play(name);
        yield return new WaitForSeconds(0.1f);
    }


}
