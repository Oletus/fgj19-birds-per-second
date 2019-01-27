using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObjectQueue : MonoBehaviour
{
    public List<Birdhouse> PlacedObjects;


    [Header("Randomization")]
    [SerializeField]
    private int RandomBirdhousesN;

    [SerializeField]
    private Birdhouse BirdhousePrefab;

    [SerializeField]
    private BirdhouseSegment BirdhouseSegmentPrefab;

    [ContextMenu("Randomize")]
    void Randomize() {
        for (int i = 0; i < RandomBirdhousesN; i++)
        {
            var pos = new Vector3(Random.Range(-8F, -2F), 0, Random.Range(-8F, -2F));
            Birdhouse bh = Instantiate(BirdhousePrefab, pos, Quaternion.Euler(0, 180, 0));

            for (int j = 0; j < Random.Range(1, 3); j++)
            {
                BirdhouseSegment bhSeg = Instantiate(BirdhouseSegmentPrefab, bh.transform);
                bhSeg.BaseColorIndex = Random.Range(1, 4);
            }
            PlacedObjects.Add(bh);
        }
    }

    // Remove the next object from the queue and return it.
    public Birdhouse ObtainNext()
    {
        if ( PlacedObjects == null || PlacedObjects.Count == 0 )
        {
            return null;
        }

        var next = PlacedObjects[0];
        PlacedObjects.RemoveAt(0);
        return next;
    }
}
