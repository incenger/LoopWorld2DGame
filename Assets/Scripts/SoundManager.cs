using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioClip clickSound, jumpSound, attackSound, finishSound;
    static AudioSource audioSource;

    void Awake()
    {
        clickSound = Resources.Load<AudioClip>("Click");
        jumpSound = Resources.Load<AudioClip>("Jump");
        attackSound = Resources.Load<AudioClip>("Attack");
        finishSound = Resources.Load<AudioClip>("Finish");
        audioSource = GetComponent<AudioSource>();
    }

    
    public static void PlaySound(string clipName)
    {
        switch (clipName)
        {
            case "Click":
                audioSource.PlayOneShot(clickSound);
                break;
            case "Jump":
                audioSource.PlayOneShot(jumpSound);
                break;
            case "Attack":
                audioSource.PlayOneShot(attackSound);
                break;
            case "Finish":
                audioSource.PlayOneShot(finishSound);
                break;
            default:
                break;

        }
    }
}
