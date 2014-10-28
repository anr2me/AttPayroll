$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ReloadGrid() {
        // Clear Search string jqGrid
        //$('input[id*="gs_"]').val("");

        var text = $('#parenttype option:selected').text();
        var value = $('#parenttype').val();

        $("#list").setGridParam({ url: base_url + 'SalaryEmployee/GetList', postData: { filters: null, ParentId: value }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        var text = $('#datehistory option:selected').text();
        var value = $('#datehistory').val();
        if (value == null) value = 0;

        $("#listdetail").setGridParam({ url: base_url + 'SalaryEmployee/GetListDetail?Id=' + value /*$("#id").val()*/, postData: { filters: null } }).trigger("reloadGrid");
    }

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ClearData() {
        $('#NIK').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        ClearErrorMessage();
    }

    function fillData() {
        //$("#datehistory").data('kode', "0");
        var kode = "0";
        var selid = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (selid) {
            var ret = jQuery("#list").jqGrid('getRowData', selid);
            kode = ret.id;
        }
        $.ajax({
            url: base_url + "SalaryEmployee/GetList",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                filters: null,
                nd: new Date().getTime(), // Date.parse(new Date()),
                rows: 32767,
                page: 1,
                sidx: "EffectiveDate",
                sord: "desc",
                full: true,
            }),
            success: function (result) {
                var objDropDown = $('#datehistory'); // $(this).parent('td').next().find('select');
                $(objDropDown).empty();
                //$(objDropDown).append('<option value="0">--Select Department--</option>');
                //$(objDropDown).append('<tr><th>ID</th><th>Code</th><th>Name</th></tr>');
                if (result != null) {
                    for (var item in result.rows) {
                        $(objDropDown).append('<option value="' + result.rows[item].cell[0] + '">' + dateEnt(result.rows[item].cell[5]) + '</option>');
                        //$(objDropDown).append('<tr><td>' + result.rows[item].cell[0] + '</td><td>' + result.rows[item].cell[1] + '</td><td>' + result.rows[item].cell[2] + '</td></tr>');
                    }
                    var e = document.getElementById('datehistory');
                    e.value = kode;
                    //var kode = $("#datehistory").data('kode');
                    //if (kode) {
                        //e.options[kode].selected = true;
                    //}
                    //for (var item in e.options) {
                    //    if (e.options[item].value == kode) {
                    //        e.options[item].selected = true; break;
                    //    }
                    //}
                }
            }
        });

    }

    $("#parenttype").live("change", function () {
        ReloadGrid();
    });

    $("#datehistory").change(function () {
        var text = $('#datehistory option:selected').text();
        var id = $('#datehistory').val();

        //ReloadGridDetail();
        var curDate = new Date($('#EffectiveDate').datebox('getValue'));
        var selid = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (selid) {
            var ret = jQuery("#list").jqGrid('getRowData', selid);
            curDate = new Date(ret.effectivedate);
        }

        
        var selDate = new Date(text);
        var isactive = (selDate >= curDate);

        //document.getElementById("EffectiveDate").disabled = !isactive;
        document.getElementById("btnEmployee").disabled = !isactive;

        if (isactive) {
            $('#btn_edit_detail').removeClass('ui-state-disabled disabled');
            $('#form_btn_save').removeClass('ui-state-disabled disabled');
            $('#form_btn_del').removeClass('ui-state-disabled disabled');
            $('#EffectiveDate').removeClass('ui-state-disabled disabled');
            $('#detail_btn_submit').removeClass('ui-state-disabled disabled');
            //$('#btnEmployee').removeAttr('disabled');
            //$('#EffectiveDate').removeAttr('data-options');
        } else {
            $('#btn_edit_detail').addClass('ui-state-disabled disabled');
            $('#form_btn_save').addClass('ui-state-disabled disabled');
            $('#form_btn_del').addClass('ui-state-disabled disabled');
            $('#EffectiveDate').addClass('ui-state-disabled disabled');
            $('#detail_btn_submit').addClass('ui-state-disabled disabled');
            //$('#btnEmployee').attr('disabled', true);
            //$('#EffectiveDate').attr('data-options', '{"mode":"calbox", "hideInput": "true", "hideButton": "true", "useInline": "true"}');
        }

        $.ajax({
            dataType: "json",
            url: base_url + "SalaryEmployee/GetInfo?Id=" + id,
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
                        $('#EffectiveDate').datebox('setValue', dateEnt(result.EffectiveDate));
                        $('#EffectiveDate2').val(dateEnt(result.EffectiveDate));
                        if (isactive) {
                            $('#Description').removeAttr('disabled');
                            $('#EffectiveDateDiv2').hide();
                            $('#EffectiveDateDiv').show();
                        } else {
                            $('#Description').attr('disabled', true);
                            $('#EffectiveDateDiv2').show();
                            $('#EffectiveDateDiv').hide();
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

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'SalaryEmployee/GetList',
        postData: { 'ParentId': function () { return $("#parenttype").val(); } },
        datatype: "json",
        colNames: ['ID', 'Employee ID', 'Employee NIK', 'Employee Name', 'Employee Title', 'Effective Date', 'Description', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'employeeid', index: 'employeeid', width: 100, hidden: true },
                  { name: 'employeenik', index: 'employeenik', width: 100 },
                  { name: 'employeename', index: 'employeename', width: 150 },
				  { name: 'titleinfoname', index: 'titleinfoname', width: 100 },
                  { name: 'effectivedate', index: 'effectivedate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'description', index: 'description', width: 200 },
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
        $('#EffectiveDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#EffectiveDateDiv2').hide();
        $('#EffectiveDateDiv').show();
        $('#btnEmployee').removeAttr('disabled');
        $('#Description').removeAttr('disabled');
        $('#btn_edit_detail').removeClass('ui-state-disabled disabled');
        $('#form_btn_save').removeClass('ui-state-disabled disabled');
        $('#EffectiveDate').removeClass('ui-state-disabled disabled');
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
        fillData();
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            vStatusSaving = 1;//edit data mode
            $.ajax({
                dataType: "json",
                url: base_url + "SalaryEmployee/GetInfo?Id=" + id,
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
                            $('#Description').val(result.Description);
                            $('#Description').removeAttr('disabled');
                            $('#btnEmployee').removeAttr('disabled');
                            $('#EffectiveDate').datebox('setValue', dateEnt(result.EffectiveDate));
                            $('#btn_edit_detail').removeClass('ui-state-disabled disabled');
                            $('#form_btn_save').removeClass('ui-state-disabled disabled');
                            $('#form_btn_del').removeClass('ui-state-disabled disabled');
                            $('#EffectiveDate').removeClass('ui-state-disabled disabled');
                            $('#detail_btn_submit').removeClass('ui-state-disabled disabled');
                            $('#EffectiveDateDiv2').hide();
                            $('#EffectiveDateDiv').show();
                            $('#tabledetail_div').hide();
                            $('#form_btn_save').show();
                            $('#form_btn_del').show();
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
            url: base_url + "SalaryEmployee/Delete",
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
            submitURL = base_url + 'SalaryEmployee/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'SalaryEmployee/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Description: $("#Description").val(), EmployeeId: $("#EmployeeId").val(),
                EffectiveDate: $("#EffectiveDate").datebox('getValue')
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
        colNames: ['SalaryEmployee ID', 'SalaryItem ID', 'SalaryItem Code', 'SalaryItem Name', 'Amount'],
        colModel: [
				  { name: 'salaryemployeeid', index: 'salaryemployeeid', hidden: true, width: 80, sortable: false },
				  { name: 'salaryitemid', index: 'salaryitemid', hidden: true, width: 80, sortable: false },
				  { name: 'salaryitemcode', index: 'salaryitemcode', width: 100, sortable: true },
				  { name: 'salaryitemname', index: 'salaryitemname', width: 150, sortable: true },
				  { name: 'amount', index: 'amount', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: true },
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
        height: $(window).height() - 500,
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
                url: base_url + "SalaryEmployee/GetInfo?Id=" + id,
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
                            $('#EffectiveDate').datebox('setValue', dateEnt(result.EffectiveDate));
                            $('#EffectiveDate2').val(dateEnt(result.EffectiveDate));
                            $('#EffectiveDateDiv').hide();
                            $('#EffectiveDateDiv2').show();
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
                url: base_url + "SalaryEmployee/GetInfoDetail?Id=" + id,
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
                        url: base_url + "SalaryEmployee/DeleteDetail",
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
            submitURL = base_url + 'SalaryEmployee/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'SalaryEmployee/InsertDetail';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, SalaryEmployeeId: $("#id").val(), SalaryItemId: $("#SalaryItemId").val(), Amount: $("#Amount").numberbox('getValue'),
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
            $('#lookup_div_employee').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });
    // ---------------------------------------------End Lookup Employee----------------------------------------------------------------

    
}); //END DOCUMENT READY