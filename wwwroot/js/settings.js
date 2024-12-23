$(document).ready(function () {
    const avatarFormControlInput = document.getElementById('avatar-form-control');
    const updateAvatarSubmitButton = document.getElementById('update-avatar-submit');

    avatarFormControlInput.onchange = () => updateAvatarSubmitButton.hidden = false;
})

document.addEventListener('DOMContentLoaded', () => {
    const backgroundButtons = document.querySelectorAll('.change-background');

    backgroundButtons.forEach(button => {
        button.addEventListener('click', async () => {
            const bgImage = button.getAttribute('data-bg');

            try {
                const response = await fetch('/user/background', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ backgroundPath: bgImage }),
                });

                if (response.ok) {
                    alert('Фон успешно изменён!');
                    document.body.style.backgroundImage = `url('${bgImage}')`;
                } else {
                    alert('Ошибка при изменении фона.');
                }
            } catch (error) {
                console.error('Ошибка:', error);
                alert('Ошибка при изменении фона.');
            }
        });
    });
});