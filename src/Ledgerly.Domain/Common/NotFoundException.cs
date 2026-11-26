namespace Ledgerly.Domain.Common;

public class NotFoundException(string message) : Exception(message);