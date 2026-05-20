using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Space(10), Header("Audiosource")]
    public AudioSource AudioSource;

    [Space(10), Header("SoundBanks")]
    public List<SoundBank> SoundBanks;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (AudioSource == null)
        {
            if(TryGetComponent<AudioSource>(out AudioSource audiosource))
            {
                AudioSource = audiosource;
            }
            else
            {
                gameObject.AddComponent<AudioSource>();
            }
        }
    }

    private void PlayRandomSound(SoundBank soundbank)
    {
        if (AudioSource == null || soundbank == null || soundbank.SoundClips.Length == 0)
            return;

        AudioSource.PlayOneShot(soundbank.SoundClips[Random.Range(0, soundbank.SoundClips.Length)]);
    }


}
