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
        $('#CheckInTolerance').numberbox('setValue', '');
        $('#CheckOutTolerance').numberbox('setValue', '');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }


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

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');

    function ReloadGridListParent() {
        $("#detail_panel").hide();
        jQuery("#list").jqGrid({
            url: base_url + 'WorkingTime/GetList',
            postData: { 'ParentId': function () { return $("#parenttype").val(); } },
            datatype: "json",
            colNames: ['ID', 'Code', 'Name', 'Min CheckIn', 'CheckIn', 'Max CheckIn', 'BreakOut', 'BreakIn', 'Min CheckOut', 'CheckOut', 'Max CheckOut', 'CheckIn Tolerance', 'CheckOut Tolerance', 'Work Interval', 'Break Interval', 'Created At', 'Updated At'],
            colModel: [
                      { name: 'id', index: 'id', width: 50, align: "center", frozen: true },
                      { name: 'code', index: 'code', width: 100, frozen: true },
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
                      rowWorkInterval = ($(this).getRowData(cl).workinterval / 60.0).toFixed(1);
                      $(this).jqGrid('setRowData', ids[i], { workinterval: rowWorkInterval });

                      rowBreakInterval = ($(this).getRowData(cl).breakinterval / 60.0).toFixed(1);
                      $(this).jqGrid('setRowData', ids[i], { breakinterval: rowBreakInterval });
                  }
              }

        });//END GRID
        $("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list").jqGrid('setFrozenColumns');
    //    $("#list").setGridParam({ url: base_url + 'WorkingTime/GetList', page: '1' }).trigger("reloadGrid");
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
            url: base_url + "WorkingDay/UpdateEnable?isEnabled=" + isEnabled,
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
            url: base_url + 'WorkingDay/GetListWorkingDay?Id=' + id,
            //postData: { 'ParentId': function () { return $("#parenttype").val(); } },
            datatype: "json",
            colNames: ['ID', 'Code', 'Name', 'Enabled', 'Min CheckIn', 'CheckIn', 'Max CheckIn', 'BreakOut', 'BreakIn', 'Min CheckOut', 'CheckOut', 'Max CheckOut', 'CheckIn Tolerance', 'CheckOut Tolerance', 'Work Interval', 'Break Interval', 'Created At', 'Updated At'],
            colModel: [
                      { name: 'id', index: 'id', width: 80, align: "center", frozen:true },
                      { name: 'code', index: 'code', width: 100, frozen: true },
                      { name: 'name', index: 'name', width: 180 },
                      {
                          name: 'isenabled', index: 'isenabled', width: 50, align: 'center', search: false,
                          editable: true,
                          edittype: 'checkbox', editoptions: { value: "1:0" },
                          formatter: cboxIsEnabled, formatoptions: { disabled: false }
                      },
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
                      rowWorkInterval = ($(this).getRowData(cl).workinterval / 60.0).toFixed(1);
                      $(this).jqGrid('setRowData', ids[i], { workinterval: rowWorkInterval });

                      rowBreakInterval = ($(this).getRowData(cl).breakinterval / 60.0).toFixed(1);
                      $(this).jqGrid('setRowData', ids[i], { breakinterval: rowBreakInterval });
                  }
              }

        });//END GRID Detail
        $("#list_detail").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list_detail").jqGrid('setFrozenColumns');
    }

    ReloadGridListParent();

    $('#btn_warehouse').click(function () {
        a = 'warehouse';
        $("#list").jqGrid('GridUnload');
        ReloadGridListParent();
    });
 
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
                url: base_url + "WorkingDay/GetInfo?Id=" + id,
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
                            //$('#IsEnabled').val(result.IsEnabled?'1':'0');
                            document.getElementById("IsEnabled").checked = result.IsEnabled;
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
            url: base_url + "WorkingDay/Delete",
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
            submitURL = base_url + 'WorkingDay/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'WorkingDay/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Code: $("#Code").val(), Name: $("#Name").val(), IsEnabled: document.getElementById("IsEnabled").checked?'true':'false',
                MinCheckIn: $("#MinCheckIn").val(), CheckIn: $("#CheckIn").val(), MaxCheckIn: $("#MaxCheckIn").val(),
                MinCheckOut: $("#MinCheckOut").val(), CheckOut: $("#CheckOut").val(), MaxCheckOut: $("#MaxCheckOut").val(),
                BreakIn: $("#BreakIn").val(), BreakOut: $("#BreakOut").val(),
                CheckInTolerance: $("#CheckInTolerance").numberbox('getValue'), CheckOutTolerance: $("#CheckOutTolerance").numberbox('getValue'),
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

}); //END DOCUMENT READY