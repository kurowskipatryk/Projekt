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
