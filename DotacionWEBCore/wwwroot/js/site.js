// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

window.onload = function () {
    var $recaptcha = document.querySelector('#g-recaptcha-response');
    if ($recaptcha) {
        $recaptcha.setAttribute("required", "required");
    }
}