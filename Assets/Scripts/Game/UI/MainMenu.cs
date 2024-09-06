using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button creditsButton;
    public GameObject creditsPanel;

    private Button selectedButton;
    private Color defaultColor;
    private Color selectedColor = Color.yellow; // 선택된 버튼의 색상
    private bool isCreditsPanelActive = false;

    void Start()
    {
        if (startButton != null && creditsButton != null)
        {
            startButton.onClick.AddListener(StartGame);
            creditsButton.onClick.AddListener(ShowCredits);

            // 초기 선택 버튼은 Start 버튼
            selectedButton = startButton;
            defaultColor = startButton.image.color;
            HighlightButton(selectedButton);
        }

        // 크레딧 패널 초기 비활성화
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isCreditsPanelActive)
        {
            // 크레딧 패널이 활성화된 상태에서 스페이스바를 누르면 패널을 닫고 게임을 재개
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HideCredits();
            }
        }
        else
        {
            HandleButtonSelection();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (selectedButton == startButton)
                {
                    StartGame();
                }
                else if (selectedButton == creditsButton)
                {
                    ShowCredits();
                }
            }
        }
    }

    void HandleButtonSelection()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            // 선택된 버튼을 전환
            if (selectedButton == startButton)
            {
                selectedButton = creditsButton;
            }
            else
            {
                selectedButton = startButton;
            }

            // 버튼 색상 변경
            HighlightButton(selectedButton);
        }
    }

    void HighlightButton(Button button)
    {
        // 모든 버튼을 기본 색상으로 리셋
        startButton.image.color = defaultColor;
        creditsButton.image.color = defaultColor;

        // 선택된 버튼을 강조 색상으로 변경
        button.image.color = selectedColor;
    }

    void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    void ShowCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
            isCreditsPanelActive = true;
            Time.timeScale = 0f; // 게임 일시정지
        }
    }

    void HideCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
            isCreditsPanelActive = false;
            Time.timeScale = 1f; // 게임 재개
        }
    }
}
