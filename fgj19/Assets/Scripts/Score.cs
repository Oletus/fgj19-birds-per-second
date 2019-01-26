using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Score : MonoBehaviour
{
  private List<Tree> trees = new List<Tree>();
  private static int score;
  public static Score instance { get; private set; }
  Text text;

  void Awake()
  {
    instance = this;
    score = 0;
    text = GetComponentInChildren<Text>();
  }

  void Start()
  {
    foreach (var tree in Object.FindObjectsOfType<Tree>())
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

      for (int i = treeIndex + dir; i < trees.Count() && i >= 0; i += dir)
      {
        Birdhouse neighbouringBirdhouse = null;
        BirdhouseSegment neighbouringSegment = null;
        foreach (var bh in trees[i].Birdhouses) {
          var bhSegIndex = 0;
          foreach (var bhSeg in bh.Segments.OrderBy(s => s.transform.position.y)) {
            if (bh.PlacementGridY + bhSegIndex == coordY) {
              neighbouringBirdhouse = bh;
              neighbouringSegment = bhSeg;
              break;
            }
            bhSegIndex++;
          }
        }

        if (neighbouringSegment) {
          if (onRightSide != neighbouringBirdhouse.OnRightSide && segment.Matches(neighbouringSegment)) {
            Debug.Log("Matched from " + treeIndex + " to " + i);
            score++;
          } else {
            // Blocked by a birdhouse facing another way OR a symbol mismatch
            break;
          }
        }
      }
    }

  }
}
