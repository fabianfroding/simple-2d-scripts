using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScreenScript : MonoBehaviour
{
    [SerializeField]
    GameObject selector;

    [SerializeField]
    GameObject[] row1;
    [SerializeField]
    GameObject[] row2;
    [SerializeField]
    GameObject[] row3;

    const int COLS = 3;
    const int ROWS = 3;

    Vector2 positionIndex;
    GameObject currentSlot;

    // declare 2d grid
    public GameObject[,] grid = new GameObject[COLS, ROWS];

    bool isMoving;


    void Start()
    {
        AddRowToGrid(0, row1);
        AddRowToGrid(1, row2);
        AddRowToGrid(2, row3);

        positionIndex = new Vector2(1, 1);
        currentSlot = grid[1, 1];
    }

    void AddRowToGrid(int index, GameObject[] row)
    {
        for (int i = 0; i < row.Length; i++)
        {
            grid[index, i] = row[i];
        }
    }


    void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");

        if (xAxis > 0 )
        {
            // Input right
            MoveSelector("right");
        }
        else if (xAxis < 0)
        {
            // Input left
            MoveSelector("left");
        }
        else if (yAxis > 0)
        {
            // Input up
            MoveSelector("up");
        }
        else if (yAxis < 0)
        {
            // Input down
            MoveSelector("down");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            string levelID = currentSlot.GetComponent<LevelSelectItemScript>().levelID;

            SceneManager.LoadScene(levelID);
        }
    }

    void MoveSelector(string direction)
    {
        if (!isMoving)
        {
            isMoving = true;

            if (direction == "right")
            {
                if (positionIndex.x < COLS - 1)
                {
                    positionIndex.x += 1;
                }
            }
            else if (direction == "left")
            {
                if (positionIndex.x > 0)
                {
                    positionIndex.x -= 1;
                }
            }
            else if (direction == "up")
            {
                if (positionIndex.y > 0)
                {
                    positionIndex.y -= 1;
                }
            }
            else if (direction == "down")
            {
                if (positionIndex.y < ROWS - 1)
                {
                    positionIndex.y += 1;
                }
            }

            currentSlot = grid[(int)positionIndex.y, (int)positionIndex.x];
            selector.transform.position = currentSlot.transform.position;

            Invoke("ResetMoving", 0.2f);

            
        }
    }

    void ResetMoving()
    {
        isMoving = false;
    }
}
