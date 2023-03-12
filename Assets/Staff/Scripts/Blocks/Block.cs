using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    public int damage = -10;

    protected bool is_switched;

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        transform.Translate(new Vector3(0, 0, -1) * Game_Controller.Instance.curve_speed * Time.deltaTime);
        if (transform.position.z < -10)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Changing_Status();
    }

    protected virtual void Changing_Status()
    {
        //改变自身状态
    }
}
