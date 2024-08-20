using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    [SerializeField] private GameObject poop;

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private GameObject panel;

    private int score;
    public bool stopTrigger = true;


    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Score : " + score;
    }


    public void Score()
    {
        score++;
        Debug.Log("score " + score);
    }

    public void GameStart()
    {
        stopTrigger = true;
        StartCoroutine(CreatePoopRoutine());
        panel.SetActive(false);
    }

    public void GameOver()
    {
        stopTrigger = false;

        StopCoroutine(CreatePoopRoutine());

        if (score >= PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore", score);
        }

        bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();

        panel.SetActive(true);
    }

    private void CreatePoop()
    {
        // Camera.main.ViewportToWorldPoint: 메인 카메라 공간을 월드 좌표로 변경해줌 , Viewport에서 카메라의 좌측끝이0, 우측끝이1
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(UnityEngine.Random.Range(0.0f, 1.0f), 1.1f, 0));
        pos.z = 0.0f;
        Instantiate(poop, pos, Quaternion.identity);
    }

    IEnumerator CreatePoopRoutine()
    {
        while (stopTrigger)
        {
            CreatePoop();
            yield return new WaitForSeconds(0.4f);
        }

    }
}
