let cuponList;
// Construir el objeto JSON
let cookieUser = Cookies.get("claveSeguridad");

if (!cookieUser) {
  // Redirigir al usuario a la página de inicio de sesión si no está autenticado
  window.location.href = "../../pagesLogin/login.html";
} else {
  cookieUser = JSON.parse(cookieUser);

  if (cookieUser.rol === "Admin") {
    $("#NavCards").attr("href", "../../pagesIndex/Cards/cards.html");
    $("#NavCards").text("Cartas");
  }
}

$(document).ready(function () {
  $.ajax({
    url: "https://localhost:7116/api/Game/cupon/getAll",
    method: "GET",
    dataType: "json",
    headers: {
      Authorization: "Bearer " + cookieUser.token,
    },
    success: function (response) {
      console.log(response);
      cuponList = response;
      llenarOferts(response);
    },
    error: function (error) {
      console.log(error);
    },
  });
});

function llenarOferts(data) {
  var divCardOferts = document.getElementById("cardOferts");

  // Limpiar contenido previo
  divCardOferts.innerHTML = "";

  for (var i = 0; i < data.length; i++) {
    var cupon = data[i];

    // Crear el contenedor de la oferta
    var ofertaDiv = document.createElement("div");
    ofertaDiv.classList.add("p-3", "mb-2", "shadow-sm");

    // Contenido de la oferta
    ofertaDiv.innerHTML = `
        <div class="d-flex align-items-center">
          <div>
            <h5 class="mb-1">Codigo:${cupon.codigo}</h5>
            <p class="mb-1">ID: ${cupon.id}</p>
            <p class="mb-1">Gold: ${cupon.numeroOro} | Diamond: ${cupon.numeroDiamantes}</p>
          </div>
          <button class="btn btn-primary ms-auto" onclick="editarOferta(${cupon.id})">Editar</button>
        </div>
      `;

    // Agregar al contenedor principal
    divCardOferts.appendChild(ofertaDiv);
  }
}

// Función de prueba para el botón Editar
function editarOferta(id) {
  // Buscar la oferta en la lista de datos
  var oferta = cuponList.find((o) => o.id === id);
  if (!oferta) {
    alert("Cupon no encontrado");
    return;
  }

  // Llenar los campos del formulario con los datos de la oferta seleccionada
  document.getElementById("ofertId").value = oferta.id;
  document.getElementById("codigo").value = oferta.codigo;
  document.getElementById("goldPrice").value = oferta.numeroOro;
  document.getElementById("diamondPrice").value = oferta.numeroDiamantes;

  // Mostrar el formulario (en caso de que esté oculto)
  document.getElementById("form").style.display = "block";
}

// vaciar formulario al cancelar
document.getElementById("cancel").addEventListener("click", function () {
  document.getElementById("ofertId").value = "";
  document.getElementById("codigo").value = "";
  document.getElementById("goldPrice").value = 0;
  document.getElementById("diamondPrice").value = 0;
});
document.getElementById("create").addEventListener("click", function () {
  // Obtener los valores del formulario
  const id = document.getElementById("ofertId").value;
  const codigo = document.getElementById("codigo").value;
  const numeroOro = document.getElementById("goldPrice").value;
  const numeroDiamantes = document.getElementById("diamondPrice").value;
  if (id === "") {
    const offerData = {
      codigo: codigo,
      numeroOro: numeroOro,
      numeroDiamantes:numeroDiamantes,
    };
    postOffer(offerData);
  } else {
    const offerData = {
        id: id,
        codigo: codigo,
        numeroOro: numeroOro,
        numeroDiamantes:numeroDiamantes,
        isDeleted: false
      };
    offerData.id = ofertId;
    updateOffer(offerData);
  }
});

// Función para realizar el POST
function postOffer(offerData) {
  fetch("https://localhost:7116/api/Game/cupon/crear", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + cookieUser.token,
    },
    body: JSON.stringify(offerData),
  })
    .then((response) => response.json())
    .then((data) => {
      console.log("Oferta creada:", data);
      alert("Oferta creada correctamente");
      location.reload();
    })
    .catch((error) => {
      console.error("Error al crear la oferta:", error);
      alert("Hubo un error al crear la oferta");
    });
}

// Función para realizar el PUT
function updateOffer(offerData) {
  fetch(`https://localhost:7116/api/Game/cupon/actualizar`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + cookieUser.token,
    },
    body: JSON.stringify(offerData),
  })
    .then((response) => response.json())
    .then((data) => {
      console.log("Oferta actualizada:", data);
      alert("Oferta actualizada correctamente");
      location.reload();
    })
    .catch((error) => {
      console.error("Error al actualizar la oferta:", error);
      alert("Hubo un error al actualizar la oferta");
    });
}
