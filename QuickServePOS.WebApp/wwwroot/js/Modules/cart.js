$(document).on("click",
    ".cart-plus-btn",
    function () {

        const orderItemId =
            $(this).data("id");

        let quantity =
            parseInt($(this).data("qty"));

        quantity++;

        updateCartItem(
            orderItemId,
            quantity);
    });

$(document).on("click",
    ".cart-minus-btn",
    function () {

        const orderItemId =
            $(this).data("id");

        let quantity =
            parseInt($(this).data("qty"));

        if (quantity <= 1) {

            showError("Minimum quantity is 1");

            return;
        }

        quantity--;

        updateCartItem(
            orderItemId,
            quantity);
    });

$(document).on("keydown",
    ".special-instruction",
    function (e) {

        if (e.key !== "Enter") {
            return;
        }

        e.preventDefault();
        const textarea = $(this);

        const orderItemId =textarea.data("id");

        const quantity =textarea.data("qty");

        const instruction = textarea.val();

            updateCartItem(
                orderItemId,
                quantity,
                instruction);

       
    });

function updateCartItem(
    orderItemId,
    quantity,
    specialInstruction = null) {

    $.ajax({

        url: "/Order/UpdateCartItem",

        type: "POST",

        data: {

            orderItemId: orderItemId,

            quantity: quantity,

            specialInstruction:
                specialInstruction
        },

        success: function (response) {

            if (response.success) {

                const orderId =
                    $("#currentOrderId").val();

                reloadCart(orderId);

                showSuccess(response.message);
            }
            else {

                showError(response.message);
            }
        },

        error: function () {

            showError("Something went wrong.");
        }
    });
}

function reloadCart(orderId) {
    $.get(
        `/Order/GetCart?orderId=${orderId}`,

        function (html) {
            $("#cartCanvasContent")
                .html(html);

            updateCartCount();

           
        });
}

//Delete cart item

$(document).on(
    "click",
    ".cart-delete-btn",
    function () {


        const orderItemId =
            $(this).data("id");

       
        console.log(orderItemId);

        DeletehandleAction(
            "/Order/DeleteCartItem",
            orderItemId,
            this,
            "delete",

            function () {

                const orderId =
                    $("#currentOrderId").val();

                reloadCart(orderId);
            });
    });

$(document).on(
    "click",
    "#confirmOrderBtn",
    function () {

        const orderId =
            $("#currentOrderId").val();

        confirmOrder(orderId);
    });
function confirmOrder(orderId) {

    $.ajax({

        url:
            `/Order/ConfirmOrder?orderId=${orderId}`,

        type: "POST",

        success: function (response) {

            if (response.success) {

                showSuccess(
                    response.message);

                setTimeout(() => {

                    window.location.href =
                        "/Order/Index";

                }, 1200);
            }
            else {

                showError(
                    response.message);
            }
        },

        error: function () {

            showError(
                "Something went wrong.");
        }
    });
}