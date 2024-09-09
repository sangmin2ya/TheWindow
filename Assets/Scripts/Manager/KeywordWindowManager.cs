using UnityEngine;
using System.Collections.Generic;
using TMPro; // TextMeshPro를 사용하기 위해 추가

public class KeywordWindowManager : MonoBehaviour
{
    public Transform canvasTransform; // 창이 나타날 부모 캔버스의 Transform
    public List<string> keywords = new List<string>(); // 키워드 리스트
    public List<GameObject> keywordWindows = new List<GameObject>(); // 각 키워드에 대응하는 창 프리팹 리스트
    private GameObject activeWindow; // 현재 활성화된 창을 저장하는 변수

    public TMP_Text resultMessageText; // 검색 결과 없음을 표시할 TextMeshPro 텍스트

    private void Start()
    {
        // LocalKeywordSearch 컴포넌트에서 이벤트 구독
        LocalKeywordSearch keywordSearch = FindObjectOfType<LocalKeywordSearch>();
        resultMessageText.gameObject.SetActive(false); // 메시지 비활성화

        if (keywordSearch != null)
        {
            keywordSearch.onKeywordFound.AddListener(ShowKeywordWindow);
            Debug.Log("Subscribed to onKeywordFound event");  // 이벤트 구독 확인 로그 추가
        }
        else
        {
            Debug.LogError("LocalKeywordSearch component not found");  // LocalKeywordSearch가 없는 경우 오류 로그
        }
    }
    // 키워드에 맞는 창을 보여주는 함수
    public void ShowKeywordWindow(string keyword)
    {
        int index = keywords.IndexOf(keyword);

        // 기존에 활성화된 창이 있으면 제거
        if (activeWindow != null)
        {
            Destroy(activeWindow);
        }

        // "검색 결과 없음" 메시지가 보이는 경우 메시지 숨김 처리
        resultMessageText.gameObject.SetActive(false);

        if (index >= 0 && index < keywordWindows.Count)
        {
            // 해당 키워드에 맞는 새 창을 인스턴스화하고 활성화된 창으로 설정
            activeWindow = Instantiate(keywordWindows[index], canvasTransform);

            // 검색된 키워드를 포함한 결과 메시지를 표시
            // switch (keyword)
            // {
            //     case "블루베리":
            //         keyword = "블루카페";
            //         break;
            //     case "블루머핀":
            //         keyword = "블루베리머핀";
            //         break;
            //     case "블루파이":
            //         keyword = "라즈베리파이";
            //         break;
            // }
            resultMessageText.text = $"\"{keyword}\" 을(를) 검색하셨나요?";  // 메시지 설정 (검색한 키워드 포함)
            resultMessageText.gameObject.SetActive(true); // 메시지 활성화
        }
        else
        {
            // 키워드를 찾지 못한 경우
            resultMessageText.text = $"\"{keyword}\" 검색 결과 없음";  // 메시지 설정 (검색한 키워드 포함)
            resultMessageText.gameObject.SetActive(true); // 메시지 활성화
            Debug.LogWarning($"Keyword \"{keyword}\" does not have a corresponding window.");
        }
    }
}
