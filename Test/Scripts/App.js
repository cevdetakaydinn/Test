$(".userId").each(function () {
    var temp = $(this)
    var user = $(this).text();
    $.ajax({
        dataType: "Json",
        url: '/Students/GetUserById/',
        type: "GET",
        data: { id: user },
        async: false,
        success: function (response) {
            temp.text(response);
        },
        error: function (error) {
            $(this).remove();
            DisplayError(error.statusText);
        }
    });
})