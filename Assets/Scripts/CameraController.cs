using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public CinemachineVirtualCamera virtualCamera;
    public Transform character;
    public float runSpeed = 5f;
    public Vector3 runTargetPosition;
    bool arrived = false;
    public Vector3 camInitialPosition;
    public Vector3 playerInitialPosition;

    Animator playerAnimator;


    [SerializeField] PlayerMovement player;


    


    void Start()
    {
        player.SetSync(false);
        // Assicurati che la camera non stia seguendo il personaggio all'inizio
        virtualCamera.Follow = null;
        Transform myTransform = this.transform;
        //myTransform.position = new Vector3(1.34f, 1.286464f, -10.0f);
	//myTransform.position = startPosition.transform.position;
	myTransform.position = camInitialPosition;
	player.playerAnimator.SetBool("Run", true);
        // Posiziona il personaggio fuori dallo schermo
        character.position = playerInitialPosition; // Regola questa posizione come necessario
        
    }

    

    void Update()
    {
        // Muovi il personaggio verso la posizione target
        if (Mathf.Abs(character.position.x - runTargetPosition.x) > 0.2f && !arrived){
            character.position = Vector2.MoveTowards(character.position, new Vector2(runTargetPosition.x, character.position.y), runSpeed * Time.deltaTime);
        }else{
            arrived = true;
            //player.playerAnimator.SetBool("Running", false);
            player.SetSync(true);
            
            virtualCamera.Follow = character; // La camera segue il personaggio
            // Permetti al giocatore di riprendere il controllo
            // Puoi attivare altre logiche una volta che il personaggio si è fermato
        }
    }
}
