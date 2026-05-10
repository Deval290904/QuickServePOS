function loadActive() {
    fetch('/Admin/GetStaffList')
        .then(res => res.text())
        .then(html => {
            document.getElementById("staffTableContainer").innerHTML = html;
            initializeDataTable("#staffTable");
        });
  
}

function loadTrash() {
    fetch('/Admin/GetDeletedStaff')
        .then(res => res.text())
        .then(html => {
            document.getElementById("staffTableContainer").innerHTML = html;
            initializeDataTable("#staffTable");
        });
    
}

function reloadStats() {

    fetch('/Admin/GetStaffStats')
        .then(res => res.text())
        .then(html => {

            document.getElementById("statsContainer").innerHTML = html;

        });
}
$(document).ready(function () {

    initializeDataTable("#staffTable");

});

function refreshDashboard() {
    loadActive();
    reloadStats();
}