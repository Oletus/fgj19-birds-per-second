using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Score : MonoBehaviour
{
    private List<Tree> trees = new List<Tree>();
    private static int score;
    public static Score instance { get; private set; }
    public List<(int y, int x1, int x2, SegmentConnection sg)> segmentConnections;
    Text text;

    [SerializeField]
    public SegmentConnection sc;

    void Awake()
    {
        instance = this;
        score = 0;
        segmentConnections = new List<(int y, int x1, int x2, SegmentConnection sg)>();
        text = GetComponentInChildren<Text>();
    }

    void Start()
    {
        foreach (var tree in UnityEngine.Object.FindObjectsOfType<Tree>())
        {
            trees.Add(tree);
        }
        trees = trees.OrderBy(t => t.transform.position.x).ToList();
    }

    void Update()
    {
        if (text)
        {
            text.text = "Score: " + score;
        }
    }

    public void OnBirdHouseAttached(Tree tree, Birdhouse house, int placementGridY, bool onRightSide)
    {
        var treeIndex = trees.FindIndex(t => GameObject.ReferenceEquals(t, tree));
        var dir = onRightSide ? 1 : -1;

        var segmentIndex = 0;
        foreach (var segment in house.Segments.OrderBy(s => s.transform.localPosition.y))
        {
            var coordY = placementGridY + segmentIndex;
            segmentIndex++;

            // Check for broken connections:
            var brokenConnection = segmentConnections.FirstOrDefault(sg => sg.y == coordY && sg.x1 < treeIndex && treeIndex < sg.x2);
            if (brokenConnection != default(ValueTuple<int, int, int, SegmentConnection>))
            {
                Destroy(brokenConnection.sg.gameObject);
                score--;
            }

            // Check for new connections:
            for (int i = treeIndex + dir; i < trees.Count() && i >= 0; i += dir)
            {
                // Look for a birdhouse segment with the same Y coordinate in a neighbouring tree:
                Birdhouse neighbouringBirdhouse = null;
                BirdhouseSegment neighbouringSegment = null;
                foreach (var bh in trees[i].Birdhouses)
                {
                    var bhSegIndex = 0;
                    foreach (var bhSeg in bh.Segments.OrderBy(s => s.transform.localPosition.y))
                    {
                        if (bh.PlacementGridY + bhSegIndex == coordY)
                        {
                            neighbouringBirdhouse = bh;
                            neighbouringSegment = bhSeg;
                            break;
                        }
                        bhSegIndex++;
                    }
                }
                if (neighbouringSegment)
                {
                    if (onRightSide != neighbouringBirdhouse.OnRightSide && segment.Matches(neighbouringSegment))
                    {
                        var fromSeg = (onRightSide ? segment : neighbouringSegment).GetComponentInChildren<SegmentConnectionAnchor>();
                        var toSeg = (onRightSide ? neighbouringSegment : segment).GetComponentInChildren<SegmentConnectionAnchor>();

                        var newConnector = Instantiate(sc);
                        var connectorLength = toSeg.transform.position.x - fromSeg.transform.position.x;
                        var direction = (toSeg.transform.position - fromSeg.transform.position);

                        newConnector.transform.localScale = new Vector3(0.4F, 0.4F, connectorLength);
                        newConnector.transform.rotation = Quaternion.LookRotation(direction);
                        newConnector.transform.eulerAngles += new Vector3(0, 0, -30);
                        newConnector.transform.position = fromSeg.transform.position + direction * 0.5f;


                        score++;
                        segmentConnections.Add((y: coordY, x1: Math.Min(treeIndex, i), x2: Math.Max(treeIndex, i), sg: newConnector));
                    }
                    // Blocked by a birdhouse facing another way OR a symbol mismatch:
                    break;
                }
            }
        }
    }
}
