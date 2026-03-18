// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Мобильное меню
document.querySelector('.mobile-menu-btn').addEventListener('click', function () {
    document.querySelector('.nav').classList.toggle('active');
});

// Закрытие меню при клике на ссылку
document.querySelectorAll('.nav a').forEach(link => {
    link.addEventListener('click', function () {
        document.querySelector('.nav').classList.remove('active');
    });
});

// Инициализация Fancybox
if (typeof Fancybox !== 'undefined') {
    Fancybox.bind("[data-fancybox]", {
        // Настройки
    });
}

setTimeout(function () {
    var alertElement = document.querySelector('.alert');
    if (alertElement) {
        var bsAlert = new bootstrap.Alert(alertElement);
        bsAlert.close();
    }
}, 4000);

// Управление отзывами через localStorage
(function () {
    function loadTestimonials() {
        let testimonials = JSON.parse(localStorage.getItem('testimonials')) || [];
        const list = document.getElementById('testimonials-list');
        const dots = document.getElementById('testimonial-dots');
        if (!list) return;

        // Если отзывов нет, создаём демо-отзыв
        if (testimonials.length === 0) {
            testimonials = [{
                name: 'Shawn Edwards',
                position: 'Должность',
                company: 'Название компании',
                text: 'Ipsum aute sunt aliquip aute et occaecat. Anim minim do cillum eiusmod enim. Consectetur magna cillum consequat minim laboris cillum laboris voluptate minim proident exercitation ullamco.',
                image: '/img/site/profile-image.png'
            }];
            localStorage.setItem('testimonials', JSON.stringify(testimonials));
        }

        let currentIndex = 0;

        function renderTestimonial(index) {
            const t = testimonials[index];
            list.innerHTML = `
                <article class="testimonial-item">
                    <div class="quote-icon">
                        <svg width="28" height="20" viewBox="0 0 28 20" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M11.3333 0L0 10.6667V20H10.6667V10.6667H4.66667L11.3333 0Z" fill="#FF5A30" />
                            <path d="M28 0L16.6667 10.6667V20H27.3333V10.6667H21.3333L28 0Z" fill="#FF5A30" />
                        </svg>
                    </div>
                    <blockquote class="testimonial-text">${t.text}</blockquote>
                    <div class="testimonial-divider"></div>
                    <div class="author-info">
                        <img src="${t.image || '/img/site/profile-image.png'}" alt="${t.name}" class="author-img">
                        <div class="author-details">
                            <h4 class="author-name">${t.name}</h4>
                            <p class="author-position">${t.position}${t.company ? ', ' + t.company : ''}</p>
                        </div>
                    </div>
                </article>
            `;
            // Обновить активную точку
            const dotItems = dots.querySelectorAll('.dot');
            dotItems.forEach((dot, i) => {
                dot.classList.toggle('active', i === index);
            });
        }

        // Создать точки навигации
        dots.innerHTML = '';
        for (let i = 0; i < testimonials.length; i++) {
            const dot = document.createElement('button');
            dot.className = 'dot' + (i === currentIndex ? ' active' : '');
            dot.setAttribute('aria-label', `Перейти к отзыву ${i + 1}`);
            dot.addEventListener('click', () => {
                currentIndex = i;
                renderTestimonial(currentIndex);
            });
            dots.appendChild(dot);
        }

        renderTestimonial(currentIndex);
    }

    // Инициализация формы добавления отзыва
    function initTestimonialForm() {
        const addBtn = document.getElementById('add-testimonial-btn');
        const formDiv = document.getElementById('testimonial-form');
        const cancelBtn = document.getElementById('cancel-testimonial');
        const form = document.getElementById('testimonial-form-inner');

        if (!addBtn || !formDiv || !cancelBtn || !form) return;

        addBtn.addEventListener('click', () => {
            formDiv.style.display = 'block';
            addBtn.style.display = 'none';
        });

        cancelBtn.addEventListener('click', () => {
            formDiv.style.display = 'none';
            addBtn.style.display = 'inline-block';
        });

        form.addEventListener('submit', (e) => {
            e.preventDefault();

            const name = document.getElementById('author-name').value.trim();
            const position = document.getElementById('author-position').value.trim();
            const company = document.getElementById('author-company').value.trim();
            const text = document.getElementById('testimonial-text').value.trim();

            if (!name || !text) {
                alert('Пожалуйста, введите ваше имя и комментарий.');
                return;
            }

            const newTestimonial = {
                name: name,
                position: position,
                company: company,
                text: text,
                image: '' // можно добавить загрузку аватара
            };

            const testimonials = JSON.parse(localStorage.getItem('testimonials')) || [];
            testimonials.push(newTestimonial);
            localStorage.setItem('testimonials', JSON.stringify(testimonials));

            // Сброс и скрытие формы
            form.reset();
            formDiv.style.display = 'none';
            addBtn.style.display = 'inline-block';

            // Перезагрузить отзывы
            loadTestimonials();
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        loadTestimonials();
        initTestimonialForm();
    });
})();