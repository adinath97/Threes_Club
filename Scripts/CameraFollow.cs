using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform[] playerOptions = new Transform[2]; //our target to follow
    public Transform currentPlayer;
    public Transform leftBounds;
    public Transform rightBounds;
    public float smoothDampTime = 0.15f;
    private Vector3 smoothDampVelocity = Vector3.zero;

    private float camWidth, camHeight, levelMinX, levelMaxX;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = playerOptions[PlayerRotator.teamPosition];

        camHeight = Camera.main.orthographicSize * 2;
        camWidth = camHeight * Camera.main.aspect;

        float leftBoundWidth = leftBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        float rightBoundWidth = rightBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;

        levelMinX = leftBounds.position.x + leftBoundWidth + (camWidth / 2);
        levelMaxX = rightBounds.position.x - rightBoundWidth - (camWidth / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerRotator.changed == true)
        {
            currentPlayer = playerOptions[PlayerRotator.teamPosition];
        }

        if(currentPlayer)
        {
            float playerX = Mathf.Max(levelMinX - 1.5f, Mathf.Min(levelMaxX, currentPlayer.position.x));
            float x = Mathf.SmoothDamp(transform.position.x, playerX, ref smoothDampVelocity.x, smoothDampTime);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
    }
}
