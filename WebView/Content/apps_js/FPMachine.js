$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
    var vModul = 'FPMachine';
    //var scripts = document.getElementsByTagName('script');
    //var lastScript = scripts[scripts.length - 1];
    //var scriptName = lastScript.src;

    //<!--SignalR script to update the chat page and send messages.-->
    $(function () {
        // Reference the auto-generated proxy for the hub.

        var feedback = $.connection.feedbackHub;
        // Create a function that the hub can call back to display messages.
        feedback.client.updateRecord = function (id, modelstr) {
            // Add the message to the page. 
            var parentid = jQuery("#list").jqGrid('getGridParam', 'selrow');
            if (id == parentid) {
                var newData = jQuery.parseJSON(modelstr);
                if (newData != null) {
                    newData['ID'] = newData.Id;
                    newData['Date Time'] = newData.Time_second;
                    newData['Verification Mode'] = newData.VerifyMode;
                    newData['Attendance State'] = newData.InOutMode;
                    newData['Created At'] = newData.CreatedAt;
                    newData['Updated At'] = newData.UpdatedAt;
                    //var newId = 'Id'; //jqGetNextId($());
                    //jQuery("#list").jqGrid('addRowData', newData.Id, newData);
                    // first change the cell in the visible part of grid
                    myGrid.jqGrid('setCell', newData.Id, 'myColumn', newValue);
                    // now change the internal local data
                    myGrid.jqGrid('getLocalRow', newData.Id).myColumn = newValue;
                }
            }
        };
        // Start the connection.
        $.connection.hub.start().done(function () {
            // Call the Send method on the hub, ensures that the connection is established before the event handler executes
            //feedback.server.sendRow(null, null);
        });
    });

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        var id = $('#parenttype option:selected').text();
        var value = $('#parenttype').val();

        $("#list").setGridParam({ url: base_url + vModul + '/GetList', postData: { filters: null, ParentId: value } }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Description').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    var myTimer = null;
    function onAutoRefresh()
    {
        var obj = document.getElementById("AutoRefresh");
        if (obj.checked) {
            var timerBusy = false;
            myTimer = setInterval(function () { TimerTick() }, 15000);
            function TimerTick() {
                if (!timerBusy) {
                    timerBusy = true;
                    //ajax, when done change to timerBusy=false
                    ReloadGrid();
                    timerBusy = false;
                }
            }
        }
        else {
            clearInterval(myTimer);
        }
    };
    $("#AutoRefresh").live("click", function () {
        onAutoRefresh();
    });

    $("#parenttype").live("change", function () {
        ReloadGrid();
    });

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#uploaduser_confirm_div").dialog('close');
    $("#dnloaduser_confirm_div").dialog('close');
    $("#lookup_div_companyinfo").dialog('close');

    // CheckBox
    function UpdateEnable(objId /*, isEnabled, isMain, isDetail*/) {
        //var userId = 0;
        //var id = jQuery("#tbl_users").jqGrid('getGridParam', 'selrow');
        //if (id) {
        //    userId = id;
        //} else {
        //    $.messager.alert('Information', 'Please Select User...!!', 'info');
        //    return;
        //};

        isAutoConnect = $("input[name=cbFormat_isautoconnect" + objId + "]").is(":checked");

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + vModul + "/UpdateEnable?isAutoConnect=" + isAutoConnect,
            data: JSON.stringify({
                Id: objId
            }),
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.Errors[key], 'warning');
                        }
                    }
                }

            }
        });
    }

    $("input[name*=cbFormat_]").live("click", function () {
        UpdateEnable($(this).attr('rel')/*, $(this).is(":checked")*/);
    });

    function cboxFormat(cellvalue, options, rowObject) {
        return '<input name="cbFormat_' + options.colModel.index + options.rowId + '" rel="' + /*rowObject.id*/options.rowId + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    //jQuery.extend($.fn.fmatter, {
    //    yesno: function (cellvalue, options, rowdata) {
    //        return cellvalue == true ? "Yes" : "No";
    //    },
    //    commtype: function (cellvalue, options, rowdata) {
    //        return document.getElementById('CommType').options[cellvalue].text;
    //    }
    //});

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + vModul + '/GetList',
        postData: { 'ParentId': function () { return $("#parenttype").val(); } },
        datatype: "json",
        //colNames: ['ID', 'Code', 'Name', 'Address', 'City', 'Postal Code', 'Phone No.', 'Fax No.', 'Email', 'Website', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'ID', index: 'id', width: 60, align: "center" },
                  { name: 'Company ID', index: 'companyinfoid', width: 60, hidden:true },
				  { name: 'Company', index: 'companyinfo', width: 150 },
                  { name: 'Machine Number', index: 'machinenumber', width: 100 },
                  { name: 'Machine Name', index: 'machinename', width: 100 },
                  { name: 'Password', index: 'password', width: 100 },
                  {
                      name: 'Auto-Connect', index: 'isautoconnect', width: 85, align: 'center', search: true,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxFormat, formatoptions: { disabled: false },
                      stype: 'select', editoptions: { value: getSelectOption(null) }
                  },
                  { name: 'Connected', index: 'isconnected', width: 65, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption(null) } },
                  { name: 'InSync', index: 'isinsync', width: 60, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption(null) } },
                  { name: 'Clear AttLog', index: 'isclearlogafterdownload', width: 80, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption(null) } },
                  { name: 'Comm Type', index: 'commtype', width: 100, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption("#CommType") } },
                  { name: 'Ethernet IP', index: 'ethernetip', width: 100 },
                  { name: 'Ethernet Port', index: 'ethernetport', width: 80 },
                  { name: 'Ethernet MAC', index: 'ethernetmac', width: 110 },
                  { name: 'Serial Port', index: 'serialport', width: 60 },
                  { name: 'Serial Baud Rate', index: 'serialbaudrate', width: 100 },
                  { name: 'Time Zone', index: 'timezone', width: 100, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption("#TimeZone") } },
                  { name: 'TZ Addon Offset', index: 'TimeZoneOffset', width: 80 },
                  { name: 'Platform', index: 'platform', width: 100 },
                  { name: 'Firmware Ver', index: 'firmwarever', width: 100 },
                  { name: 'Arithmetic Ver', index: 'arithmeticver', width: 110 },
                  { name: 'Serial Number', index: 'serialnumber', width: 100 },
                  { name: 'User Count', index: 'usercount', width: 80 },
                  { name: 'Admin Count', index: 'admincount', width: 80 },
                  { name: 'Pswd Count', index: 'passwordcount', width: 80 },
                  { name: 'FP Count', index: 'fpcount', width: 80 },
                  { name: 'Face Count', index: 'fccount', width: 80 },
                  { name: 'AttLog Count', index: 'attlogcount', width: 80 },
                  { name: 'Created At', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'Updated At', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        cmTemplate: { editable: true },
        page: '1',
        pager: $('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "DESC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        ondblClickRow: function (rowid) {
            $("#btn_edit").trigger("click");
        },
        gridComplete:
		  function () {
		      //var ids = $(this).jqGrid('getDataIDs');
		      //for (var i = 0; i < ids.length; i++) {
		      //    var cl = ids[i];
		      //    rowDel = $(this).getRowData(cl).deletedimg;
		      //    if (rowDel == 'true') {
		      //        img = "<img src ='" + base_url + "content/assets/images/remove.png' title='Data has been deleted !' width='16px' height='16px'>";

		      //    } else {
		      //        img = "";
		      //    }
		      //    $(this).jqGrid('setRowData', ids[i], { deletedimg: img });
		      //}
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    //$.ajax(
    //{
    //    type: "POST",
    //    url: base_url + vModul + '/GetModelList', // ColumnAndData
    //    postData: { 'ParentId': function () { return $("#parenttype").val(); } },
    //    data: "",
    //    dataType: "json",
    //    success: function (result) {
    //        colD = result.colData;
    //        colN = result.colNames;
    //        colM = result.colModel;

    //        jQuery("#list").jqGrid({
    //            jsonReader: {
    //                cell: "",
    //                id: "0"
    //            },
    //            url: base_url + vModul + '/GetList', //DataOnly
    //            datatype: 'jsonstring',
    //            mtype: 'POST',
    //            datastr: colD,
    //            colNames: colN,
    //            colModel: colM,
    //            pager: jQuery('#pager'),
    //            rowNum: 5,
    //            rowList: [5, 10, 20, 50],
    //            viewrecords: true
    //        })
    //    },
    //    error: function (x, e) {
    //        alert(x.readyState + " " + x.status + " " + e.msg);
    //    }
    //});
    //setTimeout(function () { $("#list").jqGrid('setGridParam', { datatype: 'json' }); }, 50);

    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        ReloadGrid();
    });

    $('#btn_print').click(function () {
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        vStatusSaving = 0; //add data mode	
        var parentname = $('#parenttype option:selected').text();
        var parentid = $('#parenttype').val();
        $('#CompanyInfoId').val(parentid);
        $('#CompanyInfoName').val(parentname);
        $('#MachineNumber').val(1);
        $('#EthernetIP').val("192.168.1.201");
        $('#EthernetPort').val("4370");
        $('#SerialBaudRate').val("115200");
        document.getElementById('IsClear').checked = false;
        $('#TimeZoneOffset').val('0');
        //var tzoffset = new Date().getTimezoneOffset(); // the difference, in minutes, between UTC and local time
        var timezone = jstz.determine();
        var tzname = timezone.name(); // eg. "Asia/Jakarta"
        var fixtzname = tzname.split('/')[1].split('_').join(' ').toUpperCase(); // convert "Asia/Jakarta" to "JAKARTA"
        //$('#TimeZone option').each(function () {
        $('#TimeZone').find('option').each(function () { // find an (uppercased) option's text which contains "JAKARTA"
            if ($(this).text().toUpperCase().indexOf(fixtzname) >= 0) { //($(this).is(':contains(' + fixtzname + ')'))
                fixtzname = $(this).val();
                return false; //break from iteration
            }
        });
        $('#TimeZone').val(fixtzname);
        $('#form_div').dialog('open');
    });

    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            vStatusSaving = 1;//edit data mode
            $.ajax({
                dataType: "json",
                url: base_url + vModul + "/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.Errors) != '{}') {
                            var error = '';
                            for (var key in result.Errors) {
                                error = error + "<br>" + key + " " + result.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $("#form_btn_save").data('kode', id);
                            $('#id').val(result.Id);
                            $('#CompanyInfoId').val(result.CompanyInfoId);
                            $('#CompanyInfoName').val(result.CompanyInfo);
                            $('#MachineNumber').val(result.MachineNumber);
                            $('#MachineName').val(result.MachineName);
                            $('#CommType')[0].selectedIndex = result.CommType;
                            $('#Password').val(result.Password);
                            $('#EthernetIP').val(result.EthernetIP);
                            $('#EthernetPort').val(result.EthernetPort);
                            $('#SerialPort').val(result.SerialPort);
                            $('#TimeZone').val(result.TimeZone);
                            $('#TimeZoneOffset').val(result.TimeZoneOffset);
                            $('#SerialBaudRate').val(result.SerialBaudRate);
                            document.getElementById('IsClear').checked = result.IsClearLogAfterDownload;
                            //$('#Website').val(result.Website);
                            $("#form_div").dialog("open");
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_connect').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            //$('#connect_confirm_btn_submit').data('Id', ret['ID']);
            //$("#connect_confirm_div").dialog("open");

            $.ajax({
                dataType: "json",
                url: base_url + vModul + "/Connect?Id=" + id,
                success: function (result) {
                    if (result.model.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.model.Errors) != '{}') {
                            var error = '';
                            for (var key in result.model.Errors) {
                                error = error + "<br>" + (key != "Generic" ? key : "") + " " + result.model.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $('#id').val(result.model.Id);
                        }
                        ReloadGrid();
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_disconnect').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            //$('#connect_confirm_btn_submit').data('Id', ret['ID']);
            //$("#connect_confirm_div").dialog("open");

            $.ajax({
                dataType: "json",
                url: base_url + vModul + "/Disconnect?Id=" + id,
                success: function (result) {
                    if (result.model.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.model.Errors) != '{}') {
                            var error = '';
                            for (var key in result.model.Errors) {
                                error = error + "<br>" + (key != "Generic" ? key : "") + " " + result.model.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $('#id').val(result.model.Id);
                        }
                        ReloadGrid();
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_downloadlog').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            //$('#connect_confirm_btn_submit').data('Id', ret['ID']);
            //$("#connect_confirm_div").dialog("open");

            $.ajax({
                dataType: "json",
                url: base_url + vModul + "/DownloadLog?Id=" + id,
                success: function (result) {
                    if (result.model.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.model.Errors) != '{}') {
                            var error = '';
                            for (var key in result.model.Errors) {
                                error = error + "<br>" + (key != "Generic" ? key : "") + " " + result.model.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $('#id').val(result.model.Id);
                        }
                        ReloadGrid();
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_download').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#dnloaduser_confirm_btn_submit').data('Id', ret['ID']);
            $("#dnloaduser_confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#dnloaduser_confirm_btn_cancel').click(function () {
        $('#dnloaduser_confirm_btn_submit').val('');
        $("#dnloaduser_confirm_div").dialog('close');
    });

    $('#dnloaduser_confirm_btn_submit').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            //$('#connect_confirm_btn_submit').data('Id', ret['ID']);
            //$("#connect_confirm_div").dialog("open");
            $("#dnloaduser_confirm_div").dialog('close');
            $.ajax({
                dataType: "json",
                url: base_url + vModul + "/Download?Id=" + id,
                success: function (result) {
                    if (result.model.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.model.Errors) != '{}') {
                            var error = '';
                            for (var key in result.model.Errors) {
                                error = error + "<br>" + (key != "Generic" ? key : "") + " " + result.model.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $('#id').val(result.model.Id);
                        }
                        ReloadGrid();
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_upload').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#uploaduser_confirm_btn_submit').data('Id', ret['ID']);
            $("#uploaduser_confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#uploaduser_confirm_btn_cancel').click(function () {
        $('#uploaduser_confirm_btn_submit').val('');
        $("#uploaduser_confirm_div").dialog('close');
    });

    $('#uploaduser_confirm_btn_submit').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            //$('#connect_confirm_btn_submit').data('Id', ret['ID']);
            //$("#connect_confirm_div").dialog("open");
            $("#uploaduser_confirm_div").dialog('close');
            $.ajax({
                dataType: "json",
                url: base_url + vModul + "/Upload?Id=" + id,
                success: function (result) {
                    if (result.model.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.model.Errors) != '{}') {
                            var error = '';
                            for (var key in result.model.Errors) {
                                error = error + "<br>" + (key != "Generic" ? key : "") + " " + result.model.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $('#id').val(result.model.Id);
                        }
                        ReloadGrid();
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_sync').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            //$('#connect_confirm_btn_submit').data('Id', ret['ID']);
            //$("#connect_confirm_div").dialog("open");

            $.ajax({
                dataType: "json",
                url: base_url + vModul + "/Sync?Id=" + id,
                success: function (result) {
                    if (result.model.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.model.Errors) != '{}') {
                            var error = '';
                            for (var key in result.model.Errors) {
                                error = error + "<br>" + (key != "Generic" ? key : "") + " " + result.model.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $('#id').val(result.model.Id);
                        }
                        ReloadGrid();
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_del').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#delete_confirm_btn_submit').data('Id', ret['ID']);
            $("#delete_confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#delete_confirm_btn_cancel').click(function () {
        $('#delete_confirm_btn_submit').val('');
        $("#delete_confirm_div").dialog('close');
    });

    $('#delete_confirm_btn_submit').click(function () {
        $("#delete_confirm_div").dialog('close');
        $.ajax({
            url: base_url + vModul + "/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#delete_confirm_btn_submit').data('Id'),
            }),
            success: function (result) {
                if (JSON.stringify(result.model.Errors) != '{}') {
                    for (var key in result.model.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.model.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.model.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.model.Errors[key], 'warning');
                        }
                    }
                }
                else {
                    ReloadGrid();
                }
            }
        });
    });

    $('#form_btn_cancel').click(function () {
        vStatusSaving = 0;
        clearForm('#frm');
        $("#form_div").dialog('close');
    });

    $("#form_btn_save").click(function () {



        ClearErrorMessage();

        var submitURL = '';
        var id = $("#form_btn_save").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + vModul + '/Update';
        }
            // Insert
        else {
            submitURL = base_url + vModul + '/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, CompanyInfoId: $("#CompanyInfoId").val(), //$("#parenttype").val(),
                MachineNumber: $("#MachineNumber").val(), MachineName: $("#MachineName").val(),
                Password: $("#Password").val(), CommType: $("#CommType").val(),
                EthernetIP: $("#EthernetIP").val(), EthernetPort: $("#EthernetPort").val(),
                SerialPort: $("#SerialPort").val(), SerialBaudRate: $("#SerialBaudRate").val(),
                TimeZone: $("#TimeZone").val(), TimeZoneOffset: $("#TimeZoneOffset").val(),
                IsClearLogAfterDownload: document.getElementById("IsClear").checked ? 'true' : 'false',
            }),
            async: false,
            cache: false,
            timeout: 30000,
            error: function () {
                return false;
            },
            success: function (result) {
                if (JSON.stringify(result.model.Errors) != '{}') {
                    for (var key in result.model.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.model.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.model.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.model.Errors[key], 'warning');
                        }
                    }
                }
                else {
                    ReloadGrid();
                    $("#form_div").dialog('close')
                }
            }
        });
    });

    // -------------------------------------------------------Look Up CompanyInfo-------------------------------------------------------
    $('#btnCompanyInfo').click(function () {
        var lookUpURL = base_url + 'CompanyInfo/GetList';
        var lookupGrid = $('#lookup_table_companyinfo');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_companyinfo').dialog('open');
    });

    jQuery("#lookup_table_companyinfo").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Name', 'Address', 'City', 'Postal Code'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'name', index: 'name', width: 180 },
                  { name: 'address', index: 'address', width: 250 },
                  { name: 'city', index: 'city', width: 100 },
                  { name: 'postalcode', index: 'postalcode', width: 100 },
        ],
        page: '1',
        pager: $('#lookup_pager_companyinfo'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_companyinfo").width() - 10,
        height: $("#lookup_div_companyinfo").height() - 110,
        ondblClickRow: function (rowid) {
            $("#lookup_btn_add_companyinfo").trigger("click");
        },
    });
    $("#lookup_table_companyinfo").jqGrid('navGrid', '#lookup_toolbar_companyinfo', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_companyinfo').click(function () {
        $('#lookup_div_companyinfo').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_companyinfo').click(function () {
        var id = jQuery("#lookup_table_companyinfo").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_companyinfo").jqGrid('getRowData', id);

            $('#CompanyInfoId').val(ret.id).data("kode", id);
            $('#CompanyInfoCode').val(ret.code);
            $('#CompanyInfoName').val(ret.name);
            $('#lookup_div_companyinfo').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });
    // ---------------------------------------------End Lookup CompanyInfo----------------------------------------------------------------

}); //END DOCUMENT READY