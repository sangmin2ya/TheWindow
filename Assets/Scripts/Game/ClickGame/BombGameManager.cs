using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BombGameManager : MonoBehaviour
{
    public static BombGameManager instance; // 싱글톤 인스턴스
    public GameObject bombPrefab;       // 폭탄 프리팹
    public TextMeshProUGUI livesText;              // 목숨 UI
    public TextMeshProUGUI currentScoreText;
    public GameObject gameOverText;           // 게임 오버 UI
    public GameObject scoreText;              // 점수 UI
    

    private int lives = 5;              // 초기 목숨 5개
    private float spawnRate = 2f;       // 초기 스폰 간격 2초
    private float timeElapsed = 0f;     // 경과 시간
    private int score = 0;              // 점수
    private int spawnCount = 1;             // 스폰된 폭탄 수
    private bool whileGame = false;
    private float spawnTimer = 0f; // 스폰 타이머

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            scoreText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = (i + 1) + ". " + MemoManager.Instance.scoreList[i].Item2 + " - " + MemoManager.Instance.scoreList[i].Item1.ToString() + " 점";
        }
        scoreText.SetActive(false);
        // InvokeRepeating("SpawnBomb", spawnRate, spawnRate); // 폭탄 스폰 반복 호출
        // UpdateLivesText();
        // gameOverText.SetActive(false);
    }

    private void Update()
    {
        if (whileGame)
        {
            currentScoreText.text = "점수: " + score;
            // 시간이 지남에 따라 스폰 간격이 짧아짐
            timeElapsed += Time.deltaTime;

            // 스폰 속도 계산 (시간에 비례하여 점점 빠르게, 최소값 0.2초)
            spawnRate = Mathf.Max(0.5f, 2f - (timeElapsed / 10f)); // 처음 2초, 10초 후엔 0.2초까지 줄어듦

            // 스폰 타이머 업데이트
            spawnTimer += Time.deltaTime;

            // 스폰 타이머가 spawnRate를 넘으면 폭탄 스폰
            if (spawnTimer >= spawnRate)
            {
                SpawnBomb();
                spawnTimer = 0f; // 타이머 초기화
            }
        }
    }

    private void SpawnBomb()
    {
        // Image 객체의 RectTransform 가져오기
        RectTransform imageRect = transform.Find("Image").GetComponent<RectTransform>();

        // 폭탄 프리팹의 크기 (100 * 100)
        float bombWidth = 100f;
        float bombHeight = 100f;

        // Image의 크기 내에서 폭탄 크기를 고려한 랜덤한 위치 계산
        float xPos = Random.Range(-imageRect.rect.width / 2 + bombWidth / 2, imageRect.rect.width / 2 - bombWidth / 2);
        float yPos = Random.Range(-imageRect.rect.height / 2 + bombHeight / 2, imageRect.rect.height / 2 - bombHeight / 2);
        Vector2 spawnPosition = new Vector2(xPos, yPos);

        // 폭탄 생성
        GameObject go = Instantiate(bombPrefab, transform.Find("Image"));

        // 폭탄의 위치를 Image 안에서 랜덤 위치로 설정
        go.GetComponent<RectTransform>().localPosition = spawnPosition;
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
        livesText.text = "목숨: " + Mathf.Max(0, lives);
    }

    private void GameOver()
    {
        whileGame = false;
        // 게임 오버 처리
        CancelInvoke("SpawnBomb");
        GameObject.FindGameObjectsWithTag("Bomb").ToList().ForEach(Destroy);
        gameOverText.SetActive(true);
        gameOverText.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = "기록: " + score;
        StopAllCoroutines();
        if (score >= MemoManager.Instance.scoreList[0].Item1)
        {
            gameOverText.transform.Find("ScoreText/Highest").gameObject.SetActive(true);
            StartCoroutine(ChangeTextColorCoroutine(gameOverText.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>()));
        }
        else
        {
            gameOverText.transform.Find("ScoreText/Highest").gameObject.SetActive(false);
            gameOverText.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().color = Color.black;
        }
        MemoManager.Instance.AddScore(score);
        for (int i = 0; i < 5; i++)
        {
            scoreText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = (i + 1) + ". " + MemoManager.Instance.scoreList[i].Item2 + " - " + MemoManager.Instance.scoreList[i].Item1.ToString() + " 점";
        }
    }
    private IEnumerator ChangeTextColorCoroutine(TextMeshProUGUI textMeshPro)
    {
        float hue = 0f; // 색상값 (0은 빨강, 1은 다시 빨강)
        float saturation = 1f; // 채도 (1은 최대 채도)
        float value = 1f; // 명도 (1은 최대 밝기)

        while (true)
        {
            // 색상을 HSV로 변환 후 텍스트 색상에 적용
            textMeshPro.color = Color.HSVToRGB(hue, saturation, value);

            // 색상(hue)값을 시간에 따라 증가시킴 (무지개 효과)
            hue += Time.deltaTime / 3;
            if (hue > 1f)
            {
                hue -= 1f; // hue는 0~1 사이 값을 가지므로 1을 초과하면 다시 0으로 리셋
            }

            // 프레임마다 업데이트
            yield return null;
        }
    }
    public void RestartGame()
    {
        whileGame = true;
        // 게임 재시작
        livesText.gameObject.SetActive(true);
        gameOverText.SetActive(false);
        lives = 5;
        score = 0;
        timeElapsed = 0f;
        spawnRate = 2f;
        spawnTimer = 0f; // 스폰 타이머
        UpdateLivesText();
        gameOverText.transform.Find("Highest")?.gameObject.SetActive(false);
        gameOverText.SetActive(false);
        InvokeRepeating("SpawnBomb", spawnRate, spawnRate);
    }
    public void ToTitle()
    {
        gameOverText.SetActive(false);
        livesText.gameObject.SetActive(false);
    }
    public void CloseGame()
    {
        Destroy(gameObject);
    }
}
