$(document).ready(function () {
  //Cambiar links cartas/acerca de
  const valor1 = JSON.parse(Cookies.get("claveSeguridad"));
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
        window.location = "../../pagesLogin/login.html";
      },
    });
  });

  const username = Cookies.get("username");

  $("#username").text(username);
  $.ajax({
    url:
      "https://localhost:7116/api/Character/GetCharactersByUserId/" + valor1.id,
    method: "GET",
    dataType: "json",
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
          alt="Tu Imagen"
          class="img-fluid card-shadow cardImage"
          onclick="showInfo(${data[i].id}, '${data[i].name}')";
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

function showInfo(id, name) {
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
        <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Rem sunt, exercitationem temporibus dicta esse aspernatur nisi, ipsam excepturi illo libero reprehenderit saepe dignissimos beatae fuga placeat error nesciunt, ab perferendis?</p>
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
