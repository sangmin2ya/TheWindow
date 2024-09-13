using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // TextMeshPro 사용을 위한 네임스페이스 추가

public class StartTextController : MonoBehaviour
{
    public TextMeshProUGUI text1; // 첫 번째 텍스트 (TextMeshPro 사용)
    public TextMeshProUGUI text2; // 두 번째 텍스트 (TextMeshPro 사용)
    public TextMeshProUGUI text3; // 세 번째 텍스트 (TextMeshPro 사용)
    public float typingSpeed = 0.05f; // 타이핑 속도 

    private string fullText1;
    private string fullText2;
    private string fullText3;
    private bool isTyping = false; // 현재 타이핑 중인지 여부
    private bool isText1Done = false; // 첫 번째 텍스트가 출력 완료되었는지 여부
    private bool isText2Done = false; // 두 번째 텍스트가 출력 완료되었는지 여부
    private bool isText3Done = false; // 세 번째 텍스트가 출력 완료되었는지 여부

    void Start()
    {
        // 초기화 - 두 텍스트에 입력된 전체 문자열 저장
        fullText1 = text1.text;
        fullText2 = text2.text;
        fullText3 = text3.text;

        // 텍스트 초기화 (빈 텍스트로 시작)
        text1.text = "";
        text2.text = "";
        text3.text = "";

        // 첫 번째 텍스트 출력 시작
        StartCoroutine(TypeText(fullText1, text1));
    }

    void Update()
    {
        // 클릭 이벤트 처리
        if (Input.GetMouseButtonDown(0))
        {
            // 현재 타이핑 중이면 스킵
            if (isTyping)
            {
                StopAllCoroutines(); // 모든 코루틴 중단
                if (!isText1Done)
                {
                    // 첫 번째 텍스트 타이핑 스킵
                    text1.text = fullText1;
                    isText1Done = true;
                    isTyping = false;
                }
                else if (!isText2Done)
                {
                    // 두 번째 텍스트 타이핑 스킵
                    text2.text = fullText2;
                    isText2Done = true;
                    isTyping = false;
                }
                else if (!isText3Done)
                {
                    // 세 번째 텍스트 타이핑 스킵
                    text3.text = fullText3;
                    isText3Done = true;
                    isTyping = false;
                }
            }
            else
            {
                // 타이핑이 완료되었으면 다음 텍스트 출력 시작
                if (isText1Done && !isText2Done)
                {
                    StartCoroutine(TypeText(fullText2, text2));
                    text1.gameObject.SetActive(false);
                }
                else if (isText2Done && !isText3Done)
                {
                    // 모든 텍스트가 출력된 후 부모 오브젝트의 투명도 조정 및 비활성화
                    StartCoroutine(TypeText(fullText3, text3));
                    text2.gameObject.SetActive(false);
                }
                else if (isText1Done && isText2Done && isText3Done)
                {
                    StartCoroutine(FadeOutAndDisable());
                    text3.gameObject.SetActive(false);
                }
            }
        }
    }

    // 텍스트를 한 글자씩 타이핑하는 코루틴
    IEnumerator TypeText(string fullText, TextMeshProUGUI targetText)
    {
        isTyping = true;
        targetText.text = ""; // 빈 텍스트로 시작

        foreach (char letter in fullText)
        {
            targetText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        // 첫 번째 텍스트가 완료되었는지 여부 설정
        if (targetText == text1)
        {
            isText1Done = true;
        }
        else if (targetText == text2)
        {
            isText2Done = true;
        }
        else if (targetText == text3)
        {
            isText3Done = true;
        }
    }

    // 부모 오브젝트의 투명도를 조정하고 비활성화하는 코루틴
    IEnumerator FadeOutAndDisable()
    {
        float fadeDuration = 2.0f;
        float elapsedTime = 0f;
        Color color = GetComponent<Image>().color;
        // 투명도를 서서히 줄이기
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Image>().color = new Color(color.r, color.b, color.g, Mathf.Lerp(1, 0, elapsedTime / fadeDuration));
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
