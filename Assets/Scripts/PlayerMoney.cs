using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerMoney : MonoBehaviour
{

    public int money;

    public TMP_Text text;


    void Update(){
        text.text = money.ToString() + "x";
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Money")){
            Destroy(other.gameObject);
            money += 1;
	    StartCoroutine(SoundCoroutine("Coin"));
        }
    }

    private IEnumerator SoundCoroutine(string name)
    {
	FindObjectOfType<AudioManager>().Play(name);
        yield return new WaitForSeconds(0.1f);
    }


}
