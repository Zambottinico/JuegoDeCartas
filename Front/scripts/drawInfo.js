const decision1 = $("#decision1");
const decision2 = $("#decision2");

const days = $("#days");
const description = $("#description");

const magicState = document.getElementById("bgMagic");
const armyState = document.getElementById("bgArmy");
const economyState = document.getElementById("bgEconomy");
const populationState = document.getElementById("bgPopulation");

const ColorState = (element, percentage) => {
  if (percentage > 85) {
    element.style.background = "rgb(89, 166, 238)";
    return;
  }
  if (percentage > 49) {
    element.style.background = "green";
    return;
  }
  if (percentage > 29) {
    element.style.background = "orange";
    return;
  }
  element.style.background = "red";
};

const DrawStates = (game) => {
  ColorState(magicState, game.magicstate);
  magicState.style.height = game.magicstate + "%";

  ColorState(armyState, game.armystate);
  armyState.style.height = game.armystate + "%";

  ColorState(economyState, game.economystate);
  economyState.style.height = game.economystate + "%";

  ColorState(populationState, game.populationstate);
  populationState.style.height = game.populationstate + "%";
};

const DrawInfo = (game) => {
  decision1.text(game.lastCard.decision1.description);
  decision2.text(game.lastCard.decision2.description);
  description.text(game.lastCard.description);
  days.empty();
  days.append(
    ` <img src="../../../img/items/vida.png" alt="" style="width: 30px;"> ${game.lives} <img src="../../../img/items/gold.png" alt="" style="width: 30px;">${game.gold}  <img src="../../../img/items/diamond.png" alt="" style="width: 30px;">${game.diamonds}<img class="ms-2" src="../../../img/items/day.png" alt="" style="width: 30px;">Dia: ${game.day}`
  );
  DrawStates(game);
};
const DrawError = (error) => {
  if (error.responseJSON == undefined) {
    description.text("A ocurrido un error inesperado");
  }else{
    description.text(error.responseJSON.details);
  }
  decision1.text("Seguir esperando...");
  decision2.text("Comprar vidas");
  appendNotLivesCard("../../img/Harverter.png", error);
};
function appendNotLivesCard(url, error) {
  const card = new Card({
    imageUrl: url,
    // onDismiss: appendNewCard,
    onLike: () => {
      console.log("decision 2");
      window.location.href =
        "http://127.0.0.1:5501/pages/pagesIndex/Tienda/tienda.html";
    },
    onDislike: () => {
      console.log("decision 1");
      DrawError(error);
    },
  });
  swiper.append(card.element);
  const cards = swiper.querySelectorAll(".cardClass:not(.dismissing)");
  cards.forEach((card, index) => {
    card.style.setProperty("--i", index);
  });
}
