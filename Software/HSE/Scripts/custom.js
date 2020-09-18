$('#ddlProjects').on('change',
    function () {
        var selectedValue = $(this).val(); // < added test values
        if (selectedValue !== "" && selectedValue !== 0) {
            var procemessage = "<option value='0'> صبرکنید...</option>";
            $("#ddlOperation").html(procemessage).show();
            $.ajax(
                {
                    url: "/UserRisks/GetOperation",
                    data: { id: selectedValue },
                    cache: false,
                    type: "POST",
                    success: function (data) {
                        var markup = "<option value='0'>انتخاب عملیات</option>";
                        for (var x = 0; x < data.length; x++) {
                            markup += "<option value=" + data[x].Value + ">" + data[x].Title + "</option>";
                        }
                        $("#ddlOperation").html(markup).show();
                    },
                    error: function (reponse) {
                        alert("error : " + reponse);
                    }
                });
        }
    });


$('#ddlOperation').on('change',
    function () {
        var selectedValue = $(this).val();

        if (selectedValue !== "" && selectedValue !== 0) {
            var procemessage = "<option value='0'> صبرکنید...</option>";

            $("#ddlAct").html(procemessage).show();

            $.ajax(
                {
                    url: "/UserRisks/GetAct",
                    data: { id: selectedValue },
                    cache: false,
                    type: "POST",
                    success: function (data) {
                        var markup = "<option value='0'>انتخاب عملیات</option>";

                        for (var x = 0; x < data.length; x++) {
                            markup += "<option value=" + data[x].Value + ">" + data[x].Title + "</option>";
                        }

                        $("#ddlAct").html(markup).show();
                    },
                    error: function (reponse) {
                        alert("error : " + reponse);
                    }
                });
        }
    });


$('#ddlAct').on('change',
    function () {
        var selectedValue = $(this).val();

        if (selectedValue !== "" && selectedValue !== 0) {
            var procemessage = "<option value='0'> صبرکنید...</option>";

            $("#ddlStage").html(procemessage).show();

            $.ajax(
                {
                    url: "/UserRisks/GetStage",
                    data: { id: selectedValue },
                    cache: false,
                    type: "POST",
                    success: function (data) {
                        var markup = "<option value='0'>انتخاب مرحله انجام کار</option>";

                        for (var x = 0; x < data.length; x++) {
                            markup += "<option value=" + data[x].Value + ">" + data[x].Title + "</option>";
                        }

                        $("#ddlStage").html(markup).show();
                    },
                    error: function (reponse) {
                        alert("error : " + reponse);
                    }
                });
        }
    });

$('#ddlStage').on('change',
    function () {
        var selectedValue = $(this).val();

        if (selectedValue !== "" && selectedValue !== 0) {
            //var procemessage = "<option value='0'> صبرکنید...</option>";

            //$("#ddlStage").html(procemessage).show();
            var markup = '';
            var cookieVal = '';
            $.ajax(
                {
                    url: "/UserRisks/GetRisk",
                    data: { id: selectedValue },
                    cache: false,
                    type: "POST",
                    success: function (data) {


                        for (var x = 0; x < data.length; x++) {
                            markup += "<tr><td>" +
                                data[x].Index +
                                "</td><td>" +
                                data[x].Code +
                                "</td><td>" +
                                data[x].Title +
                                "</td><td>" + GetProbDdl(data[x].Code, data[x].RiskProbability) +
                                "</td><td>" + GetIntDdl(data[x].Code, data[x].RiskIntensity) +
                                "</td><td id='riskNumber_" + data[x].Code +
                                "'></td>";

                            cookieVal += data[x].Code + "|";
                        }

                        $('#rsik-panel').css('display', 'block');
                        $("#risk-table").html(markup).show();
                        setCookie("risks", cookieVal, 100);
                    },
                    error: function (reponse) {
                        alert("error : " + reponse);
                    }
                });
        }
    });

function GetProbDdl(id, items) {
    var ddl = "<select id='ddlprob_" + id + "'><option value='0'>احتمال وقوع</option>";

    for (var x = 0; x < items.length; x++) {
        ddl += "<option value=" + items[x].Value + ">" + items[x].Title + "</option>";
    }

    ddl += "</select>";

    return ddl;
}


function GetIntDdl(id, items) {
    var ddl = "<select id='ddlInt_" + id + "'><option value='0'>شدت</option>";

    for (var x = 0; x < items.length; x++) {
        ddl += "<option value=" + items[x].Value + ">" + items[x].Title + "</option>";
    }

    ddl += "</select>";

    return ddl;
}


function deleteCookie(name) {
    document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}

function setCookie(name, value, days) {
    deleteCookie(name);
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}


function CalculateRisk() {
    var cookie = getCookie('risks');

    var risks = cookie.split('|');

    var riskJson = [];
    var j = 0;
    for (var i = 0; i < risks.length - 1; i++) {
        var prob = $('#ddlprob_' + risks[i]).val();
        var inten = $('#ddlInt_' + risks[i]).val();
        if (prob !== "0" && inten !== "0") {
            riskJson[j] = { "Code": risks[i], "Prob": prob, "Intent": inten };
            j++;
        }
    }
    // riskJson = JSON.stringify(riskJson);

    $.ajax(
        {
            url: "/UserRisks/CalculateRisk",
            data: { input: riskJson },
            cache: false,
            type: "POST",
            success: function (data) {


                for (var x = 0; x < data.length; x++) {
                    $('#riskNumber_' + data[x].Code).html(data[x].Result);
                }

                $('#btn-submit').css('display', 'inline-block');

            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });


}

function SubmitRisk() {
    var cookie = getCookie('risks');

    var risks = cookie.split('|');

    var riskJson = [];
    var formIsValid = true;
    var j = 0;
    for (var i = 0; i < risks.length - 1; i++) {
        var prob = $('#ddlprob_' + risks[i]).val();
        var inten = $('#ddlInt_' + risks[i]).val();
        if (prob !== "0" && inten !== "0") {
            riskJson[j] = { "Code": risks[i], "Prob": prob, "Intent": inten };
            j++;
        } else {
            formIsValid = false;
            $('#errorDiv').css('display', 'block');
            $('#errorDiv').html("برای ثبت ریسک باید احتمال و شدت همه ریسک ها را وارد نمایید.");
        }
    }
    // riskJson = JSON.stringify(riskJson);
    if (formIsValid) {
        $.ajax(
            {
                url: "/UserRisks/SubmitRisk",
                data: { input: riskJson },
                cache: false,
                type: "POST",
                success: function (data) {

                    if (data === "true") {
                        $('#errorDiv').css('display', 'none');
                        $('#successDiv').css('display', 'block');
                    } else {
                        $('#errorDiv').css('display', 'block');
                        $('#errorDiv').html("خطایی رخ داده است. لطفا دوباره تلاش کنید.");
                        $('#successDiv').css('display', 'none');
                    }
                },
                error: function (reponse) {
                    alert("error : " + reponse);
                }
            });

    }
}