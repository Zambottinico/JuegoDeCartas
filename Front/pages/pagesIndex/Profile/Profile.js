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
      <div class="div-contenedor d-flex justify-content-center">
        <img
          src="../../../img/${data[i].name}.png"
          alt="Tu Imagen"
          class="img-fluid card-shadow "
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
      <div class="div-contenedor d-flex justify-content-center">
        <img
          src="../../../img/block.png"
          alt="Tu Imagen"
          class="img-fluid card-shadow "
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
