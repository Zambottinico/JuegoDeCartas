$(document).ready(function () {
  //Cambiar links cartas/acerca de
  const valor1 = JSON.parse(Cookies.get("claveSeguridad"));
  console.log(valor1);
  if (valor1.rol === "Admin") {
    $("#NavCards").attr("href", "../Cards/cards.html");
    $("#NavCards").text("Cartas");
  }

  $("#btn-CerrarSesion").click(function () {
    Swal.fire({
      title: "Confirmar",
      text: "Esta seguro que desea cerrar la sesión?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Cerrar sesión",
      cancelButtonText: "Cancelar",
      preConfirm: () => {
        Cookies.remove("claveSeguridad");
        Cookies.remove("username");
        window.location = "../../pagesLogin/login.html";
      },
    });
  });



  $.ajax({
    url: "https://localhost:7116/api/User/GetUserById/" + valor1.id,
    method: "GET",
    dataType: "json",
    headers: {
      "Authorization": "Bearer " + valor1.token 
    },
    success: function (response) {
      showUserInfo(response);
    },
    error: function (error) {
      console.log(error);
    },
  });

  function showUserInfo(data) {
    console.log(data);
    const pCoins = $("#coins");
    pCoins.append(` <img src="../../../img/items/gold.png" alt="" style="width: 35px;"> ${data.gold} <img src="../../../img/items/diamond.png" alt="" style="width: 30px;"> ${data.diamonds}`);
    const pLifes = $("#lives");
    pLifes.append(`${data.lives} de ${data.maxLives} Vidas`);

    if (data.maxLives != data.lives) {
      const pNextLive = $("#nextLive");
      const date = new Date(data.lastLifeRecharge); // Convierte el string en un objeto Date
       // Sumar 30 minutos a la fecha
       date.setMinutes(date.getMinutes() + 30);
      // Si hay segundos, sumar un minuto
      if (date.getSeconds() !== 0) {
        date.setMinutes(date.getMinutes() + 1); // Suma 1 minuto
      }
    
      // Formato para mostrar solo la hora y los minutos
      const formattedTime = date.toLocaleTimeString("es-AR", {
        hour: "2-digit",    // Hora en formato de 2 dígitos
        minute: "2-digit",  // Minutos en formato de 2 dígitos
      });
    
      // Muestra solo la hora formateada
      pNextLive.append(formattedTime + " obtienes una vida");
    }
    
    
  }

  $("#username").text(valor1.username);
  $.ajax({
    url:
      "https://localhost:7116/api/Character/GetCharactersByUserId/" + valor1.id,
    method: "GET",
    dataType: "json",
    headers: {
      "Authorization": "Bearer " + valor1.token 
    },
    success: function (response) {
      showData(response);
    },
    error: function (error) {
      console.log(error);
    },
  });
  let contadorCartas = 0;
  function showData(data) {
    const content = $("#cards-container");

    let TotalCartas = 31;
    for (let i = 0; i < data.length; i++) {
      contadorCartas++;
      const user = data[i];
      var row = `
      <div class="col-md-2 col-sm-4 ">
      <div class=" d-flex justify-content-center">
        <img
          src="../../../img/${data[i].name}.png"
          alt="Imagen del personaje ${data[i].name}"
          class="img-fluid card-shadow cardImage"
          onclick="showInfo(${data[i].id}, '${data[i].name}','${data[i].lore}')";
        />
      </div>
    </div>
        `;
      content.append(row);
    }
    //Agregar cartas en blanco
    for (let i = 0; i < TotalCartas - contadorCartas; i++) {
      var row = `
      <div class="col-md-2 col-sm-4">
      <div class="d-flex justify-content-center">
        <img
          src="../../../img/block.png"
          alt="Tu Imagen"
          class="img-fluid card-shadow cardImage"
          onclick="showInfoNoCard(${i})";
        />
      </div>
    </div>
        `;
      content.append(row);
    }
    let TotalCartasElement = document.getElementById("totalCharacters");

    TotalCartasElement.textContent = contadorCartas + "/31";
    if (contadorCartas == 31) {
      TotalCartasElement.classList.add("text-success");
    }
  }
});

function showInfo(id, name, lore) {
  Swal.fire({
    html: `
    <div class="row">
      <div class=" col-sm-6">
        <img
          src="../../../img/${name}.png"
          alt="Tu Imagen"
          class="img-fluid card-shadow "
        />
      </div>
      <div class=" col-sm-6">
        <h2>${name}</h2>
        <p>${lore}</p>
      </div>
    </div>
    
    `,
    showConfirmButton: false,
  });
}

function showInfoNoCard(i) {
  Swal.fire({
    html: `
    <div class="row">
      <div class=" col-sm-6">
        <img
          src="../../../img/block.png"
          alt="Tu Imagen"
          class="img-fluid card-shadow "
        />
      </div>
      <div class=" col-sm-6">
        <h2 class="text-danger">Bloqueado</h2>
        <p>Sigue jugando para desbloquear esta carta</p>
      </div>
    </div>
    
    `,
    showConfirmButton: false,
  });
}
