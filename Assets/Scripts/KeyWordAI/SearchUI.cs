using UnityEngine;
using TMPro;  // TextMeshPro 네임스페이스
using UnityEngine.UI;

public class SearchUI : MonoBehaviour
{
    public HuggingFaceSearch huggingFaceSearch;
    public TMP_InputField inputField;  // TextMeshPro InputField
    public TMP_Text resultText;        // TextMeshPro Text
    public Button searchButton;

    void Start()
    {
        // 검색 버튼 클릭 시 OnSearchButtonClick 함수 호출
        searchButton.onClick.AddListener(OnSearchButtonClick);
    }

    // 검색 버튼 클릭 시 실행되는 함수
    public void OnSearchButtonClick()
    {
        string query = inputField.text;
        huggingFaceSearch.StartSearch(query);
    }

    // 검색 결과를 화면에 표시하는 함수
    public void DisplayResult(string result)
    {
        resultText.text = result;
    }
}
