/**
* Format date as a string
* @param date - a date object (usually "new Date();")
* @param format - a string format, eg. "DD-MM-YYYY"
*/
function dateFormat(date, format) {
    // Calculate date parts and replace instances in format string accordingly
    format = format.replace("DD", (date.getDate() < 10 ? '0' : '') + date.getDate()); // Pad with '0' if needed
    format = format.replace("MM", (date.getMonth() < 9 ? '0' : '') + (date.getMonth() + 1)); // Months are zero-based
    format = format.replace("YYYY", date.getFullYear());
    return format;
}

function getQueryStringByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)",
        regex = new RegExp(regexS),
        results = regex.exec(window.location.href);
    if (results == null) {
        return "";
    } else {
        return decodeURIComponent(results[1].replace(/\+/g, " "));
    }
}

function dateTimeEnt(ent_datetime) {
    if (ent_datetime == "" || ent_datetime == null)
        return "";

    var date = new Date(parseInt(ent_datetime.substr(6), 10));
    var time = date.getTime();
    var year = date.getFullYear();
    var month = leadZero(date.getMonth() + 1, 2);
    var day = leadZero(date.getDate(), 2)
    var hour = leadZero(date.getHours(), 2);
    var minute = leadZero(date.getMinutes(), 2);

    return month + '/' + day + '/' + year + ' ' + hour + ':' + minute;

}

function dateEnt(ent_date) {
    if (ent_date == "" || ent_date == null)
        return "";

    var date = new Date(parseInt(ent_date.substr(6), 10));
    //var time = date.getTime();
    var year = date.getFullYear();
    var month = leadZero(date.getMonth() + 1, 2);
    var day = leadZero(date.getDate(), 2)
    //var hour = leadZero(date.getHours(), 2);
    //var minute = leadZero(date.getMinutes(), 2);

    return month + '/' + day + '/' + year;

}

function timeEnt(ent_datetime) {
    if (ent_datetime == "" || ent_datetime == null)
        return "";

    var date = new Date(parseInt(ent_datetime.substr(6), 10));
    var time = date.getTime();
    //var year = date.getFullYear();
    //var month = leadZero(date.getMonth() + 1, 2);
    //var day = leadZero(date.getDate(), 2)
    var hour = leadZero(date.getHours(), 2);
    var minute = leadZero(date.getMinutes(), 2);

    return hour + ':' + minute;

}

function leadZero(number, width) {
    number = String(number);
    while (number.length < width) {
        number = '0' + number;
    }
    return number;
}

// Input datetime needs to be string formatted date/time instead of Int-based datetime
function dateTimeReportEnt(ent_datetime) {
    if (ent_datetime == "" || ent_datetime == null)
        return "";

    var vdates = ent_datetime.split(" ");
    var vdate = vdates[0];
    var x = vdate.split("/");
    var vtime = vdates[1];
    var y = vtime.split(":");

    var year = x[2];
    var month = leadZero(x[1], 2);
    var day = leadZero(x[0], 2)
    var hour = leadZero(y[0], 2);
    var minute = leadZero(y[1], 2);

    return day + '/' + month + '/' + year + ' ' + hour + ':' + minute;

}

function dateReportEnt(ent_datetime) {
    if (ent_datetime == "" || ent_datetime == null)
        return "";

    var vdates = ent_datetime.split(" ");
    var vdate = vdates[0];
    var x = vdate.split("/");

    var year = x[2];
    var month = leadZero(x[1], 2);
    var day = leadZero(x[0], 2)

    return day + '/' + month + '/' + year;

}

function timeReportEnt(ent_datetime) {
    if (ent_datetime == "" || ent_datetime == null)
        return "";

    var vdates = ent_datetime.split(" ");
    var vtime = vdates[1];
    var y = vtime.split(":");

    var hour = leadZero(y[0], 2);
    var minute = leadZero(y[1], 2);

    return hour + ':' + minute;

}

function isContainString(str) {
    if (str == "")
        return false;
    if (!isNaN(str))
        return false;

    var alphabet = Array("a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "y", "z");
    var found = false;
    for (i = 0; i < alphabet.length && !found; i++) {
        if (alphabet[i].toLowerCase() == str.toLowerCase()) {
            found = true;
        }
    }

    return found;
}

function numberFormat(number) {
    if (isContainString(number) || number == null)
        return '';

    var vnumber = String(number);
    var vdec = "";
    var prefix = "";
    if (vnumber.substring(0, 1) == "-") {
        prefix = "-";
        vnumber = vnumber.substring(1, vnumber.length);
    }

    if (vnumber.indexOf(".") != -1) {
        vdec = vnumber.split(".")[1];
        vnumber = vnumber.split(".")[0];
    }
    var strNumber = '';
    var idx = 0;
    for (var i = vnumber.length - 1; i >= 0; i--) {
        idx++;
        if (idx % 3 == 0)
            strNumber = "," + vnumber[i] + strNumber;
        else
            strNumber = vnumber[i] + strNumber;
    }
    if (strNumber.substring(0, 1) == ',') {
        strNumber = strNumber.substring(1);
    }

    if (vdec != "")
        return prefix + strNumber + '.' + vdec;

    return prefix + strNumber;
}

function split(val) {
    return val.split(/,\s*/);
}

function extractLast(term) {
    return split(term).pop();
}

function getSelectOption(selopt) {
    ret = ":;";
    $(selopt + " option").each(function (i) {
        ret += this.value + ":" + this.text + ";";
    });
    ret = ret.substr(0, ret.length - 1);
    return ret;
}
