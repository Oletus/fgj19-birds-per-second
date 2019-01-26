using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TreePositionSelect), typeof(PlacedObjectQueue))]
public class PlayerController : MonoBehaviour
{

    private TreePositionSelect PositionSelect;
    private PlacedObjectQueue ObjectQueue;

    private void Awake()
    {
        PositionSelect = GetComponent<TreePositionSelect>();
        ObjectQueue = GetComponent<PlacedObjectQueue>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PositionSelect.PlacedObject == null)
        {
            PositionSelect.PlacedObject = ObjectQueue.ObtainNext();
        }
    }
}
