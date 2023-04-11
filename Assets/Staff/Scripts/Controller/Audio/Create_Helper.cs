using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Helper : MonoBehaviour
{
    public static Create_Helper Instance;
    public float start_delay;
    public List<float> target_z;
    public List<float> target_low_z;
    public GameObject tip;
    public GameObject tip_1;
    public GameObject _parent;
    private void Awake()
    {
        Instance = this;
    }

    public void Move()
    {
        if (_parent==null)
        {
            return;
        }
        _parent.transform.Translate(new Vector3(0, 0, -11f) * Time.deltaTime);
    }

    [ContextMenu("¼Ó±ê¼Ç")]
    public void Add_tips()
    {
        foreach (var item in target_z)
        {
            GameObject _block = Instantiate(tip, new Vector3(0, 0, item), Quaternion.identity);
            _block.transform.SetParent(_parent.transform);
            _block.name = "Cube";
        }
    }
    public void Create_Obj(float angle)
    {
        GameObject _block = Instantiate(tip, Game_Controller.Instance.ninja.transform.position, Quaternion.identity);
        _block.transform.SetParent(_parent.transform);
        if (angle < 45 || angle > 315)
        {
            _block.GetComponent<MeshRenderer>().material.SetColor("Color", Color.blue);
        }
        else if (angle > 135 && angle < 225)
        {
            _block.GetComponent<MeshRenderer>().material.SetColor("Color", Color.yellow);
        }
        else if (angle > 45 && angle < 135)
        {
            _block.GetComponent<MeshRenderer>().material.SetColor("Color", Color.white);
        }
        else if (angle > 225 && angle < 315)
        {
            _block.GetComponent<MeshRenderer>().material.SetColor("Color", Color.black);
        }
    }
    public void Add_Tag(int i,float j)
    {
        switch (i)
        {
            case 0:
                target_z.Add(j);
                break;
            case 1:
                target_low_z.Add(j);
                break;
            default:
                break;
        }
    }
}
