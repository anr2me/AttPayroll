$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        // Clear Search string jqGrid
        //$('input[id*="gs_"]').val("");

        var id = $('#parenttype option:selected').text();
        var value = $('#parenttype').val();

        $("#list").setGridParam({ url: base_url + 'EmployeeAttendance/GetList', postData: { filters: null, ParentId: value, findDate: null }, page: '1' }).trigger("reloadGrid");
    }

    function ReloadGridByDate() {
        // Clear Search string jqGrid
        //$('input[id*="gs_"]').val("");

        var id = $('#parenttype option:selected').text();
        var value = $('#parenttype').val();
        var attDate = $('#findAttendanceDate').datebox('getValue');

        $("#list").setGridParam({ url: base_url + 'EmployeeAttendance/GetList', postData: { filters: null, ParentId: value, findDate: attDate }, page: '1' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#NIK').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#TitleInfoId').val('');
        $('#TitleInfoName').val('');
        $('#Remark').val('');
        document.getElementById("Shift").selectedIndex = 0;
        document.getElementById("Status").selectedIndex = 0;
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#parenttype").live("change", function () {
        ReloadGridByDate();
    });

    $('#branchofficetype').change(function () {
        var e = $('#parenttype');
        $(e).empty();
        branchID = $(this).val();
        $.ajax({
            dataType: "json",
            url: base_url + 'Department/GetListByBranchOffice?Id=' + branchID,
            async: false,
            success: function (result) {
                var objDropDown = $('#departmenttype'); // $(this).parent('td').next().find('select');
                $(objDropDown).empty();
                //$(objDropDown).append('<option value="0">--Select Department--</option>');
                if (result != null) {
                    for (var item in result) {
                        $(objDropDown).append('<option value="' + result[item].Id + '">' + result[item].Name + '</option>');
                    }
                }
                var id = $('#departmenttype option:selected').text();
                var value = $('#departmenttype').val();

                //ReloadGrid(); // Must be inside success! otherwise will cause error on BlockUI.js (trying to access blocked view?)
                $("#departmenttype").trigger("change");
            }
        });

    });

    $('#departmenttype').change(function () {
        deptID = $(this).val();
        $.ajax({
            dataType: "json",
            url: base_url + 'Division/GetListByDepartment?Id=' + deptID,
            async: false,
            success: function (result) {
                var objDropDown = $('#parenttype'); // $(this).parent('td').next().find('select');
                $(objDropDown).empty();
                //$(objDropDown).append('<option value="0">--Select Department--</option>');
                if (result != null) {
                    for (var item in result) {
                        $(objDropDown).append('<option value="' + result[item].Id + '">' + result[item].Name + '</option>');
                    }
                }
                var id = $('#parenttype option:selected').text();
                var value = $('#parenttype').val();

                //ReloadGrid(); // Must be inside success! otherwise will cause error on BlockUI.js (trying to access blocked view?)
                $("#parenttype").trigger("change");
            }
        });

    });

    // set options globaly
    $.timeEntry.setDefaults({ show24Hours: true, spinnerImage: 'Content/spinnerText.png', spinnerSize: [30, 20, 8] }); // , spinnerBigImage: 'Content/spinnerTextBig.png', spinnerBigSize: [60, 40, 16]
    $('#CheckIn').timeEntry();
    $('#CheckOut').timeEntry();
    $('#BreakIn').timeEntry();
    $('#BreakOut').timeEntry();

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $('#lookup_div_employee').dialog('close');
    $('#lookup_div_division').dialog('close');
    $('#lookup_div_titleinfo').dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'EmployeeAttendance/GetList',
        postData: { 'ParentId': function () { return $("#parenttype").val(); } },
        datatype: "json",
        colNames: ['ID', 'Employee ID', 'NIK', 'Name', 'Title', 'Division', 'Department', 'Branch Office', 'Attendance Date', 'Shift', 'Status', 'CheckIn', 'BreakOut', 'BreakIn', 'CheckOut', 'Remark', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'employeeid', index: 'employeeid', width: 50, hidden:true },
                  { name: 'employeenik', index: 'employeenik', width: 100 },
				  { name: 'employeename', index: 'employeename', width: 180 },
                  { name: 'title', index: 'title', width: 100 },
                  { name: 'division', index: 'division', width: 100 },
                  { name: 'department', index: 'department', width: 100 },
                  { name: 'branchoffice', index: 'branchoffice', width: 100 },
                  { name: 'attendancedate', index: 'attendancedate', search: false, width: 120, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'shift', index: 'shift', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Shift") } },
                  { name: 'status', index: 'status', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Status") } },
                  { name: 'checkin', index: 'checkin', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'H:i' } },
                  { name: 'breakout', index: 'breakout', hidden:true, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'H:i' } },
                  { name: 'breakin', index: 'breakin', hidden:true, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'H:i' } },
                  { name: 'checkout', index: 'checkout', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'H:i' } },
                  { name: 'remark', index: 'remark', width: 250 },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
        height: $(window).height() - 220,
        ondblClickRow: function (rowid) {
            $("#btn_edit").trigger("click");
        },
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowShift = document.getElementById("Shift").options[$(this).getRowData(cl).shift].text;
		          $(this).jqGrid('setRowData', ids[i], { shift: rowShift });

		          rowStatus = document.getElementById("Status").options[$(this).getRowData(cl).status].text;
		          $(this).jqGrid('setRowData', ids[i], { status: rowStatus });
		      }
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

    $('#btn_find').click(function () {
        ReloadGridByDate();
    });

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
        var e = $('#parenttype option:selected');
        $('#DivisionId').val(e.val());
        $('#DivisionName').val(e.text());
        $('#CheckIn').val('00:00'); //datebox('setValue', timeEnt(result.CheckIn));
        $('#CheckOut').val('00:00');
        $('#BreakIn').val('00:00');
        $('#BreakOut').val('00:00');
        $('#AttendanceDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        vStatusSaving = 0; //add data mode	
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
                url: base_url + "EmployeeAttendance/GetInfo?Id=" + id,
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
                            $('#TitleInfoName').val(result.Title);
                            $('#DivisionName').val(result.Division);
                            $('#Remark').val(result.Remark);
                            $('#CheckIn').val(timeEnt(result.CheckIn)); //datebox('setValue', timeEnt(result.CheckIn));
                            $('#BreakOut').val(timeEnt(result.BreakOut));
                            $('#BreakIn').val(timeEnt(result.BreakIn));
                            $('#CheckOut').val(timeEnt(result.CheckOut));
                            $('#AttendanceDate').datebox('setValue', dateEnt(result.AttendanceDate));
                            document.getElementById("Shift").selectedIndex = result.Shift;
                            document.getElementById("Status").selectedIndex = result.Status;
                            $("#form_div").dialog("open");
                        }
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
            url: base_url + "EmployeeAttendance/Delete",
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
            submitURL = base_url + 'EmployeeAttendance/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'EmployeeAttendance/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, EmployeeId: $("#EmployeeId").val(), AttendanceDate: $('#AttendanceDate').datebox('getValue'),
                Shift: document.getElementById("Shift").selectedIndex, Status: document.getElementById("Status").selectedIndex,
                CheckIn: $("#CheckIn").val(), CheckOut: $("#CheckOut").val(),
                BreakIn: $("#BreakIn").val(), BreakOut: $("#BreakOut").val(),
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
                  { name: 'title', index: 'title', width: 150 },
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
            $('#lookup_div_employee').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });
    // ---------------------------------------------End Lookup Employee----------------------------------------------------------------

    // -------------------------------------------------------Look Up Division-------------------------------------------------------
    $('#btnDivision').click(function () {
        var lookUpURL = base_url + 'Division/GetList';
        var lookupGrid = $('#lookup_table_division');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_division').dialog('open');
    });

    jQuery("#lookup_table_division").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Code', 'Name', 'Description', 'Department ID', 'Department Code', 'Department Name'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'code', index: 'code', width: 100 },
				  { name: 'name', index: 'name', width: 180 },
                  { name: 'description', index: 'description', width: 100 },
				  { name: 'departmentid', index: 'departmentid', width: 180 },
                  { name: 'departmentcode', index: 'departmentcode', width: 100 },
				  { name: 'departmentname', index: 'departmentname', width: 180 },
        ],
        page: '1',
        pager: $('#lookup_pager_division'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_division").width() - 10,
        height: $("#lookup_div_division").height() - 110,
        ondblClickRow: function (rowid) {
            $("#lookup_btn_add_division").trigger("click");
        },
    });
    $("#lookup_table_division").jqGrid('navGrid', '#lookup_toolbar_division', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_division').click(function () {
        $('#lookup_div_division').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_division').click(function () {
        var id = jQuery("#lookup_table_division").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_division").jqGrid('getRowData', id);

            $('#DivisionId').val(ret.id).data("kode", id);
            $('#DivisionCode').val(ret.code);
            $('#DivisionName').val(ret.name);
            $('#lookup_div_division').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });
    // ---------------------------------------------End Lookup Division----------------------------------------------------------------


    // -------------------------------------------------------Look Up TitleInfo-------------------------------------------------------
    $('#btnTitleInfo').click(function () {
        var lookUpURL = base_url + 'TitleInfo/GetList';
        var lookupGrid = $('#lookup_table_titleinfo');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_titleinfo').dialog('open');
    });

    function cboxIsShiftable(cellvalue, options, rowObject) {
        return '<input name="cbIsEnabled" disabled rel="' + /*rowObject.id*/options.rowId + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    function cboxIsSalaryAllIn(cellvalue, options, rowObject) {
        return '<input name="cbIsSalaryAllIn" disabled rel="' + /*rowObject.id*/options.rowId + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    jQuery("#lookup_table_titleinfo").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Code', 'Name', 'Description', 'Shiftable', 'Salary All In', 'Access Level', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'code', index: 'code', width: 100 },
				  { name: 'name', index: 'name', width: 180 },
                  { name: 'description', index: 'description', width: 250 },
                  {
                      name: 'isshiftable', index: 'isshiftable', width: 80, align: 'center', search: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxIsShiftable, formatoptions: { disabled: false }
                  },
                  {
                      name: 'issalaryallin', index: 'issalaryallin', width: 80, align: 'center', search: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxIsSalaryAllIn, formatoptions: { disabled: false }
                  },
                  { name: 'accesslevel', index: 'accesslevel', width: 100 },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#lookup_pager_titleinfo'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_titleinfo").width() - 10,
        height: $("#lookup_div_titleinfo").height() - 110,
        ondblClickRow: function (rowid) {
            $("#lookup_btn_add_titleinfo").trigger("click");
        },
    });
    $("#lookup_table_titleinfo").jqGrid('navGrid', '#lookup_toolbar_titleinfo', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_titleinfo').click(function () {
        $('#lookup_div_titleinfo').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_titleinfo').click(function () {
        var id = jQuery("#lookup_table_titleinfo").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_titleinfo").jqGrid('getRowData', id);

            $('#TitleInfoId').val(ret.id).data("kode", id);
            $('#TitleInfoName').val(ret.name);
            $('#lookup_div_titleinfo').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup TitleInfo----------------------------------------------------------------

    
}); //END DOCUMENT READY