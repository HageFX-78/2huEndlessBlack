using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;


    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.src = gameObject.AddComponent<AudioSource>();
            s.src.clip = s.clip;
            s.src.volume = s.volume;
            s.src.pitch = s.pitch;
        }
    }
    public void plyAudio(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.src.Play();
    }

}
