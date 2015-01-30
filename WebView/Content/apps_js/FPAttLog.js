$(document).ready(function () {
    var vModulChild = 'FPAttLog';
    var vModulParent = 'FPUser';
    var a;
    $("#detail_panel").hide();

    //<!--SignalR script to update the chat page and send messages.-->
    $(function () {
        // Reference the auto-generated proxy for the hub.

        var feedback = $.connection.feedbackHub;
        // Create a function that the hub can call back to display messages.
        feedback.client.addNewRecord = function (id, modelstr) {
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
                    jQuery("#list_detail").jqGrid('addRowData', newData.Id, newData);
                    $("#listdetail").setGridParam({ datatype: "local", url: base_url + vModulChild + '/GetListFPAttLog?Id=' + id }).trigger("reloadGrid");
                    $("#listdetail").setGridParam({ datatype: "json", url: base_url + vModulChild + '/GetListFPAttLog?Id=' + id });
                }
            }
        };
        // Start the connection.
        $.connection.hub.start().done(function () {
                // Call the Send method on the hub, ensures that the connection is established before the event handler executes
                //feedback.server.sendRow(null, null);
        });
    });

    //$("#parenttype").live("change", function () {
    //    //var Id = $(this).val();
    //    var Id = jQuery("#list").jqGrid('getGridParam', 'selrow');
    //    ReloadGridViewChildren(Id);
    //});

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ClearData() {
        $('#Code').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#IsEnabled').val('');
        $('#CheckInTolerance').numberbox('setValue', '');
        $('#CheckOutTolerance').numberbox('setValue', '');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }


    // set options globaly
    $.timeEntry.setDefaults({ show24Hours: true, spinnerImage: 'Content/spinnerText.png', spinnerSize: [30, 20, 8] }); // , spinnerBigImage: 'Content/spinnerTextBig.png', spinnerBigSize: [60, 40, 16]
    $('#MinCheckIn').timeEntry();
    $('#CheckIn').timeEntry();
    $('#MaxCheckIn').timeEntry();
    $('#MinCheckOut').timeEntry();
    $('#CheckOut').timeEntry();
    $('#MaxCheckOut').timeEntry();
    $('#BreakIn').timeEntry();
    $('#BreakOut').timeEntry();

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');

    function cboxFormat(cellvalue, options, rowObject) {
        return '<input name="cbFormat_' + options.colModel.index + options.rowId + '" rel="' + /*rowObject.id*/options.rowId + '" type="checkbox" disabled' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    function ReloadGridListParent() {
        $("#detail_panel").hide();
        jQuery("#list").jqGrid({
            url: base_url + vModulParent + '/GetList',
            postData: { 'ParentId': function () { return $("#parenttype").val(); } },
            datatype: "json",
            //colNames: ['ID', 'FPUser ID', 'Name', 'Min CheckIn', 'CheckIn', 'Max CheckIn', 'BreakOut', 'BreakIn', 'Min CheckOut', 'CheckOut', 'Max CheckOut', 'CheckIn Tolerance', 'CheckOut Tolerance', 'Work Interval', 'Break Interval', 'Created At', 'Updated At'],
            colModel: [
    			  { name: 'ID', index: 'id', width: 60, align: "center", frozen:true },
                  { name: 'Employee ID', index: 'employeeid', width: 100, hidden: true },
                  { name: 'Employee NIK', index: 'employeenik', width: 100 },
                  { name: 'Employee Name', index: 'employeename', width: 150 },
                  //{ name: 'User ID', index: 'userid', width: 60 },
                  { name: 'User ID', index: 'pin', width: 60 },
                  { name: 'PIN2', index: 'pin2', width: 60 },
                  { name: 'Privilege', index: 'privilege', width: 90, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption("#Privilege") } },
                  { name: 'Name', index: 'name', width: 150 },
                  { name: 'Password', index: 'password', width: 100 },
                  { name: 'Card Number', index: 'card', width: 100 },
                  { name: 'Group', index: 'group', width: 60 },
                  { name: 'Time Zones', index: 'timezones', width: 80 },
                  { name: 'Verification Mode', index: 'verifymode', width: 100, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption("#VerifyMode") } },
                  {
                      name: 'Enabled', index: 'isenabled', width: 80, align: 'center',
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxFormat, formatoptions: { disabled: false },
                      stype: 'select', editoptions: { value: getSelectOption(null) }
                  },
                  { name: 'InSync', index: 'isinsync', width: 60, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption(null) } },
                  { name: 'FP Count', index: 'fpcount', width: 80 },
                  { name: 'Reserved', index: 'reserved', width: 80 },
                  { name: 'Remark', index: 'remark', width: 200 },
                  //{ name: 'Enrollment Date', index: 'enrollmentdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'Created At', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'Updated At', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
            ],
            page: '1',
            pager: jQuery('#pager'),
            rowNum: 20,
            rowList: [20, 30, 60],
            sortname: 'id',
            viewrecords: true,
            scrollrows: true,
            shrinkToFit: false,
            sortorder: "asc",
            width: $("#toolbar").width(),
            height: $(window).height() - 190,
            onSelectRow: function (id) {
                var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
                if (id) {
                    $("#list_detail").jqGrid('GridUnload');
                    $("#detail_panel").show();
                    ReloadGridViewChildren(id);

                } else {
                    $.messager.alert('Information', 'Data Not Found...!!', 'info');
                };
            },
            gridComplete:
              function () {
                  var ids = $(this).jqGrid('getDataIDs');
                  for (var i = 0; i < ids.length; i++) {
                      var cl = ids[i];
                      rowWorkInterval = ($(this).getRowData(cl).workinterval / 60.0).toFixed(1);
                      $(this).jqGrid('setRowData', ids[i], { workinterval: rowWorkInterval });

                      rowBreakInterval = ($(this).getRowData(cl).breakinterval / 60.0).toFixed(1);
                      $(this).jqGrid('setRowData', ids[i], { breakinterval: rowBreakInterval });
                  }
              }

        });//END GRID
        $("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list").jqGrid('setFrozenColumns');
    //    $("#list").setGridParam({ url: base_url + vModulParent + '/GetList', page: '1' }).trigger("reloadGrid");
         }

    // CheckBox
    function UpdateEnable(objId, isEnabled) {
        //var userId = 0;
        //var id = jQuery("#tbl_users").jqGrid('getGridParam', 'selrow');
        //if (id) {
        //    userId = id;
        //} else {
        //    $.messager.alert('Information', 'Please Select User...!!', 'info');
        //    return;
        //};

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + vModulChild + "/UpdateEnable?isEnabled=" + isEnabled,
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

    $("input[name=cbIsEnabled]").live("click", function () {
        UpdateEnable($(this).attr('rel'), $(this).is(":checked"));
    });

    function cboxIsEnabled(cellvalue, options, rowObject) {
        return '<input name="cbIsEnabled" rel="' + /*rowObject.id*/options.rowId + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }
    // END CheckBox

    function ReloadGridViewChildren(id) {
        if (id == 0 || id == null || id == undefined) {
            id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        }
        $("#list_detail").jqGrid({
            url: base_url + vModulChild + '/GetListFPAttLog?Id=' + id,
            //postData: { 'ParentId': function () { return $("#parenttype").val(); } },
            datatype: "json",
            //colNames: ['ID', 'Code', 'Name', 'Enabled', 'Min CheckIn', 'CheckIn', 'Max CheckIn', 'BreakOut', 'BreakIn', 'Min CheckOut', 'CheckOut', 'Max CheckOut', 'CheckIn Tolerance', 'CheckOut Tolerance', 'Work Interval', 'Break Interval', 'Created At', 'Updated At'],
            colModel: [
                      { name: 'ID', index: 'id', width: 80, align: "center", frozen: true },
                      { name: 'PIN', index: 'pin', width: 100, frozen: true, hidden: true },
                      { name: 'PIN2', index: 'pin2', width: 100, frozen: true, hidden:true },
                      { name: 'Date Time', index: 'time_second', search: false, width: 120, align: "center", formatter: 'date', formatoptions: { newformat: 'Y-m-d H:i:s' } },
                      { name: 'Device ID', index: 'deviceid', width: 100, hidden:true },
                      { name: 'Verification Mode', index: 'verifymode', width: 100, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption("#VerifyMode") } },
                      { name: 'Attendance State', index: 'inoutmode', width: 100, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption("#InOutMode") } },
                      { name: 'WorkCode', index: 'workcode', width: 100 },
                      { name: 'Reserved', index: 'reserved', width: 100 },
                      { name: 'Created At', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                      { name: 'Updated At', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
            ],
            page: '1',
            pager: $('#pager_detail'),
            rowNum: 20,
            rowList: [20, 30, 60],
            sortname: 'id',
            viewrecords: true,
            scrollrows: true,
            shrinkToFit: false,
            sortorder: "asc",
            width: $("#detail_toolbar").width(),
            height: $(window).height() - 190,
            ondblClickRow: function (rowid) {
                $("#btn_edit").trigger("click");
            },
            gridComplete:
              function () {
                  var ids = $(this).jqGrid('getDataIDs');
                  for (var i = 0; i < ids.length; i++) {
                      var cl = ids[i];
                      //rowWorkInterval = ($(this).getRowData(cl).workinterval / 60.0).toFixed(1);
                      //$(this).jqGrid('setRowData', ids[i], { workinterval: rowWorkInterval });

                      //rowBreakInterval = ($(this).getRowData(cl).breakinterval / 60.0).toFixed(1);
                      //$(this).jqGrid('setRowData', ids[i], { breakinterval: rowBreakInterval });
                  }
              }

        });//END GRID Detail
        $("#list_detail").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list_detail").jqGrid('setFrozenColumns');
    }

    ReloadGridListParent();

    $('#btn_warehouse').click(function () {
        a = 'warehouse';
        $("#list").jqGrid('GridUnload');
        ReloadGridListParent();
    });
 
    //$('#btn_item').click(function () {
    //    a = 'item';
    //    $("#list").jqGrid('GridUnload');
    //    ReloadGridListItem();
    //});

    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        //if (a == 'item') {
        //    $("#list").jqGrid('GridUnload');
        //    ReloadGridListItem();
        //}
        //else 
        {
            $("#list").jqGrid('GridUnload');
            ReloadGridListParent();
        }
        
    });

    $('#detail_btn_reload').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        //if (a == 'item') {
        //    $("#list_detail").jqGrid('GridUnload');
        //    ReloadGridViewWarehouse(id);
        //}
        //else 
        {
            $("#list_detail").jqGrid('GridUnload');
            ReloadGridViewChildren(id);

        }

    });
    $('#btn_print').click(function () {
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        vStatusSaving = 0; //add data mode	
        $('#form_div').dialog('open');
    });

    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list_detail").jqGrid('getGridParam', 'selrow');
        if (id) {
            vStatusSaving = 1;//edit data mode
            $.ajax({
                dataType: "json",
                url: base_url + vModulChild + "/GetInfo?Id=" + id,
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
                            $('#Code').val(result.Code);
                            $('#Name').val(result.Name);
                            //$('#IsEnabled').val(result.IsEnabled?'1':'0');
                            document.getElementById("IsEnabled").checked = result.IsEnabled;
                            $('#MinCheckIn').val(timeEnt(result.MinCheckIn)); //datebox('setValue', timeEnt(result.MinCheckIn));
                            $('#CheckIn').val(timeEnt(result.CheckIn)); //datebox('setValue', timeEnt(result.CheckIn));
                            $('#MaxCheckIn').val(timeEnt(result.MaxCheckIn));
                            $('#MinCheckOut').val(timeEnt(result.MinCheckOut));
                            $('#CheckOut').val(timeEnt(result.CheckOut));
                            $('#MaxCheckOut').val(timeEnt(result.MaxCheckOut));
                            $('#BreakIn').val(timeEnt(result.BreakIn));
                            $('#BreakOut').val(timeEnt(result.BreakOut));
                            $('#CheckInTolerance').numberbox('setValue', result.CheckInTolerance);
                            $('#CheckOutTolerance').numberbox('setValue', result.CheckOutTolerance);
                            $('#WorkInterval').val((result.WorkInterval / 60.0).toFixed(1)); //numberbox('setValue', result.WorkInterval/60.0);
                            $('#BreakInterval').val((result.BreakInterval / 60.0).toFixed(1)); //numberbox('setValue', result.BreakInterval/60.0);
                            $("#form_div").dialog("open");
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_delete').click(function () {
        clearForm("#frm");

        var id = jQuery("#list_detail").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_detail").jqGrid('getRowData', id);
            $('#delete_confirm_btn_submit').data('Id', ret.id);
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

        $.ajax({
            url: base_url + vModulChild + "/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#delete_confirm_btn_submit').data('Id'),
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
                    $("#delete_confirm_div").dialog('close');
                }
                else {
                    $("#list_detail").jqGrid('GridUnload');
                    ReloadGridViewChildren();
                    $("#delete_confirm_div").dialog('close');
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
            submitURL = base_url + vModulChild + '/Update';
        }
            // Insert
        else {
            submitURL = base_url + vModulChild + '/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Code: $("#Code").val(), Name: $("#Name").val(), IsEnabled: document.getElementById("IsEnabled").checked?'true':'false',
                MinCheckIn: $("#MinCheckIn").val(), CheckIn: $("#CheckIn").val(), MaxCheckIn: $("#MaxCheckIn").val(),
                MinCheckOut: $("#MinCheckOut").val(), CheckOut: $("#CheckOut").val(), MaxCheckOut: $("#MaxCheckOut").val(),
                BreakIn: $("#BreakIn").val(), BreakOut: $("#BreakOut").val(),
                CheckInTolerance: $("#CheckInTolerance").numberbox('getValue'), CheckOutTolerance: $("#CheckOutTolerance").numberbox('getValue'),
            }),
            async: false,
            cache: false,
            timeout: 30000,
            error: function () {
                return false;
            },
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
                else {
                    $("#list_detail").jqGrid('GridUnload');
                    ReloadGridViewChildren();
                    $("#form_div").dialog('close')
                }
            }
        });
    });

    function clearForm(form) {
        $(':input', form).each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase(); // normalize case
            if (type == 'text' || type == 'password' || tag == 'textarea')
                this.value = "";
            else if (type == 'checkbox' || type == 'radio')
                this.checked = false;
            else if (tag == 'select')
                this.selectedIndex = 0;
        });
    }

}); //END DOCUMENT READY