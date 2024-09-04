using UnityEngine;
using TMPro;

public class WritableText : MonoBehaviour
{
    private TextMeshProUGUI textField; // TextMeshPro InputField 참조

    void Start()
    {
        if (textField == null)
        {
            textField = GetComponent<TextMeshProUGUI>();
            textField.text = MemoManager.Instance.MemoText;
        }
    }
    void Update()
    {
        
    }
    // 붙여넣기 버튼 또는 특정 조건에서 호출될 메서드
    public void PasteFromClipboard()
    {
        if (textField != null)
        {
            MemoManager.Instance.MemoText = MemoManager.Instance.MemoText + " " + MemoManager.Instance.PasteText();
            textField.text = MemoManager.Instance.MemoText;
            GetComponent<ClickableText>().UpdateOriginalText();
        }
    }
    public void ClearText()
    {
        if (textField != null)
        {
            // InputField의 텍스트를 지웁니다.
            textField.text = "";
            Debug.Log("Cleared text.");
            MemoManager.Instance.ClearMemoText();
            textField.text = MemoManager.Instance.MemoText;
            GetComponent<ClickableText>().UpdateOriginalText();
        }
    }
}