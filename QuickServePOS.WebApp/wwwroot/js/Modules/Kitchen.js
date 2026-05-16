// ========================================
// LOAD KITCHEN QUEUE
// ========================================

function loadKitchenQueue() {

    $("#kitchenQueueContainer")
        .load("/Kitchen/GetKitchenQueuePartial");
}



// ========================================
// COMMON KITCHEN STATUS UPDATE FUNCTION
// ========================================

function updateKitchenStatus(
    url,
    data,
    successMessage) {

    Swal.fire({

        title: "Are you sure?",

        text: "Status will be updated.",

        icon: "question",

        showCancelButton: true,

        confirmButtonText: "Yes",

        cancelButtonText: "Cancel"

    }).then((result) => {

        if (result.isConfirmed) {

            $.ajax({

                url: url,

                type: "POST",

                data: data,

                success: function (response) {

                    if (response.success) {

                        Swal.fire({

                            icon: "success",

                            title: successMessage,

                            timer: 1000,

                            showConfirmButton: false
                        });

                        loadKitchenQueue();
                    }
                    else {

                        Swal.fire({
                            icon: "error",
                            title: response.message
                        });
                    }
                },

                error: function () {

                    Swal.fire({
                        icon: "error",
                        title: "Something went wrong."
                    });
                }
            });
        }

    });
}



// ========================================
// DOCUMENT READY
// ========================================

$(document).ready(function () {

    // ========================================
    // MANUAL REFRESH
    // ========================================

    $("#refreshKitchenQueueBtn")
        .click(function () {

            loadKitchenQueue();
        });



    // ========================================
    // KOT STATUS UPDATE
    // ========================================

    $(document).on("click",
        ".update-status-btn",
        function () {

            const model = {

                kotId:
                    $(this).data("kot-id"),

                status:
                    $(this).data("status")
            };

            updateKitchenStatus(

                "/Kitchen/UpdateStatus",

                model,

                "KOT status updated successfully."
            );
        });



    // ========================================
    // KOT ITEM STATUS UPDATE
    // ========================================

    $(document).on("change",
        ".item-status-dropdown",
        function () {

            const model = {

                kotItemId:
                    $(this).data("kot-item-id"),

                status:
                    $(this).val()
            };

            updateKitchenStatus(

                "/Kitchen/UpdateItemStatus",

                model,

                "Item status updated successfully."
            );
            loadKitchenQueue();
        });



    // ========================================
    // VIEW DETAILS
    // ========================================

    $(document).on("click",
        ".view-details-btn",
        function () {

            const kotId =
                $(this).data("kot-id");

            OpenModal(
                "/Kitchen/Details/" + kotId,
                "KOT Details");
        });



    // ========================================
    // AUTO REFRESH
    // ========================================

    setInterval(function () {

        loadKitchenQueue();

    }, 10000);

});