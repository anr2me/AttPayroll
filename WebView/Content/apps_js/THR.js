$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ReloadGrid() {
        // Clear Search string jqGrid
        //$('input[id*="gs_"]').val("");

        var id = $('#parenttype option:selected').text();
        var value = $('#parenttype').val();

        $("#list").setGridParam({ url: base_url + 'THR/GetList', postData: { filters: null, ParentId: value }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'THR/GetListDetail?Id=' + $("#id").val(), postData: { filters: null } }).trigger("reloadGrid");
    }

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ClearData() {
        //$('#NIK').val('').text('').removeClass('errormessage');
        //$('#Name').val('').text('').removeClass('errormessage');
        ClearErrorMessage();
    }

    $("#parenttype").live("change", function () {
        ReloadGrid();
    });

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#lookup_div_employee").dialog('close');
    $("#detail_div").dialog('close');
    $('#EffectiveDateDiv2').hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'THR/GetList',
        postData: { 'ParentId': function () { return $("#parenttype").val(); } },
        datatype: "json",
        colNames: ['ID', 'SalaryItem ID', 'Code', 'Name', 'Description', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'salaryitemid', index: 'salaryitemid', width: 100, hidden: true },
                  { name: 'code', index: 'code', width: 100 },
                  { name: 'name', index: 'name', width: 150 },
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
        $('#btnEmployee').removeAttr('disabled');
        $('#Description').removeAttr('disabled');
        $('#form_btn_save').data('kode', '');
        $('#form_btn_save').show();
        $('#tabledetail_div').hide();
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
                url: base_url + "THR/GetInfo?Id=" + id,
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
                            $('#SalaryItemId').val(result.SalaryItemId);
                            $('#Code').val(result.Code);
                            $('#Name').val(result.Name);
                            $('#EffectiveDate').datebox('setValue', dateEnt(result.EffectiveDate));
                            $('#EffectiveDate2').val(dateEnt(result.EffectiveDate));
                            //$('#EffectiveDateDiv').hide();
                            //$('#EffectiveDateDiv2').show();
                            $('#Description').val(result.Description);
                            $('#Description').removeAttr('disabled');
                            $('#btnEmployee').removeAttr('disabled');
                            $('#tabledetail_div').hide();
                            $('#form_btn_save').show();
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
            url: base_url + "THR/Delete",
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
            submitURL = base_url + 'THR/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'THR/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Description: $("#Description").val(), Code: $("#Code").val(), Name: $("#Name").val(),
                EffectiveDate: $("#EffectiveDate").datebox('getValue'),
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
        colNames: ['THR ID', 'Employee ID', 'Employee NIK', 'Employee Name', 'Religion'],
        colModel: [
				  { name: 'thrid', index: 'thrid', hidden: true, width: 80, sortable: false },
				  { name: 'employeeid', index: 'employeeid', hidden: true, width: 80, sortable: false },
				  { name: 'employeenik', index: 'employeenik', width: 100, sortable: true },
				  { name: 'employeename', index: 'employeename', width: 150, sortable: true },
                  { name: 'employeereligion', index: 'employeereligion', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Religion") } },
        ],
        page: '1',
        pager: $('#pagerdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'Id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        multiselect: true,
        multiboxonly: true,
        sortorder: "ASC",
        width: $('#form_div').width() - 2, //$(window).width() - 800,
        height: $(window).height() - 500,
        ondblClickRow: function (rowid) {
            $("#btn_edit_detail").trigger("click");
        },
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowReligion = document.getElementById("Religion").options[$(this).getRowData(cl).employeereligion].text;
		          $(this).jqGrid('setRowData', ids[i], { employeereligion: rowReligion });
		      }
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
                url: base_url + "THR/GetInfo?Id=" + id,
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
                            $('#THRId').val(result.THRId);
                            $('#EmployeeId').val(result.EmployeeId);
                            $('#EmployeeNIK').val(result.EmployeeNIK);
                            $('#EmployeeName').val(result.EmployeeName);
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
        //$('#detail_div').dialog('open');
        $("#btnEmployee").trigger("click");
    });

    $('#btn_edit_detail').click(function () {
        ClearData();
        clearForm("#detail_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "THR/GetInfoDetail?Id=" + id,
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
                            $('#THRId').val(result.THRId);
                            $('#EmployeeId').val(result.EmployeeId);
                            $('#EmployeeNIK').val(result.EmployeeNIK);
                            $('#EmployeeName').val(result.EmployeeName);
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
            ClearErrorMessage();

            var submitURL = '';

            var selectedIDs = jQuery('#listdetail').jqGrid('getGridParam', 'selarrrow');
            var ret = jQuery("#listdetail").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to delete record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "THR/DeleteManyDetails?id=" + $("#id").val(),
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify(selectedIDs),
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
            submitURL = base_url + 'THR/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'THR/InsertDetail';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, THRId: $("#id").val(), EmployeeId: $("#EmployeeId").val(), 
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


    // -------------------------------------------------------Look Up & Add Employee-------------------------------------------------------
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
        colNames: ['ID', 'NIK', 'Name', 'Title', 'Religion'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'nik', index: 'nik', width: 100 },
				  { name: 'name', index: 'name', width: 180 },
                  { name: 'titleinfoname', index: 'titleinfoname', width: 150, hidden:true },
                  { name: 'religion', index: 'religion', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Religion") } },
        ],
        page: '1',
        pager: $('#lookup_pager_employee'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        multiselect: true,
        multiboxonly: true,
        sortorder: "ASC",
        width: $("#lookup_div_employee").width() - 10,
        height: $("#lookup_div_employee").height() - 110,
        ondblClickRow: function (rowid) {
            $("#lookup_btn_add_employee").trigger("click");
        },
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowReligion = document.getElementById("Religion").options[$(this).getRowData(cl).religion].text;
		          $(this).jqGrid('setRowData', ids[i], { religion: rowReligion });
		      }
		  }
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

            ClearErrorMessage();

            var submitURL = '';
            //var id = $("#detail_btn_submit").data('kode');
            var selectedIDs = jQuery('#lookup_table_employee').jqGrid('getGridParam', 'selarrrow');

            submitURL = base_url + 'THR/InsertManyDetails?id=' + $("#id").val();

            $.ajax({
                contentType: "application/json",
                type: 'POST',
                url: submitURL,
                data: JSON.stringify(selectedIDs), // recIDs
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
                        //$("#detail_div").dialog('close')
                    }
                }
            });

            $('#lookup_div_employee').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };

    });
    // ---------------------------------------------End Lookup & Add Employee----------------------------------------------------------------

    
}); //END DOCUMENT READY