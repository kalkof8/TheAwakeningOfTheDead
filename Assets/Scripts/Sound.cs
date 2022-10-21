using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioDie;
    [SerializeField] private AudioSource _audioBattle;
    [SerializeField] private AudioSource _audioAttackBuilding;
    [SerializeField] private AudioSource _music;

    public void SetMusic(bool value)
    {
        _music.enabled = value;
    }
    public void SetValueMusic(float value)
    {
        _music.volume = value;
    }
    public void SetValueVolume(float value)
    {
        AudioListener.volume = value;
    }
    public void StartAudioDie()
    {
        _audioDie.Play();
    }

    public void StartAudioBattle()
    {
        _audioBattle.Play();
    }
    public void StartAudioAttackBuilding()
    {
        _audioAttackBuilding.Play();
    }
}
