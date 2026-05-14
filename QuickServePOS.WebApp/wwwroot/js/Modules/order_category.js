// =========================
// DOCUMENT READY
// =========================

$(document).ready(function () {

    bindCategoryEvents();

});

// =========================
// CATEGORY EVENTS
// =========================

function bindCategoryEvents() {
    $(document).on(
        "click",
        ".category-tab-btn",
        onCategoryClick);
}

// =========================
// CATEGORY CLICK
// =========================

function onCategoryClick(e) {
    e.preventDefault();

    // ACTIVE TAB UI

    $(".category-tab-btn")
        .removeClass("active btn-dark")
        .addClass("btn-outline-dark");

    $(this)
        .removeClass("btn-outline-dark")
        .addClass("active btn-dark");

    // CATEGORY ID

    const categoryId =
        $(this).data("categoryid");

    // LOAD MENU ITEMS

    $("#menuItemsContainer").load(
        `/Order/GetMenuItemsByCategory?categoryId=${categoryId}`
    );
}

// =========================
// QUANTITY PLUS
// =========================

$(document).on(
    "click",
    ".qty-plus-btn",
    function (e) {
        e.preventDefault();

        const input =
            getQtyInput($(this));

        let qty =
            parseInt(input.val()) || 1;

        qty++;

        input.val(qty);
    });

// =========================
// QUANTITY MINUS
// =========================

$(document).on(
    "click",
    ".qty-minus-btn",
    function (e) {
        e.preventDefault();

        const input =
            getQtyInput($(this));

        let qty =
            parseInt(input.val()) || 1;

        if (qty > 1) {
            qty--;
        }

        input.val(qty);
    });

// =========================
// GET QTY INPUT
// =========================

function getQtyInput(element) {
    return element
        .closest(".card-body")
        .find(".item-qty-input");
}

// =========================
// ADD TO CART
// =========================

$(document).on(
    "click",
    ".add-to-cart-btn",
    function (e) {
        e.preventDefault();

        const button = $(this);

        const cardBody =
            button.closest(".card-body");

        const menuItemId =
            button.data("menuitemid");

        const quantity =
            parseInt(
                cardBody
                    .find(".item-qty-input")
                    .val()) || 1;

        const orderId =
            $("#OrderId").val();

        // DISABLE BUTTON

        button.prop("disabled", true);

        $.ajax({

            url: '/Order/AddItem',

            type: 'POST',

            data:
            {
                OrderId: orderId,
                MenuItemId: menuItemId,
                Quantity: quantity
            },

            success: function (response) {
                if (response.success) {
                    // SUCCESS TOAST

                    Swal.fire({
                        toast: true,
                        position: 'top-end',
                        icon: 'success',
                        title: response.message,
                        showConfirmButton: false,
                        timer: 1500
                    });

                    // RESET QTY

                    cardBody
                        .find(".item-qty-input")
                        .val(1);

                    // RELOAD CART

                    loadCart(orderId);
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.message
                    });
                }
            },

            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Server Error',
                    text: 'Something went wrong.'
                });
            },

            complete: function () {
                button.prop("disabled", false);
            }

        });

    });

// =========================
// LOAD CART
// =========================

function loadCart(orderId) {
    $("#cartDrawerContainer").load(
        `/Order/LoadCart?orderId=${orderId}`,
        function () {
            updateCartBadge();
        });
}

// =========================
// UPDATE CART BADGE
// =========================

function updateCartBadge() {
    const count =
        $("#cartDrawerContainer")
            .find("tbody tr")
            .length;

    $("#cartBadgeCount")
        .text(count);
}