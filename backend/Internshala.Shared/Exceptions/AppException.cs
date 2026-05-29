namespace Internshala.Shared.Exceptions;

public abstract class AppException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}

public sealed class NotFoundException(string resource, object key)
    : AppException($"{resource} with id '{key}' was not found.", 404);

public sealed class ConflictException(string message) : AppException(message, 409);

public sealed class ForbiddenException(string message = "You do not have permission to perform this action.")
    : AppException(message, 403);

public sealed class UnauthorizedException(string message = "Authentication is required.")
    : AppException(message, 401);

public sealed class ValidationException(IReadOnlyDictionary<string, string[]> errors)
    : AppException("One or more validation errors occurred.", 422)
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
}
