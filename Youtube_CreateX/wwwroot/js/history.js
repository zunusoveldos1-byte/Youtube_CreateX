document.addEventListener('DOMContentLoaded', function () {
    const historyItems = document.querySelectorAll('.history__item');
    const historyImage = document.getElementById('historyImage');
    const historyText = document.getElementById('historyText');
    const prevBtn = document.getElementById('historyPrev');
    const nextBtn = document.getElementById('historyNext');
    let currentIndex = 0;

    function updateHistory(index) {
        const item = historyItems[index];
        if (!item) return;

        // Плавное изменение (fade effect)
        historyImage.style.opacity = '0';
        historyText.style.opacity = '0';

        setTimeout(() => {
            historyImage.src = item.dataset.img;
            historyText.textContent = item.dataset.desc;
            historyImage.style.opacity = '1';
            historyText.style.opacity = '1';
        }, 200);

        // Обновить активный класс
        historyItems.forEach(i => i.classList.remove('active'));
        item.classList.add('active');
        currentIndex = index;
    }

    // Клик по пункту меню
    historyItems.forEach((item, index) => {
        item.addEventListener('click', (e) => {
            e.preventDefault();
            updateHistory(index);
        });
    });

    // Кнопки влево/вправо
    if (prevBtn) {
        prevBtn.addEventListener('click', () => {
            let newIndex = currentIndex - 1;
            if (newIndex < 0) newIndex = historyItems.length - 1;
            updateHistory(newIndex);
        });
    }

    if (nextBtn) {
        nextBtn.addEventListener('click', () => {
            let newIndex = currentIndex + 1;
            if (newIndex >= historyItems.length) newIndex = 0;
            updateHistory(newIndex);
        });
    }
});