const threshold = 10;
let seconds = 0;
let clicks = 0;
const currentScoreElement = document.getElementById("current_score");
const recordScoreElement = document.getElementById("record_score");
const profitPerClickElement = document.getElementById("profit_per_click");
const profitPerSecondElement = document.getElementById("profit_per_second");
let currentScore = Number(currentScoreElement.innerText);
let recordScore = Number(recordScoreElement.innerText);
let profitPerSecond = Number(profitPerSecondElement.innerText);
let profitPerClick = Number(profitPerClickElement.innerText);


$(document).ready(function () {
    const clickitem = document.getElementById("clickitem");

    clickitem.onclick = click;
    setInterval(addSecond, 1000)

    const boostButtons = document.getElementsByClassName("boost-button");

    for (let i = 0; i < boostButtons.length; i++) {
        const boostButton = boostButtons[i];

        boostButton.onclick = () => boostButtonClick(boostButton);
    }

    toggleBoostsAvailability();
})

function boostButtonClick(boostButton) {
    if (clicks > 0 || seconds > 0) {
        addPointsToScore();
    }
    buyBoost(boostButton);
}

function buyBoost(boostButton) {
    const boostIdElement = boostButton.getElementsByClassName("boost-id")[0];
    const boostId = boostIdElement.innerText;

    $.ajax({
        url: '/boost/buy',
        method: 'post',
        dataType: 'json',
        data: { boostId: boostId },
        success: (response) => onBuyBoostSuccess(response, boostButton),
    });
}

function onBuyBoostSuccess(response, boostButton) {
    const score = response["score"];

    const boostPriceElement = boostButton.getElementsByClassName("boost-price")[0];
    const boostQuantityElement = boostButton.getElementsByClassName("boost-quantity")[0];

    const boostPrice = Number(response["price"]);
    const boostQuantity = Number(response["quantity"]);

    boostPriceElement.innerText = boostPrice;
    boostQuantityElement.innerText = boostQuantity;

    updateScoreFromApi(score);
}

function addSecond() {
    seconds++;

    if (seconds >= threshold) {
        addPointsToScore();
    }

    if (seconds > 0) {
        addPointsFromSecond();
    }
}

function click() {
    clicks++;

    if (clicks >= threshold) {
        addPointsToScore();
    }

    if (clicks > 0) {
        addPointsFromClick();
    }
}

function updateScoreFromApi(scoreData) {
    currentScore = Number(scoreData["currentScore"]);
    recordScore = Number(scoreData["recordScore"]);
    profitPerClick = Number(scoreData["profitPerClick"]);
    profitPerSecond = Number(scoreData["profitPerSecond"]);

    updateUiScore();
}

function updateUiScore() {
    currentScoreElement.innerText = currentScore;
    recordScoreElement.innerText = recordScore;
    profitPerClickElement.innerText = profitPerClick;
    profitPerSecondElement.innerText = profitPerSecond;

    toggleBoostsAvailability();
}

function addPointsFromClick() {
    currentScore += profitPerClick;
    recordScore += profitPerClick;

    updateUiScore();
}

function addPointsFromSecond() {
    currentScore += profitPerSecond;
    recordScore += profitPerSecond;

    updateUiScore();
}

function addPointsToScore() {
    $.ajax({
        url: '/score',
        method: 'post',
        dataType: 'json',
        data: { clicks: clicks, seconds: seconds },
        success: (response) => onAddPointsSuccess(response),
    });
}

function onAddPointsSuccess(response) {
    seconds = 0;
    clicks = 0;

    updateScoreFromApi(response);
}

function toggleBoostsAvailability() {
    const boostButtons = document.getElementsByClassName("boost-button");

    for (let i = 0; i < boostButtons.length; i++) {
        const boostButton = boostButtons[i];

        const boostPriceElement = boostButton.getElementsByClassName("boost-price")[0];
        const boostPrice = Number(boostPriceElement.innerText);

        if (boostPrice > currentScore) {
            boostButton.disabled = true;
            continue;
        }

        boostButton.disabled = false;
    } 
}

// Блок с подсказками
const tips = [
    "Подсказка 1: Используйте кирку для добычи руды!",
    "Подсказка 2: Покупайте бусты, чтобы увеличить доход.",
    "Подсказка 3: Автоматические бусты добывают руду без вашего участия.",
    "Подсказка 4: Улучшайте подчинённых для увеличения прибыли.",
    "Подсказка 5: Ставьте рекорды и соревнуйтесь с друзьями!"
];

let currentTipIndex = 0;

function changeTip(direction) {
    currentTipIndex += direction;

    if (currentTipIndex < 0) {
        currentTipIndex = tips.length - 1;
    } else if (currentTipIndex >= tips.length) {
        currentTipIndex = 0;
    }

    const tipTextElement = document.getElementById("tip-text");
    tipTextElement.textContent = tips[currentTipIndex];
}

// Блок с появляющимися алмазами
document.addEventListener('DOMContentLoaded', () => {
    const oreButton = document.getElementById('clickitem');
    const oreContainer = document.getElementById('ore-container');

    oreButton.addEventListener('click', () => {
        const oreImage = document.createElement('img');
        oreImage.src = '/images/OreImages/diamond.png';
        oreImage.classList.add('ore-image');

        const containerWidth = oreContainer.offsetWidth;
        const containerHeight = oreContainer.offsetHeight;
        const randomX = Math.random() * containerWidth;
        const randomY = Math.random() * containerHeight;

        oreImage.style.left = `${randomX}px`;
        oreImage.style.top = `${randomY}px`;

        oreContainer.appendChild(oreImage);
        oreImage.addEventListener('animationend', () => {
            oreImage.remove();
        });
    });
});