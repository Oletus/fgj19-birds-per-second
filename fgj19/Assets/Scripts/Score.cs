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
    public List<(int y, int x1, int x2)> segmentConnections;
    Text text;

    [SerializeField]
    public SegmentConnection sc;

    void Awake()
    {
        instance = this;
        score = 0;
        segmentConnections = new List<(int y, int x1, int x2)>();
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
        foreach (var segment in house.Segments.OrderBy(s => s.transform.position.y))
        {
            var coordY = placementGridY + segmentIndex;
            segmentIndex++;

            // Check for broken connections:
            var brokenConnection = segmentConnections.FirstOrDefault(sg => sg.y == coordY && sg.x1 < treeIndex && treeIndex < sg.x2);
            if (brokenConnection != default(ValueTuple<int, int, int>))
            {
                Debug.Log("Broken connection at " + brokenConnection);
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
                    foreach (var bhSeg in bh.Segments.OrderBy(s => s.transform.position.y))
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
                        score++;
                        segmentConnections.Add((coordY, Math.Min(treeIndex, i), Math.Max(treeIndex, i)));

                        var padding = 0.1F;
                        var fromSeg = onRightSide ? segment : neighbouringSegment;
                        var toSeg = onRightSide ? neighbouringSegment : segment;
                        var clone = Instantiate(sc, fromSeg.transform.position, Quaternion.identity);
                        var newWidth = toSeg.transform.position.x - fromSeg.transform.position.x;
                        clone.transform.localScale = new Vector3(newWidth - padding * 2, fromSeg.transform.localScale.y, 0);
                        clone.transform.position += new Vector3(newWidth / 2 + padding, 0, 0);
                    }
                    // Blocked by a birdhouse facing another way OR a symbol mismatch:
                    break;
                }
            }
        }
    }
}
