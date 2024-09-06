using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginMenu : MonoBehaviour
{
    private GameObject loginMenu;
    public string password;
    public TextMeshProUGUI inputPassword;
    public TextMeshProUGUI wrongPassword;
    // Start is called before the first frame update
    void Start()
    {
        loginMenu = GameObject.Find("UserLogin").gameObject;
        loginMenu.SetActive(false);
    }
    void OnEnable()
    {
        wrongPassword.text = "";
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void UserLogin1()
    {
        SceneManager.LoadScene("User2");
    }
    public void UserLogin2()
    {
        SceneManager.LoadScene("User1");
    }
    public void TryLogin2()
    {
        if(MemoManager.Instance.userUnlocked)
        {
            UserLogin2();
        }
        else
        {
            loginMenu.SetActive(true);
        }
    }
    public void CancleLogin()
    {
        loginMenu.SetActive(false);
    }
    public void InputPassword()
    {
        if (inputPassword.text == password)
        {
            MemoManager.Instance.userUnlocked = true;
            UserLogin2();
        }
        else
        {
            StartCoroutine(WrongPassword());
        }
    }
    IEnumerator WrongPassword()
    {
        wrongPassword.text = "로그인 실패!";
        yield return new WaitForSeconds(2.0f);
        wrongPassword.text = "";
    }
}
