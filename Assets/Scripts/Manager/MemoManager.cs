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
        scoreList.Add(new Tuple<int, string>(8, "KJH"));
        scoreList.Add(new Tuple<int, string>(11, "KJH"));
        scoreList.Add(new Tuple<int, string>(15, "CJW"));
        scoreList.Add(new Tuple<int, string>(10, "CJW"));
        scoreList.Add(new Tuple<int, string>(13, "ME"));
        scoreList.Sort();
        scoreList.Reverse();
    }
    private string copyText = "";
    private string memoText = "";
    public string MemoText { get { return memoText; } set { memoText = value; } }
    public bool userUnlocked = false;
    public bool alertOnce = false;
    
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
