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

        $("#list").setGridParam({ url: base_url + 'Applicant/GetList', postData: { filters: null, ParentId: value }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#NIK').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#TitleInfoId').val('');
        $('#TitleInfoName').val('');
        $('#PlaceOfBirth').val('');
        $('#Address').val('');
        $('#PhoneNumber').val('');
        $('#Email').val('');
        $('#Experience').numberbox('setValue', '');
        $('#BirthDate').datebox('setValue', '');
        //document.getElementById("Sex").selectedIndex = 0;
        //document.getElementById("MaritalStatus").selectedIndex = 0;
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $('#lookup_div_position').dialog('close');
    $('#lookup_div_education').dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'Applicant/GetList',
        postData: { 'ParentId': function () { return $("#parenttype").val(); } },
        datatype: "json",
        colNames: ['ID', 'Status', 'Name', 'Birth Date', 'Address', 'City', 'Phone Number', 'Education ID', 'Education', 'Email', 'Position', 'Experience', 'Last Position', 'Source', 'Is Deleted', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'status', index: 'status', width: 100, stype: 'select', editoptions: { value: getSelectOption("#ApplicantStatus") } },
				  { name: 'name', index: 'name', width: 180 },
                  { name: 'birthdate', index: 'birthdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'address', index: 'address', width: 250 },
                  { name: 'city', index: 'city', width: 100 },
                  { name: 'phonenumber', index: 'phonenumber', width: 100 },
                  { name: 'educationid', index: 'educationid', width: 100, hidden: true },
                  { name: 'educationname', index: 'educationname', width: 100},
                  { name: 'email', index: 'email', width: 100 },
                  { name: 'position', index: 'position', width: 100 },
                  { name: 'experience', index: 'experience', width: 100 },
                  { name: 'lastposition', index: 'lastposition', width: 100 },
                  { name: 'source', index: 'source', width: 100 },
                  //{ name: 'sex', index: 'sex', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Sex") } }, // ':All;0:Male;1:Female'
                  //{ name: 'maritalstatus', index: 'maritalstatus', width: 100, stype: 'select', editoptions: { value: getSelectOption("#MaritalStatus") } },
                  { name: 'isdeleted', index: 'isdeleted', width: 100, hidden:true },
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
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          
		          //$(this).jqGrid('setRowData', ids[i], { activestatus: rowActiveStatus });

		          rowStatus = document.getElementById("Status").options[$(this).getRowData(cl).status].text;
		          $(this).jqGrid('setRowData', ids[i], { status: rowStatus });

		          //rowSex = document.getElementById("Sex").options[$(this).getRowData(cl).sex].text;
		          //$(this).jqGrid('setRowData', ids[i], { sex: rowSex });

		          //rowMarital = document.getElementById("MaritalStatus").options[$(this).getRowData(cl).maritalstatus].text;
		          //$(this).jqGrid('setRowData', ids[i], { maritalstatus: rowMarital });

		      }
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
        var e = $('#parenttype option:selected');
        $('#DivisionId').val(e.val());
        $('#DivisionName').val(e.text());
        vStatusSaving = 0; //add data mode
        $('#mainform').show();
        $('#hasiltest').hide();
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
                url: base_url + "Applicant/GetInfo?Id=" + id,
                success: function (ret) {
                    var result = ret.model;
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
                            $('#Name').val(result.Name);
                            $('#Address').val(result.Address);
                            $('#City').val(result.City);
                            $('#PhoneNumber').val(result.PhoneNumber);
                            $('#EducationId').val(result.EducationId);
                            $('#EducationName').val(result.EducationName);
                            $('#Email').val(result.Email);
                            $('#Position').val(result.Position);
                            $('#Source').val(result.Source);
                            $('#LastPosition').val(result.LastPosition);
                            $('#Experience').numberbox('setValue', result.Experience);
                            $('#BirthDate').datebox('setValue', dateEnt(result.BirthDate));
                            document.getElementById("Status").selectedIndex = result.Status;
                            //document.getElementById("Sex").selectedIndex = result.Sex;
                            //document.getElementById("MaritalStatus").selectedIndex = result.MaritalStatus;
                            $('#mainform').show();
                            $('#hasiltest').hide();
                            $("#form_div").dialog("open");
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_edittest').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            vStatusSaving = 1;//edit data mode
            $.ajax({
                dataType: "json",
                url: base_url + "Applicant/GetInfo?Id=" + id,
                success: function (ret) {
                    var result = ret.model;
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
                            $('#Name').val(result.Name);
                            $('#Address').val(result.Address);
                            $('#City').val(result.City);
                            $('#PhoneNumber').val(result.PhoneNumber);
                            $('#EducationId').val(result.EducationId);
                            $('#EducationName').val(result.EducationName);
                            $('#Email').val(result.Email);
                            $('#Position').val(result.Position);
                            $('#Source').val(result.Source);
                            $('#LastPosition').val(result.LastPosition);
                            $('#Experience').numberbox('setValue', result.Experience);
                            $('#BirthDate').datebox('setValue', dateEnt(result.BirthDate));
                            //document.getElementById("Sex").selectedIndex = result.Sex;
                            //document.getElementById("MaritalStatus").selectedIndex = result.MaritalStatus;
                            $('#mainform').hide();
                            $('#hasiltest').show();
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
            url: base_url + "Applicant/Delete",
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
            submitURL = base_url + 'Applicant/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'Applicant/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, NIK: $("#NIK").val(), Name: $("#Name").val(), 
                City: $('#City').val(), Address: $('#Address').val(), PhoneNumber: $('#PhoneNumber').val(),
                Email: $('#Email').val(), NPWP: $('#NPWP').val(), JamsostekNo: $('#Jamsostek').val(),
                Experience: $('#Experience').numberbox('getValue'), BirthDate: $('#BirthDate').datebox('getValue'),
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
        colNames: ['ID', 'Code', 'Name', 'Description', 'Shiftable', 'Salary All In', 'Access Level'],
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
        colNames: ['ID', 'Code', 'Name', 'Description', 'Department Id', 'Department Code', 'Department Name'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'code', index: 'code', width: 100 },
				  { name: 'name', index: 'name', width: 180 },
                  { name: 'description', index: 'description', width: 250 },
                  { name: 'departmentid', index: 'departmentid', width: 100, hidden: true },
                  { name: 'departmentcode', index: 'departmentcode', width: 150, hidden: true },
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

    
}); //END DOCUMENT READY