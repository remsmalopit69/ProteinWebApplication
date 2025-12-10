app.controller("ProteinWebApplicationController", function ($scope, ProteinWebApplicationService) {

    // ==================== INITIALIZATION ====================
    $scope.currentView = 'dashboard';
    $scope.categories = [];
    $scope.products = [];
    $scope.images = [];
    $scope.orders = [];
    $scope.dashboardStats = {};
    $scope.editMode = false;
    $scope.currentUser = null;
    $scope.isUserLoggedIn = false;
    $scope.showUserDropdown = false;

    // ==================== USER STATE CHECK ====================
    $scope.checkUserLogin = function () {
        var getCurrentUser = ProteinWebApplicationService.getCurrentUser();
        getCurrentUser.then(function (response) {
            if (response.data.isLoggedIn) {
                $scope.isUserLoggedIn = true;
                $scope.currentUser = {
                    userID: response.data.userID,
                    userName: response.data.userName,
                    userEmail: response.data.userEmail
                };
            } else {
                $scope.isUserLoggedIn = false;
                $scope.currentUser = null;
            }
        });
    }

    // ==================== DROPDOWN TOGGLE ====================
    $scope.toggleUserDropdown = function () {
        $scope.showUserDropdown = !$scope.showUserDropdown;
    }

    // Close dropdown when clicking outside
    angular.element(document).on('click', function (event) {
        var dropdown = angular.element(event.target).closest('.relative');
        if (dropdown.length === 0 && $scope.showUserDropdown) {
            $scope.$apply(function () {
                $scope.showUserDropdown = false;
            });
        }
    });



    // ==================== USER AUTHENTICATION ====================
    $scope.userLoginData = {};
    $scope.userRegisterData = {};

    $scope.userLogin = function () {
        if (!$scope.userLoginData.username || !$scope.userLoginData.password) {
            Swal.fire({
                icon: 'warning',
                title: 'Required Fields',
                text: 'Please enter username and password'
            });
            return;
        }

        var loginRequest = ProteinWebApplicationService.loginUser($scope.userLoginData.username, $scope.userLoginData.password);
        loginRequest.then(function (response) {
            if (response.data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Login Successful',
                    text: response.data.message,
                    timer: 1500,
                    showConfirmButton: false
                }).then(function () {
                    if (response.data.role === 'admin') {
                        window.location.href = "/Admin/Dashboard";
                    } else {
                        window.location.href = "/Shop/Index";
                    }                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Login Failed',
                    text: response.data.message
                });
            }
        });
    }

    $scope.userRegister = function () {
        // Validation
        if (!$scope.userRegisterData.fullName || !$scope.userRegisterData.username ||
            !$scope.userRegisterData.email || !$scope.userRegisterData.password) {
            Swal.fire({
                icon: 'warning',
                title: 'Required Fields',
                text: 'Please fill in all required fields'
            });
            return;
        }

        // Check if passwords match
        if ($scope.userRegisterData.password !== $scope.userRegisterData.confirmPassword) {
            Swal.fire({
                icon: 'error',
                title: 'Password Mismatch',
                text: 'Passwords do not match'
            });
            return;
        }

        // Check if terms are accepted
        if (!$scope.userRegisterData.acceptTerms) {
            Swal.fire({
                icon: 'warning',
                title: 'Terms Required',
                text: 'Please accept the terms and conditions'
            });
            return;
        }

        var registerRequest = ProteinWebApplicationService.registerUser($scope.userRegisterData);
        registerRequest.then(function (response) {
            if (response.data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Registration Successful',
                    text: response.data.message,
                    confirmButtonText: 'Go to Login'
                }).then(function () {
                    window.location.href = "/Account/Login";
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Registration Failed',
                    text: response.data.message
                });
            }
        });
    }

    $scope.userLogout = function () {
        Swal.fire({
            title: 'Are you sure?',
            text: 'You will be logged out',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#000000',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, logout'
        }).then((result) => {
            if (result.isConfirmed) {
                var logoutRequest = ProteinWebApplicationService.logoutUser();
                logoutRequest.then(function (response) {
                    window.location.href = "/Shop/Index";
                });
            }
        });
    }

    // ==================== ADMIN AUTHENTICATION ====================
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
                    window.location.href = "/Account/Login";
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
        var getStats = ProteinWebApplicationService.getDashboardStats();
        getStats.then(function (response) {
            $scope.dashboardStats = response.data;
        });

        var getSalesData = ProteinWebApplicationService.getSalesChartData();
        getSalesData.then(function (response) {
            $scope.renderSalesChart(response.data);
        });

        var getCategoryData = ProteinWebApplicationService.getProductsByCategoryData();
        getCategoryData.then(function (response) {
            $scope.renderCategoryChart(response.data);
        });

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

    // ==================== PUBLIC SHOP ====================
    $scope.products = [];
    $scope.categories = [];
    $scope.banners = [];
    $scope.selectedCategory = null;
    $scope.mobileMenuOpen = false;
    $scope.cart = [];

    $scope.toggleMobileMenu = function () {
        $scope.mobileMenuOpen = !$scope.mobileMenuOpen;
    }

    $scope.loadProducts = function () {
        var getProducts = ProteinWebApplicationService.getAllProducts();
        getProducts.then(function (response) {
            $scope.products = response.data;
        });
    }

    $scope.loadCategories = function () {
        var getCategories = ProteinWebApplicationService.getAllCategories();
        getCategories.then(function (response) {
            $scope.categories = response.data;
        });
    }

    $scope.loadBanners = function () {
        var getBanners = ProteinWebApplicationService.getBannerImages();
        getBanners.then(function (response) {
            $scope.banners = response.data;
        });
    }

    $scope.loadProductsByCategory = function (categoryID) {
        $scope.selectedCategory = categoryID;
        var getProducts = ProteinWebApplicationService.getProductsByCategory(categoryID);
        getProducts.then(function (response) {
            $scope.products = response.data;
        });
    }

    $scope.loadProductDetails = function (productID) {
        var getProduct = ProteinWebApplicationService.getProductDetails(productID);
        getProduct.then(function (response) {
            $scope.productDetails = response.data;
        });
    }

    $scope.viewProduct = function (productID) {
        window.location.href = "/Shop/ProductDetails?id=" + productID;
    }

    $scope.addToCart = function (product) {
        $scope.cart.push(product);
        Swal.fire({
            icon: 'success',
            title: 'Added to Cart',
            text: 'Product added successfully!',
            timer: 1500,
            showConfirmButton: false
        });
    }

    $scope.filterByCategory = function (categoryID) {
        if (categoryID === null) {
            $scope.loadProducts();
        } else {
            $scope.loadProductsByCategory(categoryID);
        }
    }

    // Load images by type
    $scope.loadImagesByType = function (type) {
        var getImages = ProteinWebApplicationService.getImagesByType(type);
        getImages.then(function (response) {
            $scope.availableImages = response.data;
        });
    }

    // Assign image to product
    $scope.assignProductImage = function (imageID) {
        if ($scope.productForm.productID) {
            var assign = ProteinWebApplicationService.assignImageToProduct($scope.productForm.productID, imageID);
            assign.then(function (response) {
                if (response.data.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Success!',
                        text: 'Image assigned to product',
                        timer: 1500,
                        showConfirmButton: false
                    });
                    $scope.loadProducts();
                }
            });
        }
    }

    // Assign image to category
    $scope.assignCategoryImage = function (imageID) {
        if ($scope.categoryForm.categoryID) {
            var assign = ProteinWebApplicationService.assignImageToCategory($scope.categoryForm.categoryID, imageID);
            assign.then(function (response) {
                if (response.data.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Success!',
                        text: 'Image assigned to category',
                        timer: 1500,
                        showConfirmButton: false
                    });
                    $scope.loadCategories();
                }
            });
        }
    }

    // Load hero image
    $scope.loadHeroImage = function () {
        var getHero = ProteinWebApplicationService.getHeroImage();
        getHero.then(function (response) {
            $scope.heroImage = response.data;
        });
    }

    // Load feature images
    $scope.loadFeatureImages = function () {
        var getFeatures = ProteinWebApplicationService.getFeatureImages();
        getFeatures.then(function (response) {
            $scope.featureImages = response.data;
        });
    }

    // Set as active image
    $scope.setAsActive = function (imageID, imageType) {
        var setActive = ProteinWebApplicationService.setAsActiveImage(imageID, imageType);
        setActive.then(function (response) {
            if (response.data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success!',
                    text: 'Image set as active',
                    timer: 1500,
                    showConfirmButton: false
                });
                $scope.loadImages();
            }
        });
    }
});