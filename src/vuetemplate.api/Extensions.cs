namespace VueTemplate.Api;

public static class Extensions
{
	/// <summary>
	/// This serves index.html whenever a path is requested that results in a 404.
	/// This way the Vue router takes over and handles the request.
	/// Exceptions are made for paths starting with '/api' with the assumption that
	/// all backend API endpoints are prefixed with this segment.
	/// </summary>
	/// <param name="app">The app builder.</param>
	/// <param name="routingDocument">The path of the document to serve which handles frontend routing.</param>
	/// <returns>The configured application builder.</returns>
	public static IApplicationBuilder UseFrontendRequestRouting(this IApplicationBuilder app, string routingDocument)
	{
		app.Use(async (context, next) =>
		{
			await next();

			if (
				context.Response.StatusCode == 404 &&
				context.Request.Path.HasValue &&
				!context.Request.Path.Value.StartsWith("/api")
			)
			{
				context.Response.StatusCode = 200;
				context.Response.ContentType = "text/html";
				await context.Response.SendFileAsync(routingDocument);
			}
		});

		return app;
	}
}
