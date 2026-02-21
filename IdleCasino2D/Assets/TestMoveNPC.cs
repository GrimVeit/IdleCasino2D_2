using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestMoveNPC : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation spineFrontLeft;
    [SerializeField] private SkeletonAnimation spineFrontRight;
    [SerializeField] private SkeletonAnimation spineBackLeft;
    [SerializeField] private SkeletonAnimation spineBackRight;

    [SerializeField] private List<Node> nodeEnds = new();
    [SerializeField] private Node nodeStart;

    public event Action OnPathCompleted;

    private int currentPoint = 0;

    private void Start()
    {
        StartPath(Paths.FindPath(nodeStart, nodeEnds[Random.Range(0, nodeEnds.Count)]));
    }

    private void StartPath(List<Node> path)
    {
        currentPoint = 0;
        MoveAlongPath(path);
    }

    private void MoveAlongPath(List<Node> nodes)
    {
        if (currentPoint >= nodes.Count)
        {
            spineBackRight.AnimationState.SetAnimation(0, "idle", loop: true);
            OnPathCompleted?.Invoke();
            return;
        }

        spineBackRight.AnimationState.SetAnimation(0, "walk", loop: true);

        Vector3 localTarget = nodes[currentPoint].transform.localPosition;

        Vector3 direction = (localTarget - transform.localPosition).normalized;
        UpdateSkeletonDirection(direction);

        transform.DOLocalMove(localTarget, 2f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                currentPoint += 1;
                MoveAlongPath(nodes);
            });

    }

    private void UpdateSkeletonDirection(Vector3 dir)
    {
        spineBackLeft.gameObject.SetActive(false);
        spineBackRight.gameObject.SetActive(false);
        spineFrontLeft.gameObject.SetActive(false);
        spineFrontRight.gameObject.SetActive(false);

        if (dir.y >= 0)
        {
            if (dir.x < 0)
                spineBackLeft.gameObject.SetActive(true);
            else
                spineBackRight.gameObject.SetActive(true);
        }
        else
        {
            if (dir.x < 0)
                spineFrontLeft.gameObject.SetActive(true);
            else
                spineFrontRight.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartPath(Paths.FindPath(nodeStart, nodeEnds[Random.Range(0, nodeEnds.Count)]));
        }
    }
}
