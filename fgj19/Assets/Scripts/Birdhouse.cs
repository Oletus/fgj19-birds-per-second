using System.Collections;
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
    [SerializeField] private bool UseRandomSound = true;

    public int GridY;
    public bool OnRightSide;
    private Light Light;
    private ParticleSystemForceField ForceField;

    [System.NonSerialized] public int GameStage;

    public float ParticleIntensity { get; private set; }

    private Animator Animator;

    // Start is called before the first frame update
    void Awake()
    {
        Segments = new List<BirdhouseSegment>(GetComponentsInChildren<BirdhouseSegment>());
        AudioSource = GetComponent<AudioSource>();
        Light = GetComponentInChildren<Light>();
        ForceField = GetComponentInChildren<ParticleSystemForceField>();
        Animator = GetComponentInChildren<Animator>();
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
        return (otherGridY + otherSegmentCount > GridY && GridY + SegmentCount > otherGridY);
    }

    public void SetAttachedTransform(Tree tree, int placementGridY, bool onRightSide, bool preview)
    {
        GridY = placementGridY;
        OnRightSide = onRightSide;
        transform.position = tree.GetAttachmentPosition(placementGridY, onRightSide, preview);
        transform.rotation = tree.GetAttachmentRotation(onRightSide);
    }

    private void OnBeat()
    {
        StartCoroutine(FadeOutFX((float)BeatSynchronizer.Instance.AudioSyncBeatIntervalSeconds - 2.0f / 60.0f));
        if ( Animator )
        {
            Animator.SetTrigger("Beat");
        }
        GameStages gameStages = FindObjectOfType<GameStages>();
        if ( gameStages )
        {
            gameStages.TriggerAnimations("FirstBirdHouse", GameStage);
        }
    }

    private IEnumerator FadeOutFX(float beatDuration)
    {
        float startTime = Time.time;
        while ( Time.time < startTime + beatDuration )
        {
            float beatMult = 1.0f - (Time.time - startTime) / beatDuration;
            float audioTimeMult = 1.0f - AudioSource.time / AudioSource.clip.length;
            if ( ForceField != null )
            {
                ForceField.directionY = audioTimeMult * beatMult * 200.0f;
                ForceField.rotationSpeed = audioTimeMult * beatMult * 15.0f;
            }
            if ( Light != null )
            {
                Light.intensity = audioTimeMult * beatMult * 3.0f;
            }
            ParticleIntensity = audioTimeMult * beatMult;
            yield return null;
        }
        ParticleIntensity = 0.0f;
        if ( Light )
        {
            Light.intensity = 0.0f;
        }
        if ( ForceField )
        {
            ForceField.directionY = 0.0f;
            ForceField.rotationSpeed = 0.0f;
        }
    }

    public void OnAttached()
    {
        if ( GlobalConfig.Instance.BirdhouseAudioClipPicker != null && UseRandomSound )
        {
            AudioSource.clip = GlobalConfig.Instance.BirdhouseAudioClipPicker.GetNext();
        }
        BeatSynchronizer.Instance.PlayOnNextBeat(AudioSource, true, OnBeat);
    }
}
