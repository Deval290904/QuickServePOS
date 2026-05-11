$(document).ready(function () {

    initializeDataTable("#menuItemTable");

});

function reloadMenuItemData() {

    loadActiveMenuItems();
}
function loadActiveMenuItems() {

    fetch('/MenuItem/GetMenuItemList')
        .then(res => res.text())
        .then(html => {

            document.getElementById("menuItemTableContainer").innerHTML = html;

            initializeDataTable("#menuItemTable");

        });
}

function loadTrashMenuItems() {

    fetch('/MenuItem/GetDeletedMenuItems')
        .then(res => res.text())
        .then(html => {

            document.getElementById("menuItemTableContainer").innerHTML = html;

            initializeDataTable("#menuItemTable");

        });
}

function previewMenuImage(event) {

    const input = event.target;

    const preview =
        document.getElementById("menuImagePreview");

    if (input.files && input.files[0]) {

        preview.src =
            URL.createObjectURL(input.files[0]);

        preview.classList.remove("d-none");
    }
}