using UnityEngine;
using UnityEngine.EventSystems;

public interface IWindow
{
    public WindowType windowType { get; set; }  // 창 타입
    public WindowState windowState { get; set; }  // 창 상태
    public IIcon icon { get; set; }  // 창과 연결된 아이콘
    void OnPointerDown(PointerEventData eventData);  // 창 이동 시작
    void OnDrag(PointerEventData eventData);  // 창 이동 중
    void OnPointerUp(PointerEventData eventData);  // 창 이동 종료

    void Minimize();  // 창 최소화
    void Restore();  // 창 복원
    void ToggleMaximize();  // 창 최대화/복원 토글

    void Close();  // 창 닫기
    void Open();  // 창 열기
    void Focus();  // 창 포커스
    void OnTaskbarIconClick();  // 작업 표시줄 아이콘 클릭 시 호출
    void ResetWindow();  // 창을 초기화된 상태로 리셋
}