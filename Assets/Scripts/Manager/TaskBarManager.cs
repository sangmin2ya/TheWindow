using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TaskBarManager : MonoBehaviour
{
    private static TaskBarManager _instance;
    public static TaskBarManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject TaskBarManagerObject = new GameObject("TaskBarManager");
                _instance = TaskBarManagerObject.AddComponent<TaskBarManager>();
                DontDestroyOnLoad(TaskBarManagerObject);
            }
            return _instance;
        }
    }
    private List<IIcon> _icons = new List<IIcon>();
    private List<GameObject> iconInstances = new List<GameObject>();
    private Vector2 startPosition = new Vector2(200, 60);  // 아이콘 시작 위치
    private float iconSpacing = 150;  // 아이콘 간격
    public void AddIcon(IIcon icon)
    {
        _icons.Add(icon);
        PrintIcons();
    }
    public void RemoveIcon(IIcon icon)
    {
        if (_icons.Contains(icon))
        {
            _icons.Remove(icon);
        }
        PrintIcons();
    }

    public void PrintIcons()
    {
        if (_icons == null || _icons.Count == 0) return;

        // 모든 아이콘을 그리드 레이아웃에 맞게 부모 객체에 추가
        foreach (IIcon icon in _icons)
        {
            GameObject iconObject = icon.GetGameObject();  // 아이콘의 GameObject를 가져옴
            iconObject.transform.SetParent(GameObject.Find("TaskCanvas/TaskBar").transform, false);  // 아이콘을 부모 객체의 자식으로 설정
        }
    }
    public void ClearAllIcons()
    {
        foreach (IIcon icon in _icons)
        {
            icon.Close();
        }
        _icons.Clear();
        PrintIcons();

    }
}