using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MemoManager : MonoBehaviour
{
    private static MemoManager _instance;
    public static MemoManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject memoManagerObject = new GameObject("MemoManager");
                _instance = memoManagerObject.AddComponent<MemoManager>();
                DontDestroyOnLoad(memoManagerObject);
            }
            return _instance;
        }
    }
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        scoreList.Add(new Tuple<int, string>(20, "KJH"));
        scoreList.Add(new Tuple<int, string>(18, "KJH"));
        scoreList.Add(new Tuple<int, string>(25, "CJW"));
        scoreList.Add(new Tuple<int, string>(15, "CJW"));
        scoreList.Add(new Tuple<int, string>(21, "ME"));
        scoreList.Sort();
        scoreList.Reverse();
    }
    private string copyText = "";
    private string memoText = "";
    public string MemoText { get { return memoText; } set { memoText = value; } }
    public bool userUnlocked = false;
    public List<Tuple<int, string>> scoreList = new List<Tuple<int, string>>();
    public void ClearMemoText()
    {
        memoText = "";
    }
    public void CopyText(string text)
    {
        copyText = text;
    }
    public string PasteText()
    {
        return copyText;
    }
    public void RestartMemo(GameObject go)
    {
        go.SetActive(false);
        Debug.Log("Memo restarted");
        go.SetActive(true);
    }
    public void AddScore(int score)
    {
        scoreList.Add(new Tuple<int, string>(score, "ME"));
        scoreList.Sort();
        scoreList.Reverse();
    }
}
