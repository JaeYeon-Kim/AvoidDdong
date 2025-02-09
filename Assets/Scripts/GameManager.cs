using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    // 라운드 설정 클래스 
    [Serializable]
    public class RoundSettings
    {
        public GameObject poopPrefab;       // 사용할 똥 프리팹
        public float poopSpeed;       // 똥 낙하 속도 
        public float spawnInterval;     // 똥 생성 주기 

        public int requiredScore;       // 다음 라운드로 넘어가기 위한 점수 
    }


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

    [SerializeField] private TextMeshProUGUI bestScoreText;

    [SerializeField] private TextMeshProUGUI gameScore;

    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private GameObject panel;

    [SerializeField] private SoundManager soundManager;

    // 라운드 설정 리스트 및 현재 라운드 지정
    [SerializeField] private List<RoundSettings> rounds;        // 라운드 설정 리스트 
    private int currentRound;   // 현재 라운드 


    private int score;
    public bool stopTrigger = true;

    // 생성된 똥을 추적할 리스트 
    private List<GameObject> poopObjects = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        // 모바일 프레임 60 고정
        Application.targetFrameRate = 60;
        bestScoreText.text = $"Best Score {PlayerPrefs.GetInt("BestScore", 0)}";
    }

    // Update is called once per frame
    void Update()
    {
        gameScore.text = "Score : " + score;
    }


    public void Score()
    {
        score++;
        CheckRound();

    }

    public void GameStart()
    {
        score = 0;
        currentRound = 0;
        stopTrigger = true;
        poopObjects.Clear();
        StartCoroutine(CreatePoopRoutine());
        panel.SetActive(false);
        soundManager.Play();
    }

    public void GameOver()
    {
        stopTrigger = false;

        StopCoroutine(CreatePoopRoutine());
        soundManager.Stop();

        panel.SetActive(true);

        if (score >= PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore", score);
        }

        currentScoreText.text = $"Current Score {score}";

        bestScoreText.text = $"Best Score {PlayerPrefs.GetInt("BestScore", 0)}";




        // 생성된 똥 오브젝트 들 삭제
        foreach (var poopObj in poopObjects)
        {
            Destroy(poopObj);
        }
        poopObjects.Clear();  // 똥 리스트 초화 
    }

    private void CreatePoop()
    {
        // Camera.main.ViewportToWorldPoint: 메인 카메라 공간을 월드 좌표로 변경해줌
        // 1.1: 화면 1.0보다 살짝 높은곳에서 똥을 생성하여 자연스럽게 떨어지도록 설정 
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(UnityEngine.Random.Range(0.1f, 0.9f), 1.1f, 0));
        pos.z = 0.0f;

        GameObject newPoop = Instantiate(rounds[currentRound].poopPrefab, pos, Quaternion.identity);
        newPoop.GetComponent<Poop>().SetSpeed(rounds[currentRound].poopSpeed);
        // 생성된 poop 오브젝트를 리스트에 추가
        poopObjects.Add(newPoop);
    }

    private void CheckRound()
    {
        // 현재 라운드의 다음 라운드로 넘어갈 점수를 확인
        if (currentRound < rounds.Count - 1 && score >= rounds[currentRound + 1].requiredScore)
        {
            currentRound++;

        }

    }

    IEnumerator CreatePoopRoutine()
    {
        while (stopTrigger)
        {
            CreatePoop();
            yield return new WaitForSeconds(rounds[currentRound].spawnInterval);      // 똥 생성 주기 조절 
        }

    }
}
