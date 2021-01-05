Imports System.Net.Http
Imports System.Web.Mvc

Namespace Controllers
    Public Class SpotifyAPIController
        Inherits Controller

        ' GET: SpotifyAPI
        Public Shared Function GetSpotifyClient()
            Dim client As New HttpClient With {
            .BaseAddress = New Uri("https://api.spotify.com/v1/")
            }

            Dim Key As String = "BQDZBqPjiEMlkqhoRo15ukuZhyQn7UxzkmSTz4cckz59Pg4iN4tnhIpXGxWhM9hIhI6RKRv75dZZa-w0o6c"

            client.DefaultRequestHeaders.Authorization = New Headers.AuthenticationHeaderValue("Bearer", Key)

            Return client

        End Function
    End Class
End Namespace