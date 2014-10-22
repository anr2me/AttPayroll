$(document).ready(function () {

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGridClosing() {
        $("#closing").setGridParam({ url: base_url + 'Closing/GetClosedList', postData: { filters: null }, page: 'first' }).trigger("reloadGridClosing");
    }

    function ClearData() {
        ClearErrorMessage();
    }

    $('#txtcompanyname').attr("disabled", true);

    $('#btnprint').click(function () {
        var closingId = jQuery("#closing").jqGrid('getGridParam', 'selrow');
        if (closingId) {
            window.open(base_url + "FinanceReport/ReportBalanceSheet?closingId=" + closingId);
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $.ajax({
        dataType: "json",
        url: base_url + "Company/GetDefaultInfo",
        success: function (result) {
            if (result.Id == null) {
                $.messager.alert('Information', 'Data Not Found...!!', 'info');
            }
            else {
                $('#txtcompanyname').val(result.Name);
            }
        }
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
        width: $("#closing_div").width() - 3,
        height: 100,
        gridComplete:
         function () {
             //var ids = $(this).jqGrid('getDataIDs');
             //for (var i = 0; i < ids.length; i++) {
             //    var cl = ids[i];
             //    rowIsLegacy = $(this).getRowData(cl).islegacy;
             //    if (rowIsLegacy == 'true') {
             //        rowIsLegacy = "Y";
             //    } else {
             //        rowIsLegacy = "N";
             //    }
             //    $(this).jqGrid('setRowData', ids[i], { islegacy: rowIsLegacy });
             //}
         }
    });
}); //END DOCUMENT READY