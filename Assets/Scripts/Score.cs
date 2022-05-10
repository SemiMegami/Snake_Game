using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    int MaxScore = 500;
    [SerializeField]
    int MinScore = 100;
    [SerializeField]
    int Decrement = 50;
    [SerializeField]
    float StartReductionTime = 3;
    [SerializeField]
    float StartReductionTimPeriod = 1;

    Color maxColor = Color.yellow;
    Color minColor = Color.black;
    Material material;
    int score;
    
    public int Currentccore { get { return score; } }
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        RefreshScore();
       
    }



    void ReduceScore()
    {
        score -= Decrement;
        if(score < MinScore)
        {
            score = MinScore;

        }
        material.SetColor("_Color", Color.Lerp(maxColor,minColor, 1f* (MaxScore - score) / (MaxScore - MinScore)));

    }

    public void RefreshScore()
    {
        CancelInvoke();
        score = MaxScore;
        InvokeRepeating("ReduceScore", StartReductionTime, StartReductionTimPeriod);
        material.SetColor("_Color", maxColor);

    }

    private void Update()
    {
        transform.Rotate(0, score  * Time.deltaTime, 0);
    }

}
