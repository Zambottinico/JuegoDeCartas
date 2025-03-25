let ofertsList;
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
    url: "https://localhost:7116/api/DiamondOfert",
    method: "GET",
    dataType: "json",
    headers: {
      Authorization: "Bearer " + cookieUser.token,
    },
    success: function (response) {
      console.log(response);
      ofertsList = response;
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
    var oferta = data[i];

    // Crear el contenedor de la oferta
    var ofertaDiv = document.createElement("div");
    ofertaDiv.classList.add("p-3", "mb-2", "shadow-sm");

    // Contenido de la oferta
    ofertaDiv.innerHTML = `
        <div class="d-flex align-items-center">
          <img src="../../../img/oferts/${oferta.nombre}.png" 
               alt="Imagen de ${oferta.nombre}" 
               class="img-fluid" style="width: 50px; height: 70px; margin-right: 10px;">
          <div>
            <h5 class="mb-1">${oferta.nombre}</h5>
            <p class="mb-1">ID: ${oferta.id} | montoDeDiamantes: ${oferta.montoDeDiamantes}</p>
            <p class="mb-1">Pre: ${oferta.precioEnPesos}</p>
          </div>
          <button class="btn btn-primary ms-auto" onclick="editarOferta(${oferta.id})">Editar</button>
          <button class="btn btn-danger ms-auto" onclick="eliminarOferta(${oferta.id})">Eliminar</button>
        </div>
      `;

    // Agregar al contenedor principal
    divCardOferts.appendChild(ofertaDiv);
  }
}

// Función de prueba para el botón Editar
function editarOferta(id) {
  // Buscar la oferta en la lista de datos
  var oferta = ofertsList.find((o) => o.id === id);
  if (!oferta) {
    alert("Oferta no encontrada");
    return;
  }

  // Llenar los campos del formulario con los datos de la oferta seleccionada
  const ofertId = (document.getElementById("ofertId").value = oferta.id);
  const nombre = (document.getElementById("nombre").value = oferta.nombre);
  const Price = (document.getElementById("Price").value = oferta.precioEnPesos);
  const diamond = (document.getElementById("diamond").value =
    oferta.montoDeDiamantes);

  // Mostrar el formulario (en caso de que esté oculto)
  document.getElementById("form").style.display = "block";
}
function eliminarOferta(id) {
  var userId = cookieUser.id;
  var clave = cookieUser.clave;

  $.ajax({
    url: `https://localhost:7116/api/DiamondOfert/${id}`,
    type: "DELETE",
    contentType: "application/json",
    data: JSON.stringify({
      userId: userId,
      clave: clave,
    }),
    headers: {
      Authorization: "Bearer " + cookieUser.token,
    },
    success: function (response) {
      alert("Oferta eliminada correctamente");
      location.reload();
    },
    error: function (xhr) {
      alert("Error al eliminar la oferta: " + xhr.responseJSON.message);
    },
  });
}

// vaciar formulario al cancelar
document.getElementById("cancel").addEventListener("click", function () {
  const ofertId = (document.getElementById("ofertId").value = "");
  const nombre = (document.getElementById("nombre").value = "");
  const Price = (document.getElementById("Price").value = 0);
  const diamond = (document.getElementById("diamond").value = 0);
});
document.getElementById("create").addEventListener("click", function () {
  // Obtener los valores del formulario
  const ofertId = document.getElementById("ofertId").value;
  const nombre = document.getElementById("nombre").value;
  const Price = document.getElementById("Price").value;
  const diamond = document.getElementById("diamond").value;

  if (ofertId === "") {
    const offerData = {
      userid: cookieUser.id,
      clave: cookieUser.clave,
      nombre: nombre,
      precioEnPesos: Price,
      montoDeDiamantes: diamond,
    };
    postOffer(offerData);
  } else {
    const offerData = {
      userid: cookieUser.id,
      clave: cookieUser.clave,
      nombre: nombre,
      precioEnPesos: Price,
      montoDeDiamantes: diamond,
    };
    offerData.id = ofertId;
    updateOffer(offerData, ofertId);
  }
});

// Función para realizar el POST
function postOffer(offerData) {
  fetch("https://localhost:7116/api/DiamondOfert", {
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
function updateOffer(offerData, id) {
  fetch(`https://localhost:7116/api/DiamondOfert/` + id, {
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
