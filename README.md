# dotnet-vue-template

The purpose of this project is to be a template for simple setup of a Vue 3 SPA with an accompanying backend
.NET 6.0 API. In production the app runs as a single process by placing the output from the frontend build into
the wwwroot of the backend API.

The source code is built up of two cookie-cutter app templates from a .NET 6.0 web API and a Vue 3.0 app scaffold.

The following modifications have been made in order to achieve a single process runtime built and served as a Docker
image.

In Program.cs
```CSharp
// Added for serving the SPA as static files.
app.UseDefaultFiles();
app.UseStaticFiles();

// See Extensions.cs for details on this.
app.UseFrontendRequestRouting(Path.Combine(builder.Environment.WebRootPath, "index.html"));
```

A drawback to hosting front- and backend within the same process is that we need to consider route collision when
developing our views and API endpoints. The template is configured with the intent of having all backend enpoints
served from routes prefixed with `/api`. This _should_ ensure that no conflicts arise between front- and backend.

## Dockerfile
The `Dockerfile` builds the backend and frontend in separate layers then creates a dotnet runtime where
the outputs from the two builds are put together.

Build the image in the root of the repo

`docker build . -t vue-template`

Run the image

`docker run -p 8080:80 vue-template`

Check that the app is running at `http://localhost:8080`

It should display the standard Vue 3 welcome page, with an added component showing "Tomorrow's Weather".
This component fetches data from the .NET template `WeatherForecastController` backend API endpoint as a
proof-of-concept.

## Development

In development the backend and frontend are ran in separate processes. In order to ensure consistency in API
base paths the following configuration has been added to the `vite.config.ts` in the frontend project.

It sets up the Vite dev server to act as a proxy for requests made to paths starting with `/api`. These are piped
directly into the backend localhost URL. This further enforces a convention that all backend enpoints
should have paths prefixed with `/api` in order to ensure separation between backend and frontend routes.

```TypeScript
server: {
  proxy: {
    '/api': 'http://localhost:5101',
  },
},
```
