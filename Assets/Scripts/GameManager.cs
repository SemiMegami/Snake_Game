using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public int NumberOfRows;
    public int NumberOfTailBlocks;
    public GameObject GridTile1;
    public GameObject GridTile2;
    public GameObject GridLine;
    public GameObject Wall;
    public int score;
    public int maxScore;
    int targetCountToWin = 5;
    public int TargetCountToWin { get { return targetCountToWin; } }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        maxScore = 0;
    }


    
}
