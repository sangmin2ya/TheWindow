using System.Collections;
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
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
    private string copyText = "";
    private string memoText = "";
    public string MemoText { get { return memoText; } set { memoText = value; } }
    public bool userUnlocked = false;
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
}
