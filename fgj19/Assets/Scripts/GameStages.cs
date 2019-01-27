using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStages : MonoBehaviour
{
    [System.Serializable]
    public class TreeActivation
    {
        public Tree Tree;
        public bool LeftSideActive = true;
        public bool RightSideActive = true;
    }

    [System.Serializable]
    public class Stage
    {
        public Cinemachine.CinemachineVirtualCamera Cam;
        public List<TreeActivation> ActiveTrees;
        public int MaxPlacementHeight = 6;
    }

    [SerializeField] private List<Stage> Stages;
    private int StageIndex = -1;

    private void Awake()
    {
        foreach (Tree tree in FindObjectsOfType<Tree>())
        {
            tree.LeftSideActive = false;
            tree.RightSideActive = false;
        }
        NextStage();
    }

    [ContextMenu("Next Stage")]
    void NextStage()
    {
        ++StageIndex;
        Stage stage = Stages[StageIndex];
        foreach ( TreeActivation activation in stage.ActiveTrees )
        {
            activation.Tree.LeftSideActive = activation.LeftSideActive;
            activation.Tree.RightSideActive = activation.RightSideActive;
        }
        stage.Cam.Priority = 10 * (StageIndex + 1);
        FindObjectOfType<TreePositionSelect>().MaxPlacementHeight = stage.MaxPlacementHeight;
    }
}
