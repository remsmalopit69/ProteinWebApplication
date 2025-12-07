app.controller("ProteinWebApplicationController", function ($scope, ProteinWebApplicationService) {


    // ==================== INITIALIZATION ====================
    $scope.currentView = 'dashboard';
    $scope.categories = [];
    $scope.products = [];
    $scope.images = [];
    $scope.orders = [];
    $scope.dashboardStats = {};
    $scope.editMode = false;

    // ==================== AUTHENTICATION ====================
    $scope.loginData = {};

    $scope.login = function () {
        if (!$scope.loginData.username || !$scope.loginData.password) {
            Swal.fire({
                icon: 'warning',
                title: 'Required Fields',
                text: 'Please enter username and password'
            });
            return;
        }

        var loginRequest = ProteinWebApplicationService.loginAdmin($scope.loginData.username, $scope.loginData.password);
        loginRequest.then(function (response) {
            if (response.data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Login Successful',
                    text: response.data.message,
                    timer: 1500,
                    showConfirmButton: false
                }).then(function () {
                    window.location.href = "/Admin/Dashboard";
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Login Failed',
                    text: response.data.message
                });
            }
        });
    }

    $scope.logout = function () {
        Swal.fire({
            title: 'Are you sure?',
            text: 'You will be logged out',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, logout'
        }).then((result) => {
            if (result.isConfirmed) {
                var logoutRequest = ProteinWebApplicationService.logoutAdmin();
                logoutRequest.then(function (response) {
                    window.location.href = "/Admin/Login";
                });
            }
        });
    }

    // ==================== CATEGORIES CRUD ====================
    $scope.categoryForm = {};

    $scope.loadCategories = function () {
        var getCategories = ProteinWebApplicationService.getCategories();
        getCategories.then(function (response) {
            $scope.categories = response.data;
        });
    }

    $scope.openCategoryModal = function (category) {
        if (category) {
            $scope.editMode = true;
            $scope.categoryForm = angular.copy(category);
        } else {
            $scope.editMode = false;
            $scope.categoryForm = {};
        }
    }

    $scope.saveCategory = function () {
        if (!$scope.categoryForm.categoryName) {
            Swal.fire({
                icon: 'warning',
                title: 'Required Field',
                text: 'Category name is required'
            });
            return;
        }

        if ($scope.editMode) {
            var updateCategory = ProteinWebApplicationService.updateCategory($scope.categoryForm);
            updateCategory.then(function (response) {
                if (response.data.success) {
                    $scope.categories = response.data.data;
                    Swal.fire({
                        icon: 'success',
                        title: 'Updated!',
                        text: 'Category updated successfully',
                        timer: 1500,
                        showConfirmButton: false
                    });
                    $scope.categoryForm = {};
                }
            });
        } else {
            var addCategory = ProteinWebApplicationService.addCategory($scope.categoryForm);
            addCategory.then(function (response) {
                if (response.data.success) {
                    $scope.categories = response.data.data;
                    Swal.fire({
                        icon: 'success',
                        title: 'Added!',
                        text: 'Category added successfully',
                        timer: 1500,
                        showConfirmButton: false
                    });
                    $scope.categoryForm = {};
                }
            });
        }
    }

    $scope.deleteCategory = function (categoryID) {
        Swal.fire({
            title: 'Archive Category?',
            text: 'This category will be archived',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, archive it'
        }).then((result) => {
            if (result.isConfirmed) {
                var archiveCategory = ProteinWebApplicationService.archiveCategory(categoryID);
                archiveCategory.then(function (response) {
                    if (response.data.success) {
                        $scope.categories = response.data.data;
                        Swal.fire({
                            icon: 'success',
                            title: 'Archived!',
                            text: 'Category archived successfully',
                            timer: 1500,
                            showConfirmButton: false
                        });
                    }
                });
            }
        });
    }

    // ==================== PRODUCTS CRUD ====================
    $scope.productForm = {};

    $scope.loadProducts = function () {
        var getProducts = ProteinWebApplicationService.getProducts();
        getProducts.then(function (response) {
            $scope.products = response.data;
        });
    }

    $scope.openProductModal = function (product) {
        if (product) {
            $scope.editMode = true;
            $scope.productForm = angular.copy(product);
        } else {
            $scope.editMode = false;
            $scope.productForm = {};
        }
    }

    $scope.saveProduct = function () {
        if (!$scope.productForm.productName || !$scope.productForm.price) {
            Swal.fire({
                icon: 'warning',
                title: 'Required Fields',
                text: 'Product name and price are required'
            });
            return;
        }

        if ($scope.editMode) {
            var updateProduct = ProteinWebApplicationService.updateProduct($scope.productForm);
            updateProduct.then(function (response) {
                if (response.data.success) {
                    $scope.products = response.data.data;
                    Swal.fire({
                        icon: 'success',
                        title: 'Updated!',
                        text: 'Product updated successfully',
                        timer: 1500,
                        showConfirmButton: false
                    });
                    $scope.productForm = {};
                }
            });
        } else {
            var addProduct = ProteinWebApplicationService.addProduct($scope.productForm);
            addProduct.then(function (response) {
                if (response.data.success) {
                    $scope.products = response.data.data;
                    Swal.fire({
                        icon: 'success',
                        title: 'Added!',
                        text: 'Product added successfully',
                        timer: 1500,
                        showConfirmButton: false
                    });
                    $scope.productForm = {};
                }
            });
        }
    }

    $scope.deleteProduct = function (productID) {
        Swal.fire({
            title: 'Archive Product?',
            text: 'This product will be archived',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, archive it'
        }).then((result) => {
            if (result.isConfirmed) {
                var archiveProduct = ProteinWebApplicationService.archiveProduct(productID);
                archiveProduct.then(function (response) {
                    if (response.data.success) {
                        $scope.products = response.data.data;
                        Swal.fire({
                            icon: 'success',
                            title: 'Archived!',
                            text: 'Product archived successfully',
                            timer: 1500,
                            showConfirmButton: false
                        });
                    }
                });
            }
        });
    }

    // ==================== IMAGES CRUD ====================
    $scope.loadImages = function () {
        var getImages = ProteinWebApplicationService.getImages();
        getImages.then(function (response) {
            $scope.images = response.data;
        });
    }

    $scope.deleteImage = function (imageID) {
        Swal.fire({
            title: 'Archive Image?',
            text: 'This image will be archived',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, archive it'
        }).then((result) => {
            if (result.isConfirmed) {
                var archiveImage = ProteinWebApplicationService.archiveImage(imageID);
                archiveImage.then(function (response) {
                    if (response.data.success) {
                        $scope.images = response.data.data;
                        Swal.fire({
                            icon: 'success',
                            title: 'Archived!',
                            text: 'Image archived successfully',
                            timer: 1500,
                            showConfirmButton: false
                        });
                    }
                });
            }
        });
    }

    // ==================== ORDERS ====================
    $scope.loadOrders = function () {
        var getOrders = ProteinWebApplicationService.getOrders();
        getOrders.then(function (response) {
            $scope.orders = response.data;
        });
    }

    $scope.updateStatus = function (orderID, newStatus) {
        var updateStatus = ProteinWebApplicationService.updateOrderStatus(orderID, newStatus);
        updateStatus.then(function (response) {
            if (response.data.success) {
                $scope.orders = response.data.data;
                Swal.fire({
                    icon: 'success',
                    title: 'Updated!',
                    text: 'Order status updated',
                    timer: 1500,
                    showConfirmButton: false
                });
            }
        });
    }

    $scope.deleteOrder = function (orderID) {
        Swal.fire({
            title: 'Archive Order?',
            text: 'This order will be archived',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, archive it'
        }).then((result) => {
            if (result.isConfirmed) {
                var archiveOrder = ProteinWebApplicationService.archiveOrder(orderID);
                archiveOrder.then(function (response) {
                    if (response.data.success) {
                        $scope.orders = response.data.data;
                        Swal.fire({
                            icon: 'success',
                            title: 'Archived!',
                            text: 'Order archived successfully',
                            timer: 1500,
                            showConfirmButton: false
                        });
                    }
                });
            }
        });
    }

    // ==================== DASHBOARD ====================
    $scope.loadDashboard = function () {
        // Load dashboard stats
        var getStats = ProteinWebApplicationService.getDashboardStats();
        getStats.then(function (response) {
            $scope.dashboardStats = response.data;
        });

        // Load Sales Chart
        var getSalesData = ProteinWebApplicationService.getSalesChartData();
        getSalesData.then(function (response) {
            $scope.renderSalesChart(response.data);
        });

        // Load Products by Category Chart
        var getCategoryData = ProteinWebApplicationService.getProductsByCategoryData();
        getCategoryData.then(function (response) {
            $scope.renderCategoryChart(response.data);
        });

        // Load Order Status Chart
        var getOrderData = ProteinWebApplicationService.getOrderStatusData();
        getOrderData.then(function (response) {
            $scope.renderOrderStatusChart(response.data);
        });
    }

    // ==================== CHART RENDERING ====================
    $scope.renderSalesChart = function (data) {
        var ctx = document.getElementById('salesChart').getContext('2d');
        var labels = data.map(function (item) {
            return new Date(item.date).toLocaleDateString();
        });
        var values = data.map(function (item) {
            return item.sales;
        });

        new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Daily Sales',
                    data: values,
                    borderColor: 'rgb(75, 192, 192)',
                    backgroundColor: 'rgba(75, 192, 192, 0.2)',
                    tension: 0.1
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: 'Sales Trend (Last 7 Days)'
                    }
                }
            }
        });
    }

    $scope.renderCategoryChart = function (data) {
        var ctx = document.getElementById('categoryChart').getContext('2d');
        var labels = data.map(function (item) {
            return item.categoryName;
        });
        var values = data.map(function (item) {
            return item.productCount;
        });

        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Products Count',
                    data: values,
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255, 99, 132, 1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: 'Products by Category'
                    }
                }
            }
        });
    }

    $scope.renderOrderStatusChart = function (data) {
        var ctx = document.getElementById('orderStatusChart').getContext('2d');
        var labels = data.map(function (item) {
            return item.status;
        });
        var values = data.map(function (item) {
            return item.count;
        });

        new Chart(ctx, {
            type: 'pie',
            data: {
                labels: labels,
                datasets: [{
                    data: values,
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.8)',
                        'rgba(54, 162, 235, 0.8)',
                        'rgba(255, 206, 86, 0.8)',
                        'rgba(75, 192, 192, 0.8)'
                    ]
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: 'Order Status Distribution'
                    }
                }
            }
        });
    }
});
