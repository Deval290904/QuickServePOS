

$(document).ready(function () {

    initializeDataTable("#floorTable");

});

function reloadFloorData() {

    loadActiveFloors();

}

function loadActiveFloors() {

    fetch('/Floor/GetFloorList')
        .then(res => res.text())
        .then(html => {

            document.getElementById("floorTableContainer").innerHTML = html;

            initializeDataTable("#floorTable");

        });
}

function loadTrashFloors() {

    fetch('/Floor/GetDeletedFloors')
        .then(res => res.text())
        .then(html => {

            document.getElementById("floorTableContainer").innerHTML = html;

            initializeDataTable("#floorTable");

        });
}