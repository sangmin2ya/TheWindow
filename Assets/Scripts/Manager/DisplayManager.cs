using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public static DisplayManager Instance; // 싱글턴 인스턴스
    public float targetAspect = 4.0f / 3.0f; // 4:3 비율 고정
    private Camera cam; // 카메라 참조

    void Awake()
    {
        // 싱글턴 패턴 구현: 인스턴스가 없으면 할당, 이미 있으면 파괴
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject); // 중복되는 인스턴스는 파괴
        }
    }

    void Start()
    {
        // 초기 카메라 세팅
        UpdateCameraViewport();
    }

    void OnLevelWasLoaded(int level)
    {
        // 씬이 로드될 때마다 카메라 세팅을 업데이트
        UpdateCameraViewport();
    }

    void Update()
    {
        // 해상도나 창 크기가 바뀌면 다시 뷰포트를 업데이트
        if (Mathf.Abs((float)Screen.width / (float)Screen.height - targetAspect) > 0.01f)
        {
            UpdateCameraViewport();
        }
    }

    // 카메라의 뷰포트를 4:3 비율로 고정시키는 메서드
    public void UpdateCameraViewport()
    {
        cam = Camera.main; // 현재 씬의 메인 카메라 가져오기
        if (cam == null) return;

        // 현재 화면의 비율을 계산
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // 4:3 목표 비율과 비교
        float scaleHeight = windowAspect / targetAspect;

        // 화면 비율이 4:3보다 크면 수직으로 레터박스를 추가
        if (scaleHeight < 1.0f)
        {
            Rect rect = cam.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            cam.rect = rect;
        }
        else // 화면 비율이 4:3보다 작으면 수평으로 레터박스를 추가
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = cam.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            cam.rect = rect;
        }

        // 카메라 배경색을 검은색으로 설정 (레터박스용)
        cam.backgroundColor = Color.black;
    }
}
