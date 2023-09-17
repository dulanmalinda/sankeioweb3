using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perCollectible : MonoBehaviour
{
    private bool hasConsumed;
    private GameObject consumer;
    private float maxDistanceFromConsumer;

    private void Start()
    {
        hasConsumed = false;
    }

    public bool hasConsumedGetter()
    {
        return hasConsumed;
    }

    public void setConsumer(GameObject c)
    {
        consumer = c;
        maxDistanceFromConsumer = Vector2.Distance(consumer.transform.position,transform.position);
        hasConsumed = true;
    }

    void Update()
    {
        if (hasConsumed && consumer != null)
        {
            float dis = Vector2.Distance(consumer.transform.position, transform.position);
            float speed = maxDistanceFromConsumer - dis;
            speed = speed * Time.deltaTime * 20f;
            transform.position = Vector2.MoveTowards(transform.position, consumer.transform.position, speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "snake")
        {
            scoreManager.Instance.collectedScore += 1;
            scoreManager.Instance.usedToGrow += 1;

            if (scoreManager.Instance.usedToGrow >= 5)
            {
                if (collision.transform.parent.GetComponent<snakeManager>())
                {
                    snakeManager m = collision.transform.parent.GetComponent<snakeManager>();
                    if (!m.isBoosting)
                    {
                        m.addBodyParts(m.currentBodySkin);
                        scoreManager.Instance.usedToGrow = 0;
                    }
                }
            }

            Destroy(this.gameObject);
        }

        if (collision.tag == "collectibles")
        {
            Destroy(gameObject);
        }
    }
}
