using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buff_ui : MonoBehaviour
{
    public bool if_jump;
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        Test_Dot();
    }

    public void Test_Dot()
    {
        foreach (Transform item in transform)
        {
            if (!item.gameObject.active)
            {
                continue;
            }
            if (if_jump)
            {
                if (Input.touches[0].position.y > item.position.y)
                {
                    item.gameObject.SetActive(false);
                }
            }
            else
            {
                if (Input.touches[0].position.y < item.position.y)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Reset_uis()
    {
        foreach (Transform item in transform)
        {
            if (if_jump)
            {
                if (Input.touches[0].position.y < item.position.y)
                {
                    item.gameObject.SetActive(true);
                }
            }
            else
            {
                if (Input.touches[0].position.y > item.position.y)
                {
                    item.gameObject.SetActive(true);
                }
            }
        }
    }
}
