// ========================================
// LOAD READY QUEUE
// ========================================

function loadReadyQueue() {

    $("#readyQueueContainer")
        .load("/Kitchen/ReadyQueuePartial");
}



// ========================================
// COMMON SERVE FUNCTION
// ========================================

function serveKOT(
    url,
    data,
    successMessage) {

    Swal.fire({

        title: "Are you sure?",

        text: "Order will be served.",

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

                            timer: 1200,

                            showConfirmButton: false
                        });

                        loadReadyQueue();
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

    $("#refreshReadyQueueBtn")
        .click(function () {

            loadReadyQueue();
        });



    // ========================================
    // SERVE KOT
    // ========================================

    $(document).on("click",
        ".serve-kot-btn",
        function () {

            const model = {

                kotId:
                    $(this).data("kot-id")
            };

            serveKOT(

                "/Kitchen/ServeKOT",

                model,

                "KOT served successfully."
            );
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

        loadReadyQueue();

    }, 10000);

});