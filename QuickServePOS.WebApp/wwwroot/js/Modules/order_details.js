$(document).ready(function () {

    // =========================
    // DEFAULT CATEGORY
    // =========================

    $(".category-btn:first")
        .addClass("active-category");

    const firstCategory = $(".category-btn:first")
        .data("category");

    filterCategory(firstCategory);

    // =========================
    // CATEGORY CLICK
    // =========================

    $(".category-btn").click(function () {

        $(".category-btn")
            .removeClass("active-category");

        $(this)
            .addClass("active-category");

        const categoryId = $(this)
            .data("category");

        filterCategory(categoryId);
    });

    // =========================
    // PLUS
    // =========================

    $(document).on("click", ".plus-btn", function () {

        const input = $(this)
            .siblings(".qty-input");

        let value = parseInt(input.val());

        input.val(value + 1);
    });

    // =========================
    // MINUS
    // =========================

    $(document).on("click", ".minus-btn", function () {

        const input = $(this)
            .siblings(".qty-input");

        let value = parseInt(input.val());

        if (value > 1) {
            input.val(value - 1);
        }
    });

});

// =========================
// FILTER
// =========================

function filterCategory(categoryId) {

    $(".menu-item-card").hide();

    $(`.menu-item-card[data-category='${categoryId}']`)
        .fadeIn(200);
}

// =========================
// ADD TO CART
// =========================

$(document).on("click", ".add-cart-btn", function () {

    const button = $(this);

    const orderId = button.data("order-id");

    const menuItemId = button.data("id");

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

            showToast("Item added successfully");

        },

        error: function () {

            showToast("Unable to add item", true);
        }
    });
});


function reloadCart(orderId) {

    $.get(`/Order/GetCart?orderId=${orderId}`,

        function (html) {

            $("#cartItemsContainer")
                .html(html);

            updateCartCount();
        });
}

function updateCartCount() {

    const count =
        $(".cart-item").length;

    $("#cartCount")
        .text(count);
}

function showToast(message, isError = false) {

    alert(message);
}