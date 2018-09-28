using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Global Variables
    private Platform[,] platformLayout;
    public float maxSpeed;
    public float speed;
    public float accel;
    private int theDirection;
    private int maxRow;
    private int maxCol;
    private bool makeRight;
    private bool makeLeft;

    // Class for the individual Platform Tiles
    private class Platform
    {
        private int tileRow;
        private int tileCol;
        private Vector3 tilePos;
        private bool isTile;

        public Platform()
        {
            tileRow = -1;
            tileCol = -1;
            tilePos = new Vector3(0, 0, 0);
            isTile = false;
        }

        public Platform(int row, int col, bool tile)
        {
            tileRow = 10 * row;
            tileCol = 10 * col;
            tilePos = new Vector3(10 * col, 0, 10 * row);
            isTile = tile;
        }

        public int getRow()
        {
            return tileRow;
        }

        public int getCol()
        {
            return tileCol;
        }

        // Check if the tile exists or is empty
        public bool getIsTile()
        {
            return isTile;
        }

        // Gets the midpoint of the tile
        public Vector3 getPos()
        {
            return tilePos;
        }
    }

    // Use this for initialization
    void Start()
    {
        // Set up the platform layout for the BallController class to access
        GameObject platform = GameObject.Find("Platform");
        PlatformController platformController = platform.GetComponent<PlatformController>();
        string[] layout = platformController.GetLayout();
        int rowNumber = 0;

        maxRow = layout.Length;
        maxCol = layout[0].Length;
        platformLayout = new Platform[layout.Length, layout[0].Length];

        foreach (string row in layout)
        {
            for (int col = 0; col < row.Length; col++)
            {
                if (row[col] == '*')
                {
                    platformLayout[rowNumber, col] = new Platform(rowNumber, col, true);
                }
                else
                {
                    platformLayout[rowNumber, col] = new Platform(rowNumber, col, false);
                }
            }
            rowNumber++;
        }

        // Initialize the variables that the ball should have at the beginning of the game
        //Note: for theDirection, 0 = North; 1 = East; 2 = South; 3 = West
        theDirection = 0;
        makeRight = false;
        makeLeft = false; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;

        // row and col for the tile in the platformLayout array
        int row;
        int col;

        if ((int)position.z % 10 < 5)
        {
            row = (int)position.z / 10;
        } 
        else
        {
            row = ((int)position.z / 10) + 1;
        }

        if ((int)position.x % 10 < 5)
        {
            col = (int)position.x / 10;
        }
        else
        {
            col = ((int)position.x / 10) + 1;
        }

        Platform temp = platformLayout[row, col];

        int ballStatus = getBallPos(temp, position, row, col);

        // Move ball until it reaches the end of the line of tiles
        if (ballStatus < 2)
        {
            moveBall(temp, position);
        }

        // Offsets for the midpoint relative to the ball position
        float distanceX = Mathf.Abs(position.x - temp.getCol());
        float distanceZ = Mathf.Abs(position.z - temp.getRow());

        // Move towards the center of the tile before making the turn
        if (makeRight && (distanceX < 1) && (distanceZ < 1))
        {
            theDirection = turnRight(theDirection);
            makeRight = false;
        }

        if (makeLeft && (distanceX < 1) && (distanceZ < 1))
        {
            theDirection = turnLeft(theDirection);
            makeLeft = false;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            int tempDir = turnRight(theDirection);
            if (checkNext(temp, row, col, tempDir) && (ballStatus != 1))
            {
                makeRight = true;
            }
            else
            {
                speed *= .25f;
            }
                
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int tempDir = turnLeft(theDirection);

            if (checkNext(temp, row, col, tempDir) && (ballStatus != 1))
            {
                makeLeft = true;
            }
            else
            {
                speed *= .25f;
            }
        }
    }

    // Return 0 for Before, 1 for Beyond, 2 for Blocked
    int getBallPos(Platform temp, Vector3 ballPos, int row, int col)
    {
        if (theDirection == 0)
        {
            if (ballPos.z < temp.getRow())
            {
                return 0;
            }
            else
            {
                if (checkNext(temp, row, col, theDirection))
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }
        else if (theDirection == 1)
        {
            if (ballPos.x < temp.getCol())
            {
                return 0;
            }
            else
            {
                if (checkNext(temp, row, col, theDirection))
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }
        else if (theDirection == 2)
        {
            if (ballPos.z > temp.getRow())
            {
                return 0;
            }
            else
            {
                if (checkNext(temp, row, col, theDirection))
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }
        else
        {
            if (ballPos.x > temp.getCol())
            {
                return 0;
            }
            else
            {
                if (checkNext(temp, row, col, theDirection))
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }
    }

    // Check next tile if exists
    bool checkNext(Platform temp, int row, int col, int direction)
    {
        if (direction == 0)
        {
            if ((row < (maxRow - 1)) && platformLayout[row + 1, col].getIsTile())
            {
                return true;
            }
        }
        else if (direction == 1)
        {
            if ((col < (maxCol - 1)) && platformLayout[row, col + 1].getIsTile())
            {
                return true;
            }
        }
        else if (direction == 2)
        {
            if ((row > 0) && platformLayout[row - 1, col].getIsTile())
            {
                return true;
            }
        }
        else
        {
            if ((col > 0) && platformLayout[row, col - 1].getIsTile())
            {
                return true;
            }
        }
        return false;
    }

    // Move object
    void moveBall(Platform temp, Vector3 ballPos)
    {
        if (speed < maxSpeed)
        {
            speed += (accel * Time.deltaTime);
        }
        if (theDirection == 0)
        {
            transform.position += Vector3.forward * Time.deltaTime * speed;
        }
        else if (theDirection == 1)
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
        else if (theDirection == 2)
        {
            transform.position += Vector3.back * Time.deltaTime * speed;
        }
        else
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
    }

    int turnRight(int tempDir)
    {
        return ((tempDir + 1) % 4);
    }

    int turnLeft(int tempDir)
    {
        if (tempDir == 0)
        {
            return 3;
        }
        else
        {
            return ((tempDir - 1) % 4);
        }
    }
}

