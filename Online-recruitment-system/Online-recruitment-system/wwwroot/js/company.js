$(document).ready(function () {
  
  $(window).scroll(function () {
    var scrollTop = 45;
    if (window.scrollY >= scrollTop) {
      console.log("ok");
      $(".main-nav").addClass("scrolled");
    }
    if (window.scrollY < scrollTop) {
      $(".main-nav").removeClass("scrolled");
    }
  });

  $(".imgs").owlCarousel({
    items: 3,
    loop: true,
    autoplay: true,
    dots: false,
    margin: 20,
  });
});
