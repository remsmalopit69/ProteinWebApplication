app.service("ProteinWebApplicationService", function ($http) {
    // ==================== ADMIN AUTHENTICATION ====================
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

    // ==================== USER AUTHENTICATION ====================
    this.registerUser = function (userData) {
        var response = $http({
            method: "post",
            url: "/Account/RegisterUser",
            data: userData
        });
        return response;
    }

    this.loginUser = function (username, password) {
        var response = $http({
            method: "post",
            url: "/Account/LoginUser",
            params: {
                username: username,
                password: password
            }
        });
        return response;
    }

    this.logoutUser = function () {
        return $http.get("/Account/LogoutUser");
    }

    this.getCurrentUser = function () {
        return $http.get("/Account/GetCurrentUser");
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

    // ==================== PUBLIC SHOP ====================
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

    // Get images by type
    this.getImagesByType = function (imageType) {
        return $http.get("/Admin/GetImagesByType", {
            params: { imageType: imageType }
        });
    }

    // Assign image to product
    this.assignImageToProduct = function (productID, imageID) {
        return $http({
            method: "post",
            url: "/Admin/AssignImageToProduct",
            params: {
                productID: productID,
                imageID: imageID
            }
        });
    }

    // Assign image to category
    this.assignImageToCategory = function (categoryID, imageID) {
        return $http({
            method: "post",
            url: "/Admin/AssignImageToCategory",
            params: {
                categoryID: categoryID,
                imageID: imageID
            }
        });
    }

    // Get hero image
    this.getHeroImage = function () {
        return $http.get("/Shop/GetHeroImage");
    }

    // Get feature images
    this.getFeatureImages = function () {
        return $http.get("/Shop/GetFeatureImages");
    }

    // Set as active image
    this.setAsActiveImage = function (imageID, imageType) {
        return $http({
            method: "post",
            url: "/Admin/SetAsActiveImage",
            params: {
                imageID: imageID,
                imageType: imageType
            }
        });
    }

    // Get single image by type
    this.getImageByType = function (imageType) {
        return $http.get("/Admin/GetImageByType", {
            params: { imageType: imageType }
        });
    }

    // Add these methods to Service.js in the ProteinWebApplicationService

    // Get archived images
    this.getArchivedImages = function () {
        return $http.get("/Admin/GetArchivedImages");
    }

    // Restore archived image
    this.restoreImage = function (imageID) {
        var response = $http({
            method: "post",
            url: "/Admin/RestoreImage",
            params: {
                imageID: imageID
            }
        });
        return response;
    }

    // Add these new methods to your Service.js

    // Get images for a specific product
    this.getProductImages = function (productID) {
        return $http.get("/Admin/GetProductImages", {
            params: { productID: productID }
        });
    }

    // Get images for a specific category
    this.getCategoryImages = function (categoryID) {
        return $http.get("/Admin/GetCategoryImages", {
            params: { categoryID: categoryID }
        });
    }
    // Add these methods to your Service.js in the ProteinWebApplicationService

    // ==================== CHECKOUT ====================
    this.processCheckout = function (checkoutData) {
        var response = $http({
            method: "post",
            url: "/Checkout/ProcessCheckout",
            data: checkoutData
        });
        return response;
    }

    this.getMyOrders = function () {
        return $http.get("/Checkout/GetMyOrders");
    }

    this.getOrderDetails = function (orderID) {
        return $http.get("/Checkout/GetOrderDetails", {
            params: { orderID: orderID }
        });
    }
});