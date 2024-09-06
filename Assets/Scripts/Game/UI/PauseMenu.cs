using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : Singleton<PauseMenu>
{
    public bool isPause = false;
    public Button resumeButton;
    public Button restartButton;
    public Button creditsButton;
    public Button quitButton;
    public GameObject pauseMenuCanvas;
    public GameObject creditsPanel;

    private Button selectedButton;
    private Color defaultColor;
    private Color selectedColor = Color.yellow; // 선택된 버튼의 색상
    private bool isCreditsPanelActive = false;
    public string sceneToLoad; // 로드할 씬의 이름
    void Start()
    {
        // 메뉴 비활성화
        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        creditsButton.onClick.AddListener(ShowCredits);
        quitButton.onClick.AddListener(Quit);

        // 초기 선택 버튼 설정
        selectedButton = resumeButton;
        defaultColor = resumeButton.image.color;
        HighlightButton(selectedButton);

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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HideCredits();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPause)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }

            if (isPause)
            {
                HandleButtonSelection();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (selectedButton == resumeButton)
                    {
                        Resume();
                    }
                    else if (selectedButton == restartButton)
                    {
                        Restart();
                    }
                    else if (selectedButton == creditsButton)
                    {
                        ShowCredits();
                    }
                    else if (selectedButton == quitButton)
                    {
                        Quit();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                HardReset();
            }
        }
    }

    void HandleButtonSelection()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (selectedButton == quitButton)
            {
                selectedButton = creditsButton;
            }
            else if (selectedButton == creditsButton)
            {
                selectedButton = restartButton;
            }
            else if (selectedButton == restartButton)
            {
                selectedButton = resumeButton;
            }
            else
            {
                selectedButton = quitButton;
            }

            HighlightButton(selectedButton);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (selectedButton == resumeButton)
            {
                selectedButton = restartButton;
            }
            else if (selectedButton == restartButton)
            {
                selectedButton = creditsButton;
            }
            else if (selectedButton == creditsButton)
            {
                selectedButton = quitButton;
            }
            else
            {
                selectedButton = resumeButton;
            }

            HighlightButton(selectedButton);
        }
    }

    void HighlightButton(Button button)
    {
        resumeButton.image.color = defaultColor;
        restartButton.image.color = defaultColor;
        creditsButton.image.color = defaultColor;
        quitButton.image.color = defaultColor;

        button.image.color = selectedColor;
    }

    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
    }

    public void Pause()
    {
        pauseMenuCanvas.transform.SetAsLastSibling();
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
    }

    // 버튼을 눌러 씬을 새로 로드하는 함수
    public void Quit()
    {
        // 씬을 새로 로드 (기존 씬을 닫고 새 씬을 로드)
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }

    public void Restart()
    {
        pauseMenuCanvas.SetActive(false);
        isPause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
            isCreditsPanelActive = true;
            Time.timeScale = 0f; // 게임 일시정지
        }
    }

    public void HideCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
            isCreditsPanelActive = false;
            Time.timeScale = 0f; // 다시 퍼즈 상태 유지
        }
    }

    private void HardReset()
    {
        PlayerPrefs.DeleteKey("BestScore");
    }
}
