namespace ORestClient.Interfaces {
    public interface INameMatchResolver {
        bool IsMatch(string actualName, string requestedName);
    }
}