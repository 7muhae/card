using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bgmusic;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = bgmusic; // 오디오소스에 오디오클립 할당
        audioSource.Play(); // 오디오 반복 재생
    }
}
