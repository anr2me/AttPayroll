$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ReloadGrid() {
        // Clear Search string jqGrid
        //$('input[id*="gs_"]').val("");

        var id = $('#parenttype option:selected').text();
        var value = $('#parenttype').val();

        $("#list").setGridParam({ url: base_url + 'WorkingTime/GetList', postData: { filters: null, ParentId: value }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ClearData() {
        $('#Code').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#CheckInTolerance').numberbox('setValue', '');
        $('#CheckOutTolerance').numberbox('setValue', '');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#parenttype").live("change", function () {
        ReloadGrid();
    });

    // set options globaly
    $.timeEntry.setDefaults({ show24Hours: true, spinnerImage: 'Content/spinnerText.png', spinnerSize: [30, 20, 8] }); // , spinnerBigImage: 'Content/spinnerTextBig.png', spinnerBigSize: [60, 40, 16]
    $('#MinCheckIn').timeEntry();
    $('#CheckIn').timeEntry();
    $('#MaxCheckIn').timeEntry();
    $('#MinCheckOut').timeEntry();
    $('#CheckOut').timeEntry();
    $('#MaxCheckOut').timeEntry();
    $('#BreakIn').timeEntry();
    $('#BreakOut').timeEntry();

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

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'WorkingTime/GetList',
        postData: { 'ParentId': function () { return $("#parenttype").val(); } },
        datatype: "json",
        colNames: ['ID', 'Code', 'Name', 'Min CheckIn', 'CheckIn', 'Max CheckIn', 'BreakOut', 'BreakIn','Min CheckOut', 'CheckOut', 'Max CheckOut', 'CheckIn Tolerance', 'CheckOut Tolerance', 'Work Interval', 'Break Interval', 'Time Zone', 'TZ Addon Offset', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'code', index: 'code', width: 100 },
				  { name: 'name', index: 'name', width: 180 },
                  { name: 'mincheckin', index: 'mincheckin', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'H:i' } },
                  { name: 'checkin', index: 'checkin', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'H:i' } },
                  { name: 'maxcheckin', index: 'maxcheckin', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'H:i' } },
                  { name: 'breakout', index: 'breakout', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'H:i' } },
                  { name: 'breakin', index: 'breakin', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'H:i' } },
                  { name: 'mincheckout', index: 'mincheckout', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'H:i' } },
                  { name: 'checkout', index: 'checkout', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'H:i' } },
                  { name: 'maxcheckout', index: 'maxcheckout', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { newformat: 'H:i' } },
                  { name: 'checkintolerance', index: 'checkintolerance', width: 100 },
                  { name: 'checkouttolerance', index: 'checkouttolerance', width: 100 },
                  { name: 'workinterval', index: 'workinterval', width: 100 },
                  { name: 'breakinterval', index: 'breakinterval', width: 100 },
                  { name: 'timezone', index: 'timezone', width: 100, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption("#TimeZone") } },
                  { name: 'TimeZoneOffset', index: 'TimeZoneOffset', width: 80 },
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
		          rowWorkInterval = ($(this).getRowData(cl).workinterval/60.0).toFixed(1);
		          $(this).jqGrid('setRowData', ids[i], { workinterval: rowWorkInterval });

                  rowBreakInterval = ($(this).getRowData(cl).breakinterval/60.0).toFixed(1);
                  $(this).jqGrid('setRowData', ids[i], { breakinterval: rowBreakInterval });
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
        $('#MinCheckIn').val('00:00'); //datebox('setValue', timeEnt(result.MinCheckIn));
        $('#CheckIn').val('00:00'); //datebox('setValue', timeEnt(result.CheckIn));
        $('#MaxCheckIn').val('00:00');
        $('#MinCheckOut').val('00:00');
        $('#CheckOut').val('00:00');
        $('#MaxCheckOut').val('00:00');
        $('#BreakIn').val('00:00');
        $('#BreakOut').val('00:00');
        $('#TimeZoneOffset').val('0');
        //var tzoffset = new Date().getTimezoneOffset(); // the difference, in minutes, between UTC and local time
        var timezone = jstz.determine();
        var tzname = timezone.name(); // eg. "Asia/Jakarta"
        var fixtzname = tzname.split('/')[1].split('_').join(' ').toUpperCase(); // convert "Asia/Jakarta" to "JAKARTA"
        //$('#TimeZone option').each(function () {
        $('#TimeZone').find('option').each(function () { // find an (uppercased) option's text which contains "JAKARTA"
            if ($(this).text().toUpperCase().indexOf(fixtzname) >= 0) { //($(this).is(':contains(' + fixtzname + ')'))
                fixtzname = $(this).val();
                return false; //break from iteration
            }
        });
        $('#TimeZone').val(fixtzname);
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
                url: base_url + "WorkingTime/GetInfo?Id=" + id,
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
                            $('#Code').val(result.Code);
                            $('#Name').val(result.Name);
                            $('#TimeZone').val(result.TimeZone);
                            $('#TimeZoneOffset').val(result.TimeZoneOffset);
                            $('#MinCheckIn').val(timeEnt(result.MinCheckIn)); //datebox('setValue', timeEnt(result.MinCheckIn));
                            $('#CheckIn').val(timeEnt(result.CheckIn)); //datebox('setValue', timeEnt(result.CheckIn));
                            $('#MaxCheckIn').val(timeEnt(result.MaxCheckIn));
                            $('#MinCheckOut').val(timeEnt(result.MinCheckOut));
                            $('#CheckOut').val(timeEnt(result.CheckOut));
                            $('#MaxCheckOut').val(timeEnt(result.MaxCheckOut));
                            $('#BreakIn').val(timeEnt(result.BreakIn));
                            $('#BreakOut').val(timeEnt(result.BreakOut));
                            $('#CheckInTolerance').numberbox('setValue', result.CheckInTolerance);
                            $('#CheckOutTolerance').numberbox('setValue', result.CheckOutTolerance);
                            $('#WorkInterval').val((result.WorkInterval / 60.0).toFixed(1)); //numberbox('setValue', result.WorkInterval/60.0);
                            $('#BreakInterval').val((result.BreakInterval / 60.0).toFixed(1)); //numberbox('setValue', result.BreakInterval/60.0);
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
            url: base_url + "WorkingTime/Delete",
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
            submitURL = base_url + 'WorkingTime/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'WorkingTime/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Code: $("#Code").val(), Name: $("#Name").val(), 
                MinCheckIn: $("#MinCheckIn").val(), CheckIn: $("#CheckIn").val(), MaxCheckIn: $("#MaxCheckIn").val(),
                MinCheckOut: $("#MinCheckOut").val(), CheckOut: $("#CheckOut").val(), MaxCheckOut: $("#MaxCheckOut").val(),
                BreakIn: $("#BreakIn").val(), BreakOut: $("#BreakOut").val(),
                CheckInTolerance: $("#CheckInTolerance").numberbox('getValue'), CheckOutTolerance: $("#CheckOutTolerance").numberbox('getValue'),
                TimeZone: $("#TimeZone").val(), TimeZoneOffset: $("#TimeZoneOffset").val(),
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


    
}); //END DOCUMENT READY