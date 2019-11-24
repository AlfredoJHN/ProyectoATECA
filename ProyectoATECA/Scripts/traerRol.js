
let roles = document.getElementById('division');
let arregloDatos = [];
let arregloDatos2 = [];
let valor = document.getElementById('correo').value;

const xhr = new XMLHttpRequest();
const xhr2 = new XMLHttpRequest();

xhr.onload = () => {
    let arrResp = JSON.parse(xhr.response);
    let i;
    for (i = 0; i < arrResp.recordset.length; i++) {
        arregloDatos.push(arrResp.recordset[i]);
    }

}

xhr.open('GET', 'http://138.68.55.79/roles');
xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
xhr.send('');

xhr2.onload = () => {
    let arrResp = JSON.parse(xhr2.response);
    let i;
    for (i = 0; i < arrResp.recordset.length; i++) {
        arregloDatos2.push(arrResp.recordset[i]);
    }

}

xhr2.open('GET', 'http://138.68.55.79/usuarios');
xhr2.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
xhr2.send('');

const verificar = () => {
    let valor = document.getElementById('correo').value;
  
    let id = 0;
    for (i = 0; i < arregloDatos2.length; i++) {
        if (valor == arregloDatos2[i].correo) {
            id = arregloDatos2[i].ID_rol;

        }
    }
    for (i = 0; i < arregloDatos.length; i++) {
        if (id == 2) {
            roles.style.display = "flex";
            roles.style.justifyContent = "center";
        } else {
            roles.style.display = "none";
        }
    }
}



