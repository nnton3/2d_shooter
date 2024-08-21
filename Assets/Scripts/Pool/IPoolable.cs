public interface IPoolable
{
    bool InPool { get; set; }
    void Clean();
}
