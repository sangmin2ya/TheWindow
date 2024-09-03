public interface IWindow
{
    WindowType Type { get; }
    WindowState State { get; set; }
    void Open();
    void Close();
    void Minimize();
    void Maximize();
    void SetLayer(int layer);
}