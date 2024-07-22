using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerCastle : MonoBehaviour
{
    [SerializeField] EnterCastle enterCastle;
    [SerializeField] int scene = 0;
    [SerializeField] GameObject monsterLock;
    // Start is called before the first frame update
    //
    private bool locked = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	if(monsterLock == null || monsterLock.GetComponent<MonsterHealth>().GetHealth() <= 0){
	    GetComponent<BoxCollider2D>().isTrigger = true;
	    locked = false;
	}
        
    }
     private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se il collider appartiene al player
	
	if(!locked){
	    if (other.CompareTag("Player"))
	    {
		switch (scene) {
		    case 0: enterCastle.StartSwamp(); break;
		    case 1: enterCastle.StartTown(); break;
		    case 2: enterCastle.StartChurch(); break;
		    default: enterCastle.StartSwamp(); break;
		}
	    }
	}
    }
}
