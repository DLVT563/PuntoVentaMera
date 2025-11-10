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

})();

const notificationContainer = document.getElementById('notification-container');

/**
 * Muestra una notificación nativa flotante.
 * @param {string} message - El texto del mensaje.
 * @param {string} type - 'success' o 'error'.
 * @param {number} duration - Tiempo en milisegundos para que desaparezca (por defecto 3000ms).
 * @param {boolean} hasAction - Si la notificación debe tener un botón de acción.
 */
function showNotification(message, type = 'success', duration = 3000, hasAction = false) {
    if (!notificationContainer) {
        console.error("Contenedor de notificaciones no encontrado.");
        return;
    }

    const notification = document.createElement('div');
    notification.className = `notification ${type}`;
    notification.innerHTML = `
        ${type === 'success' ? '✓' : '✗'} 
        <span class="message">${message}</span>
        ${hasAction ? '<button class="action-button">Cerrar</button>' : ''}
    `;

    notificationContainer.prepend(notification);

    setTimeout(() => {
        notification.classList.add('show');
    }, 10);

    const hideNotification = () => {
        notification.classList.remove('show');

        setTimeout(() => {
            if (notificationContainer.contains(notification)) {
                notificationContainer.removeChild(notification);
            }
        }, 400);
    };

    setTimeout(hideNotification, duration);

    if (hasAction) {
        const actionButton = notification.querySelector('.action-button');
        if (actionButton) {
            actionButton.addEventListener('click', hideNotification);
        }
    }
}

window.showNotification = showNotification;

document.addEventListener('DOMContentLoaded', () => {
    if (!notificationContainer) {
        return;
    }

    const pendingNotifications = document.querySelectorAll('[data-notification-message]');

    pendingNotifications.forEach((element) => {
        const message = element.getAttribute('data-notification-message');

        if (!message) {
            return;
        }

        const type = element.getAttribute('data-notification-type') || 'success';
        const durationAttribute = element.getAttribute('data-notification-duration');
        const duration = durationAttribute ? parseInt(durationAttribute, 10) : 3000;
        const hasAction = element.hasAttribute('data-notification-action');

        showNotification(message, type, duration, hasAction);

        if (element.parentElement) {
            element.parentElement.removeChild(element);
        } else if (element.remove) {
            element.remove();
        }
    });
});
