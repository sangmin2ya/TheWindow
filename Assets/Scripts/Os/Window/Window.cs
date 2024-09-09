using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

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
    private Shadow windowShadow;  // 그림자 컴포넌트

    public IIcon icon { get; set; } // 창과 연결된 아이콘
    private Vector2 originalSize;
    private Vector2 originalPosition;
    private Vector2 preMaximizedSize;
    private Vector2 preMaximizedPosition;
    public bool isMinimized = false;
    private bool isMaximized = false;
    private float animationDuration = 0.3f;  // 애니메이션 지속 시간

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

        icon = _icon.GetComponent<IIcon>();
        _taskbarIcon = Instantiate(_icon as MonoBehaviour, GameObject.Find("TaskCanvas").transform).gameObject;
        _taskbarIcon.GetComponent<TaskBarIcon>().window = gameObject;
        if (windowType != WindowType.Chat && windowType != WindowType.Messanger && windowType != WindowType.Alert)
        {
            WindowManager.Instance.OpenWindow(this);

            Vector2 offsetMin;
            Vector2 offsetMax;
            WindowManager.Instance.GetStartOffset(out offsetMin, out offsetMax);
            GetComponent<RectTransform>().offsetMin = offsetMin;
            GetComponent<RectTransform>().offsetMax = offsetMax;
            // 그림자 추가
            AddShadowEffect();
        }
        AddMultipleShadows();
    }

    // 그림자 효과 추가 함수
    private void AddShadowEffect()
    {
        Color newColor;
        if (ColorUtility.TryParseHtmlString("#E0E0E0", out newColor))
        {
            gameObject.GetComponent<Image>().color = newColor;
            if (windowType == WindowType.Folder || windowType == WindowType.NormalFolder)
            {
                transform.Find("Back Btn").GetComponent<Image>().color = newColor;
                transform.Find("Folder Name").GetComponent<TextMeshProUGUI>().color = Color.white;
            }
        }
        if (ColorUtility.TryParseHtmlString("#000FB2", out newColor))
        {
            if (windowType != WindowType.Feature)
                transform.Find("TopBar").GetComponent<Image>().color = newColor;
        }
    }
    // 그림자를 상하좌우에 추가하는 함수
    private void AddMultipleShadows()
    {
        // 각 방향으로 그림자를 추가
        AddShadowComponent(new Vector2(5, 5));  // 오른쪽 아래로 그림자
        AddShadowComponent(new Vector2(-5, 5)); // 왼쪽 아래로 그림자
        AddShadowComponent(new Vector2(5, -5)); // 오른쪽 위로 그림자
        AddShadowComponent(new Vector2(-5, -5)); // 왼쪽 위로 그림자
    }

    // 그림자 컴포넌트를 추가하는 함수
    private void AddShadowComponent(Vector2 effectDistance)
    {
        Shadow shadow = gameObject.AddComponent<Shadow>();
        shadow.effectDistance = effectDistance;
        shadow.effectColor = new Color(0, 0, 0, 0.5f); // 검은색 그림자, 투명도 50%
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
    public GameObject GetGameObject()
    {
        return gameObject;
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
            windowState = WindowState.Minimize;
            // 최대화된 상태가 아닌 경우만 크기와 위치를 저장
            if (!isMaximized)
            {
                preMaximizedSize = GetComponent<RectTransform>().sizeDelta;
                preMaximizedPosition = GetComponent<RectTransform>().anchoredPosition;
            }

            // 애니메이션으로 창을 화면 밖으로 보내고 크기를 줄임
            StartCoroutine(MinimizeWindow(() =>
            {
                isMinimized = true;
            }));
        }
    }

    // 창 복원 기능
    public void Restore()
    {
        if (isMinimized)
        {
            windowState = WindowState.Open;
            // 창을 원래 위치와 크기로 복원
            StartCoroutine(RestoreWindow(() =>
            {
                isMinimized = false;  // 복원이 완료되면 최소화 상태 해제
            }));
        }
    }

    // 창을 화면 밖으로 보내고 크기를 줄이는 애니메이션
    private IEnumerator MinimizeWindow(System.Action onComplete)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 startScale = rectTransform.localScale;
        Vector3 endScale = Vector3.zero;  // 크기를 0으로 줄임

        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = new Vector2(-Screen.width, -Screen.height);  // 화면 밖으로 이동

        float time = 0f;

        while (time < animationDuration)
        {
            rectTransform.localScale = Vector3.Lerp(startScale, endScale, time / animationDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, time / animationDuration);

            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = endScale;
        rectTransform.anchoredPosition = endPosition;

        onComplete?.Invoke();
    }

    // 창을 원래 위치로 복원하는 애니메이션
    private IEnumerator RestoreWindow(System.Action onComplete)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 startScale = rectTransform.localScale;
        Vector3 endScale = Vector3.one;  // 원래 크기로 복원

        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = isMaximized ? Vector2.zero : preMaximizedPosition;  // 최대화 상태면 중앙, 아니면 원래 위치

        float time = 0f;

        while (time < animationDuration)
        {
            rectTransform.localScale = Vector3.Lerp(startScale, endScale, time / animationDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, time / animationDuration);

            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = endScale;
        rectTransform.anchoredPosition = endPosition;

        // 크기도 복원
        if (!isMaximized)
        {
            rectTransform.sizeDelta = preMaximizedSize;  // 원래 크기로 복원
        }

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
        windowState = WindowState.Open;
    }
}