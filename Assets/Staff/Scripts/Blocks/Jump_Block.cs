using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Jump_Block : Block
{
    public bool once = true;
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (once && transform.position.z <= Game_Controller.Instance.ninja.transform.position.z)
        {
            Create_Helper.Instance.Add_Tag(0, Create_Helper.Instance._parent.transform.position.z);
            once = false;
        }
    }
}
