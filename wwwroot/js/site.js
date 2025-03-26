// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Function to fetch product details
function fetchProductDetails(productId) {
    if (!productId) return;

    fetch(`/api/Products/${productId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Product not found');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById('Rate').value = data.price;
            calculateTotal();
        })
        .catch(error => {
            console.error('Error fetching product details:', error);
        });
}

// Function to calculate total
function calculateTotal() {
    const quantity = parseFloat(document.getElementById('Quantity').value) || 0;
    const rate = parseFloat(document.getElementById('Rate').value) || 0;
    const total = quantity * rate;

    document.getElementById('Total').value = total.toFixed(2);
}

// Set up event listeners when the document is ready
document.addEventListener('DOMContentLoaded', function() {
    const productSelect = document.getElementById('ProductId');
    const quantityInput = document.getElementById('Quantity');
    const rateInput = document.getElementById('Rate');

    if (productSelect) {
        productSelect.addEventListener('change', function() {
            fetchProductDetails(this.value);
        });

        // Initialize with selected product
        if (productSelect.value) {
            fetchProductDetails(productSelect.value);
        }
    }

    if (quantityInput) {
        quantityInput.addEventListener('input', calculateTotal);
    }

    if (rateInput) {
        rateInput.addEventListener('input', calculateTotal);
    }
});