using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipPicker : MonoBehaviour
{
    [SerializeField] private AudioClipSet AudioClipSet;

    private List<int> UnconsumedIndices;

    private void Awake()
    {
        UnconsumedIndices = new List<int>();
    }

    // Will return all clips in a random order. Once all clips have been returned once, will start returning the same ones again in another random order.
    public AudioClip GetNext()
    {
        if (UnconsumedIndices.Count == 0)
        {
            for (int i = 0; i < AudioClipSet.Clips.Count; ++i )
            {
                UnconsumedIndices.Add(i);
            }
        }
        int indexIndex = Random.Range(0, UnconsumedIndices.Count);
        int clipIndex = UnconsumedIndices[indexIndex];
        UnconsumedIndices.RemoveAt(indexIndex);
        return AudioClipSet.Clips[clipIndex];
    }
}
