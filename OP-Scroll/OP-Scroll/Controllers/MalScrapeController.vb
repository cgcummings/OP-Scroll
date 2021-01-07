Imports System.Net
Imports System.Web.Mvc
Imports HtmlAgilityPack

Namespace Controllers
    Public Class MalScrapeController
        Inherits Controller

        Public Shared Function GetSongs(ByVal AnimeID As Integer)
            Dim BaseUrl = "https://myanimelist.net/anime/" & AnimeID.ToString()
            Dim client = New WebClient()
            Dim html As String = Nothing
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36")

            html = client.DownloadString(BaseUrl)

            If Not html Is Nothing Then
                Dim document = New HtmlDocument()
                document.LoadHtml(html)
                Dim node As HtmlNode = document.DocumentNode



                Dim SongsAndArtist As IDictionary(Of String, String) = New Dictionary(Of String, String)()


                Dim OPhtmlList = node.SelectNodes("//div[@class='theme-songs js-theme-songs opnening']/span")
                Dim EDhtmllist = node.SelectNodes("//div[@class='theme-songs js-theme-songs ending']/span")

                If Not OPhtmlList Is Nothing Then
                    For Each n In OPhtmlList
                        Dim RawSong = String.Format(YoutubeAPIController.getBetween(n.InnerText, "&quot;", "&"), Encoding.UTF8)
                        Dim RawArtist = String.Format(YoutubeAPIController.getBetween(n.InnerText, "by", "(ep"), Encoding.UTF8)

                        'convert artist
                        Dim hopefullyRecoveredRawArtist = Encoding.GetEncoding(1252).GetBytes(RawArtist)
                        Dim oughtToBeJapaneseRawArtist = Encoding.GetEncoding(932).GetString(hopefullyRecoveredRawArtist)

                        Dim Artist = Regex.Replace(oughtToBeJapaneseRawArtist, "[^\u0000-\u007F]+", String.Empty)
                        If Artist.Contains("(") Or Artist.Contains(")") Then
                            Artist = Artist.Trim(New Char() {"("c, ")"c})

                        End If

                        'convert song
                        Dim hopefullyRecovered = Encoding.GetEncoding(1252).GetBytes(RawSong)
                        Dim oughtToBeJapanese = Encoding.GetEncoding(932).GetString(hopefullyRecovered)

                        Dim songTitle = Regex.Replace(oughtToBeJapanese, "[^\u0000-\u007F]+", String.Empty)
                        If songTitle.Contains("(") Or songTitle.Contains(")") Then

                            songTitle = songTitle.Trim(New Char() {"("c, ")"c})
                        End If
                        SongsAndArtist.Add(songTitle, Artist)
                    Next
                End If

                Return SongsAndArtist
            Else
                    Return Nothing
            End If



        End Function

    End Class
End Namespace
