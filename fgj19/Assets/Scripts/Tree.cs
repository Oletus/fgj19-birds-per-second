﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private int _Height;

    [SerializeField] private int _MaxBirdhouses = 1;

    [System.NonSerialized] public int GridX;

    [System.NonSerialized] public bool LeftSideActive = true;
    [System.NonSerialized] public bool RightSideActive = true;

    private ParticleSystem ParticleSystem;

    public List<Birdhouse> Birdhouses;

    public bool HasBirdHouse { get { return Birdhouses.Count != 0; } }

    private void Awake()
    {
        Birdhouses = new List<Birdhouse>();
        ParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private float GetWidthAtY(int y)
    {
        return 0.4f;
    }

    public Vector3 GetAttachmentPosition(int placementY, bool onRightSide, bool preview = false)
    {
        float y = GlobalConfig.Instance.GridConfig.SegmentHeight * placementY;
        float horizontalOffset = GetWidthAtY(placementY);
        if ( preview )
        {
            horizontalOffset += 0.3f;
        }

        return transform.position + Vector3.up * y + GetAttachmentFrontDirection(onRightSide).normalized * horizontalOffset;
    }

    private Vector3 GetAttachmentFrontDirection(bool onRightSide)
    {
        if ( onRightSide )
        {
            return Vector3.back + Vector3.right;
        }
        else
        {
            return Vector3.back + Vector3.left;
        }
    }

    public Quaternion GetAttachmentRotation(bool onRightSide)
    {
        return Quaternion.LookRotation(GetAttachmentFrontDirection(onRightSide));
    }

    public bool CanBeAttached(int segmentCount, int placementGridY, bool onRightSide)
    {
        if (Birdhouses.Count >= _MaxBirdhouses)
        {
            return false;
        }
        if (onRightSide && !RightSideActive)
        {
            return false;
        }
        if (!onRightSide && !LeftSideActive)
        {
            return false;
        }
        foreach ( Birdhouse house in Birdhouses )
        {
            if ( house.Intersects(segmentCount, placementGridY, onRightSide) )
            {
                return false;
            }
        }
        return true;
    }

    public void PreviewBirdhouse(Birdhouse house, int placementGridY, bool onRightSide)
    {
        house.SetAttachedTransform(this, placementGridY, onRightSide, true);
    }

    public void AttachBirdhouse(Birdhouse house, int placementGridY, bool onRightSide)
    {
        house.SetAttachedTransform(this, placementGridY, onRightSide, false);
        Birdhouses.Add(house);
        house.OnAttached(this);
        if (Score.Instance)
        {
            Score.Instance.OnBirdHousesChanged();
        }
    }

    public void DetachHouse(Birdhouse house)
    {
        house.OnDetached();
        Birdhouses.Remove(house);
    }

    private void Update()
    {
        float totalParticleIntensity = 0.0f;
        float minY = 100.0f;
        foreach ( Birdhouse house in Birdhouses )
        {
            totalParticleIntensity += house.ParticleIntensity;
            minY = Mathf.Min(minY, house.transform.position.y);
        }
        Vector3 particlePos = transform.position;
        particlePos.y = minY - 1.0f;
        ParticleSystem.transform.position = particlePos;
        ParticleSystem.EmissionModule emission = ParticleSystem.emission;
        emission.rateOverTime = totalParticleIntensity * 100.0f;
    }
}
