$(document).ready(function () {
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

  const valor1 = JSON.parse(Cookies.get("claveSeguridad"));

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

  function showData(data) {
    const content = $("#cards-container");
    let contadorCartas = 0;
    let TotalCartas = 31;
    for (let i = 0; i < data.length; i++) {
      contadorCartas++;
      const user = data[i];
      var row = `
      <div class="col-md-2">
      <div class="div-contenedor">
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
      <div class="col-md-2">
      <div class="div-contenedor">
        <img
          src="../../../img/Block.png"
          alt="Tu Imagen"
          class="img-fluid card-shadow "
        />
      </div>
    </div>
        `;
      content.append(row);
    }
  }
});
