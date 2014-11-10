$(document).ready(function () {
    var a;
    $("#detail_panel").hide();

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
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }


    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#lookup_div_employee").dialog('close');
    $("#lookup_div_workingtime").dialog('close');

    function ReloadGridListParent() {
        $("#detail_panel").hide();
        jQuery("#list").jqGrid({
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
                      { name: 'educationname', index: 'educationname', width: 100 },
                      { name: 'email', index: 'email', width: 100 },
                      { name: 'position', index: 'position', width: 100 },
                      { name: 'experience', index: 'experience', width: 100 },
                      { name: 'lastposition', index: 'lastposition', width: 100 },
                      { name: 'source', index: 'source', width: 100 },
                      //{ name: 'sex', index: 'sex', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Sex") } }, // ':All;0:Male;1:Female'
                      //{ name: 'maritalstatus', index: 'maritalstatus', width: 100, stype: 'select', editoptions: { value: getSelectOption("#MaritalStatus") } },
                      { name: 'isdeleted', index: 'isdeleted', width: 100, hidden: true },
                      { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                      { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
        $("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list").jqGrid('setFrozenColumns');
    //    $("#list").setGridParam({ url: base_url + 'Applicant/GetList', page: '1' }).trigger("reloadGrid");
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
            url: base_url + "JobDescMembership/UpdateEnable?isEnabled=" + isEnabled,
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
            url: base_url + 'JobDescMembership/GetList?Id=' + id,
            //postData: { 'ParentId': function () { return $("#parenttype").val(); } },
            datatype: "json",
            colNames: ['ID', 'Applicant ID', 'Applicant Name', 'JobDesc ID', 'JobDesc Name', 'Is Deleted', 'Created At', 'Updated At'],
            colModel: [
                      { name: 'id', index: 'id', width: 80, align: "center", frozen:true },
                      { name: 'applicantid', index: 'applicantid', width: 100, frozen: true, hidden: true },
                      { name: 'applicantname', index: 'applicantname', width: 100, frozen: true, hidden: true },
                      { name: 'jobdescid', index: 'jobdescid', width: 100, frozen: true, hidden:true},
                      { name: 'jobdescname', index: 'jobdescname', width: 100, frozen: true, },
                      { name: 'isdeleted', index: 'isdeleted', width: 100, hidden:true },
                      { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                      { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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

                      
                  }
              }

        });//END GRID Detail
        $("#list_detail").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list_detail").jqGrid('setFrozenColumns');
    }

    ReloadGridListParent();

    //$('#btn_warehouse').click(function () {
    //    a = 'warehouse';
    //    $("#list").jqGrid('GridUnload');
    //    ReloadGridListParent();
    //});
 
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
        $('#StartDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#EndDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#ApplicantId').val(ret.id);
            $('#ApplicantName').val(ret.name);
        }
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
                url: base_url + "JobDescMembership/GetInfo?Id=" + id,
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
                            $('#ApplicantId').val(result.ApplicantId);
                            $('#ApplicantName').val(result.ApplicantName);
                            $('#JobDescId').val(result.JobDescId);
                            $('#JobDescName').val(result.JobDescName);
                            $('#Tahun').val(result.Tahun);
                            //$('#Sex').val(result.Sex);
                            //$('#MaritalStatus').val(result.MaritalStatus);
                            //document.getElementById("IsDeceased").checked = result.IsDeceased;
                            //$('#IsEnabled').val(result.IsEnabled?'1':'0');
                            //document.getElementById("IsEnabled").checked = result.IsEnabled;
                            //document.getElementById("IsShiftable").checked = result.IsShiftable;
                            //$('#BirthDate').datebox('setValue', dateEnt(result.BirthDate)); //datebox('setValue', timeEnt(result.MinCheckIn));
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
            url: base_url + "JobDescMembership/Delete",
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
            submitURL = base_url + 'JobDescMembership/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'JobDescMembership/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ApplicantId: jQuery("#list").jqGrid('getGridParam', 'selrow'), //IsEnabled: document.getElementById("IsEnabled").checked ? 'true' : 'false',
                //StartDate: $("#StartDate").datebox('getValue'), EndDate: $("#EndDate").datebox('getValue'), //IsShiftable: document.getElementById("IsShiftable").checked ? 'true' : 'false',
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

    // -------------------------------------------------------Look Up Applicant-------------------------------------------------------
    $('#btnApplicant').click(function () {
        var lookUpURL = base_url + 'Applicant/GetList';
        var lookupGrid = $('#lookup_table_workingtime');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_workingtime').dialog('open');
    });

    jQuery("#lookup_table_workingtime").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Code', 'Name'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'code', index: 'code', width: 100 },
				  { name: 'name', index: 'name', width: 180 },
        ],
        page: '1',
        pager: $('#lookup_pager_workingtime'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_workingtime").width() - 10,
        height: $("#lookup_div_workingtime").height() - 110,
        ondblClickRow: function (rowid) {
            $("#lookup_btn_add_workingtime").trigger("click");
        },
    });
    $("#lookup_table_workingtime").jqGrid('navGrid', '#lookup_toolbar_workingtime', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_workingtime').click(function () {
        $('#lookup_div_workingtime').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_workingtime').click(function () {
        var id = jQuery("#lookup_table_workingtime").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_workingtime").jqGrid('getRowData', id);

            $('#ApplicantId').val(ret.id).data("kode", id);
            $('#ApplicantCode').val(ret.code);
            $('#ApplicantName').val(ret.name);
            $('#lookup_div_workingtime').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });
    // ---------------------------------------------End Lookup Applicant----------------------------------------------------------------


}); //END DOCUMENT READY