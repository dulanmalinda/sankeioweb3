using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeManager : MonoBehaviour
{
    [SerializeField] bool isMouseControlled;
    [SerializeField] float distanceBetween = 0.2f;
    [SerializeField] float normalSpeed = 200;
    [SerializeField] float boostedSpeed = 300;
    [SerializeField] float turnSpeed = 180;
    [SerializeField] int minBodyParts;
    [SerializeField] Vector2 maxMovementArea;
    [SerializeField] Vector2 minMovementArea;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    public GameObject currentBodySkin;

    List<GameObject> snakeBody = new List<GameObject>();

    float speed = 200;
    float countUp = 0;

    [HideInInspector] public bool isBoosting;

    private cameraController cameracontroller;

    //Boost related
    private float currentTime;
    private float startTime;

    void Start()
    {
        cameracontroller = GameObject.FindObjectOfType<cameraController>();

        CreateBodyParts();
    }

    void FixedUpdate()
    {
        manageSnakeBody();
        snakeMovement();
    }

    void Update()
    {
        snakeBody[0].transform.position = new Vector2(
            Mathf.Clamp(snakeBody[0].transform.position.x, minMovementArea.x, maxMovementArea.x),
            Mathf.Clamp(snakeBody[0].transform.position.y, minMovementArea.y, maxMovementArea.y)
            );

        if (Input.GetMouseButton(0) && scoreManager.Instance.collectedScore > 0)
        {
            speed = boostedSpeed;
            isBoosting = true;
            currentTime = Time.time;

            if (currentTime - startTime >= 0.5f)
            {
                if (scoreManager.Instance.collectedScore > 0)
                {
                    scoreManager.Instance.collectedScore -= 1;
                    scoreManager.Instance.usedToBoost += 1;
                }

                startTime = currentTime;
            }

            if (scoreManager.Instance.usedToBoost >= 1)
            {
                removeBodyPart();
                scoreManager.Instance.usedToBoost = 0;
            }

        }
        else
        {
            speed = normalSpeed;
            isBoosting = false;
            currentTime = Time.time;
            startTime = Time.time;
        }
    }

    void manageSnakeBody()
    {
        if (bodyParts.Count > 0)
        {
            CreateBodyParts();
        }
        for (int i = 0; i<snakeBody.Count; i++)
        {
            if (snakeBody[i] == null)
            {
                snakeBody.RemoveAt(i);
                cameracontroller.targets.RemoveAt(i);
                i = i + 1;
            }
        }
        if (snakeBody.Count == 0)
        {
            Destroy(this);
        }
    }

    void snakeMovement()
    {
        snakeBody[0].GetComponent<Rigidbody2D>().velocity = -snakeBody[0].transform.up * speed * Time.deltaTime;

        if (isMouseControlled)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, snakeBody[0].transform.position- mousePosition);
            snakeBody[0].transform.rotation = Quaternion.Slerp(snakeBody[0].transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        else
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                snakeBody[0].transform.Rotate(new Vector3(0, 0, -turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal")));
            }
        }

        if (snakeBody.Count > 1)
        {
            for (int i = 1; i<snakeBody.Count; i++)
            {
                MarkerManager m = snakeBody[i - 1].GetComponent<MarkerManager>();
                snakeBody[i].transform.position = m.markerList[0].position;
                snakeBody[i].transform.rotation = m.markerList[0].rotation;
                m.markerList.RemoveAt(0);
            }
        }
    }

    void CreateBodyParts()
    {
        if (snakeBody.Count == 0)
        {
            GameObject temp = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }

            if (!temp.GetComponent<Rigidbody2D>())
            {
                temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0f;
            }
            snakeBody.Add(temp);
            cameracontroller.targets.Add(temp.transform);
            bodyParts.RemoveAt(0);
        }

        MarkerManager m = snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>();
        if (countUp == 0)
        {
            m.clearMarkerList();
        }
        countUp += Time.deltaTime;

        if (countUp >= distanceBetween)
        {
            GameObject temp = Instantiate(bodyParts[0], m.markerList[0].position, m.markerList[0].rotation,transform);
            if (!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }

            if (!temp.GetComponent<Rigidbody2D>())
            {
                temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0f;
            }
            snakeBody.Add(temp);
            cameracontroller.targets.Add(temp.transform);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().clearMarkerList();
            countUp = 0;
        }
    }

    //Call this function on scoremanager to grow the snake
    public void addBodyParts(GameObject obj)
    {
        bodyParts.Add(obj);
    }

    //used when boosting the snake
    private void removeBodyPart()
    {
        if (snakeBody.Count > minBodyParts)
        {
            cameracontroller.targets.RemoveAt(snakeBody.Count-1);
            snakeBody[snakeBody.Count - 1].transform.tag = "collectibles";
            snakeBody[snakeBody.Count - 1].transform.parent = GameObject.FindObjectOfType<collectiblesManager>().transform;
            snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>().enabled = false;
            snakeBody[snakeBody.Count - 1].AddComponent<perCollectible>();
            snakeBody.RemoveAt(snakeBody.Count-1);
        }
    }
}
