using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public VisualElement ui;
    public Button play;
    public Button quit;

    private string gamescene = "SampleScene";

    private void Awake() {
        ui = GetComponent<UIDocument>().rootVisualElement;
        // all'interno della struttura della UI facciamo la query per trovare quello che si chiama start
        play = ui.Q<Button>("Start");
        //quit = ui.Q<Button>("Quit");
        play.clicked += OnStart;
        //quit.clicked += OnQuit;

    }

    private void OnQuit(){
        Application.Quit();
    }

    private void OnStart(){
        SceneManager.LoadScene(sceneName:gamescene);
        SceneManager.UnloadScene(sceneName: "Menu");
        Debug.Log("Umberto è un coglione");
    }
}
