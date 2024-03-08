// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const { error } = require("jquery");

// Write your JavaScript code.


// cuando se actualice la combo se actualice el seatcapacity

$(function () {



    $("#cmbCursos").on("change"), function () {

        var valor = this.value;

        $.ajax({


            type: "GET",
            url: '/Student/DevuelveCapacidad',
            data: { dato: valor },
            success: function (response) {


                $("#txtCapacity").val(response);

                

            },


            error: function (response) {

                alert(response)
            }







        })


    }


})