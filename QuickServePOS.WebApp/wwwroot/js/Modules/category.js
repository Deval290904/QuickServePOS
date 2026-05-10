function reloadPageData() {
    fetch('/Category/GetCategoryList')
        .then(res => res.text())
        .then(html => {

            document.getElementById("categoryTableContainer")
                .innerHTML = html;

            initializeDataTable("#categoryTable");
        });
}

$(document).ready(function () {

    initializeDataTable("#categoryTable");

});