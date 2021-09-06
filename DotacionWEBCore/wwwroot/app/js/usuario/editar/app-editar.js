export class AppEditarUsuario {

    constructor(metodos, ui, api) {
        //
        this.metodos = metodos;
        this.ui = ui;
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
        //
        this.mensajeErrorRutInvalido = document.getElementById('mensaje-error-rut-invalido');
    }

    initApp() {
        //
    }

    domIsReadyApp() {
        //
        if (document.readyState === 'complete') {
            //
            this.api.fetchObtenerDatosUsuario(this.idUsuarioHidden.value)
                .then(data => {
                    //
                    let result = data.resultado;
                    this.ui.insertarDatosForm(result);
                });
        }
    }

    sendFormSubmit() {
        //
        this.form.addEventListener('submit', (e) => {
            //
            this.mensajeErrorRutInvalido.innerHTML = '';

            let valueRut = this.userName.value.trim();
            let validarRut = this.metodos.checkRut(valueRut);

            if (!validarRut) {
                e.preventDefault();
                this.mensajeErrorRutInvalido.innerHTML = 'El Rut debe ser v&aacute;lido.';
            }
        });
    }

    mostrarCampos() {
        //
        this.perfil.addEventListener('change', (e) => {
            //
            let targetIndex = e.target.value;
            if (this.perfil.selectedIndex === 1 && this.perfil.value === '1' && targetIndex === "1") {  // Servicio de Salud
                //
                this.servicioComunaDiv.style.display = "";
                this.servicio.selectedIndex = 0
                this.comuna.selectedIndex = 0
                this.servicio.disabled = false;
                this.comuna.disabled = false;
                //
                this.getComunaDelServicio();

            } else if (this.perfil.selectedIndex === 2 && this.perfil.value === '2' && targetIndex === "2") {   // Comuna
                //
                this.servicioComunaDiv.style.display = "";
                this.servicio.selectedIndex = 0
                this.comuna.selectedIndex = 0
                this.servicio.disabled = false;
                this.comuna.disabled = false;
                //
                this.getComunas(); 

            } else if (this.perfil.selectedIndex === 3 && this.perfil.value === '3' && targetIndex === "3") {   // MINSAL
                //
                this.servicio.selectedIndex = 0
                this.comuna.selectedIndex = 0
                this.servicio.disabled = true;
                this.comuna.disabled = true;
                this.servicioComunaDiv.style.display = "none";
            }
        });

        if (this.perfil.selectedIndex == 1 && this.perfil.value == "1") {   // Servicio de Salud
            //
            this.getComunaDelServicio();

        } else if (this.perfil.selectedIndex == 2 && this.perfil.value == "2") {    // Comuna
            //
            this.getComunas();
        }
    }

    getComunas() {
        this.servicio.addEventListener('change', e => {
            let idservicio = e.target.value;
            this.api.fetchComunasByIdServicio(idservicio)
                .then(data => {
                    let dataObject = data.resultado;
                    let objetcName = 'Comuna'
                    this.metodos.objectHtmlSelectList(dataObject, this.comuna, objetcName);
                });
        });
    }

    getComunaDelServicio() {
        this.servicio.addEventListener('change', e => {
            let idServicio = e.target.value;
            this.api.fetchComunaByIdDelSarvicio(idServicio)
                .then(data => {
                    let dataObject = data.resultado;
                    let objetcName = 'Comuna'
                    this.metodos.objectHtmlSelectList(dataObject, this.comuna, objetcName);
                });
        });
    }

    enabledOrDisableUser() {
        //
        this.habilitado.addEventListener('change', (e) => {
            //
            if (this.habilitado.checked) {
                //
                this.badgeLockoutEnabled.innerHTML = 'Inhabilitado';
                $("#badge-lockout-enabled").removeClass('badge badge-pill badge-success').addClass('badge badge-pill badge-danger');

            } else {
                //
                this.badgeLockoutEnabled.innerHTML = 'Habilitado';
                $("#badge-lockout-enabled").removeClass('badge badge-pill badge-danger').addClass('badge badge-pill badge-success');
            }
        });
    }

}