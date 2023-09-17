using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectiblesManager : MonoBehaviour
{
    [Header("Generals")]
    public Vector2 maxPlayArea;
    public Vector2 minPlayArea;
    public int requiredCollectiblesCount;

    [Header("Attachments")]
    public GameObject[] collectiblesPrefabs;
    public Transform collectiblesParent;

    void Update()
    {
        if (collectiblesParent.childCount < requiredCollectiblesCount)
        {
            spawnCollectible();
        }
    }

    private void spawnCollectible()
    {
        int x = Random.Range(0,collectiblesPrefabs.Length);
        Vector2 pos = new Vector2(Random.Range(minPlayArea.x,maxPlayArea.x),Random.Range(minPlayArea.y,maxPlayArea.y));
        Instantiate(collectiblesPrefabs[x],pos,Quaternion.identity,collectiblesParent);
    }
}
