$(document).ready(function () {

    function LoadProductList() {

        $.ajax({
            url: "/prodigious/api/products",
            type: "GET",
            dataType: "json",
            cache: false,
            success: function (data) {

                var pList = $.parseJSON(data);
                var pTemplate = $("#productTrTemplate").html();
                var pTemplateCompiled = Handlebars.compile(pTemplate);
                var pHtml = pTemplateCompiled(pList);

                $('#productListTable tr:last').after(pHtml);

                $("a.edit").click(function () {
                    $('#myModal').modal('toggle');

                    var prodString = JSON.stringify($(this).attr("data-json"));
                    var prodJson = $.parseJSON($(this).attr("data-json"));

                    $("#productId").val(prodJson.id);
                    $("#productName").val(prodJson.Name);
                    $("#productNumber").val(prodJson.Number);
                    $("#productPrice").val(prodJson.Price);
                    $("#productPhoto").val(prodJson.PhotoName);

                    $("#operationType").val("updateProduct");
                });
            },
            error: function (xhr, status, error) {
                alert(status + " : " + error + " - " + xhr.responseJSON.Message);
            }
        });

    }

    function GetProductById(id) {
        $("#errorProductId").hide();
        var rx = new RegExp(/^\d+$/);
        if ($("#searchProductId").val() === "" || !rx.test($("#searchProductId").val())) {
            $("#errorProductId").show();
        }
        else {
            $.ajax({
                url: "/prodigious/api/singleproduct",
                type: "GET",
                dataType: "json",
                cache: false,
                data: { productId: id },
                success: function (data) {
                    var p = $.parseJSON(data);
                    var resultHtml = "";

                    if (p.message !== undefined) {
                        resultHtml = "<span class=\"label label-info\">" + p.message + "</span>";
                    }
                    else {
                        var pTemplate = $("#singleProductTemplate").html();
                        var pTemplateCompiled = Handlebars.compile(pTemplate);
                        var pHtml = pTemplateCompiled(p);
                        resultHtml = pHtml;
                    }

                    $("#productSearchResult").html(resultHtml);
                },
                error: function (xhr, status, error) {
                    alert(status + " : " + error + " - " + xhr.responseJSON.Message);
                }
            });

        }
    }

    function CreateProduct() {
        if (ValidateProduct() === true) {

            var productName = $("#productName").val();
            var productNumber = $("#productNumber").val();
            var productPrice = $("#productPrice").val();
            var productPhoto = $("#productPhoto").val();

            var productObj = "{ Id: 0, Name: '" + productName +
                "', Number: '" + productNumber +
                "', Price: " + productPrice +
                ", PhotoName: '" + productPhoto + "'}";

            $.ajax({
                url: "/prodigious/api/createproduct",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(productObj),
                success: function (data) {
                    if (data === "true") {

                        alert("Product " + productName + " saved successfully!");

                        $("#productName").val("");
                        $("#productNumber").val("");
                        $("#productPrice").val("");
                        $("#productPhoto").val("");

                        //$("#productListTable tbody").empty();
                        $('#productListTable tr').not(function () { return !!$(this).has('th').length; }).remove();
                        LoadProductList();

                        $('#myModal').modal('toggle');
                    }
                },
                error: function (xhr, status, error) {
                    alert(status + " : " + error + " - " + xhr.responseJSON.Message);
                }
            });
        }        
    }

    function UpdateProduct() {
        if (ValidateProduct() === true) {

            var productId = $("#productId").val();
            var productName = $("#productName").val();
            var productNumber = $("#productNumber").val();
            var productPrice = $("#productPrice").val();
            var productPhoto = $("#productPhoto").val();

            var productObj = "{ Id: " + productId +
                " , Name: '" + productName +
                "', Number: '" + productNumber +
                "', Price: " + productPrice +
                ", PhotoName: '" + productPhoto + "'}";

            $.ajax({
                url: "/prodigious/api/updateproduct",
                type: "PUT",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(productObj),
                success: function (data) {
                    if (data === "true") {
                        alert("Product " + productName + " updated successfully!");

                        $("#productId").val("");
                        $("#productName").val("");
                        $("#productNumber").val("");
                        $("#productPrice").val("");
                        $("#productPhoto").val("");

                        //$("#productListTable tbody").empty();
                        $('#productListTable tr').not(function () { return !!$(this).has('th').length; }).remove();
                        LoadProductList();

                        $('#myModal').modal('toggle');
                    }
                },
                error: function (xhr, status, error) {
                    alert(status + " : " + error + " - " + xhr.responseJSON.Message);
                }
            });
        }
    }

    $("#btnNewProduct").click(function () {
        var operation = $("#operationType").val();

        if (operation === "createProduct") {
            CreateProduct();
            return;
        }

        if (operation === "updateProduct") {
            UpdateProduct();
            return;
        }
    });

    $('#myModal').on('hidden.bs.modal', function () {
        $("#operationType").val("createProduct");
    })

    $("#btnSearchProduct").click(function () {
        var productId = $("#searchProductId").val();
        GetProductById(productId);

    });

    //Validate home login input
    $("#Email").blur(function () {
        if (this.value === "") {
            $("#Email").get(0).setCustomValidity("Please enter a valid email");
        } else {
            this.setCustomValidity("");
        }
    });

    $("#Password").blur(function () {
        if (this.value === "") {
            $("#Password").get(0).setCustomValidity("Please enter the password");
        } else {
            this.setCustomValidity("");
        }
    });

    //Validate product form
    function ValidateProduct() {
        valid = true;

        if ($("#productName").val() === "") {
            $("#errorName").show();
            valid = false;
        } else {
            $("#errorName").hide();
        }
        
        if ($("#productNumber").val() === "") {
            $("#errorNumber").show();
            valid = false;
        } else {
            $("#errorNumber").hide();
        }

        if ($("#productPrice").val() === "" || isNaN(parseFloat($("#productPrice").val()))) {
            $("#errorPrice").show();
            valid = false;
        } else {
            $("#errorPrice").hide();
        }

        if ($("#productPhoto").val() === "") {
            $("#errorPhoto").show();
            valid = false;
        } else {
            $("#errorPhoto").hide();
        }

        return valid;
    }
        

    //Execute only for shop controller
    if ($("#productId").length > 0) {
        LoadProductList();
    }    

});