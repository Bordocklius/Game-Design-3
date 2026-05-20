using UnityEngine;

[CreateAssetMenu(fileName = "SoundBank", menuName = "Audio/SoundBank")]
public class SoundBank : ScriptableObject
{
    public AudioClip[] SoundClips;
}
