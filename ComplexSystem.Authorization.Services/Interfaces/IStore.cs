namespace ComplexSystem.Authorization.Services.Interfaces
{
    public interface IStore
    {
        void SetItem<T>(string key, T value);

        T? GetItem<T>(string key);
    }
}
