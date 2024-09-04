public interface IIcon
{
    IWindow Window { get; set; }  // 아이콘과 연결된 창
    void OnIconClick();  // 바탕화면 아이콘 클릭 시 호출
    void Close();
}