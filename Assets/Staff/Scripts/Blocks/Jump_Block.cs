using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Jump_Block : Block
{
    protected override void Ray_Cast()
    {
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, Vector3.down, out hit, 3))
        //{
        //    hit.transform.gameObject.SetActive(false);
        //};
    }
}
