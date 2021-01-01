function saveToLocalStorage(key, value) {
    localStorage.setItem(key, value);
}
function getLocalStorage(key, value) {
    return localStorage.getItem(key)
}

$(document).ready(function () {
    $("#login-form").on("submit", function (e) {
        e.preventDefault();
        const loginUrl = "api/v1/User/Login";

        var data = {
            email : $("#email").val(),
            password : $("#password").val()
        }

        $.ajax({
            type: 'POST',
            url: loginUrl,
            data: JSON.stringify(data),
            contentType: 'application/json',
            headers: {
                'Access-Control-Allow-Headers': '*',
            },
            success: function (response) {
                saveToLocalStorage("AuthToken", response.token);
                window.location = "spin.html";
            },
        })
    });

    $("#register-form").on("submit", function (e) {
        e.preventDefault();
        const registerUrl = "api/v1/User/Register";

        var data = {
            email : $("#email").val(),
            password : $("#password").val(),
            confirmPassword: $("#confirmPassword").val(),
            name : $("#name").val(),
            lastname : $("#lastname").val(),
            city : $("#city").val()
        }
        if (!(password.value == confirmPassword.value) ){
            alert("Password and confirmed password should match");
        }
        $.ajax({
            type: 'POST',
            url: registerUrl,
            data: JSON.stringify(data),
            contentType: 'application/json',
            headers: {
                'Access-Control-Allow-Headers': '*',
            },
            success: function (response) {
                window.location = "index.html";
            },
        })
    });

    $("#bet-form").on("submit", function (e) {
        e.preventDefault();
       
        var bet = $("#bet").val();
        const betUrl = "api/SlotMachine/Spin/" + bet;

       if (bet==0) {
            alert("Bet can not be Zero");
        }
        $.ajax({
            type: 'GET',
            url: betUrl,
            contentType: 'application/json',
            headers: {
                'Access-Control-Allow-Headers': '*',
                'Authorization': 'Bearer ' + getLocalStorage("AuthToken")
            },
            success: function (response) {
                console.log(response);
                $("#win").text("Win: " + response.win);
                $("#points").text("Points Earned: " +response.currentPoints);
                var data = response.spinValue;
                var s="";
                for (var i = 0; i < data.length; i++) {
                   // alert("key " + i + " and " + "Value is " + data[i]);
                    s = s + data[i] + ',';

                }
                $("#slot-value").text("Reel for this bet: " + s);

              //  $("slot-value").val() = response.slotValues;
            },
        })
    });
});