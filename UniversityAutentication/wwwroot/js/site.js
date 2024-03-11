// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $("#cmbCursos").on("change", function () {
        var valor = this.value;
        $.ajax({
            type: "GET",
            url: '/Student/DevuelveCapacidad',
            data: { dato: valor },
            success: function (response) {
                $("#txtCapacity").val(response)
            },
            error: function (response) {
                alert(response);
            }
        })
    })
    $("#cmbCursoInstructor").on("change", function () {
        var valor = this.value;
        $.ajax({
            type: "GET",
            url: "/Instructor/DevuelveMatriculas",
            data: { dato: valor },
            success: function (response) {
                $("#tableStudent").empty();
                $("#tableStudent").append('<tr><th>Student</th><th>Letter</th><th></th></tr>');
                for (var i = 0; i < response.length; i++) {
                    $('#tableStudent').append('<tr><td>' + response[i].student.studentName
                        + '</td><td class="text-right"><input type="text" id="txtGrade" value="' + String.fromCharCode(response[i].letterGrade) + '"/>'
                        + '</td><td><input type="button" id="btnCalificar" value="Post Grade" class="btn btn-success" onclick="calificar(' + response[i].student.studentId + ')"/>'
                        + '</td></tr>');
                }

            }
        });
    })
})

function calificar(studentId) {
    var grade = $(".txtGrade").val();
    $.ajax({
        type: "GET",
        url: '/Instructor/Califica',
        data: { dato: studentId, nota: grade },
        success: function (response) {
            alert('Post Grade....');
        }
    });
}