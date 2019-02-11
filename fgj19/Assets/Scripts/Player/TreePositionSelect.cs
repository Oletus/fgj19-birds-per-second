using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LPUnityUtils;

public class TreePositionSelect : MonoBehaviour
{
    public int MaxPlacementHeight = 10;  // Inclusive.

    public GridConfig GridConfig;

    public Birdhouse ObjectBeingPlaced;

    private Pointer Pointer;

    private PlacedObjectQueue PlacedObjectQueue;

    private void Awake()
    {
        Pointer = new Pointer();
        PlacedObjectQueue = GetComponent<PlacedObjectQueue>();
    }

    private void TryPlaceAtCursor()
    {
        if ( ObjectBeingPlaced != null )
        {
            RaycastHit hitInfo = new RaycastHit();
            if ( !Physics.Raycast(Pointer.GetRay(Camera.main), out hitInfo, 100.0f, 1 << LayerMask.NameToLayer("PlacementCollider")) )
            {
                ObjectBeingPlaced.gameObject.SetActive(false);
                return;
            }

            Tree tree = hitInfo.collider.GetComponentInParent<Tree>();
            if ( !tree )
            {
                ObjectBeingPlaced.gameObject.SetActive(false);
                return;
            }
            float cursorWorldYOffset = hitInfo.point.y - tree.transform.position.y;
            int cursorGridHeight = Mathf.RoundToInt(cursorWorldYOffset / GridConfig.SegmentHeight);

            int placementGridY = Mathf.Max(GridConfig.MinPlacementHeight, Mathf.Min(MaxPlacementHeight, cursorGridHeight));

            bool onRightSide = hitInfo.point.x > tree.transform.position.x;

            if ( tree.CanBeAttached(ObjectBeingPlaced.SegmentCount, placementGridY, onRightSide) )
            {
                if ( Input.GetMouseButtonDown(0) )
                {
                    ObjectBeingPlaced.gameObject.SetActive(true);
                    tree.AttachBirdhouse(ObjectBeingPlaced, placementGridY, onRightSide);
                    ObjectBeingPlaced = null;
                    return;
                }
                else
                {
                    tree.PreviewBirdhouse(ObjectBeingPlaced, placementGridY, onRightSide);
                }
                ObjectBeingPlaced.gameObject.SetActive(true);
                return;
            }
            ObjectBeingPlaced.gameObject.SetActive(false);
        }
        return;
    }

    // Returns true if found a birdhouse that could be detached underneath cursor.
    private bool TryDetachBirdhouse()
    {
        RaycastHit hitInfo = new RaycastHit();
        if ( !Physics.Raycast(Pointer.GetRay(Camera.main), out hitInfo, 100.0f, 1 << LayerMask.NameToLayer("Birdhouse")) )
        {
            return false;
        }
        Birdhouse attachedHouse = hitInfo.collider.GetComponentInParent<Birdhouse>();
        if ( !attachedHouse )
        {
            return false;
        }
        if ( attachedHouse.AttachedToTree )
        {
            // TODO: Could show preview for detaching house.
            if ( Input.GetMouseButtonDown(0) )
            {
                attachedHouse.AttachedToTree.DetachHouse(attachedHouse);
                if ( ObjectBeingPlaced != null )
                {
                    ObjectBeingPlaced.gameObject.SetActive(true);
                    ObjectBeingPlaced.PutBack();
                    PlacedObjectQueue.AddToFrontOfQueue(ObjectBeingPlaced);
                }
                ObjectBeingPlaced = attachedHouse;
                if ( Score.Instance != null )
                {
                    Score.Instance.OnBirdHousesChanged();
                }
            }
            return true;
        }
        return false;
    }

    private void Update()
    {
        bool foundDetachable = TryDetachBirdhouse();

        if ( foundDetachable && ObjectBeingPlaced )
        {
            ObjectBeingPlaced.gameObject.SetActive(false);
        }

        if ( !foundDetachable )
        {
            TryPlaceAtCursor();
        }
    }
}
