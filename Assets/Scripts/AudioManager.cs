using UnityEngine.Audio;
using UnityEngine;


public class AudioManager : MonoBehaviour
{

  public Sound[] sounds;
  public static AudioManager audioManager;

  // Use this for initialization
  private void Awake()
  {
    if (audioManager == null)
      audioManager = this;
    else
    {
      Destroy(gameObject);
      return;
    }

    foreach (Sound s in sounds)
    {
      s.source = gameObject.AddComponent<AudioSource>();
      s.source.clip = s.clip;
      s.source.volume = s.volume;
      s.source.pitch = s.pitch;
      s.source.loop = s.loop;
      s.source.playOnAwake = s.playOnWake;
    }
   
  }

  private void Start()
  {
    Play("Theme");
    Play("Background1");
  }

  public Sound this[string name]
  {
    get
    {
      foreach(Sound s in sounds)
      {
        if (s.name == name)
          return s;
      }
      return null;
    }
  }
  public static void Play(string name, bool force = true)
  {
    Sound s = audioManager[name];
    if (s == null)
      return;

    if (force || !s.source.isPlaying)
    {
      s.source.Play();
    }
  }

  public static void Stop(string name)
  {
    Sound s = audioManager[name];

    if (s == null)
      return;

    s.source.Stop();
  }
  public static bool IsPlaying(string name)
  {
    Sound s = audioManager[name];
    return s.source.isPlaying;
  }
}
