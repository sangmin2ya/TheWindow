using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlatformStaticHp : MonoBehaviour
{
    [SerializeField]
    private int hp;

    public PopUpScore scoreTextPfb;
    private PopUpScore scoreText;

    //[SerializeField]
    //private TextMeshProUGUI popText;

    /*
    [SerializeField]
    TextMeshProUGUI hpTextPfb;
    TextMeshProUGUI hpText;
    Canvas fixedCanvas;
    void Awake()
    {
        fixedCanvas = GameObject.Find("FixedCanvas").GetComponent<Canvas>();
        hpText = Instantiate(hpTextPfb, transform.position, Quaternion.identity, fixedCanvas.GetComponent<Canvas>().transform);
    }
    */

    void Start()
    {
        //hpText.SetText(hp.ToString());
    }


    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.collider.tag == "Player")
        {
            if (CanBreak())
            {
                if (!PlayerController.Instance.isDash)
                    PlayerController.Instance.Bounce(transform.position.y);
                GameManager.Instance.AddScore(CalculateScore());

                scoreText = Instantiate(scoreTextPfb, transform.position, Quaternion.identity);
                scoreText.SettingText(CalculateScore());


                gameObject.SetActive(false);
                //Destroy(hpText);
            }
            else
            {
                // 임시로 죽으면 게임 멈춤
                GameManager.Instance.FailGame();
                Debug.Log("YOU DIED");
            }

        }
    }

    public int CalculateScore()
    {
        int basicPoint = 100;
        return basicPoint * (1 + 2 * hp);

    }

    /*
    void OnDisable()
    {
        if (hpText != null)
        {
            Destroy(hpText.gameObject);
        }
    }
    */

    private bool CanBreak()
    {
        if (PlayerController.Instance.ACCStep >= hp || PlayerController.Instance.isDash)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    //=================================================================
    /*
    public void ShowText(int hp, Vector2 position)
    {
        Debug.Log("showText 호출");
        popText.transform.position = position + new Vector2(0, popText.transform.localScale.y / 2);
        popText.SetText(hp.ToString());
        popText.gameObject.SetActive(true);

        StartCoroutine(textShowing(popText));
    }

    IEnumerator textShowing(TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(0.5f);
        float duration = 0f;
        while (duration < 0.5f) ;
        {
            duration += Time.deltaTime;
            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0), Time.unscaledDeltaTime);
            text.gameObject.transform.position += Vector3.up * Time.unscaledDeltaTime;

            yield return null;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        text.gameObject.SetActive(false);

    }
    */

    //==================================================================================================
    /*
    public void ShowDamage(int damage, Vector2 position, bool isCritical = false)
    {
        TextMeshProUGUI go = Damages.Dequeue();
        go.transform.position = position + new Vector2(0, DamagePfb.transform.localScale.y / 2);
        go.SetText(damage.ToString());
        go.gameObject.SetActive(true);

        if (!isCritical)
        {
            go.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            go.transform.GetChild(0).gameObject.SetActive(true);
        }

        StartCoroutine(damageShowing(go));
    }

    IEnumerator damageShowing(TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(0.5f);

        float duration = 0f;
        while (duration < 0.5f)
        {
            duration += Time.deltaTime;

            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0), Time.unscaledDeltaTime);
            text.gameObject.transform.position += Vector3.up * Time.unscaledDeltaTime;

            yield return null;
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        Damages.Enqueue(text);
        text.gameObject.SetActive(false);
    }
    */

}
