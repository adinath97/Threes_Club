using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plundererBox : MonoBehaviour
{
    public GameObject plunderer;
    private Animator pAnim;
    public static bool destroyEnemy;

    private void Start()
    {
        pAnim = plunderer.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "enemy" && pAnim.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            destroyEnemy = true;
        }
    }
}
