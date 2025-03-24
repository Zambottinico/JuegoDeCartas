$(document).ready(function () {
  $('[data-toggle="tooltip"]').tooltip();

  //cargar datos
  //Cambiar links cartas/acerca de
  const cookieUser = JSON.parse(Cookies.get("claveSeguridad"));
  if (cookieUser.rol === "Admin") {
    $("#NavCards").attr("href", "Cards/cards.html");
    $("#NavCards").text("Cartas");
  }

  console.log(cookieUser);
  let postRequest = {
    userid: cookieUser.id,
    clave: cookieUser.clave,
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
      headers: {
        Authorization: "Bearer " + cookieUser.token,
      },
      success: function (response) {
        Respuesta = response;
        console.log(Respuesta);
        DrawInfo(response);
        character = response.lastCard.character;
        url = ["../../img/" + character + ".png"];
        appendNewCard(url);
      },
      error: function (error) {
        DrawError(error);
      },
    });

    const PlayGame = (decision) => {
      let playRequest = {
        userid: cookieUser.id,
        clave: cookieUser.clave,
        decision: decision,
      };
      //PLAY GAME
      $.ajax({
        url: "https://localhost:7116/api/Game/Play",
        method: "PUT",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(playRequest),
        headers: {
          Authorization: "Bearer " + cookieUser.token,
        },
        success: function (response) {
          Respuesta = response;
          console.log(Respuesta);
          let url2 = "../../img/" + response.lastCard.character + ".png";

          if (response.characterUnlocked) {
            Swal.fire({
              title: "Felicidades!",
              text: "Desbloqueaste un nuevo personaje!",
              imageUrl:
                "../../img/" + response.unlockableCharacterName + ".png",
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
          console.log(error.responseJSON);
          DrawError(error);
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

      const cards = swiper.querySelectorAll(".cardClass:not(.dismissing)");
      cards.forEach((card, index) => {
        card.style.setProperty("--i", index);
      });
    }
  });
});
