(function () {
    function ready(fn) {
        if (document.readyState !== "loading") {
            fn();
        } else {
            document.addEventListener("DOMContentLoaded", fn, { once: true });
        }
    }

    ready(function () {
        var toggler = document.querySelector('[data-nav-toggle]');
        var menu = document.querySelector('[data-nav-menu]');

        if (toggler && menu) {
            toggler.addEventListener('click', function () {
                var isOpen = menu.classList.toggle('is-open');
                toggler.setAttribute('aria-expanded', isOpen ? 'true' : 'false');
            });

            document.addEventListener('click', function (event) {
                if (!menu.classList.contains('is-open')) {
                    return;
                }

                if (!menu.contains(event.target) && event.target !== toggler && !toggler.contains(event.target)) {
                    menu.classList.remove('is-open');
                    toggler.setAttribute('aria-expanded', 'false');
                }
            });
        }

        var alerts = document.querySelectorAll('[data-alert]');
        alerts.forEach(function (alert) {
            var closeButton = alert.querySelector('[data-alert-close]');
            if (closeButton) {
                closeButton.addEventListener('click', function () {
                    dismissAlert(alert);
                });
            }

            var dismissDelay = parseInt(alert.getAttribute('data-alert-timeout'), 10);
            if (!isNaN(dismissDelay) && dismissDelay > 0) {
                setTimeout(function () {
                    dismissAlert(alert);
                }, dismissDelay);
            } else {
                setTimeout(function () {
                    dismissAlert(alert);
                }, 8000);
            }
        });
    });

    function dismissAlert(alert) {
        if (!alert || alert.dataset.dismissed === 'true') {
            return;
        }

        alert.dataset.dismissed = 'true';
        alert.classList.add('is-dismissing');
        alert.addEventListener('transitionend', function handleTransition() {
            alert.removeEventListener('transitionend', handleTransition);
            alert.remove();
        });

        alert.style.opacity = '0';
        alert.style.transform = 'translateY(-6px)';
    }
})();
