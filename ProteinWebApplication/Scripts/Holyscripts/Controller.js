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

    // Replace the deleteImage function in Controller.js with this:

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
                    console.log('Archive response:', response.data); // Debug log
                    if (response.data.success) {
                        $scope.images = response.data.data;
                        Swal.fire({
                            icon: 'success',
                            title: 'Archived!',
                            text: response.data.message || 'Image archived successfully',
                            timer: 1500,
                            showConfirmButton: false
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.data.message || 'Failed to archive image'
                        });
                    }
                }, function (error) {
                    console.error('Archive error:', error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'An error occurred while archiving the image'
                    });
                });
            }
        });
    }

    // Add these functions to Controller.js in the ProteinWebApplicationController

    // Initialize archived images array
    $scope.archivedImages = [];
    $scope.showArchived = false;

    // Load archived images
    $scope.loadArchivedImages = function () {
        var getArchivedImages = ProteinWebApplicationService.getArchivedImages();
        getArchivedImages.then(function (response) {
            $scope.archivedImages = response.data;
        });
    }

    // Toggle between active and archived images view
    $scope.toggleArchivedView = function () {
        $scope.showArchived = !$scope.showArchived;
        if ($scope.showArchived) {
            $scope.loadArchivedImages();
        } else {
            $scope.loadImages();
        }
    }

    // Restore archived image
    $scope.restoreImage = function (imageID) {
        Swal.fire({
            title: 'Restore Image?',
            text: 'This image will be restored to active images',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#10b981',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, restore it'
        }).then((result) => {
            if (result.isConfirmed) {
                var restoreImage = ProteinWebApplicationService.restoreImage(imageID);
                restoreImage.then(function (response) {
                    if (response.data.success) {
                        $scope.archivedImages = response.data.data;
                        Swal.fire({
                            icon: 'success',
                            title: 'Restored!',
                            text: response.data.message || 'Image restored successfully',
                            timer: 1500,
                            showConfirmButton: false
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.data.message || 'Failed to restore image'
                        });
                    }
                }, function (error) {
                    console.error('Restore error:', error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'An error occurred while restoring the image'
                    });
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

    // Add these improved functions to your Controller.js

    // ==================== IMPROVED PRODUCTS WITH IMAGE MANAGEMENT ====================

    $scope.openProductModal = function (product) {
        if (product) {
            $scope.editMode = true;
            $scope.productForm = angular.copy(product);

            // Load the current product image if it exists
            if (product.productID) {
                var getProductImages = ProteinWebApplicationService.getProductImages(product.productID);
                getProductImages.then(function (response) {
                    if (response.data && response.data.length > 0) {
                        $scope.productForm.selectedImage = response.data[0].imagePath;
                        $scope.productForm.selectedImageID = response.data[0].imageID;
                    }
                });
            }
        } else {
            $scope.editMode = false;
            $scope.productForm = {
                selectedImage: null,
                selectedImageID: null
            };
        }

        // Load available unassigned images
        $scope.loadImagesByType('product');
    }

    // Select image from gallery
    $scope.selectImageForProduct = function (image) {
        $scope.productForm.selectedImage = image.imagePath;
        $scope.productForm.selectedImageID = image.imageID;
    }

    // Upload new image for product
    $scope.uploadProductImage = function (file) {
        if (!file) return;

        // Show loading
        Swal.fire({
            title: 'Uploading...',
            text: 'Please wait while we upload your image',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        var formData = new FormData();
        formData.append('imageFile', file);
        formData.append('imageType', 'product');

        fetch('/Admin/UploadImage', {
            method: 'POST',
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                if (data.success && data.uploadedImage) {
                    $scope.$apply(function () {
                        $scope.productForm.selectedImage = data.uploadedImage.imagePath;
                        $scope.productForm.selectedImageID = data.uploadedImage.imageID;
                        $scope.loadImagesByType('product');
                    });

                    Swal.fire({
                        icon: 'success',
                        title: 'Uploaded!',
                        text: 'Image uploaded successfully',
                        timer: 1500,
                        showConfirmButton: false
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: data.message || 'Upload failed'
                    });
                }
            })
            .catch(error => {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error occurred while uploading'
                });
            });
    }

    // Updated save product with image assignment
    $scope.saveProduct = function () {
        if (!$scope.productForm.productName || !$scope.productForm.price) {
            Swal.fire({
                icon: 'warning',
                title: 'Required Fields',
                text: 'Product name and price are required'
            });
            return;
        }

        var savePromise;

        if ($scope.editMode) {
            savePromise = ProteinWebApplicationService.updateProduct($scope.productForm);
        } else {
            savePromise = ProteinWebApplicationService.addProduct($scope.productForm);
        }

        savePromise.then(function (response) {
            if (response.data.success) {
                var savedProductID = response.data.productID || $scope.productForm.productID;

                // If an image was selected, assign it to the product
                if ($scope.productForm.selectedImageID && savedProductID) {
                    var assignImage = ProteinWebApplicationService.assignImageToProduct(
                        savedProductID,
                        $scope.productForm.selectedImageID
                    );

                    assignImage.then(function (imgResponse) {
                        $scope.loadProducts();
                        Swal.fire({
                            icon: 'success',
                            title: $scope.editMode ? 'Updated!' : 'Added!',
                            text: 'Product saved with image successfully',
                            timer: 1500,
                            showConfirmButton: false
                        });
                    });
                } else {
                    $scope.loadProducts();
                    Swal.fire({
                        icon: 'success',
                        title: $scope.editMode ? 'Updated!' : 'Added!',
                        text: 'Product saved successfully',
                        timer: 1500,
                        showConfirmButton: false
                    });
                }

                $scope.productForm = {};
            }
        });
    }

    // ==================== IMPROVED CATEGORIES WITH IMAGE MANAGEMENT ====================

    $scope.openCategoryModal = function (category) {
        if (category) {
            $scope.editMode = true;
            $scope.categoryForm = angular.copy(category);

            // Load the current category image if it exists
            if (category.categoryID) {
                var getCategoryImages = ProteinWebApplicationService.getCategoryImages(category.categoryID);
                getCategoryImages.then(function (response) {
                    if (response.data && response.data.length > 0) {
                        $scope.categoryForm.selectedImage = response.data[0].imagePath;
                        $scope.categoryForm.selectedImageID = response.data[0].imageID;
                    }
                });
            }
        } else {
            $scope.editMode = false;
            $scope.categoryForm = {
                selectedImage: null,
                selectedImageID: null
            };
        }

        // Load available unassigned images for categories
        $scope.loadCategoryImages();
    }

    // Load images for category selection
    $scope.loadCategoryImages = function () {
        var getImages = ProteinWebApplicationService.getImagesByType('category');
        getImages.then(function (response) {
            $scope.availableCategoryImages = response.data;
        });
    }

    // Select image from gallery for category
    $scope.selectImageForCategory = function (image) {
        $scope.categoryForm.selectedImage = image.imagePath;
        $scope.categoryForm.selectedImageID = image.imageID;
    }

    // Upload new image for category
    $scope.uploadCategoryImage = function (file) {
        if (!file) return;

        Swal.fire({
            title: 'Uploading...',
            text: 'Please wait while we upload your image',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        var formData = new FormData();
        formData.append('imageFile', file);
        formData.append('imageType', 'category');

        fetch('/Admin/UploadImage', {
            method: 'POST',
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                if (data.success && data.uploadedImage) {
                    $scope.$apply(function () {
                        $scope.categoryForm.selectedImage = data.uploadedImage.imagePath;
                        $scope.categoryForm.selectedImageID = data.uploadedImage.imageID;
                        $scope.loadCategoryImages();
                    });

                    Swal.fire({
                        icon: 'success',
                        title: 'Uploaded!',
                        text: 'Image uploaded successfully',
                        timer: 1500,
                        showConfirmButton: false
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: data.message || 'Upload failed'
                    });
                }
            })
            .catch(error => {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error occurred while uploading'
                });
            });
    }

    // Updated save category with image assignment
    $scope.saveCategory = function () {
        if (!$scope.categoryForm.categoryName) {
            Swal.fire({
                icon: 'warning',
                title: 'Required Field',
                text: 'Category name is required'
            });
            return;
        }

        var savePromise;

        if ($scope.editMode) {
            savePromise = ProteinWebApplicationService.updateCategory($scope.categoryForm);
        } else {
            savePromise = ProteinWebApplicationService.addCategory($scope.categoryForm);
        }

        savePromise.then(function (response) {
            if (response.data.success) {
                var savedCategoryID = response.data.categoryID || $scope.categoryForm.categoryID;

                // If an image was selected, assign it to the category
                if ($scope.categoryForm.selectedImageID && savedCategoryID) {
                    var assignImage = ProteinWebApplicationService.assignImageToCategory(
                        savedCategoryID,
                        $scope.categoryForm.selectedImageID
                    );

                    assignImage.then(function (imgResponse) {
                        $scope.loadCategories();
                        Swal.fire({
                            icon: 'success',
                            title: $scope.editMode ? 'Updated!' : 'Added!',
                            text: 'Category saved with image successfully',
                            timer: 1500,
                            showConfirmButton: false
                        });
                    });
                } else {
                    $scope.loadCategories();
                    Swal.fire({
                        icon: 'success',
                        title: $scope.editMode ? 'Updated!' : 'Added!',
                        text: 'Category saved successfully',
                        timer: 1500,
                        showConfirmButton: false
                    });
                }

                $scope.categoryForm = {};
            }
        });
    }

    // Add these cart management functions to your Controller.js
    // Add these cart management functions to your Controller.js
    // Replace the existing cart functions with these fixed versions

    // ==================== SHOPPING CART ====================

    // Initialize cart from localStorage
    $scope.cart = [];
    $scope.cartTotal = 0;

    // Load cart on initialization
    $scope.loadCart = function () {
        try {
            var storedCart = localStorage.getItem('proteinCart');
            if (storedCart) {
                $scope.cart = JSON.parse(storedCart);
                $scope.calculateCartTotal();
            }
        } catch (e) {
            console.error('Error loading cart:', e);
            $scope.cart = [];
        }
    };

    // Save cart to localStorage
    $scope.saveCart = function () {
        try {
            localStorage.setItem('proteinCart', JSON.stringify($scope.cart));
            $scope.calculateCartTotal();
        } catch (e) {
            console.error('Error saving cart:', e);
        }
    };

    // Calculate cart total
    $scope.calculateCartTotal = function () {
        $scope.cartTotal = 0;
        $scope.cart.forEach(function (item) {
            $scope.cartTotal += (item.price * (item.quantity || 1));
        });
    };

    // Add to cart with quantity support
    $scope.addToCart = function (product, quantity) {
        quantity = quantity || 1;

        if (product.stockQuantity < quantity) {
            Swal.fire({
                icon: 'warning',
                title: 'Insufficient Stock',
                text: 'Only ' + product.stockQuantity + ' items available',
                timer: 2000,
                showConfirmButton: false
            });
            return;
        }

        // Check if product already exists in cart
        var existingItem = $scope.cart.find(function (item) {
            return item.productID === product.productID;
        });

        if (existingItem) {
            var newQuantity = existingItem.quantity + quantity;
            if (newQuantity > product.stockQuantity) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Stock Limit',
                    text: 'Cannot add more than available stock',
                    timer: 2000,
                    showConfirmButton: false
                });
                return;
            }
            existingItem.quantity = newQuantity;
        } else {
            var cartItem = {
                productID: product.productID,
                productName: product.productName,
                price: product.price,
                image: product.image || product.images?.[0]?.imagePath,
                stockQuantity: product.stockQuantity,
                quantity: quantity
            };
            $scope.cart.push(cartItem);
        }

        $scope.saveCart();

        Swal.fire({
            icon: 'success',
            title: 'Added to Cart',
            text: product.productName + ' added successfully!',
            timer: 1500,
            showConfirmButton: false
        });
    };

    // Update cart quantity
    $scope.updateCartQuantity = function (item, newQuantity) {
        if (newQuantity < 1) {
            $scope.removeFromCart(item);
            return;
        }

        if (newQuantity > item.stockQuantity) {
            Swal.fire({
                icon: 'warning',
                title: 'Stock Limit',
                text: 'Cannot add more than available stock',
                timer: 2000,
                showConfirmButton: false
            });
            item.quantity = item.stockQuantity;
            return;
        }

        item.quantity = newQuantity;
        $scope.saveCart();
    };

    // Remove from cart
    $scope.removeFromCart = function (index) {
        if (index >= 0 && index < $scope.cart.length) {
            var item = $scope.cart[index];

            Swal.fire({
                title: 'Remove Item?',
                text: 'Remove ' + item.productName + ' from cart?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#ef4444',
                cancelButtonColor: '#6b7280',
                confirmButtonText: 'Yes, remove it'
            }).then((result) => {
                if (result.isConfirmed) {
                    $scope.$apply(function () {
                        $scope.cart.splice(index, 1);
                        $scope.saveCart();
                    });

                    Swal.fire({
                        icon: 'success',
                        title: 'Removed!',
                        text: 'Item removed from cart',
                        timer: 1500,
                        showConfirmButton: false
                    });
                }
            });
        }
    };

    // Get cart count for navbar
    $scope.getCartCount = function () {
        var count = 0;
        $scope.cart.forEach(function (item) {
            count += (item.quantity || 1);
        });
        return count;
    };

    // Get cart subtotal
    $scope.getCartSubtotal = function () {
        return $scope.cartTotal;
    };

    // Get cart total (with shipping if needed)
    $scope.getCartTotal = function () {
        var subtotal = $scope.getCartSubtotal();
        // Free shipping in this case
        return subtotal;
    };

    // ==================== CHECKOUT FUNCTIONS ====================

    // Initialize checkout
    $scope.initCheckout = function () {
        $scope.loadCart();

        if ($scope.cart.length === 0) {
            Swal.fire({
                icon: 'info',
                title: 'Empty Cart',
                text: 'Your cart is empty. Add some products first!',
                confirmButtonText: 'Browse Products'
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = "/Shop/Products";
                }
            });
            return;
        }

        // Pre-fill user data if logged in
        if ($scope.isUserLoggedIn && $scope.currentUser) {
            $scope.checkoutForm = {
                customerName: $scope.currentUser.userName || '',
                customerEmail: $scope.currentUser.userEmail || '',
                customerPhone: '',
                shippingAddress: '',
                orderNotes: ''
            };
        } else {
            $scope.checkoutForm = {};
        }
    };

    // Process checkout
    $scope.processCheckout = function () {
        if (!$scope.isUserLoggedIn) {
            Swal.fire({
                icon: 'info',
                title: 'Login Required',
                text: 'Please login to complete your order',
                showCancelButton: true,
                confirmButtonText: 'Go to Login',
                cancelButtonText: 'Cancel'
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = "/Account/Login";
                }
            });
            return;
        }

        if ($scope.cart.length === 0) {
            Swal.fire({
                icon: 'warning',
                title: 'Empty Cart',
                text: 'Your cart is empty!'
            });
            return;
        }

        // Validate form
        if (!$scope.checkoutForm.customerName || !$scope.checkoutForm.customerEmail ||
            !$scope.checkoutForm.customerPhone || !$scope.checkoutForm.shippingAddress) {
            Swal.fire({
                icon: 'warning',
                title: 'Required Fields',
                text: 'Please fill in all required fields'
            });
            return;
        }

        // Prepare order data
        var checkoutData = {
            customerName: $scope.checkoutForm.customerName,
            customerEmail: $scope.checkoutForm.customerEmail,
            customerPhone: $scope.checkoutForm.customerPhone,
            shippingAddress: $scope.checkoutForm.shippingAddress,
            totalAmount: $scope.getCartTotal(),
            orderItems: $scope.cart.map(function (item) {
                return {
                    productID: item.productID,
                    quantity: item.quantity,
                    unitPrice: item.price
                };
            })
        };

        // Show loading
        Swal.fire({
            title: 'Processing Order...',
            text: 'Please wait',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        // Submit order
        var processCheckout = ProteinWebApplicationService.processCheckout(checkoutData);
        processCheckout.then(function (response) {
            Swal.close();

            if (response.data.success) {
                // Clear cart
                $scope.cart = [];
                localStorage.removeItem('proteinCart');

                // Show success and redirect
                Swal.fire({
                    icon: 'success',
                    title: 'Order Placed!',
                    text: 'Order #' + response.data.orderID + ' has been placed successfully!',
                    confirmButtonText: 'View Order'
                }).then(() => {
                    window.location.href = "/Checkout/Success?orderID=" + response.data.orderID;
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Order Failed',
                    text: response.data.message
                });
            }
        }, function (error) {
            Swal.close();
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'An error occurred while processing your order'
            });
        });
    };

    // Load user orders
    $scope.loadMyOrders = function () {
        if (!$scope.isUserLoggedIn) {
            window.location.href = "/Account/Login";
            return;
        }

        var getOrders = ProteinWebApplicationService.getMyOrders();
        getOrders.then(function (response) {
            if (response.data.success !== false) {
                $scope.myOrders = response.data;
            }
        });
    };

    // Initialize cart when controller loads
    $scope.loadCart();
});