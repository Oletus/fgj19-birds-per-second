using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSynchronizer : MonoBehaviour
{
    public static BeatSynchronizer Instance { get; private set; }

    public float BeatsPerMinute = 90.0f;

    private double BeatIntervalSeconds { get { return 60.0d / BeatsPerMinute; } }

    private AudioSource SyncBasis;
    private List<AudioSource> PlayingLoopingSources;

    private void Awake()
    {
        Instance = this;
        PlayingLoopingSources = new List<AudioSource>();
    }

    public void StartSyncing(AudioSource source)
    {
        SyncBasis = source;
        SyncBasis.Play();
    }

    public void PlayOnNextBeat(AudioSource source, bool loop)
    {
        source.loop = loop;
        if ( loop )
        {
            PlayingLoopingSources.Add(source);
        }
        if ( SyncBasis != null && !SyncBasis.isPlaying )
        {
            SyncBasis = null;
        }
        if ( SyncBasis == null )
        {
            StartSyncing(source);
            return;
        }
        double timeElapsed = (double)SyncBasis.timeSamples / SyncBasis.clip.frequency;
        double offsetFromBeat = timeElapsed % BeatIntervalSeconds;
        source.PlayScheduled(AudioSettings.dspTime + BeatIntervalSeconds - offsetFromBeat);
    }

    public void StopAllLoopingSounds()
    {
        foreach (var source in PlayingLoopingSources)
        {
            source.loop = false;
        }
        SyncBasis = null;
    }
}
