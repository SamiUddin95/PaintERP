// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification

// for details on configuring this project to bundle and minify static web assets.



// Sidebar Navigation Toggle
document.addEventListener('DOMContentLoaded', function() {
    const navToggles = document.querySelectorAll('[data-nav-toggle]');

    navToggles.forEach(toggle => {
        toggle.addEventListener('click', function() {
            const section = this.closest('[data-nav-section]');
            section.classList.toggle('open');
        });
    });

    // Set active nav link based on current URL
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.nav-link');

    navLinks.forEach(link => {
        const href = link.getAttribute('href');
        if (href && currentPath.startsWith(href) && href !== '/') {
            navLinks.forEach(l => l.classList.remove('active'));
            link.classList.add('active');

            // Open parent section
            const section = link.closest('[data-nav-section]');
            if (section) {
                section.classList.add('open');
            }
        }
    });
});
