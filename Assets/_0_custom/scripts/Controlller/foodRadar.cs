using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodRadar : MonoBehaviour
{
    private GameObject snakeHead;

    private void Start()
    {
        snakeHead = transform.parent.gameObject;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "collectibles")
        {
            if (collision.GetComponent<perCollectible>())
            {
                if (!collision.GetComponent<perCollectible>().hasConsumedGetter())
                {
                    collision.GetComponent<perCollectible>().setConsumer(snakeHead);
                }
            }
        }
    }
}
