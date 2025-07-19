document.addEventListener("DOMContentLoaded", function() {
    initTypingEffect();
    handleStickyHeader();
    initDarkModeToggle();
    initMenuToggle();
});

function initTypingEffect() {
    var typed = new Typed(".typing-effect", {
        strings: ["HBMARKET"],
        typeSpeed: 100,
        backSpeed: 100,
        backDelay: 2000,
        startDelay: 500,
        showCursor: false,
        loop: true
    });
}

function handleStickyHeader() {
    const header = document.querySelector("header");

    // Check initial scroll position on page load
    if (window.scrollY === 0) {
        header.classList.remove("sticky");
    } else {
        header.classList.add("sticky");
    }

    window.addEventListener("scroll", function() {
        header.classList.toggle("sticky", window.scrollY > 0);
    });
}

function initDarkModeToggle() {
    const modeToggleBtn = document.getElementById("mode-toggle");
    const body = document.body;

    // Check for saved user preference in local storage
    const savedMode = localStorage.getItem("mode");
    if (savedMode) {
        body.classList.add(savedMode);
        if (savedMode === "light-mode") {
            modeToggleBtn.checked = true; // Set the toggle switch to checked if light mode is active
        }
    }

    modeToggleBtn.addEventListener("click", function() {
        body.classList.toggle("light-mode");
        if (body.classList.contains("light-mode")) {
            localStorage.setItem("mode", "light-mode");
        } else {
            localStorage.removeItem("mode");
        }
    });
}

function initMenuToggle() {
    let menu = document.querySelector('#menu-icon');
    let navmenu = document.querySelector('.navmenu');
    menu.onclick = () => {
        menu.classList.toggle('bx-x');
        navmenu.classList.toggle('open');
    }
}



