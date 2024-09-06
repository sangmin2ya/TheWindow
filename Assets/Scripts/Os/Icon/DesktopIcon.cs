using UnityEngine;

public class DesktopIcon : MonoBehaviour, IIcon
{
    public GameObject window;
    public IconVisibleType iconType;
    public IWindow Window { get { return window.GetComponent<IWindow>(); } }
    public IconVisibleType IconType { get { return (iconType == IconVisibleType.Normal || SettingManager.Instance.Visibility) ? IconVisibleType.Normal : IconVisibleType.Hidden; } }
    public void Start()
    {
        gameObject.SetActive(IconType == IconVisibleType.Normal);
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void OnIconClick()
    {
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