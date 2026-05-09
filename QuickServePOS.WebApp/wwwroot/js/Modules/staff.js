function loadActive() {
    fetch('/Admin/GetStaffList')
        .then(res => res.text())
        .then(html => {
            document.getElementById("staffTableContainer").innerHTML = html;
        });
}

function loadTrash() {
    fetch('/Admin/GetDeletedStaff')
        .then(res => res.text())
        .then(html => {
            document.getElementById("staffTableContainer").innerHTML = html;
        });
}

function reloadStats() {

    fetch('/Admin/GetStaffStats')
        .then(res => res.text())
        .then(html => {

            document.getElementById("statsContainer").innerHTML = html;

        });
}

function refreshDashboard() {
    loadActive();
    reloadStats();
}