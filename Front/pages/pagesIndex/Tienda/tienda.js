let diamodOfertList = [];
$(document).ready(function () {
  //Cambiar links cartas/acerca de
  const valor1 = JSON.parse(Cookies.get("claveSeguridad"));
  if (valor1.rol === "Admin") {
    $("#NavCards").attr("href", "../Cards/cards.html");
    $("#NavCards").text("Cartas");
  }

  var jsonData = {
    userid: valor1.id,
    clave: valor1.clave,
  };
  console.log(jsonData);
  $.ajax({
    url: "https://localhost:7116/api/User/GetUserById/" + valor1.id,
    method: "GET",
    dataType: "json",
    success: function (response) {
      showUserInfo(response);
    },
    error: function (error) {
      console.log(error);
    },
  });

  function showUserInfo(data) {
    const pCoins = $("#coins");
    pCoins.append(
      ` <img src="../../../img/items/gold.png" alt="" style="width: 35px;"> ${data.gold} <img src="../../../img/items/diamond.png" alt="" style="width: 30px;"> ${data.diamonds}`
    );
    const pLifes = $("#lives");
    pLifes.append(`${data.lives} de ${data.maxLives} Vidas`);
  }

  $(document).ready(function () {

    $.ajax({
      url: "https://localhost:7116/api/DiamondOfert",
      method: "GET",
      dataType: "json",
      success: function (response) {
        console.log(response);
        llenarOferts(response);
      },
      error: function (error) {
        console.log(error);
      },
    });

});

function llenarOferts(data) {
  var divCardOferts = document.getElementById("diamondOferts");
  diamodOfertList = data;
  // Limpiar contenido previo
  divCardOferts.innerHTML = "";

  for (var i = 0; i < data.length; i++) {
    var oferta = data[i];

    // Crear el contenedor de la oferta
    var ofertaDiv = document.createElement("div");
    ofertaDiv.classList.add("p-3", "mb-2","row");

    // Contenido de la oferta
    ofertaDiv.innerHTML = `
      <div class="col-4 shadow-sm">
        <img src="../../../img/oferts/${oferta.nombre}.png" 
             alt="Imagen de ${oferta.nombre}" 
             class="img-fluid" style="width: 50px; height: 70px; margin-right: 10px;">
        <div>
          <h5 class="mb-1">${oferta.nombre}</h5>
          <p>${oferta.montoDeDiamantes}</p>
          <p class="mb-1">$ ${oferta.precioEnPesos}</p>
          <button class="btn btn-success" onclick="crearPreferencia(${oferta.id})">Obtener</button>
        </div>
      </div>
    `;

    // Agregar al contenedor principal
    divCardOferts.appendChild(ofertaDiv);
  }
}



  $("#rechargeLives").click(function () {
    $.ajax({
      url: "https://localhost:7116/api/lives/recharge",
      method: "POST",
      dataType: "json",
      contentType: "application/json",
      data: JSON.stringify(jsonData),
      success: function (response) {
        console.log(response);
        Swal.fire("Exito", "Se a realizado la compra", "success").then(
          (result) => {
            if (result.isConfirmed) {
              location.reload();
            }
          }
        );
      },
      error: function (error) {
        Swal.fire("Error", `${error.responseJSON.details}`, "error");
        console.log(error);
      },
    });
  });

  //Cargar cartas en venta

  $.ajax({
    url: "https://localhost:7116/api/cardoferts", // URL del endpoint
    type: "GET", // Método HTTP
    data: { userId: valor1.id }, // Parámetros de consulta
    success: function (response) {
      const content = $("#cardOferts");
      if (response.length > 0) {
        content.append(
          ` <h2 style="font-size: 50px;" class="enchanted">Cartas</h2>`
        );
      }
      for (let i = 0; i < response.length; i++) {
        let price =
          response[i].diamondPrice > 0
            ? response[i].diamondPrice
            : response[i].goldPrice;
        let currencyImage =
          response[i].diamondPrice > 0 ? "diamond.png" : "gold.png";

        var row = `
        <div class="col-md-2 col-sm-4">
          <div class="d-flex flex-column align-items-center">
            <!-- Imagen en la parte superior -->
            <img
              src="../../../img/${response[i].characterName}.png"
              alt="Imagen del personaje ${response[i].characterName}"
              class="img-fluid card-shadow cardImage"
            />
            <!-- Botón en la parte inferior -->
            <button class="btn-ofertCard w-100 d-flex justify-content-center align-items-center mt-2" 
                    data-id="${response[i].id}" 
                    data-charactername="${response[i].characterName}" 
                    data-goldprice="${response[i].goldPrice}" 
                    data-diamondprice="${response[i].diamondPrice}"
                    data-characterid="${response[i].characterId}">
                   
              <img
                src="../../../img/items/${currencyImage}"
                style="width: 30px"
                alt=""
              />
              ${price}
            </button>
          </div>
        </div>
      `;
        content.append(row);
      }

      // Agregar el event listener después de cargar los botones en el DOM
      $(".btn-ofertCard").click(function () {
        const responseData = {
          id: $(this).data("id"),
          characterId: $(this).data("characterid"),
          characterName: $(this).data("charactername"),
          goldPrice: $(this).data("goldprice"),
          diamondPrice: $(this).data("diamondprice"),
          userId: valor1.id,
        };
        handleButtonClick($(this).data("id"));
      });

      console.log("CardOferts:", response);
    },
    error: function (xhr, status, error) {
      console.error("Error:", error);
    },
  });

  function handleButtonClick(response) {
    const requestData = {
      userId: valor1.id,
      clave: valor1.clave,
      idCardOfert: response,
    };

    $.ajax({
      url: "https://localhost:7116/api/cardoferts/UnlockCharacter", // Endpoint en tu backend
      type: "POST", // Método HTTP
      contentType: "application/json", // Tipo de contenido JSON
      data: JSON.stringify(requestData), // Convertimos el objeto a JSON
      success: function (response) {
        console.log("Personaje desbloqueado con éxito:", response);
        Swal.fire({
          title: "Felicidades!",
          text: "Desbloqueaste un nuevo personaje!",
          imageUrl: "../../../img/" + response + ".png",
          customClass: {
            image: "imgSweetAlert",
          },
        }).then((result) => {
          if (result.isConfirmed) {
            location.reload();
          }
        });
      },
      error: function (error) {
        console.log(error);
        console.error("Error al desbloquear el personaje:", error);
        Swal.fire({
          icon: "error",
          title: "¡Error!",
          text: error.responseJSON.details,
        });
      },
    });
  }
});


function crearPreferencia(id) {
  const valor1 = JSON.parse(Cookies.get("claveSeguridad"));
  requestData = {
    userId: valor1.id,
    clave: valor1.clave
  }
  ofert = diamodOfertList.find((ofert) => ofert.id === id);
  $.ajax({
    url: "https://localhost:7116/api/MercadoPago/crear-preferencia/"+ofert.id, // Endpoint en tu backend
    type: "POST", // Método HTTP
    contentType: "application/json", // Tipo de contenido JSON
    data: JSON.stringify(requestData), // Convertimos el objeto a JSON
    success: function (response) {
      console.log(response);
      console.log("lista de ofertas "+diamodOfertList);
      Swal.fire({
        title: ofert.nombre,
        text: `Pagar ${ofert.precioEnPesos} ARS por ${ofert.montoDeDiamantes} diamantes`,
        imageUrl: "../../../img/oferts/" + ofert.nombre + ".png", // Asegúrate de que esta imagen existe
        customClass: {
          image: "imgSweetAlert",
        },
        showCancelButton: true,
        confirmButtonText: "Pagar con Mercado Pago",
        cancelButtonText: "Cancelar"
      }).then((result) => {
        if (result.isConfirmed) {
          window.location.href = response.initPoint; 
        }
      });
    },
    error: function (error) {
      console.log(error);
      console.error("Error al realizar la compra:", error);
      Swal.fire({
        icon: "error",
        title: "¡Error!",
        text: error.responseJSON.details,
      });
    },
  });
  }