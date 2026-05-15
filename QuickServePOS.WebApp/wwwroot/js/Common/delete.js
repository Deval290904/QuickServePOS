function DeletehandleAction(url, id, btn, actionType,callback=null) {

    let title = "";
    let text = "";
    let successTitle = "";
    let method = "post"

    if (actionType === "delete") {
        title = "Are you sure?";
        text = "This record will be deleted!";
        successTitle = "Deleted!";
    }
    else if (actionType === "restore") {
        title = "Restore this record?";
        text = "This will restore the record!";
        successTitle = "Restored!";
    }
    else if (actionType === "permanentDelete") {
        title = "Delete permanently?";
        text = "This action cannot be undone!";
        successTitle = "Deleted Permanently!";
    }

    Swal.fire({
        title: title,
        text: text,
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "Cancel"
    }).then((result) => {

        if (!result.isConfirmed) return;

        fetch(`${url}/${id}`, {
            method: method
        })
            .then(res =>
                res.json())
            .then(result => {

                if (result.success) {

                    Swal.fire({
                        icon: "success",
                        title: successTitle,
                        text: result.message || "Success"
                    });
                    if (callback &&
                        typeof callback === "function") {

                        callback();
                    }
                    else if (typeof refreshDashboard === "function") {

                        refreshDashboard();
                    }
                    else if (typeof reloadMenuItemData === "function") {
                        reloadMenuItemData();
                    }
                    else if (typeof reloadCategoryData === "function") {
                        reloadCategoryData();
                    }
                    else if (typeof reloadFloorData === "function") {
                        reloadFloorData();
                    }
                    else if (typeof reloadTableData === "function") {
                        reloadTableData();
                    }
                    else {

                        location.reload();
                    }

                    // ✅ Remove row animation
                    let row = btn.closest("tr");

                    if (row) {

                        row.style.transition = "0.3s";

                        row.style.opacity = "0";

                        setTimeout(() => {

                            row.remove();

                        }, 300);
                    }

                } else {
                    Swal.fire("Error", result.message || "Operation failed", "error");
                }
            })
            .catch((err) => {
                Swal.fire("Error", "Something went wrong", "error");
            });
    });
}