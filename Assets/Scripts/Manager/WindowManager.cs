using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class WindowManager : MonoBehaviour
{
    private static WindowManager _instance;
    public static WindowManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject windowManagerObject = new GameObject("WindowManager");
                _instance = windowManagerObject.AddComponent<WindowManager>();
                DontDestroyOnLoad(windowManagerObject);
            }
            return _instance;
        }
    }
    private List<IWindow> _windows = new List<IWindow>();
    public int WindowCount { get { return _windows.Count; } }
    public void OpenWindow(IWindow window)
    {
        foreach (IWindow w in _windows)
        {
            if (w.windowType == window.windowType)
            {
                FocusWindow(w);
                //하단 아이콘 포커스 효과
                Debug.Log("Window already opened");
                return;
            }
        }
        var newWindow = Instantiate(window as MonoBehaviour, GameObject.Find("Canvas").transform);
        _windows.Insert(0, newWindow.GetComponent<IWindow>()); //add to the front of the list
    }
    public void CloseWindow(IWindow window)
    {
        if (_windows.Contains(window))
        {
            _windows.Remove(window);
            window.Close();
        }
    }
    /// <summary>
    /// Focus the window in the list
    /// </summary>
    public void FocusWindow(IWindow window)
    {
        if (_windows.Contains(window))
        {
            _windows.Remove(window);
            _windows.Insert(0, window);
            window.Focus();
        }
    }
    public void CloseAllWindows()
    {
        for (int i = _windows.Count - 1; i >= 0; i--)
        {
            _windows[i].Close();
        }
        _windows.Clear();
    }
    public IWindow GetWindow(int index)
    {
        if (index >= 0 && index < _windows.Count)
        {
            return _windows[index];
        }
        return null;
    }
}
