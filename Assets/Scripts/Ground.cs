using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public float groundHeight;
    BoxCollider2D collider;
    Player player;
    public float groundRight;
    public float screenRight;
    bool didGenerateGround = false;

    public Obstacle box;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player").GetComponent<Player>();
        groundHeight = transform.position.y + (collider.size.y / 2);
        screenRight = Camera.main.transform.position.x * 2;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;
        groundRight = transform.position.x + (collider.size.x / 2);
        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;

        }
        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                generateGround();
            }
        }

        transform.position = pos;
    }

    void generateGround()
    {
        GameObject go = Instantiate(gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        float h1 = player.jumpVelocity * player.maxHoldJumpTime;
        float t = player.jumpVelocity / -player.gravity;
        float h2 = player.jumpVelocity * t + (player.gravity * (t * t)) * 0.5f;
        float maxJumpHeight = h1 + h2;
        float maxY = maxJumpHeight * 0.4f;
        maxY += groundHeight;
        float miny = 1;
        float actualY = Random.Range(miny, maxY);
        
        pos.y = actualY - goCollider.size.y / 2;
        if (pos.y > 2.5f)
        {
            pos.y = 2.5f;
        }

        float t1 = t + player.maxHoldJumpTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravity);
        float totalTime = t1 + t2;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.7f;
        maxX += groundRight;
        float minX = screenRight + 5;
        float actualX = Random.Range(minX, maxX);

        pos.x = actualX + goCollider.size.x / 2;
        go.transform.position = pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);
        int obstacleNum = Random.Range(0, 4);
        for(int i =0; i<obstacleNum; i++)
        {
            GameObject boxO = Instantiate(box.gameObject);
            float y = goGround.groundHeight;
            float halfwidth = goCollider.size.x / 2 - 1;
            float left = go.transform.position.x - halfwidth;
            float right = go.transform.position.x + halfwidth;
            float x = Random.Range(left, right);

            Vector2 boxPos = new Vector2(x,y);
            boxO.transform.position = boxPos;
        }
    }
}
