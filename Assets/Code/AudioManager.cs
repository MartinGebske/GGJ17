using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BitStrap.Singleton<AudioManager>
{
    public AudioSource audioSource;

    [Header("Config")]
    public AudioClip[] HappySounds;
    public AudioClip[] AngrySounds;

    public AudioClip[] CashSounds;

    public AudioClip[] OrderSounds;

    public AudioClip[] SplatSounds;
    public AudioClip[] SquishSounds;

    public AudioClip[] WischSounds;

    public void PlayHappyAngry(bool isHappy)
    {
        if (isHappy)
            PlayRandomSound(ref HappySounds);
        else
            PlayRandomSound(ref AngrySounds);
    }

    public void PlayCash()
    {
        PlayRandomSound(ref CashSounds);
    }

    public void PlayOrder()
    {
        PlayRandomSound(ref OrderSounds);
    }

    public void PlayIngredient()
    {
        PlayRandomSound(ref SplatSounds);
    }

    public void PlayBottle()
    {
        PlayRandomSound(ref SquishSounds);
    }

    public void PlayHotDog()
    {
        PlayRandomSound(ref WischSounds);
    }

    private void PlayRandomSound(ref AudioClip[] fromSounds)
    {
        AudioClip chosenClip = fromSounds[Random.Range(0, fromSounds.Length)];

        AudioSource.PlayClipAtPoint(chosenClip, Camera.main.transform.position);
    }
}
