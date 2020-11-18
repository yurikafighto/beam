using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip bulletSound, laserSound, starSound, barrierSound;
    [SerializeField]
    private GameObject player;

    private void soundBullet()
    {
        AudioSource audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        audioSource.volume = 0.05f;
        audioSource.PlayOneShot(bulletSound);
    }
    private void soundLaser()
    {
        AudioSource audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(laserSound);
    }
    private void soundStar()
    {
        AudioSource audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(starSound);
    }
    private void soundBarrier()
    {
        AudioSource audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(barrierSound);
    }

    protected void Awake()
    {
        Player.soundBullet = soundBullet;
        Player.soundLaser = soundLaser;
        Player.soundStar = soundStar;
        Player.soundBarrier = soundBarrier;

    }
}
