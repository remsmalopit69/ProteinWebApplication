app.service("ProteinWebApplicationService", function ($http) {
    // ==================== AUTHENTICATION ====================
    this.loginAdmin = function (username, password) {
        var response = $http({
            method: "post",
            url: "/Admin/LoginAdmin",
            params: {
                username: username,
                password: password
            }
        });
        return response;
    }

    this.logoutAdmin = function () {
        return $http.get("/Admin/LogoutAdmin");
    }

    // ==================== CATEGORIES ====================
    this.getCategories = function () {
        return $http.get("/Admin/GetCategories");
    }

    this.addCategory = function (categoryData) {
        var response = $http({
            method: "post",
            url: "/Admin/AddCategory",
            data: categoryData
        });
        return response;
    }

    this.updateCategory = function (categoryData) {
        var response = $http({
            method: "post",
            url: "/Admin/UpdateCategory",
            data: categoryData
        });
        return response;
    }

    this.archiveCategory = function (categoryID) {
        var response = $http({
            method: "post",
            url: "/Admin/ArchiveCategory",
            params: {
                categoryID: categoryID
            }
        });
        return response;
    }

    // ==================== PRODUCTS ====================
    this.getProducts = function () {
        return $http.get("/Admin/GetProducts");
    }

    this.addProduct = function (productData) {
        var response = $http({
            method: "post",
            url: "/Admin/AddProduct",
            data: productData
        });
        return response;
    }

    this.updateProduct = function (productData) {
        var response = $http({
            method: "post",
            url: "/Admin/UpdateProduct",
            data: productData
        });
        return response;
    }

    this.archiveProduct = function (productID) {
        var response = $http({
            method: "post",
            url: "/Admin/ArchiveProduct",
            params: {
                productID: productID
            }
        });
        return response;
    }

    // ==================== IMAGES ====================
    this.getImages = function () {
        return $http.get("/Admin/GetImages");
    }

    this.archiveImage = function (imageID) {
        var response = $http({
            method: "post",
            url: "/Admin/ArchiveImage",
            params: {
                imageID: imageID
            }
        });
        return response;
    }

    // ==================== ORDERS ====================
    this.getOrders = function () {
        return $http.get("/Admin/GetOrders");
    }

    this.updateOrderStatus = function (orderID, orderStatus) {
        var response = $http({
            method: "post",
            url: "/Admin/UpdateOrderStatus",
            params: {
                orderID: orderID,
                orderStatus: orderStatus
            }
        });
        return response;
    }

    this.archiveOrder = function (orderID) {
        var response = $http({
            method: "post",
            url: "/Admin/ArchiveOrder",
            params: {
                orderID: orderID
            }
        });
        return response;
    }

    // ==================== DASHBOARD ANALYTICS ====================
    this.getDashboardStats = function () {
        return $http.get("/Admin/GetDashboardStats");
    }

    this.getSalesChartData = function () {
        return $http.get("/Admin/GetSalesChartData");
    }

    this.getProductsByCategoryData = function () {
        return $http.get("/Admin/GetProductsByCategoryData");
    }

    this.getOrderStatusData = function () {
        return $http.get("/Admin/GetOrderStatusData");
    }

    this.getAllProducts = function () {
        return $http.get("/Shop/GetAllProducts");
    }

    this.getAllCategories = function () {
        return $http.get("/Shop/GetAllCategories");
    }

    this.getProductsByCategory = function (categoryID) {
        var response = $http({
            method: "get",
            url: "/Shop/GetProductsByCategory",
            params: {
                categoryID: categoryID
            }
        });
        return response;
    }

    this.getProductDetails = function (productID) {
        var response = $http({
            method: "get",
            url: "/Shop/GetProductDetails",
            params: {
                productID: productID
            }
        });
        return response;
    }

    this.getBannerImages = function () {
        return $http.get("/Shop/GetBannerImages");
    }
});