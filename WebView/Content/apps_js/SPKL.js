$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ReloadGrid() {
        // Clear Search string jqGrid
        //$('input[id*="gs_"]').val("");

        var id = $('#parenttype option:selected').text();
        var value = $('#parenttype').val();

        $("#list").setGridParam({ url: base_url + 'SPKL/GetList', postData: { filters: null, ParentId: value }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ClearData() {
        $('#NIK').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#Remark').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#parenttype").live("change", function () {
        ReloadGrid();
    });

    // set options globaly
    $.datetimeEntry.setDefaults({ show24Hours: true, datetimeFormat: 'O/D/Y H:M', spinnerImage: 'Content/spinnerText.png', spinnerSize: [30, 20, 8] }); // , spinnerBigImage: 'Content/spinnerTextBig.png', spinnerBigSize: [60, 40, 16]
    $('#StartTime').datetimeEntry();
    $('#EndTime').datetimeEntry();

    //$('#MinCheckIn').datetimepicker({
    //    //addSliderAccess: true,
    //    //sliderAccessArgs: { touchonly: false },
    //    //controlType: 'select',
    //    timeFormat: 'HH:mm'
    //});

    //$("#MinCheckIn").dynDateTime({
    //    showsTime: true,
    //    ifFormat: "%Y/%m/%d %H:%M",
    //    daFormat: "%l;%M %p, %e %m, %Y",
    //    align: "BR",
    //    electric: false,
    //    singleClick: false,
    //    displayArea: ".siblings('.dtcDisplayArea')",
    //    button: ".next()"
    //});

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#lookup_div_employee").dialog('close'); 
    $("#StartDateDiv2").hide();
    $("#EndDateDiv2").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'SPKL/GetList',
        postData: { 'ParentId': function () { return $("#parenttype").val(); } },
        datatype: "json",
        colNames: ['ID', 'Employee ID', 'Employee NIK', 'Employee Name', 'Start Time', 'End Time', 'OverTime Interval', 'Remark', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 60, align: "center" },
                  { name: 'employeeid', index: 'employeeid', width: 60, hidden:true },
				  { name: 'employeenik', index: 'employeenik', width: 100 },
                  { name: 'employeename', index: 'employeename', width: 150 },
                  { name: 'starttime', index: 'starttime', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'm/d/Y H:i' } },
                  { name: 'endtime', index: 'endtime', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'm/d/Y H:i' } },
                  { name: 'overtimeinterval', index: 'overtimeinterval', width: 150 },
                  { name: 'remark', index: 'remark', width: 200 },
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
        sortorder: "ASC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        ondblClickRow: function (rowid) {
            $("#btn_edit").trigger("click");
            //var row_id = $("#list").getGridParam('selrow');
            //jQuery(this).jqGrid('editGridRow', rowid//,
            //                    //{recreateForm:true, closeAfterEdit:true,
            //                    //closeOnEscape: true, reloadAfterSubmit: false}
            //                    );
        },
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowOverTimeInterval = ($(this).getRowData(cl).overtimeinterval/60.0).toFixed(1);
		          $(this).jqGrid('setRowData', ids[i], { overtimeinterval: rowOverTimeInterval });
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
        $('#StartTime').val($.datepicker.formatDate('mm/dd/yy 00:00', new Date())); //datebox('setValue', timeEnt(result.MinCheckIn));
        $('#EndTime').val($.datepicker.formatDate('mm/dd/yy 00:00', new Date())); //datebox('setValue', timeEnt(result.CheckIn));
        $('#OverTimeInterval').numberbox('setValue', '');
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
                url: base_url + "SPKL/GetInfo?Id=" + id,
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
                            $('#Remark').val(result.Remark);
                            $('#StartTime').val(dateTimeEnt(result.StartTime)); //datebox('setValue', timeEnt(result.MinCheckIn));
                            $('#EndTime').val(dateTimeEnt(result.EndTime)); //datebox('setValue', timeEnt(result.CheckIn));
                            $('#OverTimeInterval').val((result.OverTimeInterval / 60.0).toFixed(1)); //numberbox('setValue', result.WorkInterval/60.0);
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
            url: base_url + "SPKL/Delete",
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
            submitURL = base_url + 'SPKL/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'SPKL/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL + "?StartTime=" + $("#StartTime").val() + "&EndTime=" + $("#EndTime").val(),
            data: JSON.stringify({
                Id: id, EmployeeId: $("#EmployeeId").val(), Remark: $("#Remark").val(),
                //StartTime: $("#StartTime").val(), EndTime: $("#EndTime").val(),
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

    
}); //END DOCUMENT READY