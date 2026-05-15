$(document).ready(function () {

    // =========================
    // CATEGORY CLICK
    // =========================

    $(".category-btn").click(function () {

        // ACTIVE BUTTON

        $(".category-btn")
            .removeClass("active-category");

        $(this)
            .addClass("active-category");

        // CATEGORY

        const categoryId =
            $(this).data("category");

        // SHOW ALL

        if (categoryId === "all") {

            $(".menu-item-card")
                .show();

            return;
        }

        // FILTER ITEMS

        $(".menu-item-card")
            .hide();

        $(`.menu-item-card[data-category='${categoryId}']`)
            .show();
    });

    // =========================
    // PLUS QTY
    // =========================

    $(document).on("click", ".plus-btn", function () {

        const input = $(this)
            .siblings(".qty-input");

        let value =
            parseInt(input.val());

        input.val(value + 1);
    });

    // =========================
    // MINUS QTY
    // =========================

    $(document).on("click", ".minus-btn", function () {

        const input = $(this)
            .siblings(".qty-input");

        let value =
            parseInt(input.val());

        if (value > 1) {
            input.val(value - 1);
        }
    });

    // =========================
    // ADD TO CART
    // =========================

    $(document).on("click", ".add-cart-btn", function () {

        const button = $(this);

        const orderId =
            button.data("order-id");

        const menuItemId =
            button.data("id");

        const quantity = button
            .closest(".card-body")
            .find(".qty-input")
            .val();

        const dto = {

            orderId: orderId,

            menuItemId: menuItemId,

            quantity: parseInt(quantity)
        };

        $.ajax({

            url: "/Order/AddItem",

            type: "POST",

            data: dto,

            success: function (response) {

                reloadCart(orderId);

                showSuccess(response.message);
            },

            error: function () {

                showError(response.error || "Failed to add item to cart");
                    
            }
        });

    });

});

// =========================
// RELOAD CART
// =========================

function reloadCart(orderId) {
    $.get(
        `/Order/GetCart?orderId=${orderId}`,

        function (html) {
            $("#cartCanvasContent")
                .html(html);

            updateCartCount();

            // OPEN CART

            const offcanvas =
                new bootstrap.Offcanvas(
                    document.getElementById(
                        "cartCanvas"));

            offcanvas.show();
        });
}

// =========================
// CART COUNT
// =========================

function updateCartCount() {
    const count =
        $(".cart-item").length;

    $("#cartCount")
        .text(count);
}
