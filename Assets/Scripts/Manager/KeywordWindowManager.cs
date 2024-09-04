using UnityEngine;
using System.Collections.Generic;

public class KeywordWindowManager : MonoBehaviour
{
    public Transform canvasTransform; // 창이 나타날 부모 캔버스의 Transform
    public List<string> keywords = new List<string>(); // 키워드 리스트
    public List<GameObject> keywordWindows = new List<GameObject>(); // 각 키워드에 대응하는 창 프리팹 리스트
    private GameObject activeWindow; // 현재 활성화된 창을 저장하는 변수

    private void Start()
    {
        // LocalKeywordSearch 컴포넌트에서 이벤트 구독
        LocalKeywordSearch keywordSearch = FindObjectOfType<LocalKeywordSearch>();

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
        if (index >= 0 && index < keywordWindows.Count)
        {
            // 기존에 활성화된 창이 있으면 제거
            if (activeWindow != null)
            {
                Destroy(activeWindow);
            }

            // 해당 키워드에 맞는 새 창을 인스턴스화하고 활성화된 창으로 설정
            activeWindow = Instantiate(keywordWindows[index], canvasTransform);
        }
        else
        {
            Debug.LogWarning("Keyword does not have a corresponding window.");
        }
    }
}
