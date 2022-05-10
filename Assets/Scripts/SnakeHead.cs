using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public GameObject TailBlock;
    List<TailMovement> tailMovements;
    GameController gameController;
    
    // Start is called before the first frame update
    void Awake()
    {
        tailMovements = new List<TailMovement>();
       
    }

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
    // Update is called once per frame

    public void SetAction(bool moveable)
    {
       
        foreach(var tail in tailMovements)
        {
            tail.SetMoveDirection(moveable);
        }
    }
    public void CreateTailBlock()
    {
        InstantiateTailBlock(tailMovements[tailMovements.Count - 1].markPoint);
    }
    



    public Transform InstantiateTailBlock(Vector3 position)
    {
        GameObject taleBlock = Instantiate(TailBlock, position, Quaternion.identity);
        TailMovement tailMovement = taleBlock.GetComponent<TailMovement>();
        tailMovement.Speed = GetComponent<PlayerController>().Speed;
        if (tailMovements.Count == 0)
        {
            tailMovement.target = transform;
        }
        else
        {
            tailMovement.target = tailMovements[tailMovements.Count - 1].transform;
        }
        tailMovements.Add(tailMovement);
        return taleBlock.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if(other.gameObject.GetComponent<Score>() != null)
        {

            gameController.IncreaseScore(other.gameObject.GetComponent<Score>().Currentccore);
            other.GetComponent<Score>().RefreshScore();
            CreateTailBlock();
        }
        else
        {
            GameManager.instance.GetComponent<ScenesManager>().ResetScene();
        }

    }

     
}
