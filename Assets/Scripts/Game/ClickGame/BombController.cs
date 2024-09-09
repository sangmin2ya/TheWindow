using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BombController : MonoBehaviour, IPointerDownHandler
{
    public float lifetime = 1f; // 폭탄이 생성된 후 유지되는 시간
    public GameObject explosionPrefab; // 폭발 프리팹
    private Button button;

    // Event that is called when the button is pressed
    public delegate void ButtonPressedEvent();
    public event ButtonPressedEvent OnButtonPressed;

    // Event that is called when the button is released
    public event ButtonPressedEvent OnButtonReleased;

    void Start()
    {
        button = GetComponent<Button>();
        // 일정 시간이 지나면 폭탄 제거
        Invoke("Missed", lifetime);
        OnButtonPressed += OnMouseDown;
    }

    // This method is called when the mouse button is pressed down on the button
    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnButtonPressed != null)
        {
            OnButtonPressed(); // Trigger the Pressed event
        }

        Debug.Log("Button Pressed");
    }

    public void OnMouseDown()
    {
        // 폭탄 클릭 시 제거
        BombGameManager.instance.BombClicked(); // GameManager에서 점수 업데이트
        Destroy(gameObject);
    }

    private void Missed()
    {
        // 폭탄을 클릭하지 못했을 경우
        if (gameObject != null)
        {
            BombGameManager.instance.BombMissed(); // GameManager에서 목숨 감소
            Instantiate(explosionPrefab, transform.parent).transform.localPosition = transform.localPosition;
            Destroy(gameObject);
        }
    }
}
