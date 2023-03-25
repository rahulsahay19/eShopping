namespace Common.Logging.Correlation;

public interface ICorrelationIdGenerator
{
    string Get();
    void Set(string correlationId);
}