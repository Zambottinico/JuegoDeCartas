$(document).ready(function () {
  $("#form").validate({
    rules: {
      description: {
        required: true,

        minlength: 10,
      },
      characterId: {
        required: true,
        maxlength: 50,
        digits: true,
      },
      "decision1-description": {
        required: true,
        maxlength: 50,
      },
      "decision1-magic": {
        required: true,
        maxlength: 50,
      },
      "decision1-army": {
        required: true,
        maxlength: 50,
      },
      "decision1-economy": {
        required: true,
        maxlength: 50,
      },
      "decision1-population": {
        required: true,
        maxlength: 50,
      },
      "decision2-description": {
        required: true,
        maxlength: 50,
      },
      "decision2-magic": {
        required: true,
        maxlength: 50,
      },
      "decision2-army": {
        required: true,
        maxlength: 50,
      },
      "decision2-economy": {
        required: true,
        maxlength: 50,
      },
      "decision2-population": {
        required: true,
        maxlength: 50,
      },
    },

    errorClass: "is-invalid",
    successClass: "is-valid",
    debugger: true,
  });

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

  //CREAR CARD
  $("#form").submit(function () {
    console.log("asad");
    if ($("#form").valid()) {
      event.preventDefault();
      // Obtener los valores del formulario
      var typeid = 1;

      var description = $("#description").val();
      var characterId = $("#characterId").val();
      var decision1 = {
        description: $("#decision1-description").val(),
        population: parseInt($("#decision1-population").val()),
        army: parseInt($("#decision1-army").val()),
        economy: parseInt($("#decision1-economy").val()),
        magic: parseInt($("#decision1-magic").val()),
        unlockableCharacter: parseInt(
          $("#decision1-unlockableCharacter").val()
        ),
      };
      if (decision1.unlockableCharacter == "NO") {
        decision1.unlockableCharacter = null;
      }
      var decision2 = {
        description: $("#decision2-description").val(),
        population: parseInt($("#decision2-population").val()),
        army: parseInt($("#decision2-army").val()),
        economy: parseInt($("#decision2-economy").val()),
        magic: parseInt($("#decision2-magic").val()),
        unlockableCharacter: parseInt(
          $("#decision2-unlockableCharacter").val()
        ),
      };
      if (decision2.unlockableCharacter == "NO") {
        decision2.unlockableCharacter = null;
      }
      // Construir el objeto JSON
      const cookieUser = JSON.parse(Cookies.get("claveSeguridad"));
      var jsonData = {
        typeid: parseInt(typeid),
        playerId: cookieUser.id,
        clave: cookieUser.clave,
        description: description,
        characterId: parseInt(characterId),
        decision1: decision1,
        decision2: decision2,
      };

      // jsonData ahora contiene el objeto JSON lleno con los valores del formulario
      console.log(JSON.stringify(jsonData));

      $.ajax({
        url: "https://localhost:7116/api/Card/Post",
        method: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(jsonData),
        headers: {
          Authorization: "Bearer " + cookieUser.token, // Aquí agregas tu token Bearer
        },
        success: function (response) {
          console.log(response);
          Swal.fire("Exito", "Se a creado la carta con exito", "success");
          clearForm();
        },
        error: function (error) {
          console.log(error);
        },
      });
    }
  });
});

function clearForm() {
  $("#description").val("");
  $("#decision1-description").val("");
  $("#decision1-population").val(0);
  $("#decision1-army").val(0);
  $("#decision1-economy").val(0);
  $("#decision1-magic").val(0);
  $("#decision2-description").val("");
  $("#decision2-population").val(0);
  $("#decision2-army").val(0);
  $("#decision2-economy").val(0);
  $("#decision2-magic").val(0);
  $("#decision1-unlockableCharacter").val("NO");
  $("#decision2-unlockableCharacter").val("NO");
}

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
