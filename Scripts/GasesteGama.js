var accessId = "6271a36d-dcf3-44bd-b58c-bb41b619092f";

function onTaskKey(data) {
    var xml = $.parseXML(data);
    var key = $(xml).find("tonart_result").attr("key");
    console.log("Key: " + key);
    $("#result_key").text(key);
}

function onTaskFailedKey(response) {
    var data = $.parseJSON(response.responseText);
    var errorMessages = data.errors.map(function (error) {
        return error.message;
    });

    $("#result_key").text("Task failed, reason: " + errorMessages.join(","));
}

$(document).ready(function () {
    $("#start").click(function () {
        var fileData = new FormData();
        fileData.append("access_id", accessId);
        fileData.append("input_file", $("#file")[0].files[0]);

        $.ajax({
            url: "https://api.sonicAPI.com/analyze/key",
            type: "POST",
            data: fileData,
            contentType: false,
            processData: false,
            success: onTaskKey,
            error: onTaskFailedKey,
            crossDomain: true,
            dataType: "text",
        });
    });

    var dragArea = document.getElementById("fileInput");

    dragArea.addEventListener("dragover", function (e) {
        e.preventDefault();
        dragArea.classList.add("drag-over");
    });

    dragArea.addEventListener("dragleave", function (e) {
        e.preventDefault();
        dragArea.classList.remove("drag-over");
    });

    dragArea.addEventListener("drop", function (e) {
        e.preventDefault();
        dragArea.classList.remove("drag-over");

        var file = e.dataTransfer.files[0];
        var fileData = new FormData();
        fileData.append("access_id", accessId);
        fileData.append("input_file", file);

        $.ajax({
            url: "https://api.sonicAPI.com/analyze/key",
            type: "POST",
            data: fileData,
            contentType: false,
            processData: false,
            success: onTaskKey,
            error: onTaskFailedKey,
            crossDomain: true,
            dataType: "text",
        });
    });
});