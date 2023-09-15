//cargar datos
const valor1 = JSON.parse(Cookies.get("claveSeguridad"));
console.log(valor1.id);
let postRequest = {
  userid: valor1.id,
  clave: valor1.clave,
};
let character = "";
// constants
let urls = ["../../img/" + character + ".png"];
let Respuesta = {};
console.log(postRequest);

$(document).ready(function () {
  $.ajax({
    url: "https://localhost:7116/api/Game/Post",
    method: "POST",
    dataType: "json",
    contentType: "application/json",
    data: JSON.stringify(postRequest),
    success: function (response) {
      Respuesta = response;
      console.log(Respuesta);
      DrawInfo(response);
      character = response.lastCard.character;
      url = ["../../img/" + character + ".png"];
      appendNewCard(url);
    },
    error: function (error) {
      console.log(error);
    },
  });

  const PlayGame = (decision) => {
    let playRequest = {
      userid: valor1.id,
      clave: valor1.clave,
      decision: decision,
    };
    //PLAY GAME
    $.ajax({
      url: "https://localhost:7116/api/Game/Play",
      method: "PUT",
      dataType: "json",
      contentType: "application/json",
      data: JSON.stringify(playRequest),
      success: function (response) {
        Respuesta = response;
        console.log(Respuesta);
        let url2 = "../../img/" + response.lastCard.character + ".png";
        if (response.characterUnlocked) {
          Swal.fire({
            title: "Felicidades!",
            text: "Desbloqueaste un nuevo personaje!",
            imageUrl: "../../img/" + response.unlockableCharacterName + ".png",
            customClass: {
              image: "imgSweetAlert",
            },
          });
        }
        appendNewCard(url2);
        DrawInfo(response);
        if (response.gamestate == "FINISHED") {
          console.log("perdiste pai");
        }
      },
      error: function (error) {
        console.log(error);
      },
    });
  };

  // DOM
  const swiper = document.querySelector("#swiper");

  const dislike = document.querySelector("#dislike");

  // variables
  let cardCount = 0;

  // functions
  function appendNewCard(url) {
    const card = new Card({
      imageUrl: url,
      // onDismiss: appendNewCard,
      onLike: () => {
        PlayGame(2);
        console.log("decision 2");
      },
      onDislike: () => {
        PlayGame(1);
        console.log("decision 1");
      },
    });
    swiper.append(card.element);
    cardCount++;

    const cards = swiper.querySelectorAll(".card:not(.dismissing)");
    cards.forEach((card, index) => {
      card.style.setProperty("--i", index);
    });
  }
});
