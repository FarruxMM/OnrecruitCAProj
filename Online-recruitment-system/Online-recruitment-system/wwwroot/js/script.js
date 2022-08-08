$(document).ready(function () {
  const counters = document.querySelectorAll(".count-num");
    const speed = 200;

    // Immidietly Invoked Function
    (function handlePreloader() {
        if ($('.preloader').length) {
            $('.preloader').delay(300).fadeOut(600);
        }
    })();


  let togglerButton = document.querySelector("#toggler-icon");

    $(window).scroll(function () {
    let scrollTop = 45;
        if (window.scrollY >= scrollTop) {
            $(".main-nav").addClass("scrolled");
        } else {
            $(".main-nav").removeClass("scrolled");
        }
    });

    if (window.scrollY >= 45 && !($(".main-nav").hasClass("scrolled"))) {
        $(".main-nav").addClass("scrolled");
    };

    AOS.init();


  togglerButton.addEventListener("click", function () {
    if (this.nextElementSibling.classList.contains("customToggler")) {
      this.nextElementSibling.classList.remove("customToggler");
      this.nextElementSibling.style = "transition: .5s all;";
    } else {
      this.nextElementSibling.classList.add("customToggler");
      this.nextElementSibling.style.removeProperty("transition");
    }
  });

  counters.forEach((counter) => {
    const updateCount = () => {
      const target = +counter.getAttribute("data-count");
      const count = +counter.innerText;

      const inc = target / speed;

      if (count < target) {
        counter.innerText = count + inc;
        setTimeout(updateCount, 1);
      } else {
        count.innerText = target;
      }
    };

    updateCount();
  });

  $(".partner-slider").owlCarousel({
    items: 4,
    loop: true,
    autoplay: true,
    autoplayTimeout: 2000,
    autoplayHoverPause: true,
    dots: false,
    margin: 15,
  });

  $(".location-slider ").owlCarousel({
    items: 4,
    loop: true,
    autoplay: true,
    autoplayTimeout: 2000,
    autoplayHoverPause: true,
    dots: false,
    margin: 20,
  });

  $(".candidate").owlCarousel({
    items: 2,
    loop: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    dots: false,
    margin: 20,
  });

    $(".imgs").owlCarousel({
        items: 2,
        loop: true,
        autoplay: true,
        autoplayTimeout: 2000,
        autoplayHoverPause: true,
        dots: false,
        margin: 20,
    });

  $(".client-slider").owlCarousel({
    items: 1,
    loop: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    dots: false,
    margin: 20,
  });
});
