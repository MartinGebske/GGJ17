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

    public AudioClip[] ClickSounds;

    public AudioClip[] ExplodeSounds;
    public AudioClip[] LunteSounds;
    public AudioClip[] GameOverSounds;

    public AudioClip[] CarSounds;

    public AudioClip BottlePickup;
    public AudioClip IngredientPickup;

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

    public void PlayHotDog(float volume = 1f)
    {
        PlayRandomSound(ref WischSounds, volume);
    }

    public void PlayGameOver()
    {
        PlayRandomSound(ref GameOverSounds);
    }

    public void PlayLunte()
    {
        PlayRandomSound(ref LunteSounds);
    }
    public void PlayExplode()
    {
        PlayRandomSound(ref ExplodeSounds);
    }

    public void PlayCar()
    {
        PlayRandomSound(ref CarSounds, 0.5f);
    }

    public void PlayPickupBottle()
    {
        AudioSource.PlayClipAtPoint(BottlePickup, Camera.main.transform.position, 0.6f);
    }

    public void PlayPickupIngredient()
    {
        AudioSource.PlayClipAtPoint(IngredientPickup, Camera.main.transform.position, 0.6f);
    }

    private void PlayRandomSound(ref AudioClip[] fromSounds, float volume = 1f)
    {
        AudioClip chosenClip = fromSounds[Random.Range(0, fromSounds.Length)];

        AudioSource.PlayClipAtPoint(chosenClip, Camera.main.transform.position, volume);
    }
}
