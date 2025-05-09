﻿using Application.Api.Middleware;

namespace Application.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
}
