using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
  public string name;

  public AudioClip clip;

  public bool loop;
  public bool playOnWake;

  [Range(0f, 1f)]
  public float volume;
  [Range(.1f, 3f)]
  public float pitch;

  //[System.Serializable]
  //public class Options3D
  //{
  //  [Range(0f, 1f)]
  //  public float spatialBlend;
  //  public float minDistance = 1;
  //  public float maxDistance = 500;
  //  public AudioRolloffMode rolloffMode;
  //}
  //public Options3D options3D;

  [HideInInspector]
  public AudioSource source;
}
