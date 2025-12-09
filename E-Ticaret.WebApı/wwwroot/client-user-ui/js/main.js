// ===== GLOBAL VARIABLES =====
let cart = JSON.parse(localStorage.getItem('cart')) || [];
let isScrolled = false;

// ===== DOM CONTENT LOADED =====
document.addEventListener('DOMContentLoaded', function () {
    // Initialize AOS
    AOS.init({
        duration: 800,
        easing: 'ease-in-out',
        once: true
    });

    // Initialize components
    initializeNavbar();
    initializeTheme();
    initializeScrollToTop();
    initializeCart();
    initializeNewsletterForm();
    loadProductPreview();
});

// ===== NAVBAR FUNCTIONALITY =====
function initializeNavbar() {
    const navbar = document.getElementById('mainNavbar');

    window.addEventListener('scroll', function () {
        const scrollTop = window.pageYOffset || document.documentElement.scrollTop;

        if (scrollTop > 100 && !isScrolled) {
            navbar.classList.add('scrolled');
            isScrolled = true;
        } else if (scrollTop <= 100 && isScrolled) {
            navbar.classList.remove('scrolled');
            isScrolled = false;
        }
    });
}

// ===== THEME FUNCTIONALITY =====
function initializeTheme() {
    const themeToggle = document.getElementById('themeToggle');
    const themeIcon = document.getElementById('themeIcon');
    const currentTheme = localStorage.getItem('theme') || 'light';

    // Set initial theme
    document.documentElement.setAttribute('data-theme', currentTheme);
    updateThemeIcon(currentTheme, themeIcon);

    // Theme toggle event
    if (themeToggle) {
        themeToggle.addEventListener('click', function () {
            const currentTheme = document.documentElement.getAttribute('data-theme');
            const newTheme = currentTheme === 'dark' ? 'light' : 'dark';

            document.documentElement.setAttribute('data-theme', newTheme);
            localStorage.setItem('theme', newTheme);
            updateThemeIcon(newTheme, themeIcon);

            // Add animation
            themeToggle.style.transform = 'rotate(360deg)';
            setTimeout(() => {
                themeToggle.style.transform = 'rotate(0deg)';
            }, 300);
        });
    }
}

function updateThemeIcon(theme, iconElement) {
    if (iconElement) {
        iconElement.className = theme === 'dark' ? 'fas fa-sun' : 'fas fa-moon';
    }
}

// ===== SCROLL TO TOP FUNCTIONALITY =====
function initializeScrollToTop() {
    const scrollBtn = document.getElementById('scrollToTop');

    if (scrollBtn) {
        window.addEventListener('scroll', function () {
            if (window.pageYOffset > 300) {
                scrollBtn.classList.add('show');
            } else {
                scrollBtn.classList.remove('show');
            }
        });

        scrollBtn.addEventListener('click', function () {
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });
    }
}

// ===== CART FUNCTIONALITY =====
function initializeCart() {
    const cartBtn = document.getElementById('cartBtn');
    const cartModal = new bootstrap.Modal(document.getElementById('cartModal'));

    updateCartCount();

    if (cartBtn) {
        cartBtn.addEventListener('click', function () {
            displayCartItems();
            cartModal.show();
        });
    }
}

function addToCart(productId, productName, productPrice, productImage) {
    const existingItem = cart.find(item => item.id === productId);

    if (existingItem) {
        existingItem.quantity += 1;
        showNotification('Ürün sepete eklendi!', 'success');
    } else {
        cart.push({
            id: productId,
            name: productName,
            price: productPrice,
            image: productImage,
            quantity: 1
        });
        showNotification('Ürün sepete eklendi!', 'success');
    }

    localStorage.setItem('cart', JSON.stringify(cart));
    updateCartCount();

    // Add animation to cart button
    const cartBtn = document.getElementById('cartBtn');
    cartBtn.classList.add('pulse-animation');
    setTimeout(() => cartBtn.classList.remove('pulse-animation'), 600);
}

function removeFromCart(productId) {
    cart = cart.filter(item => item.id !== productId);
    localStorage.setItem('cart', JSON.stringify(cart));
    updateCartCount();
    displayCartItems();
    showNotification('Ürün sepetten kaldırıldı!', 'warning');
}

function updateCartCount() {
    const cartCount = document.getElementById('cartCount');
    if (cartCount) {
        const totalItems = cart.reduce((sum, item) => sum + item.quantity, 0);
        cartCount.textContent = totalItems;
        cartCount.style.display = totalItems > 0 ? 'inline' : 'none';
    }
}

function displayCartItems() {
    const cartItems = document.getElementById('cartItems');

    if (!cartItems) return;

    if (cart.length === 0) {
        cartItems.innerHTML = '<p class="text-center text-muted">Sepetiniz boş</p>';
        return;
    }

    const cartHTML = cart.map(item => `
        <div class="cart-item">
            <img src="${item.image}" alt="${item.name}" class="cart-item-image">
            <div class="cart-item-info">
                <div class="cart-item-title">${item.name}</div>
                <div class="cart-item-price">₺${item.price} x ${item.quantity}</div>
            </div>
            <button class="cart-item-remove" onclick="removeFromCart('${item.id}')">
                <i class="fas fa-times"></i>
            </button>
        </div>
    `).join('');

    const total = cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);

    cartItems.innerHTML = `
        ${cartHTML}
        <div class="cart-total mt-3 pt-3 border-top">
            <strong>Toplam: ₺${total.toFixed(2)}</strong>
        </div>
    `;
}

// ===== NEWSLETTER FUNCTIONALITY =====
function initializeNewsletterForm() {
    const newsletterForm = document.getElementById('newsletterForm');

    if (newsletterForm) {
        newsletterForm.addEventListener('submit', function (e) {
            e.preventDefault();
            const email = this.querySelector('input[type="email"]').value;

            // Simulate API call
            showLoadingButton(this.querySelector('button'), 'Abone olunuyor...');

            setTimeout(() => {
                showNotification('Başarıyla abone oldunuz!', 'success');
                this.reset();
                resetButton(this.querySelector('button'), 'Abone Ol');
            }, 2000);
        });
    }
}

// ===== PRODUCT PREVIEW LOADER =====
function loadProductPreview() {
    const productPreview = document.getElementById('productPreview');
    if (!productPreview) return;

    const products = [
        {
            id: '1',
            name: 'Akıllı Telefon',
            price: 2999,
            image: 'https://images.pexels.com/photos/788946/pexels-photo-788946.jpeg',
            category: 'elektronik'
        },
        {
            id: '2',
            name: 'Laptop',
            price: 8999,
            image: 'https://images.pexels.com/photos/205421/pexels-photo-205421.jpeg',
            category: 'elektronik'
        },
        {
            id: '3',
            name: 'Erkek T-Shirt',
            price: 199,
            image: 'https://images.pexels.com/photos/1040945/pexels-photo-1040945.jpeg',
            category: 'giyim'
        }
    ];

    const productsHTML = products.map((product, index) => `
        <div class="col-lg-4 col-md-6" data-aos="fade-up" data-aos-delay="${index * 100}">
            <div class="product-card">
                <div class="product-image">
                    <img src="${product.image}" alt="${product.name}">
                </div>
                <div class="product-info">
                    <h5 class="product-title">${product.name}</h5>
                    <div class="product-price">₺${product.price}</div>
                    <button class="btn btn-primary w-100 hover-effect" 
                            onclick="addToCart('${product.id}', '${product.name}', ${product.price}, '${product.image}')">
                        <i class="fas fa-cart-plus me-2"></i>Sepete Ekle
                    </button>
                </div>
            </div>
        </div>
    `).join('');

    productPreview.innerHTML = productsHTML;
}

// ===== UTILITY FUNCTIONS =====
function showNotification(message, type = 'info') {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} position-fixed`;
    notification.style.cssText = `
        top: 100px;
        right: 20px;
        z-index: 9999;
        min-width: 300px;
        animation: slideInRight 0.3s ease-out;
    `;
    notification.innerHTML = `
        <i class="fas fa-${getNotificationIcon(type)} me-2"></i>
        ${message}
        <button type="button" class="btn-close float-end" onclick="this.parentElement.remove()"></button>
    `;

    document.body.appendChild(notification);

    // Auto remove after 3 seconds
    setTimeout(() => {
        if (notification.parentElement) {
            notification.style.animation = 'slideOutRight 0.3s ease-out';
            setTimeout(() => notification.remove(), 300);
        }
    }, 3000);
}

function getNotificationIcon(type) {
    const icons = {
        success: 'check-circle',
        warning: 'exclamation-triangle',
        error: 'times-circle',
        info: 'info-circle'
    };
    return icons[type] || 'info-circle';
}

function showLoadingButton(button, loadingText) {
    const originalText = button.textContent;
    button.innerHTML = `<span class="loading me-2"></span>${loadingText}`;
    button.disabled = true;
    button.dataset.originalText = originalText;
}

function resetButton(button, text) {
    button.innerHTML = text || button.dataset.originalText || 'Gönder';
    button.disabled = false;
}

// ===== ANIMATIONS CSS =====
const animationCSS = `
@keyframes slideInRight {
    from {
        transform: translateX(100%);
        opacity: 0;
    }
    to {
        transform: translateX(0);
        opacity: 1;
    }
}

@keyframes slideOutRight {
    from {
        transform: translateX(0);
        opacity: 1;
    }
    to {
        transform: translateX(100%);
        opacity: 0;
    }
}
`;

// Inject animation CSS
const style = document.createElement('style');
style.textContent = animationCSS;
document.head.appendChild(style);