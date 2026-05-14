$(document).on("click", ".category-btn", function () {

    $(".category-btn").removeClass("active");

    $(this).addClass("active");

    const categoryId =
        $(this).data("categoryid");

    const orderId =
        $("#CurrentOrderId").val();

    $("#menuItemsContainer").load(
        `/Order/GetMenuItemsByCategory?categoryId=${categoryId}&orderId=${orderId}`
    );

});

$(document).on("click", ".add-to-cart-btn", function () {

    const button = $(this);

    const menuItemId =
        button.data("menuitemid");

    const orderId =
        button.data("orderid");

    const qty =
        button.closest(".card")
            .find(".item-qty")
            .val();

    $.ajax({

        url: "/Order/AddItem",
        type: "POST",

        data: {
            orderId: orderId,
            menuItemId: menuItemId,
            quantity: qty
        },

        success: function (response) {

            if (response.success) {

                loadCart(orderId);

            } else {

                alert(response.message);
            }
        }
    });

});

function loadCart(orderId) {

    $("#cartDrawerContainer").load(
        `/Order/LoadCart?orderId=${orderId}`
    );

}