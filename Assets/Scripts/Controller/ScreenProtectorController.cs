using UnityEngine;
using System.Collections;
using TMPro;

public class ScreenProtectorController : MonoBehaviour
{
    public GameObject bouncingObject; // TextMeshPro 오브젝트
    public Canvas canvas; // 캔버스 (Render Mode: Camera 모드)
    public float bounceSpeed = 5f; // 오브젝트의 이동 속도
    public float idleTimeThreshold = 10f; // 입력이 없는 시간 기준 (10초)

    private float idleTimer = 0f; // 입력이 없는 시간 측정
    private Vector2 direction; // 오브젝트의 이동 방향
    private Coroutine bounceCoroutine; // 코루틴 저장
    private RectTransform bouncingRectTransform; // 움직이는 TextMeshPro 오브젝트의 RectTransform
    private RectTransform canvasRectTransform; // 캔버스의 RectTransform

    void Start()
    {
        canvas.gameObject.SetActive(false); // 화면 보호기 초기에는 비활성화
        direction = GetRandomDirection(); // 시작 방향 랜덤 설정
        bouncingRectTransform = bouncingObject.GetComponent<RectTransform>(); // 오브젝트의 RectTransform
        canvasRectTransform = canvas.GetComponent<RectTransform>(); // 캔버스의 RectTransform
    }

    void Update()
    {
        // 입력 감지
        if (Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            idleTimer = 0f; // 입력이 있으면 타이머 리셋
            if (canvas.gameObject.activeSelf)
            {
                DisableScreenSaver(); // 화면 보호기가 활성화된 경우 비활성화
            }
        }
        else
        {
            idleTimer += Time.deltaTime; // 입력이 없으면 타이머 증가
        }

        // 일정 시간 동안 입력이 없으면 화면 보호기 활성화
        if (idleTimer > idleTimeThreshold && !canvas.gameObject.activeSelf)
        {
            EnableScreenSaver();
        }
    }

    // 화면 보호기 활성화
    private void EnableScreenSaver()
    {
        canvas.gameObject.SetActive(true); // 화면 보호기 오브젝트 활성화
        bouncingObject.SetActive(true); // 오브젝트 활성화
        bounceCoroutine = StartCoroutine(BounceObject()); // 코루틴 시작
    }

    // 화면 보호기 비활성화
    private void DisableScreenSaver()
    {
        canvas.gameObject.SetActive(false); // 화면 보호기 오브젝트 비활성화
        if (bounceCoroutine != null)
        {
            StopCoroutine(bounceCoroutine); // 코루틴 정지
        }
        bouncingRectTransform.anchoredPosition = Vector2.zero; // 오브젝트 위치 초기화
        bouncingObject.SetActive(false); // 오브젝트 활성화
    }

    // 오브젝트가 화면 경계를 넘으면 튕겨나가는 로직
    private IEnumerator BounceObject()
    {
        while (true)
        {
            // 현재 위치를 가져와서 이동 방향에 따라 이동
            Vector2 position = bouncingRectTransform.anchoredPosition;
            position += direction * bounceSpeed * Time.deltaTime;

            // 캔버스의 크기와 오브젝트 크기를 고려하여 경계를 설정
            float minX = 0;
            float maxX = canvasRectTransform.rect.width - bouncingRectTransform.rect.width;
            float minY = 0;
            float maxY = canvasRectTransform.rect.height - bouncingRectTransform.rect.height;

            // 상/하 경계에 닿으면 y 방향 반사
            if (position.y > maxY || position.y < minY)
            {
                direction.y = -direction.y; // y축 반사
                position.y = Mathf.Clamp(position.y, minY, maxY); // 위치 보정
                bouncingObject.GetComponent<TextMeshProUGUI>().color = new Color(Random.value, Random.value, Random.value); // 텍스트 색상 랜덤 변경
            }

            // 좌/우 경계에 닿으면 x 방향 반사
            if (position.x > maxX || position.x < minX)
            {
                direction.x = -direction.x; // x축 반사
                position.x = Mathf.Clamp(position.x, minX, maxX); // 위치 보정
                bouncingObject.GetComponent<TextMeshProUGUI>().color = new Color(Random.value, Random.value, Random.value); // 텍스트 색상 랜덤 변경
            }

            // 오브젝트의 위치를 업데이트
            bouncingRectTransform.anchoredPosition = position;
            yield return null; // 다음 프레임까지 대기
        }
    }

    // 랜덤한 방향을 반환
    private Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
