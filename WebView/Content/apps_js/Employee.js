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

        $("#list").setGridParam({ url: base_url + 'Employee/GetList', postData: { filters: null, ParentId: value }, page: 'first' }).trigger("reloadGrid");
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
        $('#NonActiveDate').datebox('setValue', '');
        document.getElementById("Sex").selectedIndex = 0;
        document.getElementById("MaritalStatus").selectedIndex = 0;
        document.getElementById("Religion").selectedIndex = 0;
        document.getElementById("WorkingStatus").selectedIndex = 0;
        document.getElementById("ActiveStatus").selectedIndex = 0;
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    //$("#fileName").pluploadQueue({
    //    // General settings
    //    runtimes: 'html5,flash,silverlight,html4',
    //    url: '/examples/upload',
    //    chunk_size: '1mb',
    //    unique_names: true,

    //    // Resize images on client-side if we can
    //    resize: { width: 320, height: 240, quality: 90 },

    //    filters: {
    //        max_file_size: '10mb',

    //        // Specify what files to browse for
    //        mime_types: [
    //            { title: "Image files", extensions: "jpg,gif,png" },
    //            { title: "Zip files", extensions: "zip" }
    //        ]
    //    },

    //    // Flash settings
    //    flash_swf_url: '/plupload/js/Moxie.swf',

    //    // Silverlight settings
    //    silverlight_xap_url: '/plupload/js/Moxie.xap',

    //    // PreInit events, bound before any internal events
    //    preinit: {
    //        Init: function (up, info) {
    //            log('[Init]', 'Info:', info, 'Features:', up.features);
    //        },

    //        UploadFile: function (up, file) {
    //            log('[UploadFile]', file);

    //            // You can override settings before the file is uploaded
    //            // up.setOption('url', 'upload.php?id=' + file.id);
    //            // up.setOption('multipart_params', {param1 : 'value1', param2 : 'value2'});
    //        }
    //    },

    //    // Post init events, bound after the internal events
    //    init: {
    //        PostInit: function () {
    //            // Called after initialization is finished and internal event handlers bound
    //            log('[PostInit]');

    //            document.getElementById('uploadfiles').onclick = function () {
    //                uploader.start();
    //                return false;
    //            };
    //        },

    //        Browse: function (up) {
    //            // Called when file picker is clicked
    //            log('[Browse]');
    //        },

    //        Refresh: function (up) {
    //            // Called when the position or dimensions of the picker change
    //            log('[Refresh]');
    //        },

    //        StateChanged: function (up) {
    //            // Called when the state of the queue is changed
    //            log('[StateChanged]', up.state == plupload.STARTED ? "STARTED" : "STOPPED");
    //        },

    //        QueueChanged: function (up) {
    //            // Called when queue is changed by adding or removing files
    //            log('[QueueChanged]');
    //        },

    //        OptionChanged: function (up, name, value, oldValue) {
    //            // Called when one of the configuration options is changed
    //            log('[OptionChanged]', 'Option Name: ', name, 'Value: ', value, 'Old Value: ', oldValue);
    //        },

    //        BeforeUpload: function (up, file) {
    //            // Called right before the upload for a given file starts, can be used to cancel it if required
    //            log('[BeforeUpload]', 'File: ', file);
    //        },

    //        UploadProgress: function (up, file) {
    //            // Called while file is being uploaded
    //            log('[UploadProgress]', 'File:', file, "Total:", up.total);
    //        },

    //        FileFiltered: function (up, file) {
    //            // Called when file successfully files all the filters
    //            log('[FileFiltered]', 'File:', file);
    //        },

    //        FilesAdded: function (up, files) {
    //            // Called when files are added to queue
    //            log('[FilesAdded]');

    //            plupload.each(files, function (file) {
    //                log('  File:', file);
    //            });
    //        },

    //        FilesRemoved: function (up, files) {
    //            // Called when files are removed from queue
    //            log('[FilesRemoved]');

    //            plupload.each(files, function (file) {
    //                log('  File:', file);
    //            });
    //        },

    //        FileUploaded: function (up, file, info) {
    //            // Called when file has finished uploading
    //            log('[FileUploaded] File:', file, "Info:", info);
    //        },

    //        ChunkUploaded: function (up, file, info) {
    //            // Called when file chunk has finished uploading
    //            log('[ChunkUploaded] File:', file, "Info:", info);
    //        },

    //        UploadComplete: function (up, files) {
    //            // Called when all files are either uploaded or failed
    //            log('[UploadComplete]');
    //        },

    //        Destroy: function (up) {
    //            // Called when uploader is destroyed
    //            log('[Destroy] ');
    //        },

    //        Error: function (up, args) {
    //            // Called when error occurs
    //            log('[Error] ', args);
    //        }
    //    }
    //});

    //$("#fileName").uploadify({
    //    height: 30,
    //    swf: 'Content/uploadify/uploadify.swf',
    //    uploader: 'Uploadify',
    //    width: 120,
    //    'buttonText': 'Browse Files',
    //    'fileDesc': 'Excel Files',
    //    'fileExt': '*.xls;*.xlsx',
    //});

    $('#fileName').uploadify({
        'height': 20,
        'fileTypeDesc': 'Excel Files',
        'fileTypeExts': '*.xls; *.xlsx',
        'swf': '../Content/uploadify/uploadify.swf',
        'uploader': '../Uploadify/UploadEmployee', //'../Content/uploadify/Upload.ashx', //uploader.php // '<%= Url.Action("Upload", "Uploadify") %>'
        //'cancelImage': '../Content/uploadify/uploadify-cancel.png', //'Content/uploadify/uploadify-cancel.png', // cancelImg
        'buttonText': 'Browse Files',
        //'script': '../Content/uploadify/Upload.ashx?prefix=BBB', // not used in v3 ?
        //'scriptData': { 'prefix': 'Employee' }, // query params
        //'folder': '/uploads', // not used in v3 ?
        //'fileSizeLimit' : '20MB',
        'queueSizeLimit': 1,
        'uploadLimit': 1,
        //'displayData': 'percentage',
        'multi': false,
        'auto': false,
        'onUploadSuccess': function (file, data, response) {
            alert('The file was successfully saved to: ' + data);
        }
    });

    $('#btn_upload').click(function () {
        //$('#fileName').uploadifySettings("scriptData", { 'prefix': 'Employee' });
        $('#fileName').uploadify('upload'); //('upload', '*')
    });

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
    $("#delete_confirm_div").dialog('close');
    $("#upload_form_div").dialog('close');
    $('#lookup_div_titleinfo').dialog('close');
    $('#lookup_div_division').dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'Employee/GetList',
        postData: { 'ParentId': function () { return $("#parenttype").val(); } },
        datatype: "json",
        colNames: ['ID', 'NIK', 'Name', 'Title', 'Religion', 'Place of Birth', 'Birth Date', 'Address', 'Phone Number', 'Email', 'Sex', 'Marital Status', 'Children', 'NPWP', 'NPWP Date', 'Jamsostek', 'Working Status', 'Start Working', 'Appointment', 'Is Active', 'Non-Active Date', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'nik', index: 'nik', width: 100 },
				  { name: 'name', index: 'name', width: 180 },
                  { name: 'titleinfoname', index: 'titleinfoname', width: 100 },
                  { name: 'religion', index: 'religion', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Religion") } },
                  { name: 'placeofbirth', index: 'placeofbirth', width: 100 },
                  { name: 'birthdate', index: 'birthdate', hidden: true, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'address', index: 'address', width: 250 },
                  { name: 'phonenumber', index: 'phonenumber', width: 100 },
                  { name: 'email', index: 'email', width: 100 },
                  { name: 'sex', index: 'sex', width: 100, stype: 'select', editoptions: { value: getSelectOption("#Sex") } }, // ':All;0:Male;1:Female'
                  { name: 'maritalstatus', index: 'maritalstatus', width: 100, stype: 'select', editoptions: { value: getSelectOption("#MaritalStatus") } },
                  { name: 'children', index: 'children', width: 100 },
                  { name: 'npwp', index: 'npwp', width: 100 },
                  { name: 'npwpdate', index: 'npwpdate', hidden:true, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'jamsostek', index: 'jamsostek', width: 100 },
                  { name: 'workingstatus', index: 'workingstatus', width: 100, stype: 'select', editoptions: { value: getSelectOption("#WorkingStatus") } },
                  { name: 'startworkingdate', index: 'startworkingdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'appointmentdate', index: 'appointmentdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'activestatus', index: 'activestatus', width: 100, stype: 'select', editoptions: { value: getSelectOption("#ActiveStatus") } },
                  { name: 'nonactivedate', index: 'nonactivedate', hidden: true, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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

		          rowBirth = $(this).getRowData(cl).placeofbirth + ", " + $(this).getRowData(cl).birthdate;
		          $(this).jqGrid('setRowData', ids[i], { placeofbirth: rowBirth });

		          rowNPWP = $(this).getRowData(cl).npwp + ", " + $(this).getRowData(cl).npwpdate;
		          $(this).jqGrid('setRowData', ids[i], { npwp: rowNPWP });

		          rowSex = document.getElementById("Sex").options[$(this).getRowData(cl).sex].text;
		          $(this).jqGrid('setRowData', ids[i], { sex: rowSex });

		          rowMarital = document.getElementById("MaritalStatus").options[$(this).getRowData(cl).maritalstatus].text;
		          $(this).jqGrid('setRowData', ids[i], { maritalstatus: rowMarital });

		          rowReligion = document.getElementById("Religion").options[$(this).getRowData(cl).religion].text;
		          $(this).jqGrid('setRowData', ids[i], { religion: rowReligion });

		          rowWorkingStatus = document.getElementById("WorkingStatus").options[$(this).getRowData(cl).workingstatus].text;
		          $(this).jqGrid('setRowData', ids[i], { workingstatus: rowWorkingStatus });
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

    $('#btn_imp').click(function () {
        ClearData();
        clearForm('#upload_frm');
        vStatusSaving = 0; //add data mode
        $('#upload_form_div').dialog('open');
    });

    $('#upload_form_btn_cancel').click(function () {
        vStatusSaving = 0;
        clearForm('#upload_frm');
        $("#upload_form_div").dialog('close');
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
                url: base_url + "Employee/GetInfo?Id=" + id,
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
                            $('#NonActiveDate').datebox('setValue', dateEnt(result.NonActiveDate));
                            document.getElementById("Sex").selectedIndex = result.Sex;
                            document.getElementById("MaritalStatus").selectedIndex = result.MaritalStatus;
                            document.getElementById("Religion").selectedIndex = result.Religion;
                            document.getElementById("WorkingStatus").selectedIndex = result.WorkingStatus;
                            document.getElementById("ActiveStatus").selectedIndex = result.ActiveStatus;
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
            url: base_url + "Employee/Delete",
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
            submitURL = base_url + 'Employee/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'Employee/Insert';
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