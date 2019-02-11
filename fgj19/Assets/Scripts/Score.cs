using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Score : MonoBehaviour
{
    private List<Tree> Trees = new List<Tree>();
    private static int ConnectionCount;
    public static Score Instance { get; private set; }
    public List<SegmentConnection> SegmentConnections;
    Text text;

    [SerializeField]
    public SegmentConnection segmentConnectionPrefab;

    void Awake()
    {
        Instance = this;
        ConnectionCount = 0;
        SegmentConnections = new List<SegmentConnection>();
        text = GetComponentInChildren<Text>();
    }

    void Start()
    {
        Trees = UnityEngine.Object.FindObjectsOfType<Tree>().ToList();
        Trees = Trees.OrderBy(t => t.transform.position.x).ToList();
        for (int index = 0; index < Trees.Count; ++index )
        {
            Trees[index].GridX = index;
        }
    }

    void Update()
    {
        if (text)
        {
            text.text = "Score: " + SegmentConnections.Count;
        }
    }

    private void CreateConnection(BirdhouseSegment left, Tree leftTree, BirdhouseSegment right, Tree rightTree, int GridY)
    {
        var newConnector = Instantiate(segmentConnectionPrefab);
        newConnector.LeftEnd = left;
        newConnector.RightEnd = right;
        newConnector.GridXLeft = leftTree.GridX;
        newConnector.GridXRight = rightTree.GridX;
        newConnector.GridY = GridY;

        var fromSeg = left.GetComponentInChildren<SegmentConnectionAnchor>();
        var toSeg = right.GetComponentInChildren<SegmentConnectionAnchor>();

        var connectorLength = toSeg.transform.position.x - fromSeg.transform.position.x;
        var direction = (toSeg.transform.position - fromSeg.transform.position);

        newConnector.transform.localScale = new Vector3(0.4F, 0.4F, connectorLength);
        newConnector.transform.rotation = Quaternion.LookRotation(direction);
        newConnector.transform.eulerAngles += new Vector3(0, 0, -30);
        newConnector.transform.position = fromSeg.transform.position + direction * 0.5f;

        SegmentConnections.Add(newConnector);
    }

    private bool IsConnectionBlocked(int GridXLeft, int GridXRight, int GridY)
    {
        foreach ( Tree tree in Trees )
        {
            foreach ( Birdhouse house in tree.Birdhouses )
            {
                if ( GridY >= house.GridY && GridY < house.GridY + house.SegmentCount &&
                    tree.GridX > GridXLeft && tree.GridX < GridXRight )
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsConnectionBlocked(SegmentConnection connection)
    {
        return IsConnectionBlocked(connection.GridXLeft, connection.GridXRight, connection.GridY);
    }

    public void OnBirdHousesChanged()
    {
        // Check for broken connections.
        for ( var connectionIndex = 0; connectionIndex < SegmentConnections.Count; )
        {
            SegmentConnection connection = SegmentConnections[connectionIndex];
            if ( IsConnectionBlocked(connection) )
            {
                connection.LeftEnd.connection = null;
                connection.RightEnd.connection = null;
                Destroy(connection.gameObject);
                SegmentConnections.RemoveAt(connectionIndex);
            }
            else
            {
                ++connectionIndex;
            }
        }

        // Look for new connections.
        // This is not very optimal but we have so few trees/birdhouses that it doesn't really matter.
        foreach ( Tree tree in Trees )
        {
            foreach ( Birdhouse house in tree.Birdhouses )
            { 
                int GridY = house.GridY;
                foreach ( BirdhouseSegment segment in house.Segments )
                {
                    // Look for new connections to the right of this house's segments.
                    if ( !segment.connection && house.OnRightSide )
                    {
                        foreach ( Tree tree2 in Trees )
                        {
                            if ( tree2.GridX <= tree.GridX )
                            {
                                continue;
                            }
                            foreach ( Birdhouse house2 in tree2.Birdhouses )
                            {
                                int GridY2 = house2.GridY;
                                foreach (BirdhouseSegment segment2 in house2.Segments)
                                {
                                    if ( GridY == GridY2 && !house2.OnRightSide && segment2.Matches(segment) && !IsConnectionBlocked(tree.GridX, tree2.GridX, GridY) )
                                    {
                                        CreateConnection(segment, tree, segment2, tree2, GridY);
                                    }
                                    ++GridY2;
                                }
                            }
                        }
                    }
                    ++GridY;
                }
            }
        }
    }
}
