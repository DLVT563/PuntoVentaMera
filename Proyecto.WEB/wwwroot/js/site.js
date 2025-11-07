(function () {
    function onReady(callback) {
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', callback, { once: true });
        } else {
            callback();
        }
    }

    function setupNavigation() {
        var toggler = document.querySelector('.js-navbar-toggle');
        var menu = document.querySelector('.js-navbar-menu');

        if (!toggler || !menu) {
            return;
        }

        toggler.addEventListener('click', function () {
            var isOpen = menu.classList.toggle('is-open');
            toggler.setAttribute('aria-expanded', isOpen ? 'true' : 'false');
        });

        document.addEventListener('click', function (event) {
            if (!menu.classList.contains('is-open')) {
                return;
            }

            var clickInsideMenu = menu.contains(event.target);
            var clickOnToggler = toggler === event.target || toggler.contains(event.target);

            if (!clickInsideMenu && !clickOnToggler) {
                menu.classList.remove('is-open');
                toggler.setAttribute('aria-expanded', 'false');
            }
        });
    }

    function setupAlerts() {
        var alerts = document.querySelectorAll('.js-alert');

        alerts.forEach(function (alert) {
            if (alert.dataset.dismissed === 'true') {
                return;
            }

            var closeButton = alert.querySelector('.js-alert-close');
            if (closeButton) {
                closeButton.addEventListener('click', function () {
                    dismissAlert(alert);
                });
            }

            setTimeout(function () {
                dismissAlert(alert);
            }, 8000);
        });
    }

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
    }

    onReady(function () {
        setupNavigation();
        setupAlerts();
    });
})();
