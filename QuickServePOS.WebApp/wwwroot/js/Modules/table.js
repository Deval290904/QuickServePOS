$(document).ready(function () {

    initializeDataTable("#tableTable");

});

function reloadTableData() {

    loadActiveTables();

}

function loadActiveTables() {

    fetch('/Table/GetTableList')
        .then(res => res.text())
        .then(html => {

            document.getElementById("tableTableContainer").innerHTML = html;

            initializeDataTable("#tableTable");

        });
}

function loadTrashTables() {

    fetch('/Table/GetDeletedTables')
        .then(res => res.text())
        .then(html => {

            document.getElementById("tableTableContainer").innerHTML = html;

            initializeDataTable("#tableTable");

        });
}