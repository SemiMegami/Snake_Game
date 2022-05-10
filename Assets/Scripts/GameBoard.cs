using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    int NumberOfRows;
    int NumberOfTailBlocks;
    
    // Start is called before the first frame update
    bool firstTimeSetTarget = true;


    void Start()
    {
        NumberOfRows = GameManager.instance.NumberOfRows;
        NumberOfTailBlocks = GameManager.instance.NumberOfTailBlocks;
        SetBoard();
        SetHead();
        PlaceTarget();

    }

  
    void SetHead()
    {
        Transform head = GameObject.Find("Head").transform;
        float xStart = -NumberOfRows / 2f;
        float zStart = -NumberOfRows / 2f;

        int i = Random.Range(NumberOfTailBlocks, NumberOfRows - 1 - NumberOfTailBlocks);
        int j = Random.Range(NumberOfTailBlocks, NumberOfRows - 1 - NumberOfTailBlocks);
        head.transform.position = new Vector3(xStart + 0.5f + i, 0.5f, zStart + 0.5f + j);
        Transform lastTailTransform = head.transform;

        int dir = Random.Range(0, 3);
        switch (dir)
        {
            case 0:
                for (int k = 0; k < NumberOfTailBlocks; k++)
                {
                    lastTailTransform = head.GetComponent<SnakeHead>().InstantiateTailBlock(lastTailTransform.position - new Vector3(1, 0, 0));
                    head.GetComponent<PlayerController>().CannotMoveDirAtStart = MoveDirection.LEFT;
                    head.GetComponent<PlayerController>().TurnFace(MoveDirection.RIGHT);
                }
            break;
            case 1:
                for (int k = 0; k < NumberOfTailBlocks; k++)
                {
                    lastTailTransform = head.GetComponent<SnakeHead>().InstantiateTailBlock(lastTailTransform.position - new Vector3(-1, 0, 0));
                    head.GetComponent<PlayerController>().CannotMoveDirAtStart = MoveDirection.RIGHT;
                    head.GetComponent<PlayerController>().TurnFace(MoveDirection.LEFT);
                }
                break;
            case 2:
                for (int k = 0; k < NumberOfTailBlocks; k++)
                {
                    lastTailTransform = head.GetComponent<SnakeHead>().InstantiateTailBlock(lastTailTransform.position - new Vector3(0, 0, 1));
                    head.GetComponent<PlayerController>().CannotMoveDirAtStart = MoveDirection.DOWN;
                    head.GetComponent<PlayerController>().TurnFace(MoveDirection.UP);
                }
                break;
            case 3:
                for (int k = 0; k < NumberOfTailBlocks; k++)
                {
                    lastTailTransform = head.GetComponent<SnakeHead>().InstantiateTailBlock(lastTailTransform.position - new Vector3(0, 0, -1));
                    head.GetComponent<PlayerController>().CannotMoveDirAtStart = MoveDirection.UP;
                    head.GetComponent<PlayerController>().TurnFace(MoveDirection.DOWN);
                }
                break;
        }
       
    }

    private Vector3 GetSnapPosition(Vector3 vector)
    {
        if(NumberOfRows% 2 == 0)
        {
            float x = Mathf.Round(vector.x + 0.5f) - 0.5f;
            float z = Mathf.Round(vector.z + 0.5f) - 0.5f;
            return new Vector3(x, 0.5f, z);
        }
        else
        {
            float x = Mathf.Round(vector.x);
            float z = Mathf.Round(vector.z);
            return new Vector3(x, 0.5f, z);
        } 
    }

    


    public void PlaceTarget()
    {
        Transform target = GameObject.Find("Target").transform;
        float xStart = -NumberOfRows / 2f;
        float zStart = -NumberOfRows / 2f;

       
        GameObject head = GameObject.Find("Head");
        List<Vector3> positions = new List<Vector3>() { 
            GetSnapPosition(head.transform.position),  
        };

        // For the first time, do not consider the target opsition.
        if (!firstTimeSetTarget)
        {
            positions.Add(GetSnapPosition(target.transform.position));
        }
        firstTimeSetTarget = false;

        TailMovement[] tails = FindObjectsOfType<TailMovement>();
      
        for(int k =0; k < tails.Length; k++)
        {
            positions.Add(GetSnapPosition(tails[k].transform.position));
        }
       
        if(positions.Count == NumberOfRows * NumberOfRows)
        {
            Destroy(target.gameObject);
            GameObject.Find("GameController").GetComponent<GameController>().SetWin();
            return;
        }
        bool needFindPlace = true;
        Vector3 newPosition = new Vector3() ;
        while (needFindPlace)
        {
            needFindPlace = false;
            int i = Random.Range(0, NumberOfRows);
            int j = Random.Range(0, NumberOfRows);

            newPosition = new Vector3(xStart + 0.5f + i, 0.5f, zStart + 0.5f + j);
            for (int k = 0; k < positions.Count; k++)
            {
                if(Vector3.Distance(positions[k],newPosition) < 0.1f)
                {
                    needFindPlace = true;
                    break;
                }
            }       
        }
      
        target.transform.position = newPosition;
    }
    void SetBoard()
    {

        float xStart = -NumberOfRows / 2f;
        float zStart = -NumberOfRows / 2f;

        for (int i = 0; i < NumberOfRows; i++)
        {
            for (int j = 0; j < NumberOfRows; j++)
            {
                if ((i + j) % 2 == 0)
                {
                    Instantiate(GameManager.instance.GridTile1, new Vector3(xStart + 0.5f + i, 0, zStart + 0.5f + j), Quaternion.identity, transform);
                }
                else
                {
                    Instantiate(GameManager.instance.GridTile2, new Vector3(xStart + 0.5f + i, 0, zStart + 0.5f + j), Quaternion.identity, transform);
                }
            }
        }
        for (int i = 1; i < NumberOfRows; i++)
        {
            GameObject line = Instantiate(GameManager.instance.GridLine, new Vector3(0, 0.01f, zStart + i), Quaternion.identity, transform);
            line.transform.localScale = new Vector3(NumberOfRows, 0.1f, 0.1f);


        }

        for (int i = 1; i < NumberOfRows; i++)
        {
            GameObject line = Instantiate(GameManager.instance.GridLine, new Vector3(xStart + i, 0.01f, 0), Quaternion.Euler(0, 90, 0), transform);
            line.transform.localScale = new Vector3(NumberOfRows, 0.1f, 0.1f);
        }

        GameObject wall = Instantiate(GameManager.instance.Wall, new Vector3(0, 0.01f, zStart), Quaternion.identity, transform);
        wall.transform.localScale = new Vector3(NumberOfRows, 1f, 0.1f);
        wall = Instantiate(GameManager.instance.Wall, new Vector3(0, 0.01f, -zStart), Quaternion.identity, transform);
        wall.transform.localScale = new Vector3(NumberOfRows, 1f, 0.1f);

        wall = Instantiate(GameManager.instance.Wall, new Vector3(xStart, 0.01f, 0), Quaternion.Euler(0, 90, 0), transform);
        wall.transform.localScale = new Vector3(NumberOfRows, 1f, 0.1f);
        wall = Instantiate(GameManager.instance.Wall, new Vector3(-xStart, 0.01f, 0), Quaternion.Euler(0, 90, 0), transform);
        wall.transform.localScale = new Vector3(NumberOfRows, 1f, 0.1f);
    }

    
}
