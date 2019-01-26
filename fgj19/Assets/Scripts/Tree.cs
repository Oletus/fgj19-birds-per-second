using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private int _Height;

    [SerializeField] private int _MaxBirdhouses = 1;

    private List<Birdhouse> Birdhouses;

    public bool HasBirdHouse { get { return Birdhouses.Count != 0; } }

    private void Awake()
    {
        Birdhouses = new List<Birdhouse>();
    }

    private float GetWidthAtY(int y)
    {
        return 1.0f;
    }

    public Vector3 GetAttachmentPosition(int placementY, bool onRightSide, bool preview = false)
    {
        float y = GlobalConfig.Instance.GridConfig.SegmentHeight * placementY;
        float horizontalOffset = GetWidthAtY(placementY) * 0.5f - 0.05f;
        if ( preview )
        {
            horizontalOffset += 0.3f;
        }
        if ( !onRightSide )
        {
            horizontalOffset = -horizontalOffset;
        }

        return transform.position + Vector3.up * y + Vector3.right * horizontalOffset + Vector3.back * 0.6f;
    }

    public bool CanBeAttached(int segmentCount, int placementGridY, bool onRightSide)
    {
        if (Birdhouses.Count >= _MaxBirdhouses)
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
        house.OnAttached();
    }
}
