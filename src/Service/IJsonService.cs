namespace Skclusive.Text.Json
{
    public interface IJsonService
    {
        string Serialize(object value);

        T Deserialize<T>(string json);
    }
}