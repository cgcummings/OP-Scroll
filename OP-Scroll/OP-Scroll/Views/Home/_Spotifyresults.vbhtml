@If Not ViewBag.SpotifyIds Is Nothing Then
    For Each ID In ViewBag.SpotifyIDs
        @<iframe src="https://open.spotify.com/embed/track/@ID" width="300" height="380" frameborder="0" allowtransparency="true" allow="encrypted-media"></iframe>
    Next
End If
