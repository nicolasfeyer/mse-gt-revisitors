using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCtrl : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    private long score;

    void IncreaseScore()
    {
        score=score+100;
        scoreText.text = score.ToString();
    }

    public void Add(long increment)
    {
        score+=increment;
        scoreText.text = score.ToString();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("IncreaseScore", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
