$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        //$("#list").setGridParam({ url: base_url + 'MstItem/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        //$('#Description').val('').text('').removeClass('errormessage');
        //$('#Name').val('').text('').removeClass('errormessage');
        //$('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#lookup_div_itemtype").dialog('close');


    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        ReloadGrid();
    });

    $('#btn_print').click(function () {
        window.open(base_url + 'Report/ReportItem?itemTypeId=' + $('#ItemTypeId').val());
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

    // -------------------------------------------------------Look Up Itemtype-------------------------------------------------------
    $('#btnItemType').click(function () {

        var lookUpURL = base_url + 'MstItemType/GetList';
        var lookupGrid = $('#lookup_table_itemtype');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_itemtype').dialog('open');
    });

    $("#lookup_table_itemtype").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name', 'Description'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 },
                  { name: 'address', index: 'address', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_itemtype'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_itemtype").width() - 10,
        height: $("#lookup_div_itemtype").height() - 110,
    });
    $("#lookup_table_itemtype").jqGrid('navGrid', '#lookup_toolbar_itemtype', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_itemtype').click(function () {
        $('#lookup_div_itemtype').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_itemtype').click(function () {
        var id = jQuery("#lookup_table_itemtype").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_itemtype").jqGrid('getRowData', id);

            $('#ItemTypeId').val(ret.id).data("kode", id);
            $('#ItemTypeName').val(ret.name);

            $('#lookup_div_itemtype').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    
    // ---------------------------------------------End Lookup ItemType----------------------------------------------------------------

    
}); //END DOCUMENT READY