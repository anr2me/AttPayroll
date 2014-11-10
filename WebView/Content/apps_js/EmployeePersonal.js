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

        $("#list").setGridParam({ url: base_url + 'EmployeePersonal/GetList', postData: { filters: null, ParentId: value }, page: 'first' }).trigger("reloadGrid");
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
        $('#NPWP').val('');
        $('#Jamsostek').val('');
        $('#Children').numberbox('setValue', '');
        $('#BirthDate').datebox('setValue', '');
        $('#NPWPDate').datebox('setValue', '');
        $('#StartWorkingDate').datebox('setValue', '');
        $('#AppointmentDate').datebox('setValue', '');
        document.getElementById("Sex").selectedIndex = 0;
        document.getElementById("MaritalStatus").selectedIndex = 0;
        document.getElementById("Religion").selectedIndex = 0;
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#parenttype").live("change", function () {
        ReloadGrid();
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

    $("#form_div").dialog('close');
    $("#fin_form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $('#lookup_div_titleinfo').dialog('close');
    $('#lookup_div_division').dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'EmployeePersonal/GetList',
        //postData: { 'ParentId': function () { return $("#parenttype").val(); } },
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
    			  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'firstname', index: 'firstname', width: 100 },
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
                  { name: 'jenisgoldarah', index: 'jenisgoldarah', hidden:true, width: 60 },
                  { name: 'maritalstatus', index: 'maritalstatus', width: 100, stype: 'select', editoptions: { value: getSelectOption("#MaritalStatus") } },
                  { name: 'marrieddate', index: 'marrieddate', hidden: true, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'lasteducation', index: 'lasteducation', width: 100 },
                  { name: 'religion', index: 'religion', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Religion") } },
                  { name: 'nationality', index: 'nationality', width: 100 },
                  { name: 'childnumber', index: 'childnumber', width: 50 },
                  { name: 'outofbrothers', index: 'outofbrothers', width: 50 },

                  { name: 'emergencycontactname', index: 'emergencycontactname', width: 100 },
                  { name: 'emergencyphonenumber', index: 'emergencyphonenumber', width: 80 },
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
                  { name: 'othergrade', index: 'othergrade', width: 100, hidden:true, stype: 'select', editoptions: { value: getSelectOption("#OtherGrade") } },
                  
                  { name: 'isdeleted', index: 'isdeleted', width: 50, hidden:true },
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
		          rowActiveStatus = $(this).getRowData(cl).activestatus;
		          if (rowActiveStatus == '0') {
		              rowActiveStatus = "NO, " + $(this).getRowData(cl).nonactivedate;
		          } else {
		              rowActiveStatus = "YES";
		          }
		          $(this).jqGrid('setRowData', ids[i], { activestatus: rowActiveStatus });

		          $(this).jqGrid('setRowData', ids[i], { goldarah: $(this).getRowData(cl).goldarah + " "+ $(this).getRowData(cl).jenisgoldarah });

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
                url: base_url + "EmployeePersonal/GetInfo?Id=" + id,
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
                            $('#NIK').val(result.NIK);
                            $('#FirstName').val(result.FirstName);
                            $('#LastName').val(result.LastName);
                            $('#NickName').val(result.NickName);
                            $('#TitleInfoId').val(result.TitleInfoId);
                            $('#TitleInfoName').val(result.TitleInfoName);
                            $('#DivisionId').val(result.DivisionId);
                            $('#DivisionName').val(result.DivisionName);
                            $('#SupervisorId').val(result.SupervisorId);
                            $('#SupervisorName').val(result.SupervisorName);
                            $('#SuperiorId').val(result.SuperiorId);
                            $('#SuperiorName').val(result.SuperiorName);
                            $('#PlaceOfBirth').val(result.PlaceOfBirth);
                            $('#Address').val(result.Address);
                            $('#City').val(result.City);
                            $('#PostalCode').val(result.PostalCode);
                            $('#PhoneNumber').val(result.PhoneNumber);
                            $('#Email').val(result.Email);
                            $('#MobileNumber').val(result.MobileNumber);
                            $('#ExtensionNumber').val(result.ExtensionNumber);
                            $('#Secretary').val(result.Secretary);
                            $('#NPWP').val(result.NPWP);
                            $('#GolDarah').val(result.GolDarah);
                            $('#JenisGolDarah').val(result.JenisGolDarah);
                            $('#Jamsostek').val(result.JamsostekNo);
                            $('#LastEducation').val(result.LastEducation);
                            $('#Nationality').val(result.Nationality);
                            $('#Remark').val(result.Remark);
                            $('#IDNumber').val(result.IDNumber);
                            $('#IDAddress').val(result.IDAddress);
                            $('#IDCity').val(result.IDCity);
                            $('#IDPostalCode').val(result.IDPostalCode);
                            $('#OtherLang').val(result.OtherLang);
                            $('#EmergencyContactName').val(result.EmergencyContactName);
                            $('#EmergencyPhoneNumber').val(result.EmergencyPhoneNumber);
                            $('#EmergencyContactRemark').val(result.EmergencyContactRemark);
                            $('#Children').numberbox('setValue', result.Children);
                            $('#ChildNumber').numberbox('setValue', result.ChildNumber);
                            $('#OutOfBrothers').numberbox('setValue', result.OutOfBrothers);
                            $('#BirthDate').datebox('setValue', dateEnt(result.BirthDate));
                            $('#MarriedDate').datebox('setValue', dateEnt(result.MarriedDate));
                            $('#ExpirationDate').datebox('setValue', dateEnt(result.ExpirationDate));
                            $('#NPWPDate').datebox('setValue', dateEnt(result.NPWPDate));
                            $('#JoinedDate').datebox('setValue', dateEnt(result.JoinedDate));
                            $('#StartingDate').datebox('setValue', dateEnt(result.StartingDate));
                            $('#EndingDate').datebox('setValue', dateEnt(result.EndingDate));
                            $('#StartWorkingDate').datebox('setValue', dateEnt(result.StartWorkingDate));
                            $('#AppointmentDate').datebox('setValue', dateEnt(result.AppointmentDate));
                            document.getElementById("IDType").selectedIndex = result.IDType;
                            document.getElementById("JobType").selectedIndex = result.JobType;
                            document.getElementById("Sex").selectedIndex = result.Sex;
                            document.getElementById("MaritalStatus").selectedIndex = result.MaritalStatus;
                            document.getElementById("Religion").selectedIndex = result.Religion;
                            document.getElementById("Inggris").selectedIndex = result.Inggris;
                            document.getElementById("Mandarin").selectedIndex = result.Mandarin;
                            document.getElementById("Indonesia").selectedIndex = result.Indonesia;
                            document.getElementById("OtherGrade").selectedIndex = result.OtherGrade;
                            $("#form_div").dialog("open");
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_edit_fin').click(function () {
        ClearData();
        clearForm("#fin_frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            vStatusSaving = 1;//edit data mode
            $.ajax({
                dataType: "json",
                url: base_url + "EmployeePersonal/GetInfo?Id=" + id,
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
                            $('#NIK').val(result.NIK);
                            $('#Name').val(result.Name);
                            $('#TitleInfoId').val(result.TitleInfoId);
                            $('#TitleInfoName').val(result.TitleInfoName);
                            $('#DivisionId').val(result.DivisionId);
                            $('#DivisionName').val(result.DivisionName);
                            $('#PlaceOfBirth').val(result.PlaceOfBirth);
                            $('#Address').val(result.Address);
                            $('#PhoneNumber').val(result.PhoneNumber);
                            $('#Email').val(result.Email);
                            $('#NPWP').val(result.NPWP);
                            $('#Jamsostek').val(result.JamsostekNo);
                            $('#Children').numberbox('setValue', result.Children);
                            $('#BirthDate').datebox('setValue', dateEnt(result.BirthDate));
                            $('#NPWPDate').datebox('setValue', dateEnt(result.NPWPDate));
                            $('#StartWorkingDate').datebox('setValue', dateEnt(result.StartWorkingDate));
                            $('#AppointmentDate').datebox('setValue', dateEnt(result.AppointmentDate));
                            document.getElementById("Sex").selectedIndex = result.Sex;
                            document.getElementById("MaritalStatus").selectedIndex = result.MaritalStatus;
                            document.getElementById("Religion").selectedIndex = result.Religion;
                            $("#fin_form_div").dialog("open");
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
            url: base_url + "EmployeePersonal/Delete",
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
            submitURL = base_url + 'EmployeePersonal/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'EmployeePersonal/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, NIK: $("#NIK").val(), Name: $("#Name").val(), TitleInfoId: $('#TitleInfoId').val(), DivisionId: $('#DivisionId').val(),
                PlaceOfBirth: $('#PlaceOfBirth').val(), Address: $('#Address').val(), PhoneNumber: $('#PhoneNumber').val(),
                Email: $('#Email').val(), NPWP: $('#NPWP').val(), JamsostekNo: $('#Jamsostek').val(),
                Children: $('#Children').numberbox('getValue'), BirthDate: $('#BirthDate').datebox('getValue'),
                NPWPDate: $('#NPWPDate').datebox('getValue'), StartWorkingDate: $('#StartWorkingDate').datebox('getValue'),
                AppointmentDate: $('#AppointmentDate').datebox('getValue'), NonActiveDate: $('#NonActiveDate').datebox('getValue'),
                Sex: document.getElementById("Sex").selectedIndex, MaritalStatus: document.getElementById("MaritalStatus").selectedIndex,
                Religion: document.getElementById("Religion").selectedIndex, WorkingStatus: document.getElementById("WorkingStatus").selectedIndex,
                ActiveStatus: document.getElementById("ActiveStatus").selectedIndex
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

    $('#fin_form_btn_cancel').click(function () {
        vStatusSaving = 0;
        clearForm('#fin_frm');
        $("#fin_form_div").dialog('close');
    });

    $("#fin_form_btn_save").click(function () {
        ClearErrorMessage();

        var submitURL = '';
        var id = $("#fin_form_btn_save").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'EmployeePersonal/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'EmployeePersonal/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, NIK: $("#NIK").val(), Name: $("#Name").val(), TitleInfoId: $('#TitleInfoId').val(), DivisionId: $('#DivisionId').val(),
                PlaceOfBirth: $('#PlaceOfBirth').val(), Address: $('#Address').val(), PhoneNumber: $('#PhoneNumber').val(),
                Email: $('#Email').val(), NPWP: $('#NPWP').val(), JamsostekNo: $('#Jamsostek').val(),
                Children: $('#Children').numberbox('getValue'), BirthDate: $('#BirthDate').datebox('getValue'),
                NPWPDate: $('#NPWPDate').datebox('getValue'), StartWorkingDate: $('#StartWorkingDate').datebox('getValue'),
                AppointmentDate: $('#AppointmentDate').datebox('getValue'), NonActiveDate: $('#NonActiveDate').datebox('getValue'),
                Sex: document.getElementById("Sex").selectedIndex, MaritalStatus: document.getElementById("MaritalStatus").selectedIndex,
                Religion: document.getElementById("Religion").selectedIndex, WorkingStatus: document.getElementById("WorkingStatus").selectedIndex,
                ActiveStatus: document.getElementById("ActiveStatus").selectedIndex
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
                    $("#fin_form_div").dialog('close')
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