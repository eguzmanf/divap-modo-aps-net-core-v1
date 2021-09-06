export class UiEditarUsuario {

    constructor(metodos, api) {
        //
        this.metodos = metodos;
        this.api = api;
        //
        this.form = document.getElementById('formEditarUsuario');
        this.idUsuarioHidden = document.getElementById('idUsuario');
        this.userName = document.getElementById('userName');
        this.email = document.getElementById('email');
        this.phone = document.getElementById('phone');
        this.servicioComunaDiv = document.getElementById('servicioComunaDiv');
        this.servicio = document.getElementById('idServicio');
        this.comuna = document.getElementById('idComuna');
        this.perfil = document.getElementById('idPerfil');
        this.habilitado = document.getElementById('habilitado');
        this.badgeLockoutEnabled = document.getElementById('badge-lockout-enabled');
    }

    insertarDatosForm(respuesta) {

        let fechaLockOutEnd = moment(respuesta.lockoutEnd).format('YYYY-MM-DD HH:mm:ss').trim();
        let fechaNow = moment().format('YYYY-MM-DD HH:mm:ss').trim()

        if (respuesta.iD_Perfil == 3) {
            //
            this.servicio.selectedIndex = 0
            this.comuna.selectedIndex = 0
            this.servicio.disabled = true;
            this.comuna.disabled = true;
            this.servicioComunaDiv.style.display = "none";
        }

        if (respuesta.iD_Perfil == 1) {     // Servicio de Salud
            //
            let idServicio = this.servicio.value;
            this.api.fetchComunaByIdDelSarvicio(idServicio)
                .then(data => {
                    let dataObject = data.resultado;
                    let objetcName = 'Comuna'
                    this.metodos.objectHtmlSelectList(dataObject, this.comuna, objetcName);
                    this.comuna.value = respuesta.iD_Comuna_U;
                });

        } else if (respuesta.iD_Perfil == 2) {      // Comuna
            //
            let idServicio = this.servicio.value;
            this.api.fetchComunasByIdServicio(idServicio)
                .then(data => {
                    let dataObject = data.resultado;
                    let objetcName = 'Comuna';
                    this.metodos.objectHtmlSelectList(dataObject, this.comuna, objetcName);
                    this.comuna.value = respuesta.iD_Comuna_U;
                });
        }

        if (respuesta.lockoutEnd != null && fechaLockOutEnd > fechaNow) {
            this.badgeLockoutEnabled.innerHTML = 'Inhabilitado';
            $("#badge-lockout-enabled").removeClass('badge badge-pill badge-success').addClass('badge badge-pill badge-danger');
            this.habilitado.checked = true;
        } else {
            this.badgeLockoutEnabled.innerHTML = 'Habilitado';
            $("#badge-lockout-enabled").removeClass('badge badge-pill badge-danger').addClass('badge badge-pill badge-success');
            this.habilitado.checked = false;
        }
    }

    checkNumeros() {
        // this.metodos.prevenirCopyPasteQuerySelectorAll("#userName, #email, #phone");
        this.metodos.checkNumeroTelefonoQuerySelectorOneText('#phone', '#mensaje-telefono-usuario-solo-numeros', 3000);
    }
}