using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

  public static AudioManager instance;

  public AudioSource sfx;
  public AudioSource bgm;

	// Use this for initialization
	void Start () {
    instance = this;
	}
	
  private void playSound(AudioClip sound, float volume = 1f, float pitch = 1f)
  {
    if(sfx.isPlaying)
    {
      sfx.Stop();
    }
    sfx.clip = sound;
    sfx.volume = volume;
    sfx.pitch = pitch;
    sfx.Play();
  }

  public static void play(AudioClip sound, float volume = 1f, float pitch = 1f)
  {
    instance.playSound(sound, volume, pitch);
  }
}
