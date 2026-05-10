function initializeDataTable(tableId) {

    // Destroy existing DataTable properly
    if ($.fn.DataTable.isDataTable(tableId)) {

        $(tableId).DataTable().clear().destroy();
    }

    // Remove old wrapper if exists
    $(tableId).removeAttr('style');

    // Initialize again
    $(tableId).DataTable({

        responsive: true,

        autoWidth: false,

        processing: true,

        destroy: true,

        pageLength: 5,

        lengthMenu: [5, 10, 25, 50],

        ordering: true,

        searching: true,

        paging: true,

        info: true,

        columnDefs: [
            {
                orderable: false,
                targets: -1 // Action column
            }
        ],

        language: {
            search: "",
            searchPlaceholder: "Search staff..."
        }
    });
}