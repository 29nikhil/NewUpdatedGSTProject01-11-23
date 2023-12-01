window.onload = function () {
    const slider = document.querySelector('.image-slider');
    const sliderItems = document.querySelectorAll('.slider-item');
    const totalWidth = sliderItems.length * slider.clientWidth;
    slider.style.width = totalWidth + 'px';

    const slideImages = () => {
        slider.style.marginLeft = '-' + slider.clientWidth + 'px';
        setTimeout(() => {
            const firstItem = sliderItems[0];
            slider.appendChild(firstItem);
            slider.style.marginLeft = '0';
        }, 1000);
    };

    setInterval(slideImages, 3000);
};
