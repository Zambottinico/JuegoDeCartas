$(document).ready(function () {
  //Cambiar links cartas/acerca de
  const valor1 = JSON.parse(Cookies.get("claveSeguridad"));
  if (valor1.rol === "Admin") {
    $("#NavCards").attr("href", "../Cards/cards.html");
    $("#NavCards").text("Cartas");
  }

  $.ajax({
    url: "https://localhost:7116/api/User/GetUsers",
    method: "GET",
    dataType: "json",
    success: function (response) {
      console.log(response);
      showData(response);
    },
    error: function (error) {
      console.log(error);
    },
  });

  function showData(data) {
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

      if (user.username == Cookies.get("username")) {
        var row = `<tbody class="bg-success enchanted">
      <th>${i + 1}</th>
      <td>${user.username}</td>
      <td>${user.maxDays} Dias</td>


     </tbody>
      `;
      } else {
        var row = `<tbody class=" enchanted">
      <th>${i + 1}</th>
      <td>${user.username}</td>
      <td>${user.maxDays} Dias</td>


     </tbody>
      `;
      }
      content.append(row);
    }
  }
});
