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
    url: "https://localhost:7116/api/Character/GetCharacters",
    method: "GET",
    dataType: "json",
    headers: {
      Authorization: "Bearer " + cookieUser.token,
    },
    success: function (response) {
      console.log(response);

      llenarSelect("characterId", response);
      llenarSelect("decision1-unlockableCharacter", response);
      llenarSelect("decision2-unlockableCharacter", response);
    },
    error: function (error) {
      console.log(error);
    },
  });

  $.ajax({
    url: "https://localhost:7116/api/cardoferts/all",
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

  function llenarSelect(selectElement, data) {
    console.log(data[1]);
    var select = document.getElementById(selectElement);

    for (var i = 0; i < data.length; i++) {
      var option = document.createElement("option");
      option.value = data[i].id;
      option.text = data[i].name + " | " + data[i].cantidadCartas + " cartas";
      select.appendChild(option);
    }
  }
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
          <img src="../../../img/${oferta.characterName}.png" 
               alt="Imagen de ${oferta.characterName}" 
               class="img-fluid" style="width: 50px; height: 70px; margin-right: 10px;">
          <div>
            <h5 class="mb-1">${oferta.characterName}</h5>
            <p class="mb-1">ID: ${oferta.id} | Character ID: ${oferta.characterId}</p>
            <p class="mb-1">Gold: ${oferta.goldPrice} | Diamond: ${oferta.diamondPrice}</p>
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
  document.getElementById("ofertId").value = oferta.id;
  document.getElementById("characterId").value = oferta.characterId;
  document.getElementById("goldPrice").value = oferta.goldPrice;
  document.getElementById("diamondPrice").value = oferta.diamondPrice;

  // Mostrar el formulario (en caso de que esté oculto)
  document.getElementById("form").style.display = "block";
}
function eliminarOferta(id) {
  var userId = cookieUser.id;
  var clave = cookieUser.clave;

  $.ajax({
    url: `https://localhost:7116/api/tienda/deleteCardOfert/${id}`,
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
  document.getElementById("ofertId").value = "";
  document.getElementById("characterId").value = "";
  document.getElementById("goldPrice").value = 0;
  document.getElementById("diamondPrice").value = 0;
});
document.getElementById("create").addEventListener("click", function () {
  // Obtener los valores del formulario
  const ofertId = document.getElementById("ofertId").value;
  const characterId = document.getElementById("characterId").value;
  const goldPrice = document.getElementById("goldPrice").value;
  const diamondPrice = document.getElementById("diamondPrice").value;

  const offerData = {
    userid: cookieUser.id,
    clave: cookieUser.clave,
    characterId: characterId,
    goldPrice: goldPrice,
    diamondPrice: diamondPrice,
  };
  if (ofertId === "") {
    const offerData = {
      userid: cookieUser.id,
      clave: cookieUser.clave,
      characterId: characterId,
      goldPrice: goldPrice,
      diamondPrice: diamondPrice,
    };
    postOffer(offerData);
  } else {
    const offerData = {
      id: ofertId,
      userid: cookieUser.id,
      clave: cookieUser.clave,
      characterId: characterId,
      goldPrice: goldPrice,
      diamondPrice: diamondPrice,
    };
    offerData.id = ofertId;
    updateOffer(offerData);
  }
});

// Función para realizar el POST
function postOffer(offerData) {
  fetch("https://localhost:7116/api/tienda/createCardOfert", {
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
  fetch(`https://localhost:7116/api/tienda/updateCardOfert`, {
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
