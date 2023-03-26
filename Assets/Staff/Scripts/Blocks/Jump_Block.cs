using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Jump_Block : Block
{
    protected override void Ray_Cast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 3))
        {
            foreach (Transform item in hit.transform.parent)
            {
                item.gameObject.SetActive(false);
            }
        };
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.up, Color.red);
    }
}
