using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public Slider loadingSlider;      // 슬라이더 UI
    public TextMeshProUGUI loadingText;          // "로딩중..." 텍스트 UI
    public TextMeshProUGUI percentageText;       // 퍼센트 텍스트 UI
    private int currentDots = 0;      // 현재 점의 개수
    private string loadingMessage = "부팅 중"; // 기본 로딩 메시지

    public void ShowBootingScree()
    {
        Invoke("ShowBooting", 1.0f);
    }
    private void ShowBooting()
    {
        GameObject.Find("Start").SetActive(false);
        StartCoroutine(LoadSceneWithProgress());
    }
    IEnumerator LoadSceneWithProgress()
    {
        // 슬라이더를 80%까지 5초 동안 채우기
        float timeElapsed = 0f;
        float duration = 5f;
        float targetFillAmount = 0.8f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float progress = Mathf.Lerp(0f, targetFillAmount, timeElapsed / duration);
            loadingSlider.value = progress;
            UpdatePercentage(progress);
            yield return null;
        }

        // 나머지 19%를 5초 동안 채우기 (99%까지)
        timeElapsed = 0f;
        targetFillAmount = 0.99f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float progress = Mathf.Lerp(0.8f, targetFillAmount, timeElapsed / duration);
            loadingSlider.value = progress;
            UpdatePercentage(progress);
            yield return null;
        }

        // 99%에서 3초간 대기
        loadingSlider.value = 0.99f;
        UpdatePercentage(0.99f);
        yield return new WaitForSeconds(3f);

        // 마지막 1%를 빠르게 채우기
        loadingSlider.value = 1f;
        UpdatePercentage(1f);

        // 로딩 텍스트 점 변화 코루틴 시작
        StartCoroutine(AnimateLoadingText());
        yield return new WaitForSeconds(3f);
        // 모든 작업이 끝나면 새로운 씬 로드 (예: "NewScene" 이름의 씬)
        SceneManager.LoadScene("Login");
    }

    // 퍼센트 텍스트 업데이트 메서드
    private void UpdatePercentage(float progress)
    {
        percentageText.text = (progress * 100).ToString("F0") + "%";
    }

    IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            // 점 개수 증가시키기
            currentDots = (currentDots % 5) + 1;

            // 로딩 메시지 업데이트
            loadingText.text = loadingMessage + new string('.', currentDots);

            yield return new WaitForSeconds(1f);
        }
    }
}
