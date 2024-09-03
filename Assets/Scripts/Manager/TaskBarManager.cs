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

    public void AddIcon(IIcon icon)
    {
        _icons.Add(icon);
    }
    public void RemoveIcon(IIcon icon)
    {
        if (_icons.Contains(icon))
        {
            _icons.Remove(icon);
        }
    }
    public void PrintIcons()
    {
        foreach (IIcon icon in _icons)
        {
            Debug.Log(icon);
        }
    }
    public void ClearAllIcons()
    {
        foreach (IIcon icon in _icons)
        {
            icon.Close();
        }
        _icons.Clear();
    }
}