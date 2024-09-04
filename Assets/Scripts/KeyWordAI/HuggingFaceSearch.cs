using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class HuggingFaceSearch : MonoBehaviour
{
    public SearchUI searchUI; // SearchUI 스크립트를 참조

    private string serverUrl = "http://127.0.0.1:5000/search"; // 서버 URL

    public void StartSearch(string query)
    {
        StartCoroutine(SearchCoroutine(query));
    }

    private IEnumerator SearchCoroutine(string query)
    {
        string jsonData = "{\"query\": \"" + query + "\"}";

        using (UnityWebRequest request = new UnityWebRequest(serverUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // 성공적으로 응답을 받으면 결과를 UTF-8로 디코딩하여 SearchUI에 표시
                string jsonResponse = request.downloadHandler.text;

                // 응답에서 키워드만 추출
                string keyword = ExtractKeywordFromResponse(jsonResponse);

                // 추출한 키워드를 SearchUI에 표시
                searchUI.DisplayResult(keyword);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }

    private string ExtractKeywordFromResponse(string jsonResponse)
    {
        // 예시: {"response":"Keyword: birthday. User input: birthDay"}
        // 위와 같은 형식에서 "Keyword: "와 "." 사이의 부분을 추출

        int keywordStart = jsonResponse.IndexOf("Keyword: ") + "Keyword: ".Length;
        int keywordEnd = jsonResponse.IndexOf(".", keywordStart);

        if (keywordStart >= 0 && keywordEnd > keywordStart)
        {
            return jsonResponse.Substring(keywordStart, keywordEnd - keywordStart).Trim();
        }

        return "No keyword found";
    }
}
