$(document).ready(function () {
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
  
  $.ajax({
    url: "https://localhost:7116/api/User/GetUsers",
    method: "GET",
    dataType: "json",
    headers: {
      Authorization: "Bearer " + cookieUser.token,
    },
    success: function (response) {
      console.log(response);
      showData(response);
    },
    error: function (error) {
      console.log(error);
    },
  });

  function showData(data) {
    console.log(data);
    const content = $("#table");
    $("#table ").html("");
    content.append(`<thead class="enchanted" style="font-size: 30px">
       <tr>
          <th scope="col">#</th>
          <th scope="col">Usuario</th>
          <th scope="col">Dias sobrevividos</th>
          </tr>
        </thead>`);

    for (let i = 0; i < data.length; i++) {
      const user = data[i];

      if (user.username == cookieUser.username) {
        var row = `<tbody class="bg-success enchanted" id="localPlayer">
      <th>${i + 1}</th>
      <td>${user.username}</td>
      <td><img src="../../../img/items/day.png" alt="" style="width: 30px;"> ${
        user.maxDays
      }  ${user.maxDays === 1 ? "Día" : "Días"}</td>


     </tbody>
      `;
      } else {
        var row = `<tbody class=" enchanted">
      <th>${i + 1}</th>
      <td>${user.username}</td>
      <td> <img src="../../../img/items/day.png" alt="" style="width: 30px;"> ${
        user.maxDays
      }  ${user.maxDays === 1 ? "Día" : "Días"}</td>


     </tbody>
      `;
      }
      content.append(row);
    }
  }
});
