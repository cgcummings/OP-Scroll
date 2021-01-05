@Code
    ViewData("Title") = "Home Page"
End Code

<style>
    .Anime_img {
        height: 100px;
        float: left;
        margin-right: 20px;
    }
</style>
<div class="logo">
    <h1>OP-Scroll</h1>
</div>

<div class="wrapper">
    <div class="search-input">
        <a href="" target="_blank" hidden></a>
        <input type="text" id="SearchAnime" placeholder="Type to search.."  onkeyup="SearchAnime();">
        <div id="autocom" class="autocom-box">

        </div>
        <div class="icon"><i class="glyphicon glyphicon-search" aria-hidden="true"></i></div>
    </div>
</div>

<div class="video-view" id="SongList">
    <h2>Opening songs:</h2>


    <h2>Ending songs:</h2>



</div>

<!-- sample spotify embed -->
<iframe src="https://open.spotify.com/embed/track/@ViewData("SongID")" width="300" height="380" frameborder="0" allowtransparency="true" allow="encrypted-media"></iframe>

<script type="text/javascript">
    // getting all required elements
    const searchWrapper = document.querySelector(".search-input");
    const inputBox = searchWrapper.querySelector("input");
    const suggBox = searchWrapper.querySelector(".autocom-box");
    const icon = searchWrapper.querySelector(".icon");
    let linkTag = searchWrapper.querySelector("a");
    let webLink;

    //$(".autocom-box").change(function () {
    //    $(".autocom-box").attr('size', 1);
    //});

    function GetSpotifySongs(ID) {
        var url = '@Url.Action("", "Home")'
        $("#SongList").html(

        //get patial view
        );
    }

    function SearchAnime() {
        var CurrentValue = $("#SearchAnime").val();
        var url = '@Url.Action("SearchAnime", "Home")?SearchKeyWord=' + CurrentValue + "&SearchImage=" + false;
        var imgurl = '@Url.Action("SearchAnime", "Home")?SearchKeyWord=' + CurrentValue + "&SearchImage=" + true;
        var selector = $(".autocom-box");
        $(".Anime_img").remove();
        $.getJSON(url, null,
            function (data) {
                //show autocomplete box
                var DisplayDropdown = selector;
                var id = 0;
                DisplayDropdown.empty();

                $.each(data, function (key, val) {
                    var action = "GetSpotifySongs(" + val + ")";
                    DisplayDropdown.append($('<li/>', {
                        text: key,
                        value: val,
                        id: id,
                        onclick: action

                    }));
                    id = id + 1;
                })

            }).done(function () {
                $(".Anime_img").remove();
                $.getJSON(imgurl, null,
                    function (data) {
                        var id = 0;

                        $.each(data, function (key, val) {

                            $("#" + id).append($('<img>', {
                                src: key,
                                id: "img_" + val,
                                class: "Anime_img"

                            }));
                            id = id + 1;
                        })

                    }).done(function () {
                        searchWrapper.classList.add("active");
                      
                        DisplayDropdown.attr('size', 3);
                    });
                //$("#autocom").find("li").each(function () {
                //    console.log("found");
                //    $(this).append('<img src="https://cdn.myanimelist.net/images/anime/1314/108941.jpg" alt="2013 Toyota Tacoma" id="itemImg">');

                //});
            });



    }
    function select(element) {
        let selectData = element.textContent;
        inputBox.value = selectData;
        // fix javascript below to work with our site
        icon.onclick = () => {
            webLink = "https://www.google.com/search?q=" + selectData;
            linkTag.setAttribute("href", webLink);
            linkTag.click();
        }
        searchWrapper.classList.remove("active");
    }
</script>
