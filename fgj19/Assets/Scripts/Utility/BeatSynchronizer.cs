using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSynchronizer : MonoBehaviour
{
    public static BeatSynchronizer Instance { get; private set; }

    public float AudioSyncBeatsPerMinute = 360.0f;
    public float CallbackBeatsPerMinute = 90.0f;

    public double AudioSyncBeatIntervalSeconds { get { return 60.0d / AudioSyncBeatsPerMinute; } }
    public double CallbackBeatIntervalSeconds { get { return 60.0d / CallbackBeatsPerMinute; } }

    private class PlayingSound
    {
        public PlayingSound(AudioSource source, OnBeatCallbackFunction onBeatCallback)
        {
            Source = source;
            OnBeatCallback = onBeatCallback;
        }

        public void Play()
        {
            Source.Play();
            NextBeatTime = AudioSettings.dspTime;
        }

        public void PlayScheduled(double dspTime)
        {
            Source.PlayScheduled(dspTime);
            NextBeatTime = dspTime;
        }

        public AudioSource Source;
        public double NextBeatTime;
        public OnBeatCallbackFunction OnBeatCallback;
    }

    // TODO: Change the sync basis automatically to another playing sound if the sync basis stops but other sounds are still playing.
    private AudioSource SyncBasis;
    private List<PlayingSound> PlayingSounds;

    private double NextBeatTime;

    public delegate void OnBeatCallbackFunction();

    private void Awake()
    {
        Instance = this;
        PlayingSounds = new List<PlayingSound>();
    }

    private void StartSyncing(PlayingSound sound)
    {
        SyncBasis = sound.Source;
        sound.Play();
    }

    public void PlayOnNextBeat(AudioSource source, bool loop, OnBeatCallbackFunction onBeatCallback = null)
    {
        source.loop = loop;
        var playingSound = new PlayingSound(source, onBeatCallback);
        PlayingSounds.Add(playingSound);
        if ( SyncBasis != null && !SyncBasis.isPlaying )
        {
            SyncBasis = null;
        }
        if ( SyncBasis == null )
        {
            StartSyncing(playingSound);
            return;
        }
        double timeElapsed = (double)SyncBasis.timeSamples / SyncBasis.clip.frequency;
        double offsetFromBeat = timeElapsed % AudioSyncBeatIntervalSeconds;
        playingSound.PlayScheduled(AudioSettings.dspTime + AudioSyncBeatIntervalSeconds - offsetFromBeat);
    }

    private void Update()
    {
        int i = 0;
        while ( i < PlayingSounds.Count )
        {
            var sound = PlayingSounds[i];
            if ( !sound.Source.isPlaying )
            {
                PlayingSounds.Remove(sound);
            }
            else
            {
                ++i;
            }
        }
        OnBeatCallbacks();
    }

    private void OnBeatCallbacks()
    {
        foreach (var sound in PlayingSounds)
        {
            if ( AudioSettings.dspTime > sound.NextBeatTime )
            {
                sound.OnBeatCallback?.Invoke();
                sound.NextBeatTime += CallbackBeatIntervalSeconds;
            }
        }
    }

    public void StopAllLoopingSounds()
    {
        foreach (var sound in PlayingSounds)
        {
            sound.Source.loop = false;
        }
        SyncBasis = null;
    }
}
