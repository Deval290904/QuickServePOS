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

            alert(
                "Minimum quantity is 1");

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

        clearTimeout(
            textarea.data("timer"));

        const wait = setTimeout(() => {

            const orderItemId =
                textarea.data("id");

            const quantity =
                textarea.data("qty");

            const instruction =
                textarea.val();

            updateCartItem(
                orderItemId,
                quantity,
                instruction);

        }, 800);

        textarea.data("timer", wait);
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

                alert(response.message);
            }
            else {

                alert(response.message);
            }
        },

        error: function () {

            alert("Something went wrong.");
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