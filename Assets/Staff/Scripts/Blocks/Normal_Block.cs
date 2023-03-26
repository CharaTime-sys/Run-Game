using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Normal_Block : Block
{
    public float time = 2.0f;
    private float rightBound = 12.0f;
    private float leftBound = -5.5f;
    public bool is_left = false;

    private void Start()
    {
        InvokeRepeating(nameof(Set_Move), 0,time);
    }

    private void Set_Move()
    {
        if (is_left)
        {
            transform.DOLocalMoveX(leftBound, time).SetEase(Ease.Linear);
        }
        else
        {
            transform.DOLocalMoveX(rightBound, time).SetEase(Ease.Linear);
        }
        is_left = !is_left;
    }
}
