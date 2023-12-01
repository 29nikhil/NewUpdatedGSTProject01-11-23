
setTimeout(function () {
    var loader=document.querySelector('.pl').style.display = 'none';

    // Show other components one after the other
    var components = document.querySelectorAll('.other-component');
    components.forEach(function (component, index) {
        setTimeout(function () {
            component.style.display = 'block';
            setTimeout(function () {
                loader.style.display = 'none';
            }, 1000); // Adjust the time the component stays visible before hiding (in milliseconds)
        }, index * 1000); // Adjust the delay (in milliseconds) between showing each component
    });
}, 4000);