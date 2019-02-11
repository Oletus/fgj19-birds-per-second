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
        public List<TreeActivation> ActiveTrees;  // Should really be activatedTrees - previous activated trees are not changed.
        public int MaxPlacementHeight = 6;
        public List<Birdhouse> BirdhousesToPlace;
        [SerializeField] public List<Animator> AnimatorsToTrigger;
    }

    [SerializeField] private List<Stage> Stages;
    private int StageIndex = -1;

    private PlayerController PlayerController;

    private void Awake()
    {
        foreach (Tree tree in FindObjectsOfType<Tree>())
        {
            tree.LeftSideActive = false;
            tree.RightSideActive = false;
        }
        PlayerController = FindObjectOfType<PlayerController>();
        NextStage();
    }

    private void Update()
    {
        if ( PlayerController.ObjectQueue.PlacedObjects.Count == 0 && PlayerController.PositionSelect.ObjectBeingPlaced == null && StageIndex + 1 < Stages.Count )
        {
            NextStage();
        }
    }

    public void TriggerAnimations(string trigger, int stage)
    {
        foreach ( var animator in Stages[stage].AnimatorsToTrigger )
        {
            animator.SetTrigger(trigger);
        }
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
        foreach ( var house in stage.BirdhousesToPlace )
        {
            house.GameStage = StageIndex;
        }
        PlayerController.ObjectQueue.PlacedObjects = stage.BirdhousesToPlace;
    }
}
