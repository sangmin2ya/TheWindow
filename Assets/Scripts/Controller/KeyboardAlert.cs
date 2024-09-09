using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardAlert : MonoBehaviour
{
    public Window AlertWindow;
    // Start is called before the first frame update
    void Start()
    {
        if (MemoManager.Instance.alertOnce == false)
        {
            WindowManager.Instance.OpenWindow(AlertWindow);
            MemoManager.Instance.alertOnce = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
