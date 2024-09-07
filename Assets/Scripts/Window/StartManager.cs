using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    private GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        menu = transform.Find("StartMenu").gameObject;
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && menu.activeSelf == true)
        {
            Vector2 mousePosition = Input.mousePosition;

            // RectTransform의 화면 좌표 내에 마우스 위치가 있는지 확인
            if (!RectTransformUtility.RectangleContainsScreenPoint(transform.Find("StartMenu").GetComponent<RectTransform>(), mousePosition, Camera.main))
            {
                // 오브젝트가 비활성화되거나 다른 동작을 수행
                menu.SetActive(false);
                Debug.Log("Clicked outside the UI object. Deactivating.");
            }
        }
    }
    public void MenuPopUp()
    {
        menu.SetActive(true);
    }
    public void TurnOff()
    {
        Application.Quit();
    }
    public void LogOff()
    {
        Debug.Log("Log Off");
        SceneManager.LoadScene("Login");
    }
}