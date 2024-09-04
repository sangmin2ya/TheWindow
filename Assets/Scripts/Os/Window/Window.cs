using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Window : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IWindow
{
    // IWindow 인터페이스의 속성 구현
    public WindowType windowType
    {
        get { return _windowType; }
        set { _windowType = value; }
    }
    public WindowState windowState
    {
        get { return _windowState; }
        set { _windowState = value; }
    }
    public IIcon icon { get; set; } // 창과 연결된 아이콘
    private Vector2 originalSize;
    private Vector2 originalPosition;
    private Vector2 preMaximizedSize;
    private Vector2 preMaximizedPosition;
    private bool isMinimized = false;
    private bool isMaximized = false;
    public float animationDuration = 0.3f;  // 애니메이션 지속 시간

    // Inspector에 노출될 백업 필드
    [SerializeField] private WindowType _windowType;  // 창 타입
    [SerializeField] private WindowState _windowState;  // 창 상태
    public Button _icon; // 최소화 아이콘
    public Button _maximizeIcon; // 최대화 아이콘

    private Vector2 lastMousePosition;
    private GameObject _taskbarIcon;
    private bool isDragging = false;
    private float draggableHeight = 450f; // 드래그 가능한 상단 영역의 높이

    void Start()
    {
        windowType = _windowType;

        windowState = WindowState.Open;
        if (windowType != WindowType.Chat)
        {
            icon = _icon.GetComponent<IIcon>();
            _taskbarIcon = Instantiate(_icon as MonoBehaviour, GameObject.Find("TaskCanvas").transform).gameObject;
            _taskbarIcon.GetComponent<TaskBarIcon>().window = gameObject;
            Vector2 offsetMin;
            Vector2 offsetMax;
            WindowManager.Instance.GetStartOffset(out offsetMin, out offsetMax);
            GetComponent<RectTransform>().offsetMin = offsetMin;
            GetComponent<RectTransform>().offsetMax = offsetMax;
        }
    }
    // 창 이동 시작 시 호출
    public void OnPointerDown(PointerEventData eventData)
    {
        Focus();
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, Camera.main, out localMousePosition);

        // 마우스 클릭 위치가 상단 특정 픽셀 이내인지 확인
        if (localMousePosition.y <= rectTransform.rect.height && localMousePosition.y >= rectTransform.rect.height - draggableHeight)
        {
            if (!isMaximized && !isMinimized)
            {
                isDragging = true;
                lastMousePosition = eventData.position;
                originalPosition = rectTransform.anchoredPosition;
            }
        }
    }
    // 창 이동 중 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 currentMousePosition = eventData.position;
            Vector2 diff = currentMousePosition - lastMousePosition;
            GetComponent<RectTransform>().anchoredPosition += diff;
            lastMousePosition = currentMousePosition;
        }
    }

    // 창 이동 종료 시 호출
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    // 창 최소화 기능
    public void Minimize()
    {
        if (!isMinimized)
        {
            // 최대화된 상태가 아닌 경우만 크기와 위치를 저장
            if (!isMaximized)
            {
                preMaximizedSize = GetComponent<RectTransform>().sizeDelta;
                preMaximizedPosition = GetComponent<RectTransform>().anchoredPosition;
            }

            // 애니메이션으로 창을 아래로 슬라이드하고 스케일 줄임
            StartCoroutine(SlideWindowOffscreenAndShrink(() =>
            {
                // 창을 비활성화하고 작업 표시줄 아이콘만 남김
                gameObject.SetActive(false);
                isMinimized = true;
            }));
        }
    }


    // 창 복원 기능
    // 창 복원 기능
    public void Restore()
    {
        if (isMinimized)
        {
            // 창을 활성화하고 애니메이션으로 다시 위로 슬라이드 및 스케일 확대
            gameObject.SetActive(true);
            StartCoroutine(SlideWindowOnscreenAndGrow(() =>
            {
                if (isMaximized)
                {
                    // 최대화된 상태로 복원
                    RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
                    GetComponent<RectTransform>().offsetMin = Vector2.zero;  // 화면의 좌상단(0,0)
                    GetComponent<RectTransform>().offsetMax = Vector2.zero;  // 화면의 우하단(0,0)
                    GetComponent<RectTransform>().anchoredPosition = Vector2.zero;  // 중앙 위치로 설정
                }
                else
                {
                    // 기본 크기와 위치로 복원
                    GetComponent<RectTransform>().sizeDelta = preMaximizedSize;
                    GetComponent<RectTransform>().anchoredPosition = preMaximizedPosition;
                }

                isMinimized = false;
            }));
        }
    }

    // 창을 아래로 슬라이드하고 작아지게 하는 애니메이션
    private IEnumerator SlideWindowOffscreenAndShrink(System.Action onComplete)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 targetPosition = new Vector2(startPosition.x, -rectTransform.rect.height);  // 화면 아래로 슬라이드
        Vector3 startScale = rectTransform.localScale;
        Vector3 targetScale = new Vector3(0.5f, 0.5f, 1f);  // 최소화될 때의 스케일
        float time = 0f;

        while (time < animationDuration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time / animationDuration);
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, time / animationDuration); // 스케일 줄임
            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        rectTransform.localScale = targetScale;  // 스케일을 최종적으로 설정

        // 애니메이션 완료 후 실행할 콜백 함수 호출
        onComplete?.Invoke();
    }

    // 창을 위로 슬라이드하고 커지게 하는 애니메이션
    private IEnumerator SlideWindowOnscreenAndGrow(System.Action onComplete)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 startPosition = new Vector2(rectTransform.anchoredPosition.x, -rectTransform.rect.height);  // 시작 위치는 화면 아래
        Vector2 targetPosition = preMaximizedPosition;  // 원래 창의 위치
        Vector3 startScale = new Vector3(0.5f, 0.5f, 1f);  // 최소화된 상태에서 시작
        Vector3 targetScale = Vector3.one;  // 원래 크기로 복원
        float time = 0f;

        while (time < animationDuration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time / animationDuration);
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, time / animationDuration);  // 스케일 확대
            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        rectTransform.localScale = targetScale;  // 스케일을 최종적으로 설정

        // 애니메이션 완료 후 실행할 콜백 함수 호출
        onComplete?.Invoke();
    }


    public void Focus()
    {
        // 창을 최상위로 올려 포커스
        if (isMinimized)
        {
            Restore();
        }
        transform.SetAsLastSibling();
    }
    // 창 최대화 기능
    // 창 최대화 기능
    public void ToggleMaximize()
    {
        if (!isMaximized)
        {
            // 현재 크기와 위치 저장 (최대화 전의 상태)
            preMaximizedSize = GetComponent<RectTransform>().sizeDelta;
            preMaximizedPosition = GetComponent<RectTransform>().anchoredPosition;

            // 캔버스의 크기를 가져와 창을 최대화
            RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            GetComponent<RectTransform>().offsetMin = Vector2.zero;  // 좌상단(0,0)
            GetComponent<RectTransform>().offsetMax = Vector2.zero;  // 우하단(0,0)
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            isMaximized = true;
        }
        else
        {
            // 창이 최대화된 상태라면 기본 크기와 위치로 복원
            GetComponent<RectTransform>().sizeDelta = preMaximizedSize;
            GetComponent<RectTransform>().anchoredPosition = preMaximizedPosition;
            isMaximized = false;
        }
    }

    // 창 닫기 및 열기 기능
    public void Close()
    {
        _taskbarIcon?.GetComponent<IIcon>()?.Close();
        if (windowType == WindowType.Messanger)
        {
            GameObject go = GameObject.FindWithTag("Chat");
            if (go != null)
            {
                WindowManager.Instance.CloseWindow(go.GetComponent<IWindow>());
            }
        }
        Destroy(gameObject);
    }
    public void OnCloseButtonClick()
    {
        WindowManager.Instance.CloseWindow(this);
    }
    public void Open()
    {
        gameObject.SetActive(true);
        Focus();
    }
    // 작업 표시줄 아이콘 클릭 시 호출
    public void OnTaskbarIconClick()
    {
        if (!isMinimized && gameObject.activeSelf)
        {
            // 창이 활성화된 상태라면 최소화
            Minimize();
        }
        else
        {
            Restore();
        }
    }

    // 창을 초기화된 상태로 리셋
    public void ResetWindow()
    {
        // 창의 크기와 위치를 초기화된 상태로 리셋
        GetComponent<RectTransform>().sizeDelta = new Vector2(945, 774); // 기본 크기
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); // 기본 위치 (중앙으로 설정 가능)
        isMinimized = false;
        isMaximized = false;
    }
}
