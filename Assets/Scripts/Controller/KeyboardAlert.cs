using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardAlert : MonoBehaviour
{
    public Window AlertWindow;
    // Start is called before the first frame update
    void Start()
    {
        if (MemoManager.Instance.userUnlocked == false)
        {
            WindowManager.Instance.OpenWindow(AlertWindow);
            MemoManager.Instance.userUnlocked = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
