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
            }
            return _instance;
        }
    }
    private List<IWindow> _windows = new List<IWindow>();
    public Vector2 startOffsetMin = new Vector2(500, 250);
    public Vector2 startOffsetMax = new Vector2(-500, -250);
    public int WindowCount { get { return _windows.Count; } }
    public void OpenWindow(IWindow window)
    {
        foreach (IWindow w in _windows)
        {
            if (w == window)
            {
                FocusWindow(w);
                //하단 아이콘 포커스 효과
                Debug.Log("Window already opened");
                return;

            if (window.windowType == WindowType.Email || window.windowType == WindowType.Setting || window.windowType == WindowType.Trashcan || window.windowType == WindowType.Messanger || window.windowType == WindowType.Game || window.windowType == WindowType.Alert)
            {
                if (w.windowType == window.windowType)
                {
                    FocusWindow(w);
                    //하단 아이콘 포커스 효과
                    Debug.Log("Window already opened");
                    return;
                }
            }
            if (window.windowType == WindowType.Error)
            {
                if (w.windowType == WindowType.Error)
                {
                    CloseWindow(w);
                }
            }
        }
        if (window.windowType == WindowType.Chat)
        {
            GameObject go = GameObject.FindWithTag("Chat");
            if (go != null)
            {
                CloseWindow(go.GetComponent<IWindow>());
                Debug.Log("Chat Window already opened");
            }
            var chat = Instantiate(window as MonoBehaviour, GameObject.Find("Canvas/Desktop/Msger_Window(Clone)").transform);
            _windows.Insert(0, chat.GetComponent<IWindow>()); //add to the front of the list
            return;
        }

        // 태그가 "Lock"인 경우 비밀번호 창을 띄우기
        if (window.GetGameObject().CompareTag("Lock"))
        {
            Debug.Log("Lock");  // 로그 추가로 확인

            FolderSystem folderSystem = FindObjectOfType<FolderSystem>();
            if (folderSystem != null)
            {
                Debug.Log("비밀번호 창을 띄웁니다.");  // 로그 추가로 확인
                folderSystem.ShowPasswordPrompt("흐림", () =>
                {
                    var newWindow = Instantiate(window as MonoBehaviour, GameObject.Find("Canvas/Desktop").transform);
                    _windows.Insert(0, newWindow.GetComponent<IWindow>());
                });
            }
            return;
        }
        var newWindow = Instantiate(window as MonoBehaviour, GameObject.Find("Canvas/Desktop").transform);
        _windows.Insert(0, newWindow.GetComponent<IWindow>()); //add to the front of the list
    }
    public void Minimize(IWindow window)
    {
        if (_windows.Contains(window))
        {
            window.Minimize();
        }
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
    public void GetStartOffset(out Vector2 offsetMin, out Vector2 offsetMax)
    {
        Transform desktop = GameObject.Find("Desktop").transform;
        if (GetLastChildPosition(desktop) == null)
        {
            offsetMin = startOffsetMin;
            offsetMax = startOffsetMax;
            return;
        }
        else
        {
            Vector2 offmin = GetLastChildPosition(desktop).GetComponent<RectTransform>().offsetMin + new Vector2(50, -50);
            Vector2 offmax = GetLastChildPosition(desktop).GetComponent<RectTransform>().offsetMax + new Vector2(50, -50);
            if (offmin.x < 0 || offmin.y < 0 || offmax.x > 0 || offmax.y > 0)
            {
                offsetMin = startOffsetMin;
                offsetMax = startOffsetMax;
                return;
            }
            offsetMin = GetLastChildPosition(desktop).GetComponent<RectTransform>().offsetMin + new Vector2(50, -50);
            offsetMax = GetLastChildPosition(desktop).GetComponent<RectTransform>().offsetMax + new Vector2(50, -50);
            Debug.Log("마지막 창의 offsetMin: " + offsetMin + ", offsetMax: " + offsetMax);
        }
    }
    private GameObject GetLastChildPosition(Transform parent)
    {
        // 자식이 없는 경우 부모 오브젝트의 포지션 반환
        if (parent.childCount == 0)
        {
            Debug.LogWarning("자식이 없습니다.");
            return null;
        }

        // Hierarchy에서 마지막 자식의 포지션 가져오기
        Transform lastChild = parent.GetChild(parent.childCount - 2);
        if (lastChild.GetComponent<Window>() == null && lastChild.GetComponent<SettingWindow>() == null)
        {
            Debug.LogWarning("마지막 자식이 Window나 SettingWindow가 아닙니다.");
            return null;
        }
        if (lastChild.GetComponent<Window>()?.windowType == WindowType.Chat || lastChild.GetComponent<Window>()?.windowType == WindowType.Messanger)
        {
            Debug.LogWarning("마지막 자식이 Chat입니다.");
            return null;
        }
        return lastChild.gameObject;
    }
}
