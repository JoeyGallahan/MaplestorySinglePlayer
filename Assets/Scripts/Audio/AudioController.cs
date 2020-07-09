using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudioController : MonoBehaviour
{
    AudioSource source;
    [SerializeField] List<AudioClip> sfx = new List<AudioClip>();

    private static AudioController instance = null;
    private static readonly object padlock = new object();

    AudioController() { }

    public static AudioController Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new AudioController();
                }
                return instance;
            }
        }
    }

    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(int index)
    {
        source.PlayOneShot(sfx[index]);
    }

    public void PlayMusic()
    {

    }
}
