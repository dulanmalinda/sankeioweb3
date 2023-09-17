using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreManager : MonoBehaviour
{
    [HideInInspector]public int collectedScore;
    [HideInInspector]public int usedToBoost;
    [HideInInspector]public int usedToGrow;

    [Header("Attachments")]
    public TextMeshProUGUI[] scoretexts;

    public static scoreManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        foreach (TextMeshProUGUI i in scoretexts)
        {
            i.text = collectedScore.ToString();
        }
    }

}
