function loadActive() {
    fetch('/Admin/GetStaffList')
        .then(res => res.text())
        .then(html => {
            document.getElementById("staffTableContainer").innerHTML = html;
            initializeDataTable("#staffTable");
            setActiveButton("active");
        });
  
}

function loadTrash() {
    fetch('/Admin/GetDeletedStaff')
        .then(res => res.text())
        .then(html => {
            document.getElementById("staffTableContainer").innerHTML = html;
            initializeDataTable("#staffTable");
            setActiveButton("trash");
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

function setActiveButton(type) {

    const activeBtn = document.getElementById("activeBtn");
    const trashBtn = document.getElementById("trashBtn");

    // RESET
    activeBtn.classList.remove("btn-secondary", "active-filter-btn");
    activeBtn.classList.add("btn-outline-secondary");

    trashBtn.classList.remove("btn-danger", "active-filter-btn");
    trashBtn.classList.add("btn-outline-danger");

    // ACTIVE MODE
    if (type === "active") {

        activeBtn.classList.remove("btn-outline-secondary");
        activeBtn.classList.add("btn-secondary", "active-filter-btn");
    }

    // TRASH MODE
    else if (type === "trash") {

        trashBtn.classList.remove("btn-outline-danger");
        trashBtn.classList.add("btn-danger", "active-filter-btn");
    }
}