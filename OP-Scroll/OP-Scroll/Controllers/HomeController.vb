Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports System.Net.Http
Imports System.Linq
Imports System.Net.Http.Headers
Imports System.Net
Imports Newtonsoft.Json.Linq

Public Class HomeController
    Inherits System.Web.Mvc.Controller


    <HttpGet>
    Function Index() As ActionResult
        ' GetSpotifyLinks()
        'Controllers.MalScrapeController.GetSongs(5114)
        'SearchResult()
        'ViewData("SongID") = GetSpotifyLinks()








        Return View()
    End Function


    Function SearchResult(Optional ByVal ID As Integer = 5114)
        Dim SongsAndArtist As IDictionary(Of String, String) = Controllers.MalScrapeController.GetSongs(ID)
        Dim SpotifyIDs As New List(Of String)
        For Each n In SongsAndArtist
            Dim link = GetSpotifyLinks(n.Key, n.Value)
            If Not link Is Nothing Then
                SpotifyIDs.Add(link)
            End If

        Next

        ViewBag.SpotifyIDs = SpotifyIDs


        Return PartialView("_Spotifyresults")

    End Function
    Function GetSpotifyLinks(ByVal song As String, ByVal Artist As String)
        Dim client As HttpClient = Controllers.SpotifyAPIController.GetSpotifyClient()
        Dim QueryString As String = "search?type=track&limit=1&q=" & song & "%20artist:" & Artist
        Using client
            Dim responseTask = client.GetAsync(QueryString)
            responseTask.Wait()

            Dim result = responseTask.Result
            Dim JsonDrillDown = Nothing
            If result.IsSuccessStatusCode Then
                Dim readTask = result.Content.ReadAsStringAsync().Result



                Dim lsObj = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(readTask)





                Dim data As String = readTask.ToString()

                Dim dilimit As String() = {"track"}
                Dim pieces As String() = data.Split(dilimit, StringSplitOptions.None)
                Dim SongID = Nothing

                For Each piece In pieces
                    If piece.StartsWith("/") Then
                        SongID = Controllers.YoutubeAPIController.getBetween(piece, "/", """"c)
                    End If

                Next



                Return SongID

                'For Each item In AnimeList
                '    Console.WriteLine(item)
                'Next

                'AnimeList.RemoveAt(0)
                'Return Json(AnimeList, JsonRequestBehavior.AllowGet)
            Else
                Return Nothing
            End If


        End Using
    End Function
    Function GetYoutubeLinks(Optional ByVal SelectedAnimeID As Integer = 5114)

        ' Getting lists of OPS and EDS for searched Anime 
        Dim SongList As New List(Of String)
        Dim OpeningSongs As New List(Of String)
        Dim EndingSongs As New List(Of String)
        Dim YouTubeLinks As New List(Of String)
        Dim i = 0
        Dim ED_Start As Integer = Nothing
        SongList = Controllers.MalScrapeController.GetSongs(SelectedAnimeID)

        For Each song In SongList

            If song = "EDS" Then
                ED_Start = i
            End If
            i = i + 1
        Next

        For j As Integer = 0 To ED_Start - 1
            OpeningSongs.Add(SongList(j))

        Next

        For k As Integer = ED_Start + 1 To SongList.Count - 1
            EndingSongs.Add(SongList(k))
        Next
        '''''''''''''''''''''''''''''''''''''''''''''
        Dim a = OpeningSongs.Count
        Dim b = EndingSongs.Count
        If Not OpeningSongs Is Nothing Then
            For Each song In OpeningSongs
                YouTubeLinks.Add(Controllers.YoutubeAPIController.GetVideoURL(song))
                Console.WriteLine(song)
            Next
        End If

        If Not EndingSongs Is Nothing Then
            YouTubeLinks.Add("EDS")
            For Each song In EndingSongs
                Console.WriteLine(song)
                YouTubeLinks.Add(Controllers.YoutubeAPIController.GetVideoURL(song))
            Next
        End If



        If Not YouTubeLinks Is Nothing Then
            Return YouTubeLinks
        Else
            Return Nothing
        End If
    End Function


    Function SearchAnime(ByVal SearchKeyWord As String, ByVal SearchImage As Boolean) As JsonResult

        ' MAL API query here to return list... make contract 
        Dim AnimePictureList As New List(Of String)
        Dim Anime_list = New Dictionary(Of String, List(Of String))()
        Dim animelist = New Dictionary(Of String, Integer)


        Dim QueryList = Nothing
        Dim QueryString As String = "anime?q=" & SearchKeyWord.ToString() & "&limit=4"

        If QueryString.Length > 2 Then
            Try
                Dim client As HttpClient = Controllers.APIController.AuthorizeAPI()

                Using client
                    Dim responseTask = client.GetAsync(QueryString)
                    responseTask.Wait()

                    Dim result = responseTask.Result
                    Dim JsonDrillDown = Nothing
                    If result.IsSuccessStatusCode Then
                        Dim readTask = result.Content.ReadAsStringAsync().Result



                        Dim lsObj = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(readTask)

                        For Each obj In lsObj
                            If obj.Key = "data" Then

                                Dim data As String = obj.Value.ToString()
                                Dim dilimit As String() = {"node"}
                                Dim pieces As String() = data.Split(dilimit, StringSplitOptions.None)
                                If Not SearchImage Then
                                    For Each piece In pieces
                                        Dim AnimeTitle = Controllers.YoutubeAPIController.getBetween(piece, "title", "main_picture")
                                        Dim AnimeID = CInt(Controllers.YoutubeAPIController.getBetween(piece, "id", "title"))

                                        If Not AnimeTitle Is Nothing Then
                                            animelist.Add(AnimeTitle, AnimeID)
                                        End If

                                    Next
                                Else
                                    For Each piece In pieces

                                        Dim AnimeID = CInt(Controllers.YoutubeAPIController.getBetween(piece, "id", "title"))
                                        Dim Picture = Controllers.YoutubeAPIController.getBetween(piece, "medium", ",")
                                        If Not Picture Is Nothing Then
                                            animelist.Add(Picture, AnimeID)
                                        End If

                                    Next
                                End If



                            End If


                        Next





                        Return Json(animelist, JsonRequestBehavior.AllowGet)
                    Else
                        Return Nothing
                    End If


                End Using
            Catch ex As Exception

            End Try


        End If




    End Function


    Function About() As ActionResult
        ViewData("Message") = "Your application description page."

        Return View()
    End Function

    Function Contact() As ActionResult
        ViewData("Message") = "Your contact page."

        Return View()
    End Function
End Class
