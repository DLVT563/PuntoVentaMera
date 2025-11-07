(function () {
    const navToggle = document.querySelector('[data-nav-toggle]');
    const navMenu = document.querySelector('[data-nav-menu]');

    if (navToggle && navMenu) {
        navToggle.addEventListener('click', () => {
            const isExpanded = navToggle.getAttribute('aria-expanded') === 'true';
            const newState = !isExpanded;
            navToggle.setAttribute('aria-expanded', String(newState));
            navMenu.classList.toggle('is-open', newState);
        });

        document.addEventListener('click', (event) => {
            if (!navMenu.classList.contains('is-open')) {
                return;
            }

            const target = event.target;
            if (target instanceof Element && !navMenu.contains(target) && !navToggle.contains(target)) {
                navMenu.classList.remove('is-open');
                navToggle.setAttribute('aria-expanded', 'false');
            }
        });
    }

    const notifications = document.querySelectorAll('[data-notification]');

    notifications.forEach((notification) => {
        const closeButton = notification.querySelector('[data-notification-close]');

        const hideNotification = () => {
            if (notification.classList.contains('is-hidden')) {
                return;
            }

            notification.classList.add('is-hidden');
            notification.addEventListener('transitionend', () => {
                notification.remove();
            }, { once: true });
        };

        if (closeButton) {
            closeButton.addEventListener('click', hideNotification);
        }

        const autoDismissMs = 6000;
        window.setTimeout(hideNotification, autoDismissMs);
    });
})();
