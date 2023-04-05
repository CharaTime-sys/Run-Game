using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_block : MonoBehaviour
{
    public bool is_playing;
    public bool could_ani;
    // Update is called once per frame
    void Update()
    {
        if (!Game_Controller.Instance.game_started)
        {
            return;
        }
        Ray_Cast();
    }

    protected void Ray_Cast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 3))
        {
            if (hit.transform.GetComponent<Ninja>()!=null )
            {
                foreach (Transform item in transform.parent)
                {
                    item.GetComponent<Animator>().Play("return");
                    item.GetComponent<Base_block>().could_ani = false;
                }
            }
        };
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!could_ani)
        {
            return;
        }
        if (other.name.StartsWith("3"))
        {
            GetComponent<Animator>().Play("start");
        }
    }

    public void iterate_item(string name)
    {
        foreach (Transform item in transform.parent)
        {
            if (item.GetComponent<Base_block>().is_playing)
            {
                return;
            }
            item.GetComponent<Animator>().Play(name);
            item.GetComponent<Base_block>().is_playing = true;
        }
    }
}
