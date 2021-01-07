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

            Dim Key As String = "BQDGm1GB6oWblx4DOBpGYUpQnW3TCUXVaOKbqE3KvHuPakS2mPAHWuaV_CdJ0idMjSVrFq2BhK6UHnFxlA8"

            client.DefaultRequestHeaders.Authorization = New Headers.AuthenticationHeaderValue("Bearer", Key)

            Return client

        End Function
    End Class
End Namespace