﻿window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

function SetLanguage(culture) {
    console.log(window.invokeCSharpAction != null);
    if (window.invokeCSharpAction != null) {
        window.invokeCSharpAction(culture);
    }
}

function openMenu() {
    const menuEl = document.getElementById("menu-list");
    menuEl.classList.toggle('active');
}

//$(document).ready(function () {
//    $('.slider').slick({
//        infinite: true,
//        slidesToShow: 1,
//        slidesToScroll: 1
//    });
//});


//function Slice() {

//    let items = document.querySelectorAll('.carousel .carousel-item')

//    items.forEach((el) => {
//        // number of slides per carousel-item
//        const minPerSlide = 9
//        let next = el.nextElementSibling
//        for (var i = 1; i < minPerSlide; i++) {
//            if (!next) {
//                // wrap carousel by using first child
//                next = items[0]
//            }
//            let cloneChild = next.cloneNode(true)
//            el.appendChild(cloneChild.children[0])
//            next = next.nextElementSibling
//        }
//    })

//    //$('.slider').slick({
//    //    slidesToShow: 8,
//    //    slidesToScroll: 1,
//    //    infinite: true,
//    //    arrows: true,
//    //    autoplay: true,
//    //    autoplaySpeed: 5000,
//    //    prevArrow: $('.prev-arrow'),
//    //    nextArrow: $('.next-arrow'),

//    //    //responsive: [
//    //    //    {
//    //    //        breakpoint: 576,
//    //    //        settings: {
//    //    //            slidesToShow: 1,
//    //    //            centerMode: true,
//    //    //        }
//    //    //    }
//    //    //]
//    //});
//}



function Scroll() {

    //window.addEventListener('scroll', reveal2);
        console.log('scroll');
}

function reveal2() {
    var cont = document.getElementById('containerScroll');

    cont.onscroll = function () { reveal() };
    //window.onscroll = function () { Scroll() };
    
   
}



function reveal() {
    var reveals = document.querySelectorAll('.reveal');

    for (var i = 0; i < reveals.length; i++) {
        var windowHeight = window.innerHeight;
        var revealTop = reveals[i].getBoundingClientRect().top;
        var revealPoint = 150;

        if (revealTop < windowHeight - revealPoint) {
            reveals[i].classList.add('active');
        }
        else {
            reveals[i].classList.remove('active');
        }
    }
}


function Slice2() {
    $(document).ready(function () {
        var carousel = $('.owl-carousel');
        var $owl = carousel.owlCarousel({
            loop: true,
            margin: 10,
            responsiveClass: true,
            nav: true,
            responsive: {
                0: {
                    items: 1,
                    nav: true
                },
                600: {
                    items: 4,
                    nav: true
                },
                1000: {
                    items: 6,
                    nav: true
                },
                // 1240: {
                //     items: 4,
                //     nav: true,
                //     loop: true
                // },
                1400: {
                    items: 9,
                    nav: true
                }
            },
            dotsEach: true,
            autoplay: true,
            checkVisible: true,
            autoplayTimeout: 5000,
            autoplayHoverPause: true
        });

    });
}



//dragElement(document.getElementById("square"));



function dragElement(dotNetObjRef) {
    var waterMark = document.getElementById("mydivheader");
    const square = document.getElementById("square");

    waterMark.addEventListener('touchmove', function (e) {
        // grab the location of touch
        var touchLocation = e.targetTouches[0];

        console.log(touchLocation);

        // assign box new coordinates based on the touch.
        waterMark.style.left = touchLocation.pageX + 'px';
        waterMark.style.top = touchLocation.pageY + 'px';


    })
    //waterMark.addEventListener('touchend', function (e) {
    //    // current box position.
    //    var x = parseInt(waterMark.style.left);
    //    var y = parseInt(waterMark.style.top);
    //})

    var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
    if (document.getElementById(waterMark.id + "header")) {
        /* if present, the header is where you move the DIV from:*/
        document.getElementById(waterMark.id + "header").onmousedown = dragMouseDown;
    } else {
        /* otherwise, move the DIV from anywhere inside the DIV:*/
        waterMark.onmousedown = dragMouseDown;
    }

    function dragMouseDown(e) {
        e = e || window.event;
        e.preventDefault();
        // get the mouse cursor position at startup:
        pos3 = e.clientX;
        pos4 = e.clientY;
        document.onmouseup = closeDragElement;
        // call a function whenever the cursor moves:
        document.onmousemove = elementDrag;
    }

    function elementDrag(e) {
        //var asd = elmnt.offsetLeft;
        console.log(waterMark.offsetLeft);
        let elementLeft = waterMark.offsetLeft + waterMark.offsetWidth;
        e = e || window.event;
        e.preventDefault();
        pos1 = pos3 - e.clientX;
        pos2 = pos4 - e.clientY;
        pos3 = e.clientX;
        pos4 = e.clientY;
        if (square.offsetWidth < elementLeft) {
            //waterMark.style.top = (waterMark.offsetTop - pos2) + "px";
            waterMark.style.left = (square.offsetWidth - waterMark.offsetWidth) + "px";
        }
        else if (waterMark.offsetLeft <= -1) {

            //waterMark.style.top = (waterMark.offsetTop - pos2) + "px";
            waterMark.style.left = 0 + "px";
        }
        else {
            waterMark.style.left = (waterMark.offsetLeft - pos1) + "px";

        }
        if (waterMark.offsetTop <= -1) {
            waterMark.style.top = 0 + "px";
        }
        else if (waterMark.offsetTop >= square.offsetHeight - waterMark.offsetHeight + 1) {
            waterMark.style.top = square.offsetHeight - waterMark.offsetHeight + "px";
        }
        else {

            // set the element's new position:
            waterMark.style.top = (waterMark.offsetTop - pos2) + "px";
        }

    }

    function closeDragElement() {
        /* stop moving when mouse button is released:*/
        document.onmouseup = null;
        document.onmousemove = null;

        let left = waterMark.offsetLeft + waterMark.offsetWidth / 2;
        let top = waterMark.offsetTop + waterMark.offsetHeight / 2;


        const squareRect = document.getElementById("square").getBoundingClientRect();
        let squareWidth = squareRect.width;
        let squareHeight = squareRect.height;

        let percentLeft = left / squareWidth;
        let percentTop = top / squareHeight;

        dotNetObjRef.invokeMethodAsync("GetPercentFromJS", percentLeft, percentTop, waterMark.offsetWidth, waterMark.offsetHeight);
    }
}

function applyStyle(style) {

    //document.getElementById("mydivheader").style.fontSize = "20px";
    document.getElementById(`${style.element}`).style.fontSize = `${style.value}px`;
    //window.applyStyleForBody = function (style) {

    //document.style.element.style[style.attrib] = style.value;
}

function applyStyle2(style) {

    document.getElementById(`${style.element}`).style.width = `${style.value}%`;
}