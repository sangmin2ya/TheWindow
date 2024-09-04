using UnityEngine;
public interface IIcon
{
    IWindow Window { get; }  // 아이콘과 연결된 창
    IconVisibleType IconType { get; }  // 아이콘 타입
    GameObject GetGameObject();  // 아이콘 게임 오브젝트 반환
    void OnIconClick();  // 바탕화면 아이콘 클릭 시 호출
    void Close();
    void DrawIcon(Vector2 position);  // 아이콘을 화면에 그리기
}