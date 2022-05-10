using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public Text score;
    public Text maxScore;
    public GameObject Pause;
    public GameObject Win;
    bool winStage = false;
    public bool WindStage { get { return winStage; } }
    // Start is called before the first frame update
    
    private void Start()
    {
        ResetScore();
    }
    public void IncreaseScore(int score)
    {
        GetComponent<GameBoard>().PlaceTarget();
        GameManager.instance.score += score;
        if (GameManager.instance.score > GameManager.instance.maxScore)
        {
            GameManager.instance.maxScore = GameManager.instance.score;
        }
        RefreshScoreUI();
    }

    private void ResetScore()
    {
        winStage = false;
        GameManager.instance.score = 0;
        Time.timeScale = 1;
        RefreshScoreUI();
    }

    private void RefreshScoreUI()
    {
        score.text = "Score : " + GameManager.instance.score;
        maxScore.text = "Score : " + GameManager.instance.maxScore;
    }

    public void SetWin()
    {
        winStage = true;
        Win.SetActive(true);
        GameObject.Find("Head").GetComponent<PlayerController>().moveable = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown (KeyCode.Escape))
        {
            if (winStage)
            {
                GameManager.instance.GetComponent<ScenesManager>().ResetScene();
            }
            else
            {
                if (!Pause.activeInHierarchy)
                {
                    Time.timeScale = 0;
                    Pause.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    Pause.SetActive(false);
                }
            }
            
        }

    }
}
