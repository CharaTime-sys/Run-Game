using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_block : MonoBehaviour
{
    public bool is_playing;
    // Update is called once per frame
    void Update()
    {
        Ray_Cast();
    }

    protected void Ray_Cast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 3))
        {
            if (hit.transform.GetComponent<Ninja>()!=null && !is_playing)
            {
                foreach (Transform item in transform.parent)
                {
                    item.GetComponent<Animator>().Play("return");
                    item.GetComponent<Base_block>().is_playing = true;
                }
            }
        };
    }
}
