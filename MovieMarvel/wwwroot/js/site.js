
let isOpen = false;

function ToggleCart() {

    if (!isOpen) { // open cart
        $('#cartPanel').animate({
            top: 0
        }, 220, 'swing');
        $('#cartIcon').attr('class', 'fas fa-shopping-cart');
        isOpen = true;
    }
    else { // close cart
        $('#cartPanel').animate({
            top: -271
        }, 220, 'swing');
        $('#cartIcon').attr('class', 'fas fa-cart-arrow-down');
        isOpen = false;
    }
} // ToggleCart()



let images = new Array();
let imageTimer;
let count = -1;
function loadImage(rawImages) {
    images = rawImages.split(';');
    imageTimer = setInterval('NextImage()', 5000);
}

function NextImage() {
    count++;
    if (count === images.length - 1)
        count = 0;
    $('#bioPicture').attr('style',
        'background: url(https://image.tmdb.org/t/p/w500/' +
        images[count] + '); background-size: cover; background-position: center; width: 320px; height: 320px;');
}