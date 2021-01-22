using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChecker : MonoBehaviour
{
    public GameObject block;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OUT HERE");
        if ((collision.tag == "Gunner" || collision.tag == "Speedster" || collision.tag == "Plunderer") && block != null)
        {
            //Debug.Log("HELLO!");
            block.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -8f);
            Destroy(block, 1f);
            Destroy(this.gameObject);
        }
    }
}
