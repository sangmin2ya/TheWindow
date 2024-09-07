using UnityEngine;
using UnityEngine.UI;

public class BombGameManager : MonoBehaviour
{
    public static BombGameManager instance; // 싱글톤 인스턴스
    public GameObject bombPrefab;       // 폭탄 프리팹
    public Text livesText;              // 목숨 UI
    public GameObject gameOverText;           // 게임 오버 UI

    private int lives = 5;              // 초기 목숨 5개
    private float spawnRate = 2f;       // 초기 스폰 간격 2초
    private float timeElapsed = 0f;     // 경과 시간
    private int score = 0;              // 점수

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating("SpawnBomb", spawnRate, spawnRate); // 폭탄 스폰 반복 호출
        UpdateLivesText();
        gameOverText.SetActive(false);
    }

    private void Update()
    {
        // 시간이 지남에 따라 스폰 간격이 짧아짐
        timeElapsed += Time.deltaTime;
        if (timeElapsed > 10f) // 10초마다 스폰 속도 증가
        {
            spawnRate = Mathf.Max(0.5f, spawnRate - 0.2f); // 최소 스폰 속도 0.5초
            CancelInvoke("SpawnBomb");
            InvokeRepeating("SpawnBomb", spawnRate, spawnRate);
            timeElapsed = 0f;
        }
    }

    private void SpawnBomb()
    {
        // 화면의 랜덤 위치에 폭탄 스폰
        Vector2 spawnPosition = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));
        Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
    }

    public void BombClicked()
    {
        // 폭탄 클릭 시 처리 (추가 점수나 효과는 여기서 구현 가능)
        score++;
    }

    public void BombMissed()
    {
        // 폭탄을 놓쳤을 때 목숨 감소
        lives--;
        UpdateLivesText();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateLivesText()
    {
        livesText.text = "목숨: " + lives;
    }

    private void GameOver()
    {
        // 게임 오버 처리
        CancelInvoke("SpawnBomb");
        gameOverText.SetActive(true);
        gameOverText.transform.Find("ScoreText").GetComponent<Text>().text = "기록: " + score;
        if (score >= MemoManager.Instance.scoreList[4])
        {
            gameOverText.transform.Find("Highest").gameObject.SetActive(true);
        }
        MemoManager.Instance.AddScore(score);
        for (int i = 0; i < 5; i++)
        {
            gameOverText.transform.Find((i + 1).ToString()).GetComponent<Text>().text = (i + 1) + ". " + MemoManager.Instance.scoreList[i].ToString() + "점";
        }
    }
    public void RestartGame()
    {
        // 게임 재시작
        lives = 5;
        score = 0;
        timeElapsed = 0f;
        spawnRate = 2f;
        UpdateLivesText();
        gameOverText.transform.Find("Highest").gameObject.SetActive(false);
        gameOverText.SetActive(false);
        InvokeRepeating("SpawnBomb", spawnRate, spawnRate);
    }
    public void CloseGame()
    {
        Destroy(gameObject);
    }
}
