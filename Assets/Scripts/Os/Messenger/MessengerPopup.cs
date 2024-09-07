using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessengerPopup : MonoBehaviour
{
    public IWindow messengerPopup;  // 메신저 팝업 창
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenMessengerPopup()
    {
        WindowManager.Instance.OpenWindow(messengerPopup);
    }
}