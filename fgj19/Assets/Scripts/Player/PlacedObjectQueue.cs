using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObjectQueue : MonoBehaviour
{
    public List<Birdhouse> PlacedObjects;

    // Remove the next object from the queue and return it.
    public Birdhouse ObtainNext()
    {
        if (PlacedObjects == null || PlacedObjects.Count == 0)
        {
            return null;
        }
        var next = PlacedObjects[0];
        PlacedObjects.RemoveAt(0);
        return next;
    }
}
