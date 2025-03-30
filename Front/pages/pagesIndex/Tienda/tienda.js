let diamodOfertList = [];
$(document).ready(function () {
  //ajustar tamaño del slider
  const swiperEl = document.querySelectorAll("swiper-container");

  function checkScreenSize() {
    for (let i = 0; i < swiperEl.length; i++) {
      if (window.innerWidth < 700) {
        swiperEl[i].setAttribute("navigation", "false");
      } else {
        swiperEl[i].setAttribute("navigation", "true");
      }
    }
  }

  // Ejecutar la función cuando la página se carga
  window.addEventListener("load", checkScreenSize);

  // Ejecutar la función cada vez que se cambia el tamaño de la ventana
  window.addEventListener("resize", checkScreenSize);
  //Cambiar links cartas/acerca de
  let cookieUser = Cookies.get("claveSeguridad");

  if (!cookieUser) {
    // Redirigir al usuario a la página de inicio de sesión si no está autenticado
    window.location.href = "../../pagesLogin/login.html";
  } else {
    cookieUser = JSON.parse(cookieUser);

    if (cookieUser.rol === "Admin") {
      $("#NavCards").attr("href", "../Cards/cards.html");
      $("#NavCards").text("Cartas");
    }
  }

  var jsonData = {
    userid: cookieUser.id,
    clave: cookieUser.clave,
  };
  console.log(jsonData);
  $.ajax({
    url: "https://localhost:7116/api/User/GetUserById/" + cookieUser.id,
    method: "GET",
    dataType: "json",
    headers: {
      Authorization: "Bearer " + cookieUser.token,
    },
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
      headers: {
        Authorization: "Bearer " + cookieUser.token,
      },
      success: function (response) {
        console.log(response);
        llenarOferts(response);
      },
      error: function (error) {
        console.log(error);
      },
    });
  });
  $.ajax({
    url: "https://localhost:7116/api/Game/config",
    type: "GET",
    headers: {
      Authorization: "Bearer " + cookieUser.token,
    },
    success: function (response) {
      // Rellenar los campos con los datos obtenidos
      $("#rechargeLives").append(response.lifeRechargePrice);
    },
    error: function (xhr, status, error) {
      alert("Hubo un error al obtener los datos.");
      console.error(error);
    },
  });

  function llenarOferts(data) {
    var divCardOferts = document.getElementById("diamondOferts");
    diamodOfertList = data;
    // Limpiar contenido previo
    divCardOferts.innerHTML = "";

    for (var i = 0; i < data.length; i++) {
      var oferta = data[i];

      // Crear el contenedor de la oferta
      var ofertaDiv = document.createElement("swiper-slide");
      ofertaDiv.classList.add("d-flex", "justify-content-center");

      ofertaDiv.innerHTML = `
            <div class="card" style="width: 18rem">
              <img
                src="../../../img/items/vida.png"
                class="card-img-top img-fluid"
                width="100px"
                alt="..."
              />
              <div class="card-body">
                <h5 class="card-title enchanted">${oferta.nombre}</h5>
                <p><img width="20px" src="../../../img/items/diamond.png">${oferta.montoDeDiamantes}</p>
                <p class="mb-1">$ ${oferta.precioEnPesos}</p>
                <button id="btn-${oferta.id}" class="btn btn-success" onclick="crearPreferencia(${oferta.id})">Obtener</button>
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
      headers: {
        Authorization: "Bearer " + cookieUser.token,
      },
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
    data: { userId: cookieUser.id }, // Parámetros de consulta
    headers: {
      Authorization: "Bearer " + cookieUser.token,
    },
    success: function (response) {
      var content = document.getElementById("cardOferts");
      //if (response.length > 0) {
      //  content.append(
      //    ` <h2 style="font-size: 50px;" class="enchanted">Cartas</h2>`
      // );
      // }
      for (let i = 0; i < response.length; i++) {
        if (response.length > 0) {
          $("#ofertCardsTitle").text("Cartas");
        }
        let price =
          response[i].diamondPrice > 0
            ? response[i].diamondPrice
            : response[i].goldPrice;
        let currencyImage =
          response[i].diamondPrice > 0 ? "diamond.png" : "gold.png";

        // Crear el contenedor de la oferta
        var ofertaDiv = document.createElement("swiper-slide");
        ofertaDiv.classList.add("d-flex", "justify-content-center");

        ofertaDiv.innerHTML = `
            <div class="card" style="width: 18rem">
              <img
              src="../../../img/${response[i].characterName}.png"
              alt="Imagen del personaje ${response[i].characterName}"
              class="img-fluid card-shadow cardImage"
            />
              <div class="card-body">
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
        // Agregar al contenedor principal
        content.appendChild(ofertaDiv);
      }

      // Agregar el event listener después de cargar los botones en el DOM
      $(".btn-ofertCard").click(function () {
        const responseData = {
          id: $(this).data("id"),
          characterId: $(this).data("characterid"),
          characterName: $(this).data("charactername"),
          goldPrice: $(this).data("goldprice"),
          diamondPrice: $(this).data("diamondprice"),
          userId: cookieUser.id,
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
      userId: cookieUser.id,
      clave: cookieUser.clave,
      idCardOfert: response,
    };

    $.ajax({
      url: "https://localhost:7116/api/cardoferts/UnlockCharacter", // Endpoint en tu backend
      type: "POST", // Método HTTP
      contentType: "application/json", // Tipo de contenido JSON
      data: JSON.stringify(requestData), // Convertimos el objeto a JSON
      headers: {
        Authorization: "Bearer " + cookieUser.token,
      },
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

  $("#canjearCodigo").click(function () {
    let codigo = $("#codigo").val().trim();
    requestData = {
      userId: cookieUser.id,
      clave: cookieUser.clave,
    };
    if (codigo) {
      $.ajax({
        url: "https://localhost:7116/api/Game/canjear/" + codigo,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(requestData),
        headers: {
          Authorization: "Bearer " + cookieUser.token,
        },
        success: function (response) {
          if (response.ok) {
            Swal.fire({
              icon: "success",
              title: "codigo: " + response.codigo,
              text: `${response.message}, obtuviste ${response.numeroDiamantes} diamantes y ${response.numeroOro} monedas de oro`,
              confirmButtonText: "Ok",
            });
          } else {
            Swal.fire({
              icon: "error",
              title: "codigo: " + response.codigo,
              text: `${response.message}`,
              confirmButtonText: "Ok",
            });
          }
          console.log(response);
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
  });
});

function crearPreferencia(id) {
  const cookieUser = JSON.parse(Cookies.get("claveSeguridad"));
  btnOfertId = document.getElementById("btn-" + id);
  btnOfertId.disabled = true;
  requestData = {
    userId: cookieUser.id,
    clave: cookieUser.clave,
  };
  ofert = diamodOfertList.find((ofert) => ofert.id === id);
  $.ajax({
    url: "https://localhost:7116/api/MercadoPago/crear-preferencia/" + ofert.id, // Endpoint en tu backend
    type: "POST", // Método HTTP
    contentType: "application/json", // Tipo de contenido JSON
    data: JSON.stringify(requestData), // Convertimos el objeto a JSON
    headers: {
      Authorization: "Bearer " + cookieUser.token,
    },
    success: function (response) {
      console.log(response);

      Swal.fire({
        title: ofert.nombre,
        text: `Pagar ${ofert.precioEnPesos} ARS por ${ofert.montoDeDiamantes} diamantes`,
        imageUrl: "../../../img/oferts/" + ofert.nombre + ".png", // Asegúrate de que esta imagen existe
        customClass: {
          image: "imgSweetAlert",
        },
        showCancelButton: true,
        confirmButtonText: "Pagar con Mercado Pago",
        cancelButtonText: "Cancelar",
      }).then((result) => {
        if (result.isConfirmed) {
          window.location.href = response.initPoint;
        } else {
          btnOfertId.disabled = false;

          cancelInvoice(response.invoiceId);
          location.reload();
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

function cancelInvoice(id) {
  console.log(id);
  const cookieUser = JSON.parse(Cookies.get("claveSeguridad"));
  requestData = {
    userId: cookieUser.id,
    clave: cookieUser.clave,
  };
  $.ajax({
    url: "https://localhost:7116/api/MercadoPago/cancelInvoice/" + id,
    type: "POST",
    contentType: "application/json",
    data: JSON.stringify(requestData),
    headers: {
      Authorization: "Bearer " + cookieUser.token,
    },
    success: function (response) {
      console.log(response);
    },
    error: function (error) {
      console.error("Error al realizar la compra:", error);
    },
  });
}
