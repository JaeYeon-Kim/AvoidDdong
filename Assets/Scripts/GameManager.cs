using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    [SerializeField] private TextMeshProUGUI bestScoreText;

    [SerializeField] private TextMeshProUGUI gameScore;

    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private GameObject panel;

    [SerializeField] private SoundManager soundManager;

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
       // Debug.Log("score " + score);
    }

    public void GameStart()
    {
        score = 0;
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

        if (score >= PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore", score);
        }

        currentScoreText.text = $"Current Score {score}";

        bestScoreText.text = $"Best Score {PlayerPrefs.GetInt("BestScore", 0)}";

        panel.SetActive(true);


        // 생성된 똥 오브젝트 들 삭제
        foreach (var poopObj in poopObjects)
        {
            Destroy(poopObj);
        }
        poopObjects.Clear();  // 똥 리스트 초화 
    }

    private void CreatePoop()
    {
        // Camera.main.ViewportToWorldPoint: 메인 카메라 공간을 월드 좌표로 변경해줌 , Viewport에서 카메라의 좌측끝이0, 우측끝이1
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(UnityEngine.Random.Range(0.0f, 1.0f), 1.1f, 0));
        pos.z = 0.0f;
        GameObject newPoop = Instantiate(poop, pos, Quaternion.identity);

        // 생성된 poop 오브젝트를 리스트에 추가
        poopObjects.Add(newPoop);
    }

    IEnumerator CreatePoopRoutine()
    {
        while (stopTrigger)
        {
            CreatePoop();
            yield return new WaitForSeconds(0.4f);      // 똥 생성 주기 조절 
        }

    }
}
