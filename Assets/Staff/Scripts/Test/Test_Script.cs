using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Script : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * 5f * Time.deltaTime);
        if (transform.position.z < 0f)
        {
            animator.Play("test");
        }
    }
}
