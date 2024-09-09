using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailInitializer : MonoBehaviour
{
    public GameObject mailContent;
    public string title;
    public string date;
    public string sender;
    public string receiver;
    

    [TextArea(5, 10)]
    public string content;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Title").GetComponent<TMPro.TextMeshProUGUI>().text = title;
        transform.Find("Date").GetComponent<TMPro.TextMeshProUGUI>().text = date;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenEmail()
    {
        GameObject emailContent = GameObject.Find("Emails").transform.Find("EmailContent/Scroll View/Viewport/Content").gameObject;

        mailContent.SetActive(true);

        emailContent.transform.Find("Title").GetComponent<TMPro.TextMeshProUGUI>().text = "제목: " + title;
        emailContent.transform.Find("Date").GetComponent<TMPro.TextMeshProUGUI>().text = date;
        emailContent.transform.Find("Sender").GetComponent<TMPro.TextMeshProUGUI>().text = sender;
        emailContent.transform.Find("Receiver").GetComponent<TMPro.TextMeshProUGUI>().text = receiver;
        emailContent.transform.Find("Content").GetComponent<TMPro.TextMeshProUGUI>().text = content;

        emailContent.transform.Find("Title").GetComponent<ClickableText>().UpdateOriginalText();
        emailContent.transform.Find("Date").GetComponent<ClickableText>().UpdateOriginalText();
        emailContent.transform.Find("Sender").GetComponent<ClickableText>().UpdateOriginalText();
        emailContent.transform.Find("Receiver").GetComponent<ClickableText>().UpdateOriginalText();
        emailContent.transform.Find("Content").GetComponent<ClickableText>().UpdateOriginalText();
        emailContent.transform.Find("Content/404").gameObject.SetActive(title == "19금 동영상, 지금 바로 무료로 보세요!");
    }
}
