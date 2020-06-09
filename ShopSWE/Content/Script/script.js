function timkiem() {
    var a = document.getElementById("search");
    a.classList.toggle("active");
}
$(document).ready(function () {
    $("#left").addClass("left-click");
    $("#right").removeClass("right-click");
    $('#sign-in').addClass("sign-in-click");
});
$("#right").click(function () {
    $("#right").addClass("right-click");
    $('#left').removeClass('left-click');
    $("#left").css({ "color": "rgb(109, 107, 107)" });
    $("#right").css({ "color": "black" });
    $('#sign-up').addClass("sign-up-click");
    $('#sign-in').removeClass("sign-in-click");
});
$('#left').click(function () {
    $('#left').addClass('left-click');
    $('#right').removeClass('right-click');
    $("#right").css({ "color": "rgb(109, 107, 107)" });
    $("#left").css({ "color": "black" });
    $('#sign-up').removeClass("sign-up-click");
    $('#sign-in').addClass("sign-in-click");
})
$('#Sign-up-2').click(function () {
    $('#sign-up').addClass("sign-up-click");
    $('#sign-in').removeClass("sign-in-click");
})
$('#Sign-in-2').click(function () {
    $('#sign-up').removeClass("sign-up-click");
    $('#sign-in').addClass("sign-in-click");
})
function sign() {
    var a = document.getElementById("Username").value;
    var b = document.getElementById('Password').value;
    if (a === "") {
        document.getElementById('check-user').style.display = "block";
        document.getElementById('warning').style.display = "block";
        document.getElementById('success').style.display = "none";
    }
    else {
        document.getElementById('check-user').style.display = "none";
        document.getElementById('warning').style.display = "none";
        document.getElementById('success').style.display = "block";
    }
    if (b === "") {
        document.getElementById('check-pass').style.display = "block";
        document.getElementById('warning-2').style.display = "block";
        document.getElementById('success-2').style.display = "none";
    }
    else {
        document.getElementById('check-pass').style.display = "none";
        document.getElementById('warning-2').style.display = "none";
        document.getElementById('success-2').style.display = "block";
    }
}
//slide-store
var timer = 4500;
var i = 0;
var max = $('#c > li').length;
$("#c > li").eq(i).addClass('active').css('left', '0');
$("#c > li").eq(i + 1).addClass('active').css('left', '20%');
$("#c > li").eq(i + 2).addClass('active').css('left', '40%');

setInterval(function () {
    $("#c > li").removeClass('active');
    $("#c > li").eq(i).css('transition-delay', '0.25s');
    $("#c > li").eq(i + 1).css('transition-delay', '0.5s');
    $("#c > li").eq(i + 2).css('transition-delay', '0.75s');

    if (i < max - 3) {
        i = i + 3;
    }
    else {
        i = 0;
    }
    $("#c > li").eq(i).css('left', '0').addClass('active').css('transition-delay', '1.25s');
    $("#c > li").eq(i + 1).css('left', '20%').addClass('active').css('transition-delay', '1.5s');
    $("#c > li").eq(i + 2).css('left', '40%').addClass('active').css('transition-delay', '1.75s');
}, timer);

