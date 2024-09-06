using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmailLogin : MonoBehaviour
{
    public string password;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("WrongPassword").gameObject.SetActive(false);
        if (SettingManager.Instance.UnrockEmail)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {

    }
    public void TryLogin()
    {
        if (transform.Find("Password").GetChild(0).GetComponent<TextMeshProUGUI>().text == password)
        {
            SettingManager.Instance.UnrockEmail = true;
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(WrongPassword());
        }

    }
    IEnumerator WrongPassword()
    {
        transform.Find("WrongPassword").gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        transform.Find("WrongPassword").gameObject.SetActive(false);
    }
}
