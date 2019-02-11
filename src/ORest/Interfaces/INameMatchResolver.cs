namespace ORest.Interfaces {
    public interface INameMatchResolver {
        bool IsMatch(string actualName, string requestedName);
    }
}