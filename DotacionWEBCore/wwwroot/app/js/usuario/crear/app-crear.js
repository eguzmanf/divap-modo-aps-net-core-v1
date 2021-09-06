export class AppCrearUsuario {

    constructor(metodos, ui, api) {
        //
        this.metodos = metodos;
        this.ui = ui;
        this.api = api;
        //
        this.form = document.getElementById('formCrearUsuario');
        this.userName = document.getElementById('userName');
        this.email = document.getElementById('email');
        this.phone = document.getElementById('phone');
        this.perfil = document.getElementById('idPerfil');
        this.servicioComunaDiv = document.getElementById('servicioComunaDiv');
        this.servicio = document.getElementById('idServicio');
        this.comuna = document.getElementById('idComuna');
        this.passwordHash = document.getElementById('passwordHash');
        this.togglePassword = document.querySelector('#togglePassword');
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

            if ((this.perfil.selectedIndex === 1 && this.perfil.value === '1' && targetIndex === "1") && (this.perfil.selectedIndex != 2 && this.perfil.value != '2' && targetIndex != "2")) {  // Servicio de Salud
                //
                this.servicioComunaDiv.style.display = "";
                this.servicio.selectedIndex = 0
                this.comuna.selectedIndex = 0
                this.servicio.disabled = false;
                this.comuna.disabled = false;
                //
                this.servicio.addEventListener('change', e => {
                    let idServicio = e.target.value;
                    this.api.fetchComunaByIdDelSarvicio(idServicio)
                        .then(data => {
                            //
                            let dataObject = data.resultado;
                            let comuna = this.comuna;
                            comuna.length = 0;
                            $("#idComuna").empty();

                            let defaultOption = document.createElement('option');
                            defaultOption.value = '';
                            defaultOption.text = '---Seleccionar Comuna---';
                            this.comuna.add(defaultOption);
                            this.comuna.selectedIndex = 0;

                            let option;
                            Object.keys(dataObject).forEach(function (key) {
                                option = document.createElement('option');
                                option.text = dataObject[key].text;
                                option.value = dataObject[key].value;
                                comuna.add(option);
                            });
                        });
                });

            } else if ((this.perfil.selectedIndex === 2 && this.perfil.value === '2' && targetIndex === "2") && (this.perfil.selectedIndex != 1 && this.perfil.value != '1' && targetIndex != "1")) {   // Comuna
                //
                this.servicioComunaDiv.style.display = "";
                this.servicio.selectedIndex = 0
                this.comuna.selectedIndex = 0
                this.servicio.disabled = false;
                this.comuna.disabled = false;
                //
                this.servicio.addEventListener('change', e => {
                    let idservicio = e.target.value;
                    this.api.fetchComunasByIdServicio(idservicio)
                        .then(data => {
                            //
                            let dataObject = data.resultado;
                            let comuna = this.comuna;
                            comuna.length = 0;
                            $('#idComuna option').remove();

                            let defaultOption = document.createElement('option');
                            defaultOption.value = '';
                            defaultOption.text = '---Seleccionar Comuna---';
                            this.comuna.add(defaultOption);
                            this.comuna.selectedIndex = 0;

                            let option;
                            Object.keys(dataObject).forEach(function (key) {
                                option = document.createElement('option');
                                option.text = dataObject[key].text;
                                option.value = dataObject[key].value;
                                comuna.add(option);
                            });
                        });
                });

            } else if (this.perfil.selectedIndex === 3 && this.perfil.value === '3' && targetIndex === "3") {   // MINSAL
                //
                this.servicio.selectedIndex = 0
                this.comuna.selectedIndex = 0
                this.servicio.disabled = true;
                this.comuna.disabled = true;
                this.servicioComunaDiv.style.display = "none";
            }
        });
    }

    togglePasswordEye() {
        //
        this.togglePassword.addEventListener("click", e => {
            // toggle the type attribute
            let type = this.passwordHash.getAttribute("type") == "password" ? "text" : "password";
            this.passwordHash.setAttribute("type", type);
            // toggle the eye slash icon
            this.togglePassword.classList.toggle("fa-eye-slash");
        });
    }

}