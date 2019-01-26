﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(AudioSource))]
public class Birdhouse : MonoBehaviour
{
    [SerializeField] private GridConfig Config;

    public enum State
    {
        Detached,
        PickedUp,
        AttachedToTree
    }

    public List<BirdhouseSegment> Segments;
    public int SegmentCount { get { return (Segments != null) ? Segments.Count : 0; } }
    private AudioSource AudioSource;

    public int PlacementGridY;
    public bool OnRightSide;
    private Light Light;

    // Start is called before the first frame update
    void Awake()
    {
        Segments = new List<BirdhouseSegment>(GetComponentsInChildren<BirdhouseSegment>());
        AudioSource = GetComponent<AudioSource>();
        Light = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( !Application.isPlaying )
        {
            Segments = new List<BirdhouseSegment>(GetComponentsInChildren<BirdhouseSegment>());
        }

        int i = 0;
        foreach (BirdhouseSegment segment in Segments)
        {
            segment.transform.localPosition = Vector3.zero + Vector3.up * i * Config.SegmentHeight;
            if ( segment.GetComponent<SpriteRenderer>() )
            {
                segment.GetComponent<SpriteRenderer>().sortingOrder = i;
            }
            ++i;
        }

        if ( !Application.isPlaying )
        {
            return;
        }

        // Gameplay logic here.
    }

    public bool Intersects(int otherSegmentCount, int otherGridY, bool otherOnRightSide)
    {
        if ( otherOnRightSide != OnRightSide )
        {
            return false;
        }
        return (otherGridY + otherSegmentCount > PlacementGridY && PlacementGridY + SegmentCount > otherGridY);
    }

    public void SetAttachedTransform(Tree tree, int placementGridY, bool onRightSide, bool preview)
    {
        PlacementGridY = placementGridY;
        OnRightSide = onRightSide;
        transform.position = tree.GetAttachmentPosition(placementGridY, onRightSide, preview);
        transform.rotation = tree.GetAttachmentRotation(onRightSide);
    }

    private void OnBeat()
    {
        if ( Light != null )
        {
            StartCoroutine(FadeOutLight((float)BeatSynchronizer.Instance.AudioSyncBeatIntervalSeconds - 2.0f / 60.0f));
        }
    }

    private IEnumerator FadeOutLight(float beatDuration)
    {
        float startTime = Time.time;
        while ( Time.time < startTime + beatDuration )
        {
            float beatMult = 1.0f - (Time.time - startTime) / beatDuration;
            float audioTimeMult = 1.0f - AudioSource.time / AudioSource.clip.length;
            Light.intensity = audioTimeMult * beatMult * 3.0f;
            yield return null;
        }
    }

    public void OnAttached()
    {
        BeatSynchronizer.Instance.PlayOnNextBeat(AudioSource, true, OnBeat);
    }
}
