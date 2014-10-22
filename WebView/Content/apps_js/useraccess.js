$(document).ready(function () {
    // Initialize
    var usercode = "";
    var group = "master";

    /*================================================ JQuery Tabs ================================================*/
    $('ul.tabs').each(function () {
        // For each set of tabs, we want to keep track of
        // which tab is active and it's associated content
        var $active, $content, $links = $(this).find('a');

        // If the location.hash matches one of the links, use that as the active tab.
        // If no match is found, use the first link as the initial active tab.
        $active = $($links.filter('[href="' + location.hash + '"]')[0] || $links[0]);
        $active.addClass('active');

        $content = $($active[0].hash);

        // Hide the remaining content
        $links.not($active).each(function () {
            $(this.hash).hide();
        });

        // Bind the click event handler
        $(this).on('click', 'a', function (e) {
            // Make the old tab inactive.
            $active.removeClass('active');
            $content.hide();

            // Update the variables with the new link and content
            $active = $(this);
            $content = $(this.hash);

            // Make the tab active.
            $active.addClass('active');
            $content.show();

            // Prevent the anchor's default click action
            e.preventDefault();
        });
    });
    /*================================================ END JQuery Tabs ================================================*/

    // Reload
    $('#ua_form_btn_reload').click(function () {
        $("#tbl_users").trigger("reloadGrid");

        // Clear and Reload all grid
        $("#tbl_access_master").jqGrid("clearGridData", true).trigger("reloadGrid");
        $("#tbl_access_report").jqGrid("clearGridData", true).trigger("reloadGrid");
        $("#tbl_access_transaction").jqGrid("clearGridData", true).trigger("reloadGrid");
        $("#tbl_access_setting").jqGrid("clearGridData", true).trigger("reloadGrid");

    });
    
    function GetUserAccess() {
        $.ajax({
            dataType: "json",
            url: base_url + "UserAccess/GetUserAccess?UserId=" + usercode + "&groupName=" + group,
            success: function (result) {
                PopulateUserAccess(group, result);
            }
        });
    }

    function PopulateUserAccess(group, objUserAccess) {
        if (objUserAccess != null) {
            for (var i = 0; i < objUserAccess.model.length; i++) {
                var newData = {};
                newData.manualpricing = objUserAccess.model[i].AllowSpecialPricing;
                newData.code = objUserAccess.model[i].Id;
                newData.name = objUserAccess.model[i].Name;
                newData.read = objUserAccess.model[i].AllowView;
                newData.write = objUserAccess.model[i].AllowCreate;
                newData.edit = objUserAccess.model[i].AllowEdit;
                newData.delete = objUserAccess.model[i].AllowDelete;
                newData.undelete = objUserAccess.model[i].AllowUndelete;
                newData.confirm = objUserAccess.model[i].AllowConfirm;
                newData.unconfirm = objUserAccess.model[i].AllowUnconfirm;
                newData.paid = objUserAccess.model[i].AllowPaid;
                newData.unpaid = objUserAccess.model[i].AllowUnpaid;
                newData.reconcile = objUserAccess.model[i].AllowReconcile;
                newData.unreconcile = objUserAccess.model[i].AllowUnreconcile;
                newData.print = objUserAccess.model[i].AllowPrint;

                // New Record
                var group_menu = objUserAccess.model[i].GroupName.toLowerCase();
                jQuery("#tbl_access_" + group_menu).jqGrid('addRowData', $("#tbl_access_" + group_menu).getGridParam("reccount") + 1, newData);
            }
        }
    }

    // ---- View
    function UpdateAllow(menuId, isAllow, colName) {
        var userId = 0;
        var id = jQuery("#tbl_users").jqGrid('getGridParam', 'selrow');
        if (id) {
            userId = id;
        } else {
            $.messager.alert('Information', 'Please Select User...!!', 'info');
            return;
        };

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "UserAccess/Update?colName=" + colName + "&isAllow=" + isAllow,
            data: JSON.stringify({
                Id: menuId
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

    $("input[name=cbAllowView]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "View");
    });

    function cboxAllowView(cellvalue, options, rowObject) {
        return '<input name="cbAllowView" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END View

    // ---- Edit
    $("input[name=cbAllowEdit]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "Edit");
    });

    function cboxAllowEdit(cellvalue, options, rowObject) {
        return '<input name="cbAllowEdit" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END Edit

    // ---- ManualPricing
    $("input[name=cbAllowManualPricing]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "ManualPricing");
    });

    function cboxAllowManualPricing(cellvalue, options, rowObject) {
        return '<input name="cbAllowManualPricing" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END ManualPricing

    // ---- Delete
    $("input[name=cbAllowDelete]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "Delete");
    });

    function cboxAllowDelete(cellvalue, options, rowObject) {
        return '<input name="cbAllowDelete" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END Delete

    // ---- UnDelete
    $("input[name=cbAllowUnDelete]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "Undelete");
    });

    function cboxAllowUnDelete(cellvalue, options, rowObject) {
        return '<input name="cbAllowUnDelete" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END UnDelete

    // ---- Confirm
    $("input[name=cbAllowConfirm]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "Confirm");
    });

    function cboxAllowConfirm(cellvalue, options, rowObject) {
        return '<input name="cbAllowConfirm" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END Confirm

    // ---- UnConfirm
    $("input[name=cbAllowUnConfirm]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "UnConfirm");
    });

    function cboxAllowUnConfirm(cellvalue, options, rowObject) {
        return '<input name="cbAllowUnConfirm" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END UnConfirm

    // ---- Paid
    $("input[name=cbAllowPaid]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "Paid");
    });

    function cboxAllowPaid(cellvalue, options, rowObject) {
        return '<input name="cbAllowPaid" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END Paid

    // ---- UnPaid
    $("input[name=cbAllowUnPaid]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "UnPaid");
    });

    function cboxAllowUnPaid(cellvalue, options, rowObject) {
        return '<input name="cbAllowUnPaid" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END UnPaid

    // ---- Reconcile
    $("input[name=cbAllowReconcile]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "Reconcile");
    });

    function cboxAllowReconcile(cellvalue, options, rowObject) {
        return '<input name="cbAllowReconcile" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END Reconcile

    // ---- UnReconcile
    $("input[name=cbAllowUnReconcile]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "UnReconcile");
    });

    function cboxAllowUnReconcile(cellvalue, options, rowObject) {
        return '<input name="cbAllowUnReconcile" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END UnReconcile

    // ---- Print
    $("input[name=cbAllowPrint]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "Print");
    });

    function cboxAllowPrint(cellvalue, options, rowObject) {
        return '<input name="cbAllowPrint" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END Print

    // ---- Create
    $("input[name=cbAllowCreate]").live("click", function () {
        UpdateAllow($(this).attr('rel'), $(this).is(":checked"), "Create");
    });

    function cboxAllowCreate(cellvalue, options, rowObject) {
        return '<input name="cbAllowCreate" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END Create

    // Table User Access Group - Master
    jQuery("#tbl_access_master").jqGrid({
       // url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Name', 'View', 'Create', 'Edit', 'Delete', 'Confirm', 'UnConfirm', /*'Paid', 'UnPaid', 'Reconcile', 'UnReconcile',*/ 'Print'],
        colModel: [{ name: 'code', index: 'code', width: 50, align: 'center', frozen: true },
                  { name: 'name', index: 'name', width: 160, frozen: true },
                  {
                      name: 'read', index: 'read', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowView, formatoptions: { disabled: false }
                  },
                  {
                      name: 'write', index: 'write', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowCreate, formatoptions: { disabled: false }
                  },
                  {
                      name: 'edit', index: 'edit', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowEdit, formatoptions: { disabled: false }
                  },
                  {
                      name: 'delete', index: 'delete', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowDelete, formatoptions: { disabled: false }
                  },
                  {
                      name: 'confirm', index: 'confirm', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowConfirm, formatoptions: { disabled: false }
                  },
                  {
                      name: 'unconfirm', index: 'unconfirm', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowUnConfirm, formatoptions: { disabled: false }
                  },
                  //{
                  //    name: 'paid', index: 'paid', width: 60, align: 'center', sortable: false,
                  //    editable: true,
                  //    edittype: 'checkbox', editoptions: { value: "1:0" },
                  //    formatter: cboxAllowPaid, formatoptions: { disabled: false }
                  //},
                  //{
                  //    name: 'unpaid', index: 'unpaid', width: 60, align: 'center', sortable: false,
                  //    editable: true,
                  //    edittype: 'checkbox', editoptions: { value: "1:0" },
                  //    formatter: cboxAllowUnPaid, formatoptions: { disabled: false }
                  //},
                  //{
                  //    name: 'reconcile', index: 'reconcile', width: 60, align: 'center', sortable: false,
                  //    editable: true,
                  //    edittype: 'checkbox', editoptions: { value: "1:0" },
                  //    formatter: cboxAllowReconcile, formatoptions: { disabled: false }
                  //},
                  //{
                  //    name: 'unreconcile', index: 'unreconcile', width: 60, align: 'center', sortable: false,
                  //    editable: true,
                  //    edittype: 'checkbox', editoptions: { value: "1:0" },
                  //    formatter: cboxAllowUnReconcile, formatoptions: { disabled: false }
                  //},
                  {
                      name: 'print', index: 'print', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowPrint, formatoptions: { disabled: false }
                  }
        ],
        sortname: 'kode',
        viewrecords: true,
        gridview: true,
        shrinkToFit: false,
        scroll: 1,
        sortorder: "asc",
        width: 580,
        height: 380
    });
    $("#tbl_access_master").jqGrid('navGrid', '#toolbar_lookup_table_so_container', { del: false, add: false, edit: false, search: false });
    jQuery("#tbl_access_master").jqGrid('setFrozenColumns');
    // END Table User Access Group - Master

    // Table User Access Group - Transaction
    jQuery("#tbl_access_transaction").jqGrid({
       // url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Name', 'View', 'Create', 'Edit', 'Delete', 'Confirm', 'UnConfirm', 'Paid', 'UnPaid', 'Reconcile', 'UnReconcile', 'Manual Pricing', 'Print'],
        colModel: [{ name: 'code', index: 'code', width: 50, align: 'center', frozen: true },
                  { name: 'name', index: 'name', width: 160, frozen: true },
                  {
                      name: 'read', index: 'read', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowView, formatoptions: { disabled: false }
                  },
                  {
                      name: 'write', index: 'write', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowCreate, formatoptions: { disabled: false }
                  },
                  {
                      name: 'edit', index: 'edit', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowEdit, formatoptions: { disabled: false }
                  },
                  {
                      name: 'delete', index: 'delete', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowDelete, formatoptions: { disabled: false }
                  },
                  {
                      name: 'confirm', index: 'confirm', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowConfirm, formatoptions: { disabled: false }
                  },
                  {
                      name: 'unconfirm', index: 'unconfirm', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowUnConfirm, formatoptions: { disabled: false }
                  },
                  {
                      name: 'paid', index: 'paid', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowPaid, formatoptions: { disabled: false }
                  },
                  {
                      name: 'unpaid', index: 'unpaid', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowUnPaid, formatoptions: { disabled: false }
                  },
                  {
                      name: 'reconcile', index: 'reconcile', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowReconcile, formatoptions: { disabled: false }
                  },
                  {
                      name: 'unreconcile', index: 'unreconcile', width: 65, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowUnReconcile, formatoptions: { disabled: false }
                  },
                  {
                      name: 'manualpricing', index: 'manualpricing', width: 85, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowManualPricing, formatoptions: { disabled: false }
                  },
                  {
                      name: 'print', index: 'print', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowPrint, formatoptions: { disabled: false }
                  }
        ],
        sortname: 'kode',
        viewrecords: true,
        gridview: true,
        shrinkToFit: false,
        scroll: 1,
        sortorder: "asc",
        width: 580,
        height: 380
    });
    $("#tbl_access_transaction").jqGrid('navGrid', '#toolbar_lookup_table_so_container', { del: false, add: false, edit: false, search: false });
    jQuery("#tbl_access_transaction").jqGrid('setFrozenColumns');
    // END Table User Access Group - File

    // Table User Access Group - Report
    jQuery("#tbl_access_report").jqGrid({
      // url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Name', 'View', 'Create', 'Edit', 'Delete', 'Confirm', 'UnConfirm', /*'Paid', 'UnPaid', 'Reconcile', 'UnReconcile',*/ 'Print'],
        colModel: [{ name: 'code', index: 'code', width: 50, align: 'center', frozen: true },
                  { name: 'name', index: 'name', width: 160, frozen: true },
                  {
                      name: 'read', index: 'read', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowView, formatoptions: { disabled: false }
                  },
                  {
                      name: 'write', index: 'write', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowCreate, formatoptions: { disabled: false }
                  },
                  {
                      name: 'edit', index: 'edit', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowEdit, formatoptions: { disabled: false }
                  },
                  {
                      name: 'delete', index: 'delete', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowDelete, formatoptions: { disabled: false }
                  },
                  {
                      name: 'confirm', index: 'confirm', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowConfirm, formatoptions: { disabled: false }
                  },
                  {
                      name: 'unconfirm', index: 'unconfirm', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowUnConfirm, formatoptions: { disabled: false }
                  },
                  //{
                  //    name: 'paid', index: 'paid', width: 60, align: 'center', sortable: false,
                  //    editable: true,
                  //    edittype: 'checkbox', editoptions: { value: "1:0" },
                  //    formatter: cboxAllowPaid, formatoptions: { disabled: false }
                  //},
                  //{
                  //    name: 'unpaid', index: 'unpaid', width: 60, align: 'center', sortable: false,
                  //    editable: true,
                  //    edittype: 'checkbox', editoptions: { value: "1:0" },
                  //    formatter: cboxAllowUnPaid, formatoptions: { disabled: false }
                  //},
                  //{
                  //    name: 'reconcile', index: 'reconcile', width: 60, align: 'center', sortable: false,
                  //    editable: true,
                  //    edittype: 'checkbox', editoptions: { value: "1:0" },
                  //    formatter: cboxAllowReconcile, formatoptions: { disabled: false }
                  //},
                  //{
                  //    name: 'unreconcile', index: 'unreconcile', width: 60, align: 'center', sortable: false,
                  //    editable: true,
                  //    edittype: 'checkbox', editoptions: { value: "1:0" },
                  //    formatter: cboxAllowUnReconcile, formatoptions: { disabled: false }
                  //},
                  {
                      name: 'print', index: 'print', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowPrint, formatoptions: { disabled: false }
                  }
        ],
        sortname: 'kode',
        viewrecords: true,
        gridview: true,
        shrinkToFit: false,
        scroll: 1,
        sortorder: "asc",
        width: 580,
        height: 380
    });
    $("#tbl_access_report").jqGrid('navGrid', '#toolbar_lookup_table_so_container', { del: false, add: false, edit: false, search: false });
    jQuery("#tbl_access_report").jqGrid('setFrozenColumns');
    // END Table User Access Group - Report

    // Table User Access Group - Setting
    jQuery("#tbl_access_setting").jqGrid({
        //url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Name', 'View', 'Create', 'Edit', 'Delete', 'Confirm', 'UnConfirm', /*'Paid', 'UnPaid', 'Reconcile', 'UnReconcile',*/ 'Print'],
        colModel: [{ name: 'code', index: 'code', width: 50, align: 'center', frozen:true },
                  { name: 'name', index: 'name', width: 160, frozen:true },
                  {
                      name: 'read', index: 'read', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowView, formatoptions: { disabled: false }
                  },
                  {
                      name: 'write', index: 'write', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowCreate, formatoptions: { disabled: false }
                  },
                  {
                      name: 'edit', index: 'edit', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowEdit, formatoptions: { disabled: false }
                  },
                  {
                      name: 'delete', index: 'delete', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowDelete, formatoptions: { disabled: false }
                  },
                  {
                      name: 'confirm', index: 'confirm', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowConfirm, formatoptions: { disabled: false }
                  },
                  {
                      name: 'unconfirm', index: 'unconfirm', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowUnConfirm, formatoptions: { disabled: false }
                  },
                  //{
                  //    name: 'paid', index: 'paid', width: 60, align: 'center', sortable: false,
                  //    editable: true,
                  //    edittype: 'checkbox', editoptions: { value: "1:0" },
                  //    formatter: cboxAllowPaid, formatoptions: { disabled: false }
                  //},
                  //{
                  //    name: 'unpaid', index: 'unpaid', width: 60, align: 'center', sortable: false,
                  //    editable: true,
                  //    edittype: 'checkbox', editoptions: { value: "1:0" },
                  //    formatter: cboxAllowUnPaid, formatoptions: { disabled: false }
                  //},
                  //{
                  //    name: 'reconcile', index: 'reconcile', width: 60, align: 'center', sortable: false,
                  //    editable: true,
                  //    edittype: 'checkbox', editoptions: { value: "1:0" },
                  //    formatter: cboxAllowReconcile, formatoptions: { disabled: false }
                  //},
                  //{
                  //    name: 'unreconcile', index: 'unreconcile', width: 60, align: 'center', sortable: false,
                  //    editable: true,
                  //    edittype: 'checkbox', editoptions: { value: "1:0" },
                  //    formatter: cboxAllowUnReconcile, formatoptions: { disabled: false }
                  //},
                  {
                      name: 'print', index: 'print', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowPrint, formatoptions: { disabled: false }
                  }
        ],
        sortname: 'kode',
        viewrecords: true,
        gridview: true,
        shrinkToFit: false,
        scroll: 1,
        sortorder: "asc",
        width: 580,
        height: 380
    });
    $("#tbl_access_setting").jqGrid('navGrid', '#toolbar_lookup_table_so_container', { del: false, add: false, edit: false, search: false });
    jQuery("#tbl_access_setting").jqGrid('setFrozenColumns');
    // END Table User Access Group - Setting

    function cboxIsAdmin(cellvalue, options, rowObject) {
        return '<input name="cbIsAdmin" disabled rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // Table User List
    jQuery("#tbl_users").jqGrid({
        url: base_url + 'User/GetList',
        datatype: "json",
        colNames: ['ID', 'UserName', 'Name', 'Description', 'Is Admin'],
        colModel: [{ name: 'id', index: 'id', width: 50, frozen: true },
                  { name: 'username', index: 'username', width: 100, frozen:true },
                  { name: 'name', index: 'name', width: 150 },
                  { name: 'description', index: 'description', width: 150 },
                  {
                      name: 'isadmin', index: 'isadmin', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxIsAdmin, formatoptions: { disabled: true }
                  }
        ],
        onSelectRow: function (id) {

            //var url = window.location.href.split('#');
            //if (url[1] != undefined && url[1] != '')
            //    group = url[1].substr(5, url[1].length - 5);

            // Clear and Reload all grid
            $("#tbl_access_master").jqGrid("clearGridData", true).trigger("reloadGrid");
            $("#tbl_access_report").jqGrid("clearGridData", true).trigger("reloadGrid");
            $("#tbl_access_transaction").jqGrid("clearGridData", true).trigger("reloadGrid");
            $("#tbl_access_setting").jqGrid("clearGridData", true).trigger("reloadGrid");

            usercode = id;
            GetUserAccess();
        },
        sortname: 'id',
        viewrecords: true,
        gridview: true,
        shrinkToFit: false,
        scroll: 1,
        sortorder: "DESC",
        width: 450,
        height: 420
    });
    $("#tbl_users").jqGrid('navGrid', '#toolbar_lookup_table_so_container', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    jQuery("#tbl_users").jqGrid('setFrozenColumns');
    // Table User List

    // Save
    $('#ua_form_btn_save').click(function () {

        // Menu Type List
        var MenuTypeList = PopulateMenuTypeList();
        // END Menu Type List

        if (MenuTypeList == undefined || MenuTypeList == null || MenuTypeList.length == 0) {
            $.messager.alert('Information', 'Data is empty..', 'info');
            return;
        }


        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "UserAccess/SaveUserAccessType",
            data: JSON.stringify({
                UserCode: usercode, MenuTypeList: MenuTypeList
            }),
            success: function (result) {
                if (result.isValid) {
                    $.messager.alert('Information', result.message, 'info', function () {
                        window.location = base_url + "UserAccess";
                    });
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }


            }
        });
    });

    // Menu Type List
    function PopulateMenuTypeList() {
        var menuList = [];
        // Master
        var ids = $("#tbl_access_master").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var datagrid = jQuery("#tbl_access_master").jqGrid('getRowData', ids[i]);
            var obj = {};
            obj['Input'] = datagrid.write;
            obj['Lihat'] = datagrid.read;
            obj['Edit'] = datagrid.edit;
            obj['Hapus'] = datagrid.delete;
            obj['Un_Hapus'] = datagrid.undelete;
            obj['Print'] = datagrid.print;
            obj['Kode'] = datagrid.code;
            menuList.push(obj);
        }

        // File
        var ids = $("#tbl_access_transaction").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var datagrid = jQuery("#tbl_access_transaction").jqGrid('getRowData', ids[i]);
            var obj = {};
            obj['Input'] = datagrid.write;
            obj['Lihat'] = datagrid.read;
            obj['Edit'] = datagrid.edit;
            obj['Hapus'] = datagrid.delete;
            obj['Un_Hapus'] = datagrid.undelete;
            obj['Print'] = datagrid.print;
            obj['Kode'] = datagrid.code;
            menuList.push(obj);
        }

        // Report
        var ids = $("#tbl_access_report").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var datagrid = jQuery("#tbl_access_report").jqGrid('getRowData', ids[i]);
            var obj = {};
            obj['Input'] = datagrid.write;
            obj['Lihat'] = datagrid.read;
            obj['Edit'] = datagrid.edit;
            obj['Hapus'] = datagrid.delete;
            obj['Un_Hapus'] = datagrid.undelete;
            obj['Print'] = datagrid.print;
            obj['Kode'] = datagrid.code;
            menuList.push(obj);
        }

        // Proses
        var ids = $("#tbl_access_setting").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var datagrid = jQuery("#tbl_access_setting").jqGrid('getRowData', ids[i]);
            var obj = {};
            obj['Input'] = datagrid.write;
            obj['Lihat'] = datagrid.read;
            obj['Edit'] = datagrid.edit;
            obj['Hapus'] = datagrid.delete;
            obj['Un_Hapus'] = datagrid.undelete;
            obj['Print'] = datagrid.print;
            obj['Kode'] = datagrid.code;
            menuList.push(obj);
        }

        return menuList
    }
    // END Menu Type List
});