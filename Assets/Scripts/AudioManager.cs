using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    void Awake()
    {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audio;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }       
    }

    void Start(){
    }

    public void Play(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
	if(s == null)
	    Debug.Log("DIOMPESHTATO");
	else
	    Debug.Log(s.name);
        s.source.Play();
    }


}
