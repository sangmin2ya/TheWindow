using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;

public class LocalKeywordSearch : MonoBehaviour
{
    public TextMeshProUGUI inputField; // TextMeshPro InputField
    public TMP_Text resultText; // TextMeshPro Text to display the result

    // 사전 정의된 키워드 리스트
    private List<string> keywords = new List<string> { "날씨", "메리포저", "존에리드", "강수희", "블루베리", "블루머핀", "블루파이" };

    // 유사한 키워드를 찾았을 때 이벤트로 전달
    public UnityEvent<string> onKeywordFound = new UnityEvent<string>();

    // 최소 유사도 임계값 (0 ~ 1 사이의 값)
    private float similarityThreshold = 0.5f;

    // 검색 버튼 클릭 시 호출되는 함수
    public void OnSearchButtonClick()
    {
        string userInput = inputField.text.ToLower();
        string bestMatch = FindMostSimilarKeyword(userInput);

        if (bestMatch != null)
        {
            resultText.text = "Most similar keyword: " + bestMatch;
            Debug.Log("Best match found: " + bestMatch);  // 디버그 로그 추가
            onKeywordFound.Invoke(bestMatch); // 키워드를 이벤트로 전달
            Debug.Log("전달완료 " + bestMatch);  // 디버그 로그 추가

        }
        else
        {
            resultText.text = "No similar keywords found.";
            Debug.Log("No similar keyword found");  // 디버그 로그 추가
            onKeywordFound.Invoke(userInput); // 키워드를 이벤트로 전달

        }
    }


    // 유사도가 가장 높은 키워드를 찾는 함수
    private string FindMostSimilarKeyword(string input)
    {
        string bestMatch = null;
        float bestSimilarity = 0f;

        foreach (var keyword in keywords)
        {
            float similarity = CalculateSimilarity(input, keyword);

            if (similarity > bestSimilarity && similarity >= similarityThreshold)
            {
                bestSimilarity = similarity;
                bestMatch = keyword;
            }
        }

        return bestMatch;
    }

    // 두 문자열 간의 유사도를 계산하는 함수 (Levenshtein 거리 기반)
    private float CalculateSimilarity(string source, string target)
    {
        int stepsToSame = LevenshteinDistance(source, target);
        return 1.0f - ((float)stepsToSame / Mathf.Max(source.Length, target.Length));
    }

    // Levenshtein 거리 계산 함수
    private int LevenshteinDistance(string source, string target)
    {
        int[,] matrix = new int[source.Length + 1, target.Length + 1];

        for (int i = 0; i <= source.Length; i++)
            matrix[i, 0] = i;

        for (int j = 0; j <= target.Length; j++)
            matrix[0, j] = j;

        for (int i = 1; i <= source.Length; i++)
        {
            for (int j = 1; j <= target.Length; j++)
            {
                int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                matrix[i, j] = Mathf.Min(
                    Mathf.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost
                );
            }
        }

        return matrix[source.Length, target.Length];
    }
}
