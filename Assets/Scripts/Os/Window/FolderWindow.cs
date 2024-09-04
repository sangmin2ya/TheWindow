using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class FolderWindow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IWindow
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

    // 파일 목록을 추가할 변수
    public Transform fileGrid; // 파일들이 들어갈 그리드
    public List<GameObject> filePrefabs = new List<GameObject>(); // 추가할 파일 목록

    private Vector2 originalSize;
    private Vector2 originalPosition;
    private Vector2 preMaximizedSize;
    private Vector2 preMaximizedPosition;
    private bool isMinimized = false;
    private bool isMaximized = false;

    // Inspector에 노출될 필드
    [SerializeField] private WindowType _windowType;
    [SerializeField] private WindowState _windowState;
    public Button _icon;
    public Button _maximizeIcon;

    private Vector2 lastMousePosition;
    private GameObject _taskbarIcon;
    private bool isDragging = false;
    private float draggableHeight = 450f;

    void Start()
    {
        windowType = _windowType;
        icon = _icon.GetComponent<IIcon>();
        windowState = WindowState.Open;

        _taskbarIcon = Instantiate(_icon as MonoBehaviour, GameObject.Find("TaskCanvas").transform).gameObject;
        _taskbarIcon.GetComponent<DesktopIcon>().window = gameObject;

        // 파일 목록 초기화
        PopulateFiles();
    }

    // 파일 목록을 그리드에 추가하는 함수
    public void PopulateFiles()
    {
        // 그리드 내의 기존 파일 버튼 제거
        foreach (Transform child in fileGrid)
        {
            Destroy(child.gameObject);
        }

        // 파일 프리팹을 그리드에 직접 추가
        foreach (GameObject filePrefab in filePrefabs)
        {
            // 파일 프리팹을 그리드에 배치
            GameObject fileInstance = Instantiate(filePrefab, fileGrid);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Focus();
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 localMousePosition = rectTransform.InverseTransformPoint(eventData.position);

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

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void Minimize()
    {
        if (!isMinimized)
        {
            if (!isMaximized)
            {
                preMaximizedSize = GetComponent<RectTransform>().sizeDelta;
                preMaximizedPosition = GetComponent<RectTransform>().anchoredPosition;
            }

            gameObject.SetActive(false);
            isMinimized = true;
        }
    }

    public void Restore()
    {
        if (isMinimized)
        {
            gameObject.SetActive(true);

            if (isMaximized)
            {
                RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
                GetComponent<RectTransform>().sizeDelta = canvasRect.sizeDelta;
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                GetComponent<RectTransform>().sizeDelta = preMaximizedSize;
                GetComponent<RectTransform>().anchoredPosition = preMaximizedPosition;
            }

            isMinimized = false;
        }
    }

    public void Focus()
    {
        if (isMinimized)
        {
            Restore();
        }
        transform.SetAsLastSibling();
    }

    public void ToggleMaximize()
    {
        if (!isMaximized)
        {
            preMaximizedSize = GetComponent<RectTransform>().sizeDelta;
            preMaximizedPosition = GetComponent<RectTransform>().anchoredPosition;

            RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            GetComponent<RectTransform>().sizeDelta = canvasRect.sizeDelta;
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            isMaximized = true;
        }
        else
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(945, 774);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            isMaximized = false;
        }
    }

    public void Close()
    {
        _taskbarIcon.GetComponent<IIcon>().Close();
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

    public void OnTaskbarIconClick()
    {
        if (!isMinimized && gameObject.activeSelf)
        {
            Minimize();
        }
        else
        {
            Restore();
        }
    }

    public void ResetWindow()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(945, 774);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        isMinimized = false;
        isMaximized = false;
    }
}
