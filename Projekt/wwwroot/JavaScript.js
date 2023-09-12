window.downloadFileFromStream = async (fileName, contentStreamReference) => {
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

//$(document).ready(function () {
//    $('.slider').slick({
//        infinite: true,
//        slidesToShow: 1,
//        slidesToScroll: 1
//    });
//});

function Slice() {
    $('.slider').slick({
        slidesToShow: 8,
        slidesToScroll: 1,
        infinite: true,
        arrows: true,
        //autoplay: true,
        //autoplaySpeed: 5000,
        prevArrow: $('.prev-arrow'),
        nextArrow: $('.next-arrow'),

        //responsive: [
        //    {
        //        breakpoint: 576,
        //        settings: {
        //            slidesToShow: 1,
        //            centerMode: true,
        //        }
        //    }
        //]
    });
}
//$(document).ready(function () {
//    var x = $('.slider');
//    $('.slider').slick({
//        slidesToShow: 5,
//        slidesToScroll: 1,
//        infinite: true,
//        arrows: true,
//        autoplay: true,
//        autoplaySpeed: 5000,
//        prevArrow: $('.prev-arrow'),
//        nextArrow: $('.next-arrow'),

//        responsive: [
//            {
//                breakpoint: 576,
//                settings: {
//                    slidesToShow: 1,
//                    centerMode: true,
//                }
//            }
//        ]
//    });
//});



//dragElement(document.getElementById("square"));

function dragElement(dotNetObjRef) {
    var waterMark = document.getElementById("mydivheader");
    const square = document.getElementById("square");

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
        else if (waterMark.offsetTop >= square.offsetHeight - waterMark.offsetHeight  + 1) {
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

        let left = waterMark.offsetLeft;
        let top = waterMark.offsetTop;


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