using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PasswordPrompt : MonoBehaviour
{
    public TextMeshProUGUI passwordInput; // 비밀번호 입력 필드
    public Button confirmButton;  // 확인 버튼
    public Button cancelButton;   // 취소 버튼
    public TextMeshProUGUI feedbackText;  // 피드백 메시지 출력

    private string correctPassword;
    private System.Action onPasswordCorrect;
    private System.Action onPasswordIncorrect;

    private int failedAttempts = 0;  // 틀린 시도 횟수
    private bool isLocked = false;  // 확인 버튼이 잠긴 상태인지 확인
    private float lockDuration = 30f;  // 잠금 시간 (초)
    private float remainingLockTime;  // 남은 잠금 시간

    // 비밀번호 프롬프트 초기화
    public void Initialize(string password, System.Action onCorrect, System.Action onIncorrect)
    {
        correctPassword = password;
        onPasswordCorrect = onCorrect;
        onPasswordIncorrect = onIncorrect;

        confirmButton.onClick.AddListener(CheckPassword);
        cancelButton.onClick.AddListener(ClosePrompt);
    }

    // 비밀번호 체크 함수
    private void CheckPassword()
    {
        if (isLocked)
        {
            feedbackText.text = $"잠겼습니다. {Mathf.CeilToInt(remainingLockTime)}초 후에 다시 시도하세요.";
            return;
        }

        if (passwordInput.text == correctPassword)
        {
            feedbackText.text = "비밀번호가 맞았습니다!";
            onPasswordCorrect?.Invoke();
            ClosePrompt();
        }
        else
        {
            failedAttempts++;
            feedbackText.text = "비밀번호가 틀렸습니다.";
            onPasswordIncorrect?.Invoke();

            if (failedAttempts >= 5)
            {
                LockConfirmButton();
            }
        }
    }

    // 확인 버튼 잠금
    private void LockConfirmButton()
    {
        isLocked = true;
        remainingLockTime = lockDuration;
        StartCoroutine(LockTimer());
    }

    // 잠금 시간 카운트다운 코루틴
    private IEnumerator LockTimer()
    {
        while (remainingLockTime > 0)
        {
            remainingLockTime -= Time.deltaTime;
            feedbackText.text = $"잠겼습니다. {Mathf.CeilToInt(remainingLockTime)}초 후에 다시 시도하세요.";
            yield return null;
        }

        // 잠금 해제
        isLocked = false;
        failedAttempts = 0; // 틀린 시도 횟수 초기화
        feedbackText.text = "다시 시도할 수 있습니다.";
    }

    // 비밀번호 창 닫기
    private void ClosePrompt()
    {
        Destroy(gameObject);
    }
}
