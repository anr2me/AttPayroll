$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ReloadGrid() {
        // Clear Search string jqGrid
        //$('input[id*="gs_"]').val("");

        var id = $('#parenttype option:selected').text();
        var value = $('#parenttype').val();
        var curPage = $('#list').getGridParam('page');

        $("#list").setGridParam({ url: base_url + 'SalarySlip/GetList', postData: { filters: null, ParentId: value }, page: curPage }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'SalarySlip/GetListDetail?Id=' + $("#id").val(), postData: { filters: null } }).trigger("reloadGrid");
    }

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ClearData() {
        $('#Code').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        ClearErrorMessage();
    }

    //$("#datatable").multicolselect({
    //    buttonImage: "Content/spinnerText.png", //"selectbutton.gif",
    //    valueCol: 1,
    //    hideCol: 0
    //});

    function fillData() {
        $("#FirstSalaryItem").data('kode', "0");
        $("#SecondSalaryItem").data('kode', "0");
        $.ajax({
            url: base_url + "SalaryItem/GetList",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                filters: null,
                nd: new Date().getTime(), // Date.parse(new Date()),
                rows: 32767,
                page: 1,
                sidx: "Id",
                sord: "asc",
            }),
            success: function (result) {
                var objDropDown = $('#FirstSalaryItem'); // $(this).parent('td').next().find('select');
                var objDropDown2 = $('#SecondSalaryItem');
                $(objDropDown).empty();
                $(objDropDown2).empty();
                //$(objDropDown).append('<option value="0">--Select Department--</option>');
                //$(objDropDown).append('<tr><th>ID</th><th>Code</th><th>Name</th></tr>');
                if (result != null) {
                    for (var item in result.rows) {
                        $(objDropDown).append('<option value="' + result.rows[item].cell[0] + '">' + result.rows[item].cell[1] + '</option>');
                        $(objDropDown2).append('<option value="' + result.rows[item].cell[0] + '">' + result.rows[item].cell[1] + '</option>');
                        //$(objDropDown).append('<tr><td>' + result.rows[item].cell[0] + '</td><td>' + result.rows[item].cell[1] + '</td><td>' + result.rows[item].cell[2] + '</td></tr>');
                    }
                    document.getElementById('FirstSalaryItem').options[$("#FirstSalaryItem").data('kode')].selected = true;
                    document.getElementById('SecondSalaryItem').options[$("#SecondSalaryItem").data('kode')].selected = true;
                }
            }
        });
        
    }

    $("#parenttype").live("change", function () {
        ReloadGrid();
    });

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $('#lookup_div_salaryitem').dialog('close');
    $("#detail_div").dialog('close');

    // CheckBox
    function UpdateEnable(objId /*, isEnabled, isMain, isDetail*/) {
        //var userId = 0;
        //var id = jQuery("#tbl_users").jqGrid('getGridParam', 'selrow');
        //if (id) {
        //    userId = id;
        //} else {
        //    $.messager.alert('Information', 'Please Select User...!!', 'info');
        //    return;
        //};

        isEnabled = $("input[name=cbFormat_isenabled" + objId + "]").is(":checked");
        isMain = $("input[name=cbFormat_mainsalary" + objId + "]").is(":checked");
        isDetail = $("input[name=cbFormat_detailsalary" + objId + "]").is(":checked");

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "SalarySlip/UpdateEnable?isEnabled=" + isEnabled + "&isMain=" + isMain + "&isDetail=" + isDetail,
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

    $("input[name*=cbFormat_]").live("click", function () {
        UpdateEnable($(this).attr('rel')/*, $(this).is(":checked")*/);
    });

    function cboxFormat(cellvalue, options, rowObject) {
        return '<input name="cbFormat_' + options.colModel.index + options.rowId + '" rel="' + /*rowObject.id*/options.rowId + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    //$("input[name*=cbIsEnabled]").live("click", function () {
    //    UpdateEnable($(this).attr('rel')/*, $(this).is(":checked")*/);
    //});

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'SalarySlip/GetList',
        postData: { 'ParentId': function () { return $("#parenttype").val(); } },
        datatype: "json",
        colNames: ['ID', 'Index', 'Code', 'Name', 'SalaryItem ID', 'Salary Sign', 'Enabled', 'Main Salary', 'Detail Salary', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'index', index: 'index', width: 50 },
				  { name: 'code', index: 'code', width: 100 },
                  { name: 'name', index: 'name', width: 200 },
                  { name: 'salaryitemid', index: 'salaryitemid', width: 50, hidden: true },
                  { name: 'salarysign', index: 'salarysign', width: 100, stype: 'select', editoptions: { value: getSelectOption("#SalarySign") } },
                  {
                      name: 'isenabled', index: 'isenabled', width: 80, align: 'center', search: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxFormat, formatoptions: { disabled: false }
                  },
                  {
                      name: 'mainsalary', index: 'mainsalary', width: 80, align: 'center', search: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxFormat, formatoptions: { disabled: false }
                  },
                  {
                      name: 'detailsalary', index: 'detailsalary', width: 80, align: 'center', search: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxFormat, formatoptions: { disabled: false }
                  },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'index',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
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
		          var rowSign = "Income";
		          if ($(this).getRowData(cl).salarysign < 0) {
		              rowSign = "Expense";
		          }
		          //rowSign = document.getElementById("SalarySign").options[$(this).getRowData(cl).salarysign].text;
		          $(this).jqGrid('setRowData', ids[i], { salarysign: rowSign });
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
        $('#Description').val('').text('').removeClass('errormessage');
        //$('#EffectiveDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        //$('#EffectiveDateDiv2').hide();
        //$('#EffectiveDateDiv').show();
        $('#Index').numberbox('setValue', '');
        $('#btnSalaryItem').removeAttr('disabled');
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
                url: base_url + "SalarySlip/GetInfo?Id=" + id,
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
                            $('#SalaryItemName').val(result.SalaryItemName);
                            $('#Index').numberbox('setValue', result.Index);
                            $('#Code').val(result.Code);
                            $('#Name').val(result.Name);
                            $('#SalarySign').val(result.SalarySign);
                            document.getElementById('IsEnabled').checked = result.IsEnabled;
                            document.getElementById('IsMain').checked = result.IsMainSalary;
                            document.getElementById('IsDetail').checked = result.IsDetailSalary;
                            $('#Description').val(result.Description);
                            $('#Description').removeAttr('disabled');
                            $('#btnSalaryItem').removeAttr('disabled');
                            //$('#EffectiveDate').datebox('setValue', dateEnt(result.EffectiveDate));
                            //$('#EffectiveDateDiv2').hide();
                            //$('#EffectiveDateDiv').show();
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
            url: base_url + "SalarySlip/Delete",
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
            submitURL = base_url + 'SalarySlip/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'SalarySlip/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Name: $("#Name").val(), Code: $("#Code").val(),
                Index: $("#Index").numberbox('getValue'), SalarySign: $('#SalarySign').val(),
                IsEnabled: (document.getElementById("IsEnabled").checked ? 'true' : 'false'),
                IsMainSalary: (document.getElementById("IsMain").checked ? 'true' : 'false'),
                IsDetailSalary: (document.getElementById("IsDetail").checked ? 'true' : 'false'),
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
        colNames: ['Index', 'SalarySlip ID', 'Salary Sign', 'Formula ID', 'First Operand', 'Operator', 'Second Operand', 'Has MinValue', 'Min Value', 'Has MaxValue', 'Max Value'],
        colModel: [
                  { name: 'index', index: 'index', width: 50, sortable: true },
				  { name: 'salaryslipid', index: 'salaryslipid', hidden: true, width: 50, sortable: false },
				  { name: 'salarysign', index: 'salarysign', width: 80, sortable: true, stype: 'select', editoptions: { value: getSelectOption("#SalarySign") } },
                  { name: 'formulaid', index: 'formulaid', hidden: true, width: 50, sortable: false },
				  { name: 'firstcode', index: 'firstcode', width: 100, sortable: true },
				  { name: 'formulaop', index: 'formulaop', width: 60, sortable: true, stype: 'select', editoptions: { value: getSelectOption("#FormulaOp") } },
                  { name: 'secondcode', index: 'secondcode', width: 100, sortable: true },
                  { name: 'hasminvalue', index: 'hasminvalue', width: 100, sortable: true, stype: 'select', editoptions: { value: ":;false:No;true:Yes" } },
                  { name: 'minvalue', index: 'minvalue', width: 100, hidden: true, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: true },
                  { name: 'hasmaxvalue', index: 'hasmaxvalue', width: 100, sortable: true, stype: 'select', editoptions: { value: ":;false:No;true:Yes" } },
				  { name: 'maxvalue', index: 'maxvalue', width: 100, hidden: true, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: true },
        ],
        page: '1',
        pager: $('#pagerdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'index',
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
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowMinVal = $(this).getRowData(cl).hasminvalue;
		          if (rowMinVal == 'true') {
		              rowMinVal = "YES, " + $(this).getRowData(cl).minvalue;
		          } else {
		              rowMinVal = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { hasminvalue: rowMinVal });

		          rowMaxVal = $(this).getRowData(cl).hasmaxvalue;
		          if (rowMaxVal == 'true') {
		              rowMaxVal = "YES, " + $(this).getRowData(cl).maxvalue;
		          } else {
		              rowMaxVal = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { hasmaxvalue: rowMaxVal });

		          var rowSign = "Income";
		          if ($(this).getRowData(cl).salarysign < 0) {
		              rowSign = "Expense";
		          }
		          //rowSign = document.getElementById("SalarySign").options[$(this).getRowData(cl).salarysign].text;
		          $(this).jqGrid('setRowData', ids[i], { salarysign: rowSign });

		          rowOp = $(this).getRowData(cl).formulaop;
		          var e = document.getElementById("FormulaOp");
		          for (var item in e.options) {
		              if (e.options[item].value == rowOp) {
		                  rowOp = e.options[item].text; break;
		              }
		          }
		          $(this).jqGrid('setRowData', ids[i], { formulaop: rowOp });
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
                url: base_url + "SalarySlip/GetInfo?Id=" + id,
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
                            $('#SalaryItemId').val(result.SalaryItemId);
                            $('#SalaryItemName').val(result.SalaryItemName);
                            $('#Index').numberbox('setValue', result.Index);
                            $('#Code').val(result.Code);
                            $('#Name').val(result.Name);
                            $('#SalarySign').val(result.SalarySign);
                            document.getElementById('IsEnabled').checked = result.IsEnabled;
                            document.getElementById('IsMain').checked = result.IsMainSalary;
                            document.getElementById('IsDetail').checked = result.IsDetailSalary;
                            $('#Description').val(result.Description);
                            $('#Description').removeAttr('disabled');
                            $('#btnSalaryItem').removeAttr('disabled');
                            //$('#EffectiveDate').datebox('setValue', dateEnt(result.EffectiveDate));
                            //$('#EffectiveDateDiv2').hide();
                            //$('#EffectiveDateDiv').show();
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
        //ClearData();
        clearForm('#detail_div');
        fillData();
        $('#detailIndex').numberbox('setValue', '');
        $('#SecondValue').numberbox('setValue', '0');
        $('#MinValue').numberbox('setValue', '0');
        $('#MaxValue').numberbox('setValue', '0');
        $('#detail_div').dialog('open');
    });

    $('#btn_edit_detail').click(function () {
        //ClearData();
        clearForm("#detail_div");
        fillData();
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "SalarySlip/GetInfoDetail?Id=" + id,
                //async: false,
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
                            $('#SalarySlipId').val(result.SalarySlipId);
                            $('#detailIndex').numberbox('setValue', result.Index);
                            $('#SalaryItemSign').val(result.SalarySign);
                            //setTimeout(function (){  }, 1000);
                            var e1 = document.getElementById('FirstSalaryItem');
                            for (var item in e1.options) {
                                if (e1.options[item].text == result.FirstCode) {
                                    $("#FirstSalaryItem").data('kode', item);
                                    e1.options[item].selected = true; break;
                                }
                            }
                            $('#FormulaOp').val(result.FormulaOp);
                            var e2 = document.getElementById('SecondSalaryItem');
                            for (var item in e2.options) {
                                if (e2.options[item].text == result.SecondCode) {
                                    $("#SecondSalaryItem").data('kode', item);
                                    e2.options[item].selected = true; break;
                                }
                            }
                            document.getElementById("HasMinValue").checked = result.HasMinValue;
                            document.getElementById("HasMaxValue").checked = result.HasMaxValue;
                            document.getElementById("IsSecondValue").checked = result.IsSecondValue;
                            $('#SecondValue').numberbox('setValue', result.SecondValue);
                            $('#MinValue').numberbox('setValue', result.MinValue);
                            $('#MaxValue').numberbox('setValue', result.MaxValue);
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
                        url: base_url + "SalarySlip/DeleteDetail",
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
            submitURL = base_url + 'SalarySlip/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'SalarySlip/InsertDetail';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL + "?FirstSalaryItem=" + $("#FirstSalaryItem").val() + "&FormulaOp=" + $("#FormulaOp").val() + "&SecondSalaryItem=" + $("#SecondSalaryItem").val() + "&IsSecondValue=" + (document.getElementById("IsSecondValue").checked ? 'true' : 'false') + "&SecondValue=" + $("#SecondValue").numberbox('getValue'),
            data: JSON.stringify({
                Id: id, SalarySlipId: $("#id").val(),
                Index: $("#detailIndex").numberbox('getValue'), SalarySign: $('#SalaryItemSign').val(),
                HasMinValue: (document.getElementById("HasMinValue").checked ? 'true' : 'false'), MinValue: $("#MinValue").numberbox('getValue'),
                HasMaxValue: (document.getElementById("HasMaxValue").checked ? 'true' : 'false'), MaxValue: $("#MaxValue").numberbox('getValue'),
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


    // -------------------------------------------------------Look Up SalaryItem-------------------------------------------------------
    $('#btnSalaryItem').click(function () {
        var lookUpURL = base_url + 'SalaryItem/GetList';
        var lookupGrid = $('#lookup_table_salaryitem');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_salaryitem').dialog('open');
    });

    function cboxIsShiftable(cellvalue, options, rowObject) {
        return '<input name="cbIsEnabled" disabled rel="' + /*rowObject.id*/options.rowId + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    function cboxIsSalaryAllIn(cellvalue, options, rowObject) {
        return '<input name="cbIsSalaryAllIn" disabled rel="' + /*rowObject.id*/options.rowId + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    jQuery("#lookup_table_salaryitem").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Code', 'Name', 'Description', 'Shiftable', 'Salary All In', 'Access Level', 'Created At', 'Updated At'],
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
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#lookup_pager_salaryitem'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_salaryitem").width() - 10,
        height: $("#lookup_div_salaryitem").height() - 110,
        ondblClickRow: function (rowid) {
            $("#lookup_btn_add_salaryitem").trigger("click");
        },
    });
    $("#lookup_table_salaryitem").jqGrid('navGrid', '#lookup_toolbar_salaryitem', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_salaryitem').click(function () {
        $('#lookup_div_salaryitem').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_salaryitem').click(function () {
        var id = jQuery("#lookup_table_salaryitem").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_salaryitem").jqGrid('getRowData', id);

            $('#SalaryItemId').val(ret.id).data("kode", id);
            $('#SalaryItemName').val(ret.name);
            $('#lookup_div_salaryitem').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup SalaryItem----------------------------------------------------------------

    
}); //END DOCUMENT READY