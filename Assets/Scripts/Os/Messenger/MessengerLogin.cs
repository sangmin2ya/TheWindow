using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessengerLogin : MonoBehaviour
{
    public string password;
    private GameObject loginPanel;
    public TextMeshProUGUI passwordText;
    private GameObject loginError;
    private GameObject rockLogin;
    private AudioSource audioSource;  // 부모 오브젝트의 오디오 소스 참조

    private int errorCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        loginPanel = transform.Find("Title/LoginPanel").gameObject;
        loginError = loginPanel.transform.Find("InputBG/Wrong").gameObject;
        rockLogin = transform.Find("Title/LoginError").gameObject;
        loginPanel.SetActive(true);
        loginError.GetComponent<TextMeshProUGUI>().text = "";
        rockLogin.GetComponent<TextMeshProUGUI>().text = "";
        gameObject.SetActive(!SettingManager.Instance.UnrockLogin);
        audioSource = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Login()
    {
        SettingManager.Instance.UnrockLogin = true;
        StopAllCoroutines();
        // 로그인 성공 시 부모 오브젝트의 오디오 소스에서 한 번만 재생
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioSource.clip);  // 한 번만 재생 (오디오 소스를 유지)
        }
        gameObject.SetActive(false);
    }
    public void InputPassword()
    {
        if (passwordText.text == "")
        {
            StartCoroutine(InputPasswordError());
            return;
        }
        if (passwordText.text == password)
        {
            Login();
        }
        else
        {
            passwordText.text = "";
            if (errorCount >= 4)
            {
                StartCoroutine(RockLogin());
                errorCount = 0;
                return;
            }
            StartCoroutine(LoginError());
        }
    }
    IEnumerator LoginError()
    {
        errorCount++;
        loginError.GetComponent<TextMeshProUGUI>().text = "비밀번호 오류\n남은 횟수 " + (5 - errorCount) + " 회";
        yield return new WaitForSeconds(4.0f);
        loginError.GetComponent<TextMeshProUGUI>().text = "";
    }
    IEnumerator InputPasswordError()
    {
        loginError.GetComponent<TextMeshProUGUI>().text = "비밀번호를 입력해주세요";
        yield return new WaitForSeconds(4.0f);
        loginError.GetComponent<TextMeshProUGUI>().text = "";
    }
    IEnumerator RockLogin()
    {
        loginPanel.SetActive(false);
        float time = 0.0f;
        while (time < 10f)
        {
            rockLogin.GetComponent<TextMeshProUGUI>().text = "로그인 시도 횟수 초과\n" + (10 - (int)time) + "초 후 재시도";

            time += Time.deltaTime;
            yield return null;
        }
        rockLogin.GetComponent<TextMeshProUGUI>().text = "";
        loginPanel.SetActive(true);
    }
}
