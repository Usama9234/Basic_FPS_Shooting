using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    static AudioSource audioSource;

    public AudioClip reload;

    void Start()
    {
        audioSource= GetComponent<AudioSource>();
    }

    public void PlaySound(string soundClip)
    {
        if(soundClip == "reload")
            audioSource.PlayOneShot(reload);
    }


}
