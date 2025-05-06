using System.Net;

namespace Application.Domain.Exceptions;

public class ValidacaoException(string error, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : Exception(error)
{
    public HttpStatusCode HttpStatusCode { get; } = httpStatusCode;

    public static void When(bool hasError, string error)
    {
        if (hasError)
            throw new ValidacaoException(error);
    }
}
