$(document).ready(function () {

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'ValidComb/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadFilteredGrid(ClosingId) {
        $("#list").setGridParam({ url: base_url + 'ValidComb/GetListByDate?', postData: { ClosingId: ClosingId }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridClosing() {
        $("#closing").setGridParam({ url: base_url + 'Closing/GetClosedList', postData: { filters: null }, page: 'first' }).trigger("reloadGridClosing");
    }

    function ClearData() {
        ClearErrorMessage();
    }

    $("#search_div").dialog('close');

    $("#list").jqGrid({
        url: base_url + 'ValidComb/GetList',
        datatype: "json",

        colNames: ['Id', 'Account Code', 'Account Name', 'Period', 'Year', 'Beginning', 'End Date', 'Amount'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center", hidden: true },
				  { name: 'code', index: 'code', width: 80 },
				  { name: 'name', index: 'name', width: 150 },
				  { name: 'period', index: 'period', width: 60 },
				  { name: 'yearperiod', index: 'yearperiod', width: 60 },
                  { name: 'beginningperiod', index: 'beginningperiod', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
                  { name: 'enddateperiod', index: 'enddateperiod', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
                  { name: 'amount', index: 'amount', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },

        ],
        page: '1',
        pager: $('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        scrollrows: true,
        viewrecords: true,
        shrinkToFit: false,
        sortorder: "DESC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        gridComplete:
         function () {
            var ids = $(this).jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                rowIsLegacy = $(this).getRowData(cl).islegacy;
                if (rowIsLegacy == 'true') {
                    rowIsLegacy = "Y";
                } else {
                    rowIsLegacy = "N";
                }
                $(this).jqGrid('setRowData', ids[i], { islegacy: rowIsLegacy });
            }
        }
    });
    $("#list").jqGrid('navGrid', '#toolbar', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#btn_reload').click(function () {
        ReloadGrid();
    });

    $('#btn_search').click(function () {
        ReloadGridClosing()
        $("#search_div").dialog("open");
    });

    $('#btn_print').click(function () {
        window.open(base_url + 'Print_Forms/PrintValidComb.aspx');
    });

    $('#btn_add_new').click(function () {
        clearForm('#frm');
        $('#form_div').dialog('open');
    });

    $('#form_btn_cancel').click(function () {
        clearForm('#frm');
        $("#form_div").dialog('close');
    });

    $('#search_btn_cancel').click(function () {
        $("#search_div").dialog('close');
    });
    
    $('#search_btn_submit').click(function () {
        var ClosingId = jQuery("#closing").jqGrid('getGridParam', 'selrow');
        ReloadFilteredGrid(ClosingId);
        $("#search_div").dialog('close');
    });
    
    $("#closing").jqGrid({
        url: base_url + 'Closing/GetClosedList',
        datatype: "json",
        colNames: ['ID', 'Period', 'Year', 'Beginning', 'End Date'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center", hidden: true, search: false },
				  { name: 'period', index: 'period', width: 60 },
				  { name: 'year', index: 'yearperiod', width: 60 },
                  { name: 'beginning', index: 'beginningperiod', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" }, search: false },
                  { name: 'enddate', index: 'enddateperiod', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" }, search: false },
        ],
        page: '1',
        pager: $('#closingpager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        scrollrows: true,
        viewrecords: true,
        shrinkToFit: false,
        sortorder: "DESC",
        width: $("#search_div").width() - 3,
        height: 200,
        gridComplete:
         function () {
             var ids = $(this).jqGrid('getDataIDs');
             for (var i = 0; i < ids.length; i++) {
                 var cl = ids[i];
                 rowIsLegacy = $(this).getRowData(cl).islegacy;
                 if (rowIsLegacy == 'true') {
                     rowIsLegacy = "Y";
                 } else {
                     rowIsLegacy = "N";
                 }
                 $(this).jqGrid('setRowData', ids[i], { islegacy: rowIsLegacy });
             }
         }
    });

    $("#closing").jqGrid('navGrid', '#search_toolbar', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    function clearForm(form) {
        $(':input', form).each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase(); // normalize case
            if (type == 'text' || type == 'password' || tag == 'textarea')
                this.value = "";
            else if (type == 'checkbox' || type == 'radio')
                this.checked = false;
            else if (tag == 'select')
                this.selectedIndex = -1;
            $('#Code').data('kode', 0);
        });
    }
}); //END DOCUMENT READY