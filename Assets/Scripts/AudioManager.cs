using UnityEngine.Audio;
using UnityEngine;


public class AudioManager : MonoBehaviour
{

  public Sound[] sounds;

  public static AudioManager instance;

  // Use this for initialization
  private void Awake()
  {
    if (instance == null)
      instance = this;
    else
    {
      Destroy(gameObject);
      return;
    }

    //No interruption beetween scenes(?)
    DontDestroyOnLoad(gameObject);

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
  }

  public static void Play(string name, bool force = true)
  {
    Sound s = System.Array.Find(instance.sounds, sound => sound.name == name);
    if (s == null)
      return;
    if (force || !s.source.isPlaying)
    {
      s.source.Play();
    }
  }

  public static void Stop(string name)
  {
    Sound s = System.Array.Find(instance.sounds, sound => sound.name == name);
    if (s == null)
      return;
    s.source.Stop();
  }
}
