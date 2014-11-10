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
    $("#approve_form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#lookup_div_employee").dialog('close');
    $("#lookup_div_workingtime").dialog('close');

    function ReloadGridListParent() {
        $("#detail_panel").hide();
        jQuery("#list").jqGrid({
            url: base_url + 'EmployeePersonal/GetList',
            postData: { 'ParentId': function () { return $("#parenttype").val(); } },
            datatype: "json",
            colNames: ['ID', 'First Name', 'Last Name', 'NickName', 'Start Working Date',
                    'NIK', 'Job Type', 'Joined Date', 'Title ID', 'Title', 'Division ID', 'Division', 'Supervisor ID', 'Supervisor', 'Superior ID', 'Superior', 'Starting Date', 'Ending Date', 'Remark',
                    'ID Type', 'ID Number', 'ID Expiration Date', 'ID Name', 'Place of Birth', 'Date of Birth', 'Sex', 'Gol Darah', 'Jenis Gol Darah', 'Marital Status', 'Married Date', 'Last Education', 'Religion', 'Nationality', 'Anak Ke', 'Dari Bersaudara',
                    'Emergency Contact Name', 'Emergency Contact Phone', 'Emergency Contact Remark',
                    'ID Address', 'ID City', 'ID Postal Code',
                    'Address', 'City', 'Postal Code', 'Phone Number',
                    'Tertanggung(PPH21)', 'NPWP', 'NPWP Name', 'NPWP Address', 'NPWP RT', 'NPWP RW', 'NPWP Kelurahan', 'NPWP Kecamatan', 'NPWP Kota', 'NPWP KodePos',
                    'Bank Account Name', 'Bank Name', 'Bank Branch', 'Bank Account Number', 'Bank Account Currency',
                    'Email', 'Mobile Number', 'Extension', 'Secretary',
                    'English', 'Mandarin', 'Indonesia', 'Other', 'Other Grade',
                    'Is Deleted', 'Created At', 'Updated At'],
            colModel: [
                      { name: 'id', index: 'id', width: 80, align: "center", frozen: true },
                      { name: 'firstname', index: 'firstname', width: 100, frozen: true },
                      { name: 'lastname', index: 'lastname', width: 100 },
                      { name: 'nickname', index: 'nickname', width: 100 },
                      { name: 'startworkingdate', index: 'startworkingdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },

                      { name: 'nik', index: 'nik', width: 100 },
                      { name: 'jobtype', index: 'jobtype', width: 80, stype: 'select', editoptions: { value: getSelectOption("#JobType") } },
                      { name: 'joineddate', index: 'joineddate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                      { name: 'titleinfoid', index: 'titleinfoid', width: 50, hidden: true },
                      { name: 'titleinfoname', index: 'titleinfoname', width: 100 },
                      { name: 'divisionid', index: 'divisionid', width: 50, hidden: true },
                      { name: 'divisionname', index: 'divisionname', width: 100 },
                      { name: 'supervisorid', index: 'supervisorid', width: 50, hidden: true },
                      { name: 'supervisorname', index: 'supervisorname', width: 100 },
                      { name: 'superiorid', index: 'superiorid', width: 50, hidden: true },
                      { name: 'superiorname', index: 'superiorname', width: 100 },
                      { name: 'startingdate', index: 'startingdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                      { name: 'endingdate', index: 'endingdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                      { name: 'remark', index: 'remark', width: 150 },

                      { name: 'idtype', index: 'idtype', width: 100, stype: 'select', editoptions: { value: getSelectOption("#IDType") } },
                      { name: 'idnumber', index: 'idnumber', width: 100 },
                      { name: 'expirationdate', index: 'expirationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                      { name: 'idname', index: 'idname', width: 180 },
                      { name: 'placeofbirth', index: 'placeofbirth', width: 100 },
                      { name: 'birthdate', index: 'birthdate', hidden: true, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                      { name: 'sex', index: 'sex', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Sex") } }, // ':All;0:Male;1:Female'
                      { name: 'goldarah', index: 'goldarah', width: 50 },
                      { name: 'jenisgoldarah', index: 'jenisgoldarah', hidden: true, width: 60 },
                      { name: 'maritalstatus', index: 'maritalstatus', width: 100, stype: 'select', editoptions: { value: getSelectOption("#MaritalStatus") } },
                      { name: 'marrieddate', index: 'marrieddate', hidden: true, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                      { name: 'lasteducation', index: 'lasteducation', width: 100 },
                      { name: 'religion', index: 'religion', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Religion") } },
                      { name: 'nationality', index: 'nationality', width: 100 },
                      { name: 'childnumber', index: 'childnumber', width: 50 },
                      { name: 'outofbrothers', index: 'outofbrothers', width: 50 },

                      { name: 'emergencycontactname', index: 'emergencycontactname', width: 100 },
                      { name: 'emergencycontactphone', index: 'emergencycontactphone', width: 80 },
                      { name: 'emergencycontactremark', index: 'emergencycontactremark', width: 100 },

                      { name: 'idaddress', index: 'idaddress', width: 250 },
                      { name: 'idcity', index: 'idcity', width: 80 },
                      { name: 'idpostalcode', index: 'idpostalcode', width: 60 },

                      { name: 'address', index: 'address', width: 250 },
                      { name: 'city', index: 'city', width: 80 },
                      { name: 'postalcode', index: 'postalcode', width: 60 },
                      { name: 'phonenumber', index: 'phonenumber', width: 80 },

                      { name: 'tertanggungpph21', index: 'tertanggungpph21', width: 100 },
                      { name: 'npwp', index: 'npwp', width: 80 },
                      { name: 'npwp_name', index: 'npwp_name', width: 180 },
                      { name: 'npwp_address', index: 'npwp_address', width: 250 },
                      { name: 'npwp_rt', index: 'npwp_rt', width: 60 },
                      { name: 'npwp_rw', index: 'npwp_rw', width: 60 },
                      { name: 'npwp_kelurahan', index: 'npwp_kelurahan', width: 100 },
                      { name: 'npwp_kecamatan', index: 'npwp_kecamatan', width: 100 },
                      { name: 'npwp_kota', index: 'npwp_kota', width: 80 },
                      { name: 'npwp_kodepos', index: 'npwp_kodepos', width: 60 },

                      { name: 'bankaccountname', index: 'bankaccountname', width: 180 },
                      { name: 'bankname', index: 'bankname', width: 80 },
                      { name: 'bankbranch', index: 'bankbranch', width: 100 },
                      { name: 'bankaccountnumber', index: 'bankaccountnumber', width: 100 },
                      { name: 'bankaccountcurrency', index: 'bankaccountcurrency', width: 50, stype: 'select', editoptions: { value: getSelectOption("#BankAccountCurrency") } },

                      { name: 'email', index: 'email', width: 100 },
                      { name: 'mobilenumber', index: 'mobilenumber', width: 80 },
                      { name: 'extensionnumber', index: 'extensionnumber', width: 50 },
                      { name: 'secretary', index: 'secretary', width: 100 },

                      { name: 'inggris', index: 'inggris', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Inggris") } },
                      { name: 'mandarin', index: 'mandarin', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Mandarin") } },
                      { name: 'indonesia', index: 'indonesia', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Indonesia") } },
                      { name: 'otherlang', index: 'otherlang', width: 80 },
                      { name: 'othergrade', index: 'othergrade', width: 100, hidden: true, stype: 'select', editoptions: { value: getSelectOption("#OtherGrade") } },

                      { name: 'isdeleted', index: 'isdeleted', width: 50, hidden: true },
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
		          rowActiveStatus = $(this).getRowData(cl).activestatus;
		          if (rowActiveStatus == '0') {
		              rowActiveStatus = "NO, " + $(this).getRowData(cl).nonactivedate;
		          } else {
		              rowActiveStatus = "YES";
		          }
		          $(this).jqGrid('setRowData', ids[i], { activestatus: rowActiveStatus });

		          $(this).jqGrid('setRowData', ids[i], { goldarah: $(this).getRowData(cl).goldarah + " " + $(this).getRowData(cl).jenisgoldarah });

		          rowBirth = $(this).getRowData(cl).placeofbirth + ", " + $(this).getRowData(cl).birthdate;
		          $(this).jqGrid('setRowData', ids[i], { placeofbirth: rowBirth });

		          //rowNPWP = $(this).getRowData(cl).npwp + ", " + $(this).getRowData(cl).npwpdate;
		          //$(this).jqGrid('setRowData', ids[i], { npwp: rowNPWP });

		          rowJobType = document.getElementById("JobType").options[$(this).getRowData(cl).jobtype].text;
		          $(this).jqGrid('setRowData', ids[i], { jobtype: rowJobType });

		          rowSex = document.getElementById("Sex").options[$(this).getRowData(cl).sex].text;
		          $(this).jqGrid('setRowData', ids[i], { sex: rowSex });

		          rowMarital = document.getElementById("MaritalStatus").options[$(this).getRowData(cl).maritalstatus].text;
		          if (rowMarital == "Married") rowMarital += ", " + $(this).getRowData(cl).marrieddate;
		          $(this).jqGrid('setRowData', ids[i], { maritalstatus: rowMarital });

		          rowReligion = document.getElementById("Religion").options[$(this).getRowData(cl).religion].text;
		          $(this).jqGrid('setRowData', ids[i], { religion: rowReligion });

		          rowIDType = document.getElementById("IDType").options[$(this).getRowData(cl).idtype].text;
		          $(this).jqGrid('setRowData', ids[i], { idtype: rowIDType });

		          rowCurrency = document.getElementById("BankAccountCurrency").options[$(this).getRowData(cl).bankaccountcurrency].text;
		          $(this).jqGrid('setRowData', ids[i], { bankaccountcurrency: rowCurrency });

		          rowEnglish = document.getElementById("Inggris").options[$(this).getRowData(cl).inggris].text;
		          $(this).jqGrid('setRowData', ids[i], { inggris: rowEnglish });

		          rowMandarin = document.getElementById("Mandarin").options[$(this).getRowData(cl).mandarin].text;
		          $(this).jqGrid('setRowData', ids[i], { mandarin: rowMandarin });

		          rowIndonesia = document.getElementById("Indonesia").options[$(this).getRowData(cl).indonesia].text;
		          $(this).jqGrid('setRowData', ids[i], { indonesia: rowIndonesia });

		          rowOtherLang = $(this).getRowData(cl).otherlang;
		          if (rowOtherLang != null && rowOtherLang != "") {
		              rowOther = document.getElementById("OtherGrade").options[$(this).getRowData(cl).othergrade].text;
		              rowOther = rowOtherLang + ", " + $(this).getRowData(cl).othergrade;
		              $(this).jqGrid('setRowData', ids[i], { otherlang: rowOther });
		          }
		      }
		  }

        });//END GRID
        $("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list").jqGrid('setFrozenColumns');
        //    $("#list").setGridParam({ url: base_url + 'EmployeePersonal/GetList', page: '1' }).trigger("reloadGrid");
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
            url: base_url + "EmployeeOtherIncome/UpdateEnable?isEnabled=" + isEnabled,
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
            url: base_url + 'EmployeeOtherIncome/GetListDetail?Id=' + id,
            //postData: { 'ParentId': function () { return $("#parenttype").val(); } },
            datatype: "json",
            colNames: ['ID', 'OtherIncome ID', 'Employee ID', 'Employee NIK', 'Employee Name', 'Income Type', 'Salary Status', 'Amount', 'Is Approved', 'Effective Date', 'Recurring', 'Remark', 'Created At', 'Updated At'],
            colModel: [
                      { name: 'id', index: 'id', width: 80, align: "center" },
                      { name: 'otherincomeid', index: 'otherincomeid', width: 100, hidden: true },
                      { name: 'employeeid', index: 'employeeid', width: 100, hidden: true },
                      { name: 'employeenik', index: 'employeenik', width: 100, hidden: true },
                      { name: 'employeename', index: 'employeename', width: 100, hidden: true },
                      { name: 'otherincometype', index: 'otherincometype', width: 100, stype: 'select', editoptions: { value: getSelectOption("#OtherIncomeType") } },
                      { name: 'salarystatus', index: 'salarystatus', width: 100, stype: 'select', editoptions: { value: getSelectOption("#SalaryStatus") } },
                      { name: 'amount', index: 'amount', width: 100, },
                      { name: 'isapproved', index: 'isapproved', width: 100, },
                      { name: 'effectivedate', index: 'effectivedate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                      { name: 'recurring', index: 'recurring', width: 100 },
                      { name: 'remark', index: 'remark', width: 200 },
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

                      rowIncomeType = document.getElementById("OtherIncomeType").options[$(this).getRowData(cl).otherincometype].text;
                      $(this).jqGrid('setRowData', ids[i], { otherincometype: rowIncomeType });

                      rowStatus = document.getElementById("SalaryStatus").options[$(this).getRowData(cl).salarystatus].text;
                      $(this).jqGrid('setRowData', ids[i], { salarystatus: rowStatus });
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
        $('#Amount').numberbox('setValue', '');
        $('#Recurring').numberbox('setValue', '');
        $('#EffectiveDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#EmployeePersonalId').val(ret.id);
            $('#EmployeePersonalNIK').val(ret.nik);
            $('#EmployeePersonalName').val(ret.idname);
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
                url: base_url + "EmployeeOtherIncome/GetInfoDetail?Id=" + id,
                success: function (ret) {
                    var result = ret; //.model;
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
                            $('#EmployeePersonalId').val(result.EmployeePersonalId);
                            $('#EmployeePersonalNIK').val(result.EmployeePersonalNIK);
                            $('#EmployeePersonalName').val(result.EmployeePersonalName);
                            $('#OtherIncomeType').val(result.OtherIncomeType);
                            $('#SalaryStatus').val(result.SalaryStatus);
                            $('#Amount').numberbox('setValue', result.Amount);
                            $('#Recurring').numberbox('setValue', result.Recurring);
                            $('#Remark').val(result.Remark);
                            //document.getElementById("IsApproved").checked = result.IsApproved;
                            //$('#IsEnabled').val(result.IsEnabled?'1':'0');
                            //document.getElementById("IsEnabled").checked = result.IsEnabled;
                            //document.getElementById("IsShiftable").checked = result.IsShiftable;
                            $('#EffectiveDate').datebox('setValue', dateEnt(result.EffectiveDate)); //datebox('setValue', timeEnt(result.MinCheckIn));
                            var pid = jQuery("#list").jqGrid('getGridParam', 'selrow');
                            if (pid) {
                                var ret = jQuery("#list").jqGrid('getRowData', pid);
                                $('#EmployeePersonalId').val(ret.id);
                                $('#EmployeePersonalNIK').val(ret.nik);
                                $('#EmployeePersonalName').val(ret.idname);
                            }
                            $("#form_div").dialog("open");
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_approve').click(function () {
        ClearData();
        clearForm("#approve_frm");
        var id = jQuery("#list_detail").jqGrid('getGridParam', 'selrow');
        if (id) {
            vStatusSaving = 1;//edit data mode
            $.ajax({
                dataType: "json",
                url: base_url + "EmployeeOtherIncome/GetInfoDetail?Id=" + id,
                success: function (ret) {
                    var result = ret; //.model;
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
                            $("#approve_form_btn_save").data('kode', id);
                            $('#id').val(result.Id);
                            $('#approveEmployeePersonalId').val(result.EmployeePersonalId);
                            $('#approveEmployeePersonalNIK').val(result.EmployeePersonalNIK);
                            $('#approveEmployeePersonalName').val(result.EmployeePersonalName);
                            $('#approveOtherIncomeType').val(result.OtherIncomeType);
                            $('#approveSalaryStatus').val(result.SalaryStatus);
                            $('#approveAmount').numberbox('setValue', result.Amount);
                            $('#approveRemark').val(result.Remark);
                            document.getElementById("IsApproved").checked = result.IsApproved;
                            $('#Recurring').numberbox('setValue', result.Recurring);
                            //document.getElementById("IsApproved").checked = result.IsApproved;
                            //$('#IsEnabled').val(result.IsEnabled?'1':'0');
                            //document.getElementById("IsEnabled").checked = result.IsEnabled;
                            //document.getElementById("IsShiftable").checked = result.IsShiftable;
                            $('#EffectiveDate').datebox('setValue', dateEnt(result.EffectiveDate)); //datebox('setValue', timeEnt(result.MinCheckIn));
                            var pid = jQuery("#list").jqGrid('getGridParam', 'selrow');
                            if (pid) {
                                var ret = jQuery("#list").jqGrid('getRowData', pid);
                                $('#approveEmployeePersonalId').val(ret.id);
                                $('#approveEmployeePersonalNIK').val(ret.nik);
                                $('#approveEmployeePersonalName').val(ret.idname);
                            }
                            $("#approve_form_div").dialog("open");
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#approve_form_btn_cancel').click(function () {
        vStatusSaving = 0;
        clearForm('#approvefrm');
        $("#approve_form_div").dialog('close');
    });

    $("#approve_form_btn_save").click(function () {
        ClearErrorMessage();

        var submitURL = '';
        var id = $("#approve_form_btn_save").data('kode');

        // Update
        submitURL = base_url + 'EmployeeOtherIncome/ApproveDetail';

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, 
                EffectiveDate: $("#EffectiveDate").datebox('getValue'),
                Recurring: $("#Recurring").numberbox('getValue'),
                IsApproved: (document.getElementById("IsApproved").checked?true:false),
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
                    $("#approve_form_div").dialog('close')
                }
            }
        });
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
            url: base_url + "EmployeeOtherIncome/DeleteDetail",
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
            submitURL = base_url + 'EmployeeOtherIncome/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'EmployeeOtherIncome/InsertDetail';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, EmployeeId: $("#EmployeePersonalId").val(), OtherIncomeType: $("#OtherIncomeType").val(),
                EffectiveDate: $("#EffectiveDate").datebox('getValue'),
                Amount: $("#Amount").numberbox('getValue'), Recurring: $("#Recurring").numberbox('getValue'),
                SalaryStatus: $("#SalaryStatus").val(), Remark: $("#Remark").val()
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

    // -------------------------------------------------------Look Up EmployeePersonal-------------------------------------------------------
    $('#btnEmployeePersonal').click(function () {
        var lookUpURL = base_url + 'EmployeePersonal/GetList';
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

            $('#EmployeePersonalId').val(ret.id).data("kode", id);
            $('#EmployeePersonalCode').val(ret.code);
            $('#EmployeePersonalName').val(ret.name);
            $('#lookup_div_workingtime').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });
    // ---------------------------------------------End Lookup EmployeePersonal----------------------------------------------------------------


}); //END DOCUMENT READY