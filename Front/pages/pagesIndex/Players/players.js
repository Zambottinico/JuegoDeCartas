$(document).ready(function () {
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
    content.append(`<thead>
       <tr>
          <th scope="col">#</th>
          <th scope="col">Usuario</th>
          <th scope="col">Dias sobrevividos</th>
          </tr>
        </thead>`);

    for (let i = 0; i < data.length; i++) {
      const user = data[i];
      console.log(data[i]);
      var row = `<tbody>
      <th>${i + 1}</th>
      <td>${user.username}</td>
      <td>${user.maxDays} Dias</td>


     </tbody>
      `;
      content.append(row);
    }
  }
});
