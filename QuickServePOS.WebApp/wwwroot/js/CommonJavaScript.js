function OpenModal(url, title) {

    fetch(url)
        .then(res => res.text())
        .then(html => {

            document.getElementById("modalBody").innerHTML = html;
            document.querySelector(".modal-title").innerText = title;

            const modalElement = document.getElementById("AllModals");

            const modal = bootstrap.Modal.getOrCreateInstance(modalElement, {
                backdrop: 'static',
                keyboard: false
            });

            modal.show();

            // ✅ Safe validation binding
            if (window.$ && $.validator && $.validator.unobtrusive) {
                $.validator.unobtrusive.parse("#modalBody");
            }

            bindModalForm(); // ✅ attach submit
        });
}

function bindModalForm() {

    const form = document.querySelector("#modalBody form");
    if (!form) return;

    form.onsubmit = async function (e) {

        e.preventDefault();

        const $form = $(form);

        // ✅ Safe validation
        if ($.validator && $form.valid && !$form.valid()) return;

        const formdata = new FormData(form);

        const actionUrl = form.action.toLowerCase();

        let proceed = true;

        // ✅ Confirm only for update
        if (actionUrl.includes("edit") || actionUrl.includes("update")) {

            const confirmResult = await Swal.fire({
                title: "Are you sure?",
                text: "Do you want to update this record?",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Yes, update it!",
                cancelButtonText: "Cancel"
            });

            proceed = confirmResult.isConfirmed;
        }

        if (!proceed) return;

        fetch(form.action, {
            method: "POST",
            body: formdata
        })
            .then(async res => {

                const text = await res.text(); // ALWAYS read text first

                let result;

                try {
                    result = JSON.parse(text);
                } catch (e) {

                    // If HTML returned → validation error
                    document.getElementById("modalBody").innerHTML = text;

                    if ($.validator && $.validator.unobtrusive) {
                        $.validator.unobtrusive.parse("#modalBody");
                    }

                    bindModalForm();
                    return null;
                }

                return result;
            })
            .then(result => {

                if (!result) return;

                if (result.success === true) {

                    closeAllModals();

                    Swal.fire({
                        icon: "success",
                        title: "Success",
                        text: result.message || "Saved successfully!"
                    }).then(() => {
                        refreshDashboard();
                    });

                } else {

                    Swal.fire({
                        icon: "error",
                        title: "Error",
                        text: result.message || "Operation failed"
                    });
                }
            })
            .catch((err) => {
               
                Swal.fire("Error", "Something went wrong", "error");
            });
    };
}

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

function DeletehandleAction(url, id, btn, actionType) {

    let title = "";
    let text = "";
    let successTitle = "";
    let method="POST"

    if (actionType === "delete") {
        title = "Are you sure?";
        text = "This record will be deleted!";
        successTitle = "Deleted!";
    }
    else if (actionType === "restore") {
        title = "Restore this record?";
        text = "This will restore the staff!";
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
            .then(res => res.json())
            .then(result => {

                if (result.success) {

                    Swal.fire({
                        icon: "success",
                        title: successTitle,
                        text: result.message || "Success"
                    });
                    refreshDashboard();

                    // ✅ Remove row animation
                    let row = btn.closest("tr");

                    row.style.transition = "0.3s";
                    row.style.opacity = "0";

                    setTimeout(() => {
                        row.remove();
                    }, 300);

                } else {
                    Swal.fire("Error", result.message || "Operation failed", "error");
                }
            })
            .catch(() => {
                Swal.fire("Error", "Something went wrong", "error");
            });
    });
}

function closeAllModals() {
    const modalElement = document.getElementById("AllModals");
    const modal = bootstrap.Modal.getInstance(modalElement);
    if (modal) modal.hide();
}

