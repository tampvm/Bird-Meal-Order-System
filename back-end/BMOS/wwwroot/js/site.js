// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




//This is a method for check validate Phone in Register.cshtml
function validatePhone() {
    var phone = document.getElementById("phone");
    var pattern = /^(03|05|07|08|09)+\d{1}[-\s]?\d{3}[-\s]?\d{4}$/;
    if (pattern.test(phone.value)) {
        phone.setCustomValidity('');       
    }
    else {
        phone.setCustomValidity("Số điện thoại đã nhập không tồn tại.");
    }
}

phone.onkeyup = validatePhone;

//This is a method for check Validate Password in Register.cshtml and ChangePassword.cshtml
var password = document.getElementById("password")
    , confirm_password = document.getElementById("confirm_password");

function validatePassword() {
    if (password.value != confirm_password.value) {
        confirm_password.setCustomValidity("Mật khẩu đã nhập không trùng khớp.");
    } else {
        confirm_password.setCustomValidity('');
    }
}

password.onchange = validatePassword;
confirm_password.onkeyup = validatePassword;

