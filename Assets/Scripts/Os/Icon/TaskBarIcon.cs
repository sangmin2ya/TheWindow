using UnityEngine;

public class TaskBarIcon : MonoBehaviour, IIcon
{
    public GameObject window;
    public IconVisibleType iconType;
    public IWindow Window { get { return window.GetComponent<IWindow>(); } }
    public IconVisibleType IconType { get { return iconType; } }
    public void Start()
    {
        TaskBarManager.Instance.AddIcon(this);
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void OnIconClick()
    {
        if(Window.windowState == WindowState.Minimize)
        {
            WindowManager.Instance.OpenWindow(window.GetComponent<IWindow>());
        }
        else
        {
            WindowManager.Instance.Minimize(window.GetComponent<IWindow>());
        }
        Debug.Log(window.GetComponent<IWindow>().windowType + " Clicked");
    }
    public void Close()
    {
        TaskBarManager.Instance.RemoveIcon(this);
        Destroy(gameObject);
    }
    public void DrawIcon(Vector2 position)
    {
        transform.position = position;
    }
}