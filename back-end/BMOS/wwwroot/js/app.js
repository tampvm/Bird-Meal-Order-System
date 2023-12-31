$(document).ready(function () {

    $('.product-warpper').slick({
        slidesToShow: 4,
        slidesToScroll: 2,
        infinite: false,
        arrows: true,
        prevArrow: "<button type='button' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>"
    });


    $('.banner-warpper').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        infinite: false,
        arrows: false,
        autoplay: true,
        autoplaySpeed: 1000,
        infinite: true,
    });

    $('.blog-warpper').slick({
        slidesToShow: 4,
        slidesToScroll: 1,
        infinite: false,
        arrows: false,
        autoplay: true,
        autoplaySpeed: 1000,
        infinite: true,
        arrows: false,
        dots: true,
        prevArrow: "<button type='button' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>"
    });

    $('.meal-wrapper').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        autoplay: true,
        autoplaySpeed: 2000,
        infinite: false,
        arrows: true,
        prevArrow: "<button type='button' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>"
    });

});

var loader = document.getElementById("preloader");
var inputNumber = document.getElementById("number");
window.addEventListener("load", function () {
    loader.classList.add("disapear");
});

var limitProudct = parseInt($('input[name="limitQuantity"]').val());


inputNumber.addEventListener("input", function () {
    var inputValue = parseInt(inputNumber.value);
    if (inputValue <= 0) {
        inputNumber.value = 1;
    } else if (inputValue > limitProudct) {

        inputNumber.value = limitProudct;
    }
});





function increaseValue() {
    var value = parseInt(document.getElementById('number').value, 10);
    value = isNaN(value) ? 1 : value;
    value++;
    if (value >= limitProudct) {
        document.getElementById('number').value = limitProudct;
    } else {
        document.getElementById('number').value = value;
    }
}

function decreaseValue() {
    var value = parseInt(document.getElementById('number').value, 10);
    value = isNaN(value) ? 1 : value;
    value < 1 ? value = 1 : '';
    value--;
    if (value <= 1) {
        document.getElementById('number').value = 1;
    } else {
        document.getElementById('number').value = value;
    }
}


const imgs = document.querySelectorAll('.img-select a');
const imgBtns = [...imgs];
let imgId = 1;

imgBtns.forEach((imgItem) => {
    imgItem.addEventListener('click', (event) => {
        event.preventDefault();
        imgId = imgItem.dataset.id;
        slideImage();
    });
});

function slideImage() {
    const displayWidth = document.querySelector('.img-showcase img:first-child').clientWidth;

    document.querySelector('.img-showcase').style.transform = `translateX(${- (imgId - 1) * displayWidth}px)`;
}

window.addEventListener('resize', slideImage);

