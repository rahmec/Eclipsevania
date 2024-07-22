using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class NewMenu : MonoBehaviour
{

    PlayerMoney playermoney;
    PlayerHealth playerhealth;
    PlayerAttack playerattack;
    public GameObject panel;

    void Start() {
    playermoney = FindObjectOfType<PlayerMoney>(); // Ottieni l'istanza di PlayerMoney nel mondo
    playerhealth = FindObjectOfType<PlayerHealth>();
    playerattack = FindObjectOfType<PlayerAttack>();
    }



    public void PlayGame(){
        SceneManager.LoadScene("SampleScene");
        //SceneManager.UnloadScene("NewMenu");
    }

    public void OnResume(){
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnQuit(){
	Application.Quit();
    }

    public void OnMainMenu(){
        Debug.Log("cliccato");
        SceneManager.LoadScene("NewMenu");
        Time.timeScale = 1f;
        //SceneManager.UnloadScene(sceneName: "SampleScene");
    }

    

    public void AumentaAttacco(){
        if (playermoney.money >= 10){
            playermoney.money -= 10;
            playerattack.damage += 1;
        }
        else {
            Debug.Log("monete insufficenti");
        }
        }

    public void AumentaVita(){
        if (playermoney.money >= 10){
            playermoney.money -= 10;
            playerhealth.maxHealth += 10;
	    GameObject healthbar = GameObject.FindGameObjectWithTag("Health");
	    Slider slider = healthbar.GetComponent<Slider>();
	    slider.maxValue += 10;
	    
        }
        else {
            Debug.Log("monete insufficenti");
        }
        }
        
}
