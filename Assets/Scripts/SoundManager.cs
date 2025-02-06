using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;  // 반복 재생을 기본 설정
    }

    public void Play() {
        if (!audioSource.isPlaying) {
            audioSource.clip = audioClip;  // 오디오 클립을 설정
            audioSource.Play();  // 오디오를 재생
        }
    }

    public void Stop() {
        if(audioSource.isPlaying) {
            audioSource.Stop();  // 오디오를 중지
        }
    }
}
