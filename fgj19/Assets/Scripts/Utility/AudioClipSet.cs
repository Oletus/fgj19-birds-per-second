using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipSet", menuName = "Custom/Audio Clip Set")]
public class AudioClipSet : ScriptableObject
{
    [SerializeField] private List<AudioClip> _Clips;
    public List<AudioClip> Clips { get { return _Clips; } }
}
