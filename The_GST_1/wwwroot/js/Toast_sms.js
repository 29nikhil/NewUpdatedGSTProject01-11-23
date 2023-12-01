

function ToastPopup(message) {
    var x = document.getElementById("snackbar");
    x.innerText = message;
    x.className = "show";
    setTimeout(function () {
        x.className = x.className.replace("show", "");
    }, 3000);
}







