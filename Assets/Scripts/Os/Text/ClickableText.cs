using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ClickableText : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI textMeshPro;
    private int lastWordIndex = -1; // 마지막으로 마우스가 위치한 단어의 인덱스
    private string originalText; // 원래 텍스트를 저장합니다.
    private string modifiedText; // 변경된 텍스트를 저장합니다.
    private Color hoverColor = Color.blue; // 마우스 호버 시 글자 색상

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();

        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI component not found!");
        }

        // Raycast Target이 설정되어 있는지 확인합니다.
        if (!textMeshPro.raycastTarget)
        {
            Debug.LogWarning("Raycast Target is not enabled on TextMeshProUGUI. Enabling it now.");
            textMeshPro.raycastTarget = true;
        }

        // 원래 텍스트 저장
        originalText = textMeshPro.text;
        modifiedText = originalText;
    }

    void Update()
    {
        // 텍스트 메쉬 데이터를 강제로 업데이트합니다.
        textMeshPro.ForceMeshUpdate();

        // 마우스 위치를 감지하여 단어 위에 있을 때 색상을 변경
        int wordIndex = TMP_TextUtilities.FindIntersectingWord(textMeshPro, Input.mousePosition, Camera.main);

        if (wordIndex != -1 && wordIndex != lastWordIndex)
        {
            // 이전에 변경된 단어 색상을 원래대로 돌립니다.
            if (lastWordIndex != -1)
            {
                RestoreWordColor();
            }

            // 현재 마우스가 있는 단어의 색상을 파란색으로 변경
            ApplyWordColor(wordIndex);
            lastWordIndex = wordIndex;
        }
        else if (wordIndex == -1 && lastWordIndex != -1)
        {
            // 마우스가 텍스트를 벗어난 경우 이전 단어 색상 복원
            RestoreWordColor();
            lastWordIndex = -1;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            return;
        }
        // 클릭한 위치에서 텍스트의 단어 인덱스를 얻습니다.
        int wordIndex = TMP_TextUtilities.FindIntersectingWord(textMeshPro, eventData.position, Camera.main);

        if (wordIndex != -1)
        {
            // 선택된 단어를 가져옵니다.
            string word = GetWordAtIndex(wordIndex);

            // 단어를 클립보드에 복사합니다.
            CopyToClipboard(word);

            // 클릭된 단어를 디버그 로그에 출력합니다.
            Debug.Log($"Clicked and copied word: {word}");
        }
    }

    private string GetWordAtIndex(int wordIndex)
    {
        // 선택한 단어의 인덱스를 통해 텍스트에서 단어를 추출합니다.
        TMP_WordInfo wordInfo = textMeshPro.textInfo.wordInfo[wordIndex];
        return wordInfo.GetWord();
    }

    private void CopyToClipboard(string text)
    {
        // 클립보드에 텍스트를 복사합니다.
        MemoManager.Instance.CopyText(text);
        Debug.Log($"Copied '{text}' to clipboard.");
    }

    private void ApplyWordColor(int wordIndex)
    {
        TMP_WordInfo wordInfo = textMeshPro.textInfo.wordInfo[wordIndex];

        // 텍스트를 수정하여 단어를 강조
        string word = wordInfo.GetWord();
        modifiedText = originalText.Substring(0, wordInfo.firstCharacterIndex)
                      + $"<b><color=#{ColorUtility.ToHtmlStringRGBA(hoverColor)}>{word}</color></b>"
                      + originalText.Substring(wordInfo.firstCharacterIndex + word.Length);
        textMeshPro.text = modifiedText;
    }

    private void RestoreWordColor()
    {
        // 원래 텍스트로 복원
        textMeshPro.text = originalText;
    }
    public void UpdateOriginalText()
    {
        originalText = textMeshPro.text;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 필요 시 추가 동작을 구현할 수 있습니다.
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 마우스가 텍스트를 벗어날 때 색상을 복원
        RestoreWordColor();
        lastWordIndex = -1;
    }
}
