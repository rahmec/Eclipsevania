using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject panel;

    public Canvas canvashop;


    // Start is called before the first frame update
    public void Onpause(InputAction.CallbackContext context)
    {
        Debug.Log("premuto ESC");
        if (context.performed)
        {
            if (panel.activeSelf)
            {
                // Se il pannello è attivo, disattivalo e riprendi il gioco
                panel.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                Debug.Log("pannello pause non è attivo");
                if (canvashop.gameObject.activeSelf){
                    Debug.Log("attivo lo shop");
                    canvashop.gameObject.SetActive(false);
                    Time.timeScale = 1f;
                }
                else{

                    // Se il pannello è inattivo, attivalo e metti in pausa il gioco
                    panel.SetActive(true);
                    Time.timeScale = 0f;
                }
                
            }
        }
    }
}

