// ==================== ОБЩИЕ ФУНКЦИИ ДЛЯ АВТОРИЗАЦИИ ====================

function showToast(message, type = 'success') {
    const toast = document.getElementById('toastMessage');
    if (!toast) return;
    toast.textContent = message;
    toast.className = 'toast-notification show ' + type;
    setTimeout(() => {
        toast.classList.remove('show');
    }, 3000);
}

function updateProfileIcon() {
    const profileIcon = document.getElementById('profileIcon');
    if (!profileIcon) return;
    const avatarUrl = localStorage.getItem('userAvatar');
    if (avatarUrl) {
        profileIcon.innerHTML = `<img src="${avatarUrl}" alt="Avatar" style="width: 36px; height: 36px; border-radius: 50%; object-fit: cover;">`;
    } else {
        profileIcon.innerHTML = '<i class="fa-regular fa-circle-user" style="font-size: 32px;"></i>';
    }
}

function checkAuth() {
    const isLoggedIn = window.user && window.user.isAuthenticated;
    const loginBtn = document.getElementById('loginBtn');
    const registerBtn = document.getElementById('registerBtn');
    const profileIcon = document.getElementById('profileIcon');

    if (isLoggedIn) {
        if (loginBtn) loginBtn.style.display = 'none';
        if (registerBtn) registerBtn.style.display = 'none';
        if (profileIcon) {
            profileIcon.style.display = 'flex';
            updateProfileIcon();
        }
    } else {
        if (loginBtn) loginBtn.style.display = 'inline-block';
        if (registerBtn) registerBtn.style.display = 'inline-block';
        if (profileIcon) profileIcon.style.display = 'none';
    }
}

// ==================== ЛОГИКА СТРАНИЦЫ ПРОФИЛЯ ====================

function initProfilePage() {
    if (!document.getElementById('profileForm')) return;

    const nameInput = document.getElementById('profileName');
    const emailInput = document.getElementById('profileEmail');
    const phoneInput = document.getElementById('profilePhone');
    const avatarPreview = document.getElementById('avatarPreview');
    const profileForm = document.getElementById('profileForm');
    const logoutBtn = document.getElementById('logoutBtn');
    const avatarInput = document.getElementById('profileAvatar');

    updateProfileIcon();

    if (!window.user || !window.user.isAuthenticated) {
        window.location.href = '/';
        return;
    }

    // Заполняем поля данными из localStorage (в реальности должны быть с сервера)
    nameInput.value = localStorage.getItem('userName') || window.user.userName || '';
    emailInput.value = localStorage.getItem('userEmail') || window.user.userEmail || '';
    if (phoneInput) phoneInput.value = localStorage.getItem('userPhone') || '';

    const savedAvatar = localStorage.getItem('userAvatar');
    if (savedAvatar) {
        avatarPreview.src = savedAvatar;
    }

    // Обработка отправки формы
    profileForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        // Здесь можно отправить данные на сервер через fetch
        // Для демо сохраняем в localStorage
        localStorage.setItem('userName', nameInput.value);
        localStorage.setItem('userEmail', emailInput.value);
        if (phoneInput) localStorage.setItem('userPhone', phoneInput.value);

        const submitBtn = e.target.querySelector('button[type="submit"]');
        submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Сохранение...';
        submitBtn.disabled = true;

        // Имитация запроса к серверу
        setTimeout(() => {
            submitBtn.innerHTML = '<i class="fas fa-save me-2"></i>Сохранить изменения';
            submitBtn.disabled = false;
            showToast('Данные успешно сохранены!', 'success');
        }, 1000);
    });

    // Предпросмотр аватара
    if (avatarInput) {
        avatarInput.addEventListener('change', function (e) {
            const file = e.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (ev) {
                    avatarPreview.src = ev.target.result;
                    localStorage.setItem('userAvatar', ev.target.result);
                    updateProfileIcon();
                    showToast('Аватар обновлён', 'success');
                };
                reader.readAsDataURL(file);
            }
        });
    }

    // Выход из системы
    if (logoutBtn) {
        logoutBtn.addEventListener('click', function () {
            if (confirm('Вы действительно хотите выйти?')) {
                localStorage.removeItem('userName');
                localStorage.removeItem('userEmail');
                localStorage.removeItem('userPhone');
                localStorage.removeItem('userAvatar');
                window.location.href = '/Identity/Account/Logout';
            }
        });
    }
}

// Эффект скролла для шапки
window.addEventListener('scroll', function () {
    const header = document.getElementById('header');
    if (header) {
        if (window.scrollY > 50) {
            header.classList.add('scrolled');
        } else {
            header.classList.remove('scrolled');
        }
    }
});

// Запуск при загрузке
document.addEventListener('DOMContentLoaded', function () {
    checkAuth();
    // Если текущая страница – профиль (проверяем наличие формы)
    if (document.getElementById('profileForm')) {
        initProfilePage();
    }
});

// Экспортируем функции для возможного использования в других скриптах
window.showToast = showToast;
window.updateProfileIcon = updateProfileIcon;
window.checkAuth = checkAuth;