using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 10f;
    [SerializeField] float health;

    MonsterMovement monsterMovement;
    GhoulMovement ghoulMovement;
    Animator monsterAnimator;

    public Slider healtslider;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healtslider.value = health;
        monsterAnimator = GetComponent<Animator>();
        monsterMovement = GetComponent<MonsterMovement>();
        ghoulMovement = GetComponent<GhoulMovement>();
    }

     public void TakeDamage(float damage) {
	if (monsterMovement != null)
	    monsterMovement.takeHit();
        health -= damage;
        if (health <= 0) {
	    if (monsterMovement != null)
		monsterMovement.Die();
	    else if (ghoulMovement != null)
		ghoulMovement.Die();
        }
    }

     public float GetHealth(){
	 return health;
     }
    // Update is called once per frame
    void Update()
    {
        healtslider.value = health;
    }
}
