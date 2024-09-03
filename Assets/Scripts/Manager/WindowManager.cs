using UnityEngine;
using System.Collections.Generic;

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
        window.Open();
        _windows.Insert(0, window); //add to the front of the list
        UpdateLayers();
    }

    public void CloseWindow(IWindow window)
    {
        if (_windows.Contains(window))
        {
            window.Close();
            _windows.Remove(window);
            UpdateLayers();
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
            UpdateLayers();
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
    private void UpdateLayers()
    {
        for (int i = 0; i < _windows.Count; i++)
        {
            _windows[i].SetLayer(i);
        }
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
