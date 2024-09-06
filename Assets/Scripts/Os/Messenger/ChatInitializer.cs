using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatInitializer : MonoBehaviour
{
    public List<ChatSet> chatList = new List<ChatSet>(); //name, message
    public GameObject myChatPrefab;
    public GameObject otherChatPrefab;
    public GameObject chatBox;
    private int chatIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var chat in chatList)
        {
            GameObject go;

            if (chat.name == "나")
            {
                go = Instantiate(otherChatPrefab, chatBox.transform);
            }
            else
            {
                go = Instantiate(myChatPrefab, chatBox.transform);
            }

            // 생성된 오브젝트에서 텍스트 컴포넌트를 찾아 텍스트 설정
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y - chatIndex * 110, 0);
            go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = chat.name;
            go.transform.Find("msg").GetComponent<TextMeshProUGUI>().text = chat.message;

            chatIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
