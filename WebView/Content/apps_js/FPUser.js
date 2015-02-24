$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
    var vModul = 'FPUser';

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
                    newData['FPUser ID'] = newData.FPUserId;
                    newData['FPUser PIN'] = newData.FPUserPIN;
                    newData['FPUser Name'] = newData.FPUser;
                    newData['Finger ID'] = newData.FingerID;
                    newData['InSync'] = newData.IsInSync;
                    newData['Created At'] = newData.CreatedAt;
                    newData['Updated At'] = newData.UpdatedAt;
                    //var newId = 'Id'; //jqGetNextId($());
                    jQuery("#listdetail").jqGrid('addRowData', newData.Id, newData);
                }
            }
        };

        feedback.client.delRecord = function (id, modelstr) {
            // Add the message to the page. 
            var parentid = jQuery("#list").jqGrid('getGridParam', 'selrow');
            if (id == parentid) {
                var newData = jQuery.parseJSON(modelstr);
                if (newData != null) {
                    jQuery("#listdetail").jqGrid('delRowData', newData.Id);
                }
            }
        };
        // Start the connection.
        $.connection.hub.start().done(function () {
            // Call the Send method on the hub, ensures that the connection is established before the event handler executes
            //feedback.server.sendRow(null, null);
        });
    });

    function ReloadGrid() {
        // Clear Search string jqGrid
        //$('input[id*="gs_"]').val("");

        var text = $('#parenttype option:selected').text();
        var value = $('#parenttype').val();

        $("#list").setGridParam({ url: base_url + vModul + '/GetList', postData: { filters: null, ParentId: value } }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'FPTemplate/GetList?ParentId=' + $("#id").val(), postData: { filters: null } }).trigger("reloadGrid");
    }

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ClearData() {
        $('#NIK').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        ClearErrorMessage();
    }

    $("#parenttype").live("change", function () {
        ReloadGrid();
    });

    //unused here
    $("#datehistory").change(function () {
        var text = $('#datehistory option:selected').text();
        var id = $('#datehistory').val();

        //ReloadGridDetail();
        var curDate = new Date($('#EnrollmentDate').datebox('getValue'));
        var selid = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (selid) {
            var ret = jQuery("#list").jqGrid('getRowData', selid);
            curDate = new Date(ret.enrollmentdate);
        }

        
        var selDate = new Date(text);
        var isactive = (selDate >= curDate);

        //document.getElementById("EnrollmentDate").disabled = !isactive;
        document.getElementById("btnEmployee").disabled = !isactive;

        if (isactive) {
            $('#btn_edit_detail').removeClass('ui-state-disabled disabled');
            $('#form_btn_save').removeClass('ui-state-disabled disabled');
            $('#form_btn_del').removeClass('ui-state-disabled disabled');
            $('#EnrollmentDate').removeClass('ui-state-disabled disabled');
            $('#detail_btn_submit').removeClass('ui-state-disabled disabled');
            //$('#btnEmployee').removeAttr('disabled');
            //$('#EnrollmentDate').removeAttr('data-options');
        } else {
            $('#btn_edit_detail').addClass('ui-state-disabled disabled');
            $('#form_btn_save').addClass('ui-state-disabled disabled');
            $('#form_btn_del').addClass('ui-state-disabled disabled');
            $('#EnrollmentDate').addClass('ui-state-disabled disabled');
            $('#detail_btn_submit').addClass('ui-state-disabled disabled');
            //$('#btnEmployee').attr('disabled', true);
            //$('#EnrollmentDate').attr('data-options', '{"mode":"calbox", "hideInput": "true", "hideButton": "true", "useInline": "true"}');
        }

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
                        $('#EmployeeId').val(result.EmployeeId);
                        $('#EmployeeNIK').val(result.EmployeeNIK);
                        $('#EmployeeName').val(result.EmployeeName);
                        $('#Description').val(result.Description);
                        $('#EnrollmentDate').datebox('setValue', dateEnt(result.EnrollmentDate));
                        $('#EnrollmentDate2').val(dateEnt(result.EnrollmentDate));
                        if (isactive) {
                            $('#Description').removeAttr('disabled');
                            $('#EnrollmentDateDiv2').hide();
                            $('#EnrollmentDateDiv').show();
                        } else {
                            $('#Description').attr('disabled', true);
                            $('#EnrollmentDateDiv2').show();
                            $('#EnrollmentDateDiv').hide();
                        }
                        ReloadGridDetail();
                    }
                }
            }
        });
    });

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#lookup_div_employee").dialog('close');
    $("#detail_div").dialog('close');

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

        isEnabled = $("input[name=cbFormat_isenabled" + objId + "]").is(":checked");

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + vModul + "/UpdateEnable?isEnabled=" + isEnabled,
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
    //    privilege: function (cellvalue, options, rowdata) {
    //        return document.getElementById('Privilege').options[cellvalue].text;
    //    },
    //    verifymode: function (cellvalue, options, rowdata) {
    //        return document.getElementById('VerifyMode').options[cellvalue].text;
    //    }
    //});

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + vModul + '/GetList',
        postData: { 'ParentId': function () { return $("#parenttype").val(); } },
        datatype: "json",
        //colNames: ['ID', 'Employee ID', 'Employee NIK', 'Employee Name', 'Employee Title', 'Enrollment Date', 'Remark', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'ID', index: 'id', width: 60, align: "center" },
                  { name: 'Employee ID', index: 'employeeid', width: 100, hidden: true },
                  { name: 'Employee NIK', index: 'employeenik', width: 100 },
                  { name: 'Employee Name', index: 'employeename', width: 150 },
                  //{ name: 'User ID', index: 'userid', width: 60 },
                  { name: 'User ID', index: 'pin', width: 60 },
                  { name: 'Tag', index: 'pin2', width: 60 },
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
        $('#Description').val('').text('').removeClass('errormessage');
        document.getElementById('IsEnabled').checked = true;
        $('#Group').val(1);
        $('#TimeZones').val('1::');
        $('#VerifyMode').val('-1');
        $('#EnrollmentDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#EnrollmentDateDiv2').hide();
        $('#EnrollmentDateDiv').show();
        $('#PIN').attr('disabled', true);
        $('#btnEmployee').removeAttr('disabled');
        $('#Description').removeAttr('disabled');
        $('#btn_edit_detail').removeClass('ui-state-disabled disabled');
        $('#form_btn_save').removeClass('ui-state-disabled disabled');
        $('#EnrollmentDate').removeClass('ui-state-disabled disabled');
        $('#detail_btn_submit').removeClass('ui-state-disabled disabled');
        //$('#form_btn_del').addClass('ui-state-disabled disabled');
        $('#form_btn_save').data('kode', '');
        $('#form_btn_save').show();
        $('#form_btn_del').hide();
        $('#tabledetail_div').hide();
        vStatusSaving = 0; //add data mode	
        $('#form_div').dialog('open');
    });

    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        //fillData();
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
                            $("#datehistory").data('kode', result.Id); // .toString()
                            $('#id').val(result.Id);
                            $('#EmployeeId').val(result.EmployeeId);
                            $('#EmployeeNIK').val(result.EmployeeNIK);
                            $('#EmployeeName').val(result.EmployeeName);
                            $('#Remark').val(result.Remark);
                            $('#PIN').val(result.PIN);
                            $('#PIN2').val(result.PIN2);
                            $('#PIN').val(result.PIN);
                            $('#Privilege')[0].selectedIndex = result.Privilege;
                            $('#Name').val(result.Name);
                            $('#Password').val(result.Password);
                            $('#Card').val(result.Card);
                            $('#Group').val(result.Group);
                            $('#TimeZones').val(result.TimeZones);
                            $('#VerifyMode').val(result.VerifyMode.toString());//$('#VerifyMode')[0].selectedIndex = result.VerifyMode;
                            document.getElementById('IsEnabled').checked = result.IsEnabled;
                            $('#Description').removeAttr('disabled');
                            $('#btnEmployee').removeAttr('disabled');
                            $('#EnrollmentDate').datebox('setValue', dateEnt(result.EnrollmentDate));
                            $('#btn_edit_detail').removeClass('ui-state-disabled disabled');
                            $('#form_btn_save').removeClass('ui-state-disabled disabled');
                            //$('#form_btn_del').removeClass('ui-state-disabled disabled');
                            $('#EnrollmentDate').removeClass('ui-state-disabled disabled');
                            $('#detail_btn_submit').removeClass('ui-state-disabled disabled');
                            $('#EnrollmentDateDiv2').hide();
                            $('#EnrollmentDateDiv').show();
                            $('#tabledetail_div').hide();
                            $('#form_btn_save').show();
                            //$('#form_btn_del').show();
                            $('#tabledetail_div').show();
                            ReloadGridDetail();
                            $("#form_div").dialog("open");
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#form_btn_del').click(function () {
        //clearForm("#frm");

        var id = $('#id').val(); // jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            //var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#delete_confirm_btn_submit').data('Id', id);
            $("#delete_confirm_div").dialog("open");
            $("#form_div").dialog('close');
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
        $.ajax({
            url: base_url + vModul + "/Delete",
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
                    ReloadGrid();
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
                Id: id, Remark: $("#Remark").val(), EmployeeId: $("#EmployeeId").val(),
                PIN: $("#PIN").val(), PIN2: $("#PIN2").val(), Privilege: $("#Privilege").val(),
                Name: $("#Name").val(), Password: $("#Password").val(), Card: $("#Card").val(),
                Group: $("#Group").val(), TimeZones: $("#TimeZones").val(),
                IsEnabled: document.getElementById("IsEnabled").checked ? 'true' : 'false',
                VerifyMode: $("#VerifyMode").val()
                //EnrollmentDate: $("#EnrollmentDate").datebox('getValue')
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
                    ReloadGrid();
                    $("#form_div").dialog('close')
                }
            }
        });
    });

    //GRID Detail+++++++++++++++
    $("#listdetail").jqGrid({
        url: base_url,
        datatype: "json",
        //colNames: ['FPUser ID', 'PIN', 'Password'],
        colModel: [
                  { name: 'ID', index: 'id', hidden: true, width: 60, sortable: true },
                  { name: 'PIN', index: 'pin', hidden: true, width: 60, sortable: true },
				  { name: 'FPUser ID', index: 'fpuserid', hidden: true, width: 60, sortable: true },
                  { name: 'FPUser PIN', index: 'fpuserpin', hidden: true, width: 70, sortable: true },
                  { name: 'FPUser Name', index: 'fpuser', hidden: true, width: 100, sortable: true },
                  { name: 'Finger ID', index: 'fingerid', width: 60, sortable: true },
                  { name: 'Valid', index: 'valid', width: 60, sortable: true },
                  { name: 'InSync', index: 'isinsync', width: 60, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption(null) } },
                  { name: 'Size', index: 'size', width: 60, sortable: true },
				  { name: 'Template', index: 'template', width: 255, sortable: true },
                  { name: 'Created At', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'Updated At', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#pagerdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'Id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $('#form_div').width() - 2, //$(window).width() - 800,
        height: $(window).height() - 600,
        ondblClickRow: function (rowid) {
            $("#btn_edit_detail").trigger("click");
        },
        gridComplete:
		  function () {
		  }
    });//END GRID Detail
    $("#listdetail").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: false })
	                .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    $('#btn_add_detail').click(function () {
        ClearData();
        clearForm('#frm');
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
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
                            $("#form_btn_save").data('kode', result.Id);
                            $('#id').val(result.Id);
                            $('#EmployeeId').val(result.EmployeeId);
                            $('#EmployeeNIK').val(result.EmployeeNIK);
                            $('#EmployeeName').val(result.EmployeeName);
                            $('#Description').val(result.Description);
                            $('#EnrollmentDate').datebox('setValue', dateEnt(result.EnrollmentDate));
                            $('#EnrollmentDate2').val(dateEnt(result.EnrollmentDate));
                            $('#EnrollmentDateDiv').hide();
                            $('#EnrollmentDateDiv2').show();
                            $('#Description').attr('disabled', true);
                            $('#btnEmployee').attr('disabled', true);
                            $('#form_btn_save').hide();
                            $('#tabledetail_div').show();
                            ReloadGridDetail();
                            $('#form_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_add_new_detail').click(function () {
        ClearData();
        clearForm('#detail_div');
        $("#detail_btn_submit").data('kode', '');
        $('#Amount').numberbox('setValue', '');
        $('#detail_div').dialog('open');
    });

    $('#btn_edit_detail').click(function () {
        ClearData();
        clearForm("#detail_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "FPTemplate/GetInfo?Id=" + id,
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
                            $("#detail_btn_submit").data('kode', result.Id);
                            $('#SalaryItemId').val(result.SalaryItemId);
                            $('#SalaryItemName').val(result.SalaryItemName);
                            $('#Amount').numberbox('setValue', result.Amount);
                            $('#detail_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_del_detail').click(function () {
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#listdetail").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to delete record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "FPTemplate/Delete",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({
                            Id: id,
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
                            else {
                                ReloadGridDetail();
                                ReloadGrid();
                                $("#delete_confirm_div").dialog('close');
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });
    //--------------------------------------------------------Dialog Detail-------------------------------------------------------------
    // detail_btn_submit

    $("#detail_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#detail_btn_submit").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'FPTemplate/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'FPTemplate/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, FPUserId: $("#id").val(), PIN: $("#PIN").val(), FingerID: $("#FingerID").val(), Template: $("#FPTemplate").val(),
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
                    ReloadGridDetail();
                    ReloadGrid();
                    $("#detail_div").dialog('close')
                }
            }
        });
    });


    // item_btn_cancel
    $('#detail_btn_cancel').click(function () {
        clearForm('#detail_div');
        $("#detail_div").dialog('close');
    });
    //--------------------------------------------------------END Dialog Detail-------------------------------------------------------------


    // -------------------------------------------------------Look Up Employee-------------------------------------------------------
    $('#btnEmployee').click(function () {
        var lookUpURL = base_url + 'Employee/GetList';
        var lookupGrid = $('#lookup_table_employee');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_employee').dialog('open');
    });

    jQuery("#lookup_table_employee").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'NIK', 'Name', 'Title'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'nik', index: 'nik', width: 100 },
				  { name: 'name', index: 'name', width: 180 },
                  { name: 'titleinfoname', index: 'titleinfoname', width: 150 },
        ],
        page: '1',
        pager: $('#lookup_pager_employee'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_employee").width() - 10,
        height: $("#lookup_div_employee").height() - 110,
        ondblClickRow: function (rowid) {
            $("#lookup_btn_add_employee").trigger("click");
        },
    });
    $("#lookup_table_employee").jqGrid('navGrid', '#lookup_toolbar_employee', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_employee').click(function () {
        $('#lookup_div_employee').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_employee').click(function () {
        var id = jQuery("#lookup_table_employee").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_employee").jqGrid('getRowData', id);

            $('#EmployeeId').val(ret.id).data("kode", id);
            $('#EmployeeNIK').val(ret.nik);
            $('#EmployeeName').val(ret.name);
            $('#PIN2').val(ret.nik);
            $('#Name').val(ret.name);
            $('#lookup_div_employee').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });
    // ---------------------------------------------End Lookup Employee----------------------------------------------------------------

    
}); //END DOCUMENT READY