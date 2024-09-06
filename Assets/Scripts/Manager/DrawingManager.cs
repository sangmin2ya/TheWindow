using UnityEngine;
using UnityEngine.UI;

public class DrawingManager : MonoBehaviour
{
    public RawImage drawingCanvas;  // 그림을 그릴 캔버스 (RawImage)
    public Slider brushSizeSlider;  // 브러시 크기 조절 슬라이더
    public Color selectedColor = Color.black;  // 기본 색상은 검정
    private Texture2D texture;
    private bool isDrawing = false;
    private Vector2 lastMousePosition;
    private bool hasStartedDrawing = false; // 처음 클릭했을 때만 좌표를 저장하기 위한 플래그

    void Start()
    {
        // 캔버스의 크기에 맞는 Texture2D 생성
        int width = (int)drawingCanvas.rectTransform.rect.width;
        int height = (int)drawingCanvas.rectTransform.rect.height;

        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point; // 필터 모드를 설정해 픽셀화 느낌을 유지
        drawingCanvas.texture = texture;

        ClearCanvas();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // 마우스를 누르면 그리기 시작
        {
            isDrawing = true;
            lastMousePosition = GetMouseTexturePosition();  // 첫 클릭 시 마우스 위치 저장
            hasStartedDrawing = true; // 첫 클릭 플래그 설정
        }

        if (Input.GetMouseButtonUp(0))  // 마우스에서 손을 떼면 그리기 중지
        {
            isDrawing = false;
            hasStartedDrawing = false;  // 마우스를 떼면 다음 클릭 시 새로 시작
        }

        if (isDrawing && hasStartedDrawing)
        {
            DrawOnCanvas();
        }
    }

    // 마우스 위치를 텍스처 좌표로 변환하는 함수
    Vector2 GetMouseTexturePosition()
    {
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingCanvas.rectTransform, Input.mousePosition, Camera.main, out localMousePosition);

        // 로컬 좌표를 텍스처 좌표로 변환
        float canvasWidth = drawingCanvas.rectTransform.rect.width;
        float canvasHeight = drawingCanvas.rectTransform.rect.height;

        int x = Mathf.FloorToInt((localMousePosition.x / canvasWidth + 0.5f) * texture.width);
        int y = Mathf.FloorToInt((localMousePosition.y / canvasHeight + 0.5f) * texture.height);

        // 텍스처 범위 내로 좌표를 클램프
        x = Mathf.Clamp(x, 0, texture.width - 1);
        y = Mathf.Clamp(y, 0, texture.height - 1);

        return new Vector2(x, y);
    }
    // 색상 버튼에서 이미지 색상을 자동으로 받아오는 함수
    public void SelectColorFromButton(Image buttonImage)
    {
        selectedColor = buttonImage.color;  // 버튼의 이미지 색상 받아옴
    }

    // 그림 그리기 함수
    void DrawOnCanvas()
    {
        Vector2 currentMousePosition = GetMouseTexturePosition();  // 현재 마우스 위치

        // 두 점 사이에 선을 그리기
        DrawLine(lastMousePosition, currentMousePosition);

        // 현재 마우스 위치를 다음 프레임을 위해 저장
        lastMousePosition = currentMousePosition;

        texture.Apply();  // 텍스처 변경 사항 적용
    }

    // 두 점 사이에 선을 그리는 함수 (Bresenham's Line Algorithm 사용)
    void DrawLine(Vector2 start, Vector2 end)
    {
        int brushSize = Mathf.FloorToInt(brushSizeSlider.value);

        int x0 = Mathf.RoundToInt(start.x);
        int y0 = Mathf.RoundToInt(start.y);
        int x1 = Mathf.RoundToInt(end.x);
        int y1 = Mathf.RoundToInt(end.y);

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            DrawBrush(x0, y0, brushSize);  // 현재 좌표에 브러시로 그리기

            if (x0 == x1 && y0 == y1) break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    // 브러시 크기에 맞게 픽셀을 칠하는 함수
    void DrawBrush(int x, int y, int brushSize)
    {
        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                if (Mathf.Sqrt(i * i + j * j) <= brushSize)  // 원형 브러시 효과
                {
                    int px = Mathf.Clamp(x + i, 0, texture.width - 1);
                    int py = Mathf.Clamp(y + j, 0, texture.height - 1);
                    texture.SetPixel(px, py, selectedColor);
                }
            }
        }
    }

    // 캔버스 초기화 함수
    public void ClearCanvas()
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, Color.white);  // 기본 배경은 흰색
            }
        }
        texture.Apply();
    }
}