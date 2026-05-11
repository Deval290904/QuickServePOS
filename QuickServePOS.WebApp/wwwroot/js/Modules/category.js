$(document).ready(function () {

    initializeDataTable("#categoryTable");

});

function reloadCategoryData() {

    loadActiveCategories();

}
function loadActiveCategories() {

    fetch('/Category/GetCategoryList')
        .then(res => res.text())
        .then(html => {

            document.getElementById("categoryTableContainer").innerHTML = html;

            initializeDataTable("#categoryTable");
      
        });
}

function loadTrashCategories() {

    fetch('/Category/GetDeletedCategories')
        .then(res => res.text())
        .then(html => { 

            document.getElementById("categoryTableContainer").innerHTML = html;
          
            initializeDataTable("#categoryTable");
        });
}



