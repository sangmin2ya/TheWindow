using UnityEngine;
using TMPro;
public class DesktopIcon : MonoBehaviour, IIcon
{
    public GameObject window;
    public IconVisibleType iconType;
    public IWindow Window { get { return window.GetComponent<IWindow>(); } }
    public IconVisibleType IconType { get { return (iconType == IconVisibleType.Normal || SettingManager.Instance.Visibility) ? IconVisibleType.Normal : IconVisibleType.Hidden; } }
    private TextMeshProUGUI buttonText; // 버튼의 텍스트를 표시할 컴포넌트

    public void Start()
    {
        // 자식 오브젝트에서 TextMeshProUGUI 컴포넌트 자동으로 가져옴
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        gameObject.SetActive(IconType == IconVisibleType.Normal);
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void OnIconClick()
    {


        // 창의 TopBar에 버튼 텍스트를 미리 저장만 함
        if (window != null)
        {
            var windowScript = window.GetComponent<Window>();
            if (windowScript != null && buttonText != null)
            {
                windowScript.storedButtonText = buttonText.text; // 텍스트를 저장만 함
                Debug.Log("Stored button text: " + buttonText.text);
            }
            else
            {
                Debug.LogError("TopBar text or buttonText is null.");
            }
        }
        WindowManager.Instance.OpenWindow(window.GetComponent<IWindow>());
        Debug.Log(window.GetComponent<IWindow>().windowType + " Clicked");
    }


    public void Close()
    {
        TaskBarManager.Instance.RemoveIcon(this);
        Destroy(gameObject);
    }
    public void DrawIcon(Vector2 position)
    {
        //transform.position = position;
    }
}