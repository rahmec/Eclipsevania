using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;

public class EnterCastle : MonoBehaviour
{

    public TransitionSettings transition;
    TransitionManager manager;
    float delay = 0f;

    // Metodo chiamato quando un altro collider entra in questo trigger
    public void StartSwamp()
    {
        TransitionManager.Instance().Transition("SwampScene", transition, delay);
    }

    public void StartTown()
    {
        TransitionManager.Instance().Transition("TownScene", transition, delay);
    }

    public void StartChurch()
    {
        TransitionManager.Instance().Transition("ChurchScene", transition, delay);
    }

}
