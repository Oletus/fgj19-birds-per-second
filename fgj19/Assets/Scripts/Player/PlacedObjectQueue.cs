using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObjectQueue : MonoBehaviour
{
    public List<Birdhouse> PlacedObjects;
    public List<Birdhouse> randomPlacedObjects;
    public int birdhousesN;

    [SerializeField]
    public Birdhouse birdhouse;

    [SerializeField]
    public BirdhouseSegment birdhouseSegment;

    void Start() {
        randomPlacedObjects = new List<Birdhouse>();

        for (int i = 0; i < birdhousesN; i++)
        {
            var pos = new Vector3(Random.Range(-8F, -2F), 0, Random.Range(-8F, -2F));
            Birdhouse bh = Instantiate(birdhouse, pos, Quaternion.Euler(0, 180, 0));

            for (int j = 0; j < Random.Range(1, 3); j++)
            {
                BirdhouseSegment bhSeg = Instantiate(birdhouseSegment, bh.transform);
                bhSeg.BaseColorIndex = Random.Range(1, 4);
            }

            bh.Segments = new List<BirdhouseSegment>(bh.GetComponentsInChildren<BirdhouseSegment>());
            randomPlacedObjects.Add(bh);
        }
    }

    // Remove the next object from the queue and return it.
    public Birdhouse ObtainNext()
    {
        if (randomPlacedObjects.Count > 0) {
            var next = randomPlacedObjects[0];
            randomPlacedObjects.RemoveAt(0);
            return next;
        }

        return null;
    }
}
