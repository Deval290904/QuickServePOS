function OpenModal(url, title) {

    fetch(url)
        .then(res => res.text())
        .then(html => {

            document.getElementById("modalBody").innerHTML = html;

            document.querySelector(".modal-title").innerText = title;

            const modalElement =
                document.getElementById("AllModals");

            const modal =
                bootstrap.Modal.getOrCreateInstance(
                    modalElement,
                    {
                        backdrop: 'static',
                        keyboard: false
                    });

            modal.show();

            if (window.$ &&
                $.validator &&
                $.validator.unobtrusive) {

                $.validator.unobtrusive.parse("#modalBody");
            }

            bindModalForm();
        });
}

function bindModalForm() {

    const form =
        document.querySelector("#modalBody form");

    if (!form) return;

    form.onsubmit = async function (e) {

        e.preventDefault();

        const $form = $(form);

        if ($.validator &&
            $form.valid &&
            !$form.valid()) return;

        const formdata = new FormData(form);

        const actionUrl =
            form.action.toLowerCase();

        let proceed = true;

        if (actionUrl.includes("edit") ||
            actionUrl.includes("update")) {

            const confirmResult =
                await Swal.fire({
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

                const text = await res.text();

                let result;

                try {

                    result = JSON.parse(text);

                } catch {

                    document.getElementById("modalBody")
                        .innerHTML = text;

                    if ($.validator &&
                        $.validator.unobtrusive) {

                        $.validator.unobtrusive
                            .parse("#modalBody");
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
                        text: result.message
                            || "Saved successfully!"
                    })
                        .then(() => {

                            if (typeof reloadPageData === "function") {

                                reloadPageData();
                            }
                            else {

                                location.reload();
                            }
                        });

                } else {

                    Swal.fire({
                        icon: "error",
                        title: "Error",
                        text: result.message
                            || "Operation failed"
                    });
                }
            })
            .catch(() => {

                Swal.fire(
                    "Error",
                    "Something went wrong",
                    "error");
            });
    };
}

function closeAllModals() {

    const modalElement =
        document.getElementById("AllModals");

    const modal =
        bootstrap.Modal.getInstance(modalElement);

    if (modal) modal.hide();
}