public interface IService
{
    public void Initialize();

    public void DeInitialize();

    public bool IsInitialized { get; set; }
}
