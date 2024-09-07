using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class WritableText : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI textField; // TextMeshPro InputField 참조

    void Start()
    {
        if (textField == null)
        {
            textField = GetComponent<TextMeshProUGUI>();
            if (gameObject.tag == "Memo")
                textField.text = MemoManager.Instance.MemoText;
        }
    }
    void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // 마우스 오른쪽 클릭인지 확인
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Raycast를 사용하여 올바른 UI 객체 감지
            if (IsPointerOverUIObject(eventData))
            {
                if (gameObject.tag == "Memo")
                {
                    PasteFromClipboard();
                }
                else
                {
                    textField.text = MemoManager.Instance.PasteText();
                }
            }
        }
    }

    private bool IsPointerOverUIObject(PointerEventData eventData)
    {
        // Raycast 결과를 저장할 리스트 생성
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Raycast 결과 중 현재 오브젝트가 포함되어 있는지 확인
        foreach (var result in results)
        {
            if (result.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
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