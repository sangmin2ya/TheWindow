using UnityEngine;

public class DesktopIcon : MonoBehaviour, IIcon
{
    public GameObject window;
    public IWindow Window { get { return window.GetComponent<IWindow>(); } set { } }
    public void Start()
    {
        if (window != null)
            Window = window.GetComponent<IWindow>();
        TaskBarManager.Instance.AddIcon(this);
    }
    public void OnIconClick()
    {
        WindowManager.Instance.OpenWindow(window.GetComponent<IWindow>());
        Debug.Log(window.GetComponent<Window>().windowType + " Clicked");
    }
    public void Close()
    {
        TaskBarManager.Instance.RemoveIcon(this);
        Destroy(gameObject);
    }
}