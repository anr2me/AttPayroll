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
    var ret = ":;";
    if (selopt == null) return ret + "true:Yes;false:No";
    $(selopt + " option").each(function (i) {
        var v = this.value.replace(/:/g, "|").replace(/;/g, "|").replace(/"/g, "'");
        var t = this.text.replace(/:/g, "|").replace(/;/g, "|").replace(/"/g, "'");
        ret += v + ":" + t + ";";
    });
    ret = ret.substr(0, ret.length - 1);
    return ret;
}

function clearForm(form) {
    $(':input', form).each(function () {
        var type = this.type;
        var tag = this.tagName.toLowerCase(); // normalize case
        if (type == 'text' || type == 'password' || type == 'number' || tag == 'textarea')
            this.value = "";
        else if (type == 'checkbox' || type == 'radio')
            this.checked = false;
        else if (tag == 'select')
            this.selectedIndex = 0;
        if ($(this).hasClass('easyui-numberbox'))
            $(this).numberbox('clear');
    });
}

jQuery.extend($.fn.fmatter, { // allow using custom (ie. "yesno" formatter)
    yesno: function (cellvalue, options, rowdata) {
        return cellvalue == true ? "Yes" : "No";
    },
});

// For jQuery < 1.8
jQuery.expr[':'].icontains = function (a, i, m) { // allow using custom is ":icontains" for case-insensitive (like .is(":contains(str)"))
    return jQuery(a).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
};

// For jQuery 1.8++
jQuery.expr[":"].icontains = jQuery.expr.createPseudo(function (arg) {
    return function (elem) {
        return jQuery(elem).text().toUpperCase().indexOf(arg.toUpperCase()) >= 0;
    };
});

function jqGetNextId(objJqGrid) {
    var nextid = 0;
    if (objJqGrid.getDataIDs().length > 0)
        nextid = parseInt(objJqGrid.getDataIDs()[objJqGrid.getDataIDs().length - 1]) + 1;

    return nextid;
}

function EnableButton(btnObject, isEnabled) {
    if (isEnabled) {
        btnObject.removeClass('ui-state-disabled disabled');
    } else {
        btnObject.addClass('ui-state-disabled disabled');
    }
}

// Create Base64 Object
var Base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = Base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9\+\/\=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = Base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/\r\n/g, "\n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } }

// This optional function html-encodes messages for display in the page, to prevent script injection
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
