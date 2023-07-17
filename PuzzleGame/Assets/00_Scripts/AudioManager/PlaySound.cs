using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField] private string AudioName;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        Play(AudioName);
    }

    public void Play(string sound)
    {
        audioManager.Play(sound);
    }

}
