using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TreePositionSelect), typeof(PlacedObjectQueue))]
public class PlayerController : MonoBehaviour
{

    private TreePositionSelect _PositionSelect;
    public TreePositionSelect PositionSelect { get { return _PositionSelect; } }
    private PlacedObjectQueue _ObjectQueue;
    public PlacedObjectQueue ObjectQueue { get { return _ObjectQueue; } }

    private void Awake()
    {
        _PositionSelect = GetComponent<TreePositionSelect>();
        _ObjectQueue = GetComponent<PlacedObjectQueue>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_PositionSelect.ObjectBeingPlaced == null)
        {
            _PositionSelect.ObjectBeingPlaced = _ObjectQueue.ObtainNext();
        }
    }
}
