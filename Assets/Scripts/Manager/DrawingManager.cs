using UnityEngine;
using UnityEngine.UI;

public class DrawingManager : MonoBehaviour
{
    public RawImage drawingCanvas;       // 그림을 그릴 캔버스 (RawImage)
    public Color selectedColor = Color.black;  // 선택된 색상 (기본 검은색)
    public int brushSize = 5;            // 브러시 크기
    public Texture2D canvasTexture;      // 그림을 그릴 텍스처
    private bool isDrawing = false;      // 그리기 상태 확인

    void Start()
    {
        // 그림을 그릴 캔버스 텍스처 초기화
        canvasTexture = new Texture2D((int)drawingCanvas.rectTransform.rect.width, (int)drawingCanvas.rectTransform.rect.height);
        drawingCanvas.texture = canvasTexture;

        ClearCanvas();
    }

    void Update()
    {
        // 마우스 클릭 중일 때 그리기
        if (Input.GetMouseButton(0))
        {
            Vector2 localMousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingCanvas.rectTransform, Input.mousePosition, null, out localMousePos);

            // 마우스가 캔버스 위에 있을 때만 그리기
            if (drawingCanvas.rectTransform.rect.Contains(localMousePos))
            {
                Draw(localMousePos);
            }
        }
    }

    // 그림을 그리는 함수
    void Draw(Vector2 localMousePos)
    {
        int x = (int)(localMousePos.x + drawingCanvas.rectTransform.rect.width / 2);
        int y = (int)(localMousePos.y + drawingCanvas.rectTransform.rect.height / 2);

        // 브러시 크기만큼의 영역을 그리기
        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                if (x + i >= 0 && x + i < canvasTexture.width && y + j >= 0 && y + j < canvasTexture.height)
                {
                    canvasTexture.SetPixel(x + i, y + j, selectedColor);
                }
            }
        }

        canvasTexture.Apply();  // 변경된 텍스처 적용
    }

    // 캔버스를 초기화하는 함수 (모든 픽셀을 흰색으로 설정)
    public void ClearCanvas()
    {
        for (int x = 0; x < canvasTexture.width; x++)
        {
            for (int y = 0; y < canvasTexture.height; y++)
            {
                canvasTexture.SetPixel(x, y, Color.white);
            }
        }
        canvasTexture.Apply();
    }

    // 색상 버튼을 클릭했을 때 호출되는 함수
    public void SelectColor(Color color)
    {
        selectedColor = color;
    }

    // 브러시 크기를 슬라이더로 조정하는 함수
    public void SetBrushSize(float size)
    {
        brushSize = Mathf.RoundToInt(size);
    }
}
