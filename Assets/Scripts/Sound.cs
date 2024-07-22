using UnityEngine.Audio;
using UnityEngine;

// Custom Class for Sound to be played with Audio Manager
[System.Serializable]
public class Sound
{

    public AudioClip audio;
    [HideInInspector] //lo nascondiamo dall'inspector dato che lo popoliamo da codice
    public AudioSource source;
    public string name;
    [Range(0f,1f)]
    public float volume;
    [Range(0f,1f)]
    public float pitch;


}
