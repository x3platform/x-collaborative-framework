$(document).ready(function()
{
    x.ui.windows.getApplicationSettingListWindow('applicationSettingListWindow', {

        applicationId: x.dom('#searchApplicationId').val(),
        applicationName: x.dom('#searchApplicationName').val(),

        treeViewContainer: x.dom('#treeViewContainer'),
        searchText: x.dom('#searchText'),
        btnFilter: x.dom('#btnFilter'),

        tableHeader: x.dom('#window-main-table-header'),
        tableContainer: x.dom('#window-main-table-container'),
        tableFooter: x.dom('#window-main-table-footer')
    });
});