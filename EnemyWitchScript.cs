using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWitchScript : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";

    string facingDirection;
    Vector3 baseScale;

    [SerializeField]
    Transform castPos;

    [SerializeField]
    float baseCastDist;

    Rigidbody2D rb2d;
    float moveSpeed = 1;
    
    void Start()
    {
        baseScale = transform.localScale;
        facingDirection = RIGHT;
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    // Physics-related goes in FixedUpdate btw
    void FixedUpdate()
    {
        float vX = moveSpeed;
        if (facingDirection == LEFT)
        {
            vX = -moveSpeed;
        }

        rb2d.velocity = new Vector2(vX, rb2d.velocity.y);
        if (isHittingWall() || isNearEdge())
        {
            print("hit wall");
            if (facingDirection == LEFT)
            {
                changeFacingDirection(RIGHT);
            }
            else if (facingDirection == RIGHT)
            {
                changeFacingDirection(LEFT);
            }
        }
    }

    void changeFacingDirection(string newDir)
    {
        Vector3 newScale = baseScale;
        if (newDir == LEFT)
        {
            newScale.x = -baseScale.x;
        }
        else
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;
        facingDirection = newDir;
    }

    bool isHittingWall()
    {
        bool val = false;

        // Define cast dist for left and right
        float castDist = baseCastDist;
        if (facingDirection == LEFT)
        {
            castDist = -baseCastDist;
        }

        Vector3 targetPos = castPos.position;

        // determine target desitnationbased on cast dist.
        targetPos.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.blue);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground"))) {
            val = true;
        }
        else
        {
            val = false;
        }

        return val;
    }

    bool isNearEdge()
    {
        bool val = true;

        // Define cast dist for left and right
        float castDist = baseCastDist;

        Vector3 targetPos = castPos.position;

        // determine target desitnationbased on cast dist.
        targetPos.y -= castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.red);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = false;
        }
        else
        {
            val = true;
        }

        return val;
    }
}
