export class UiCrearUsuario {

    constructor(metodos, api) {
        //
        this.metodos = metodos;
        this.api = api;
        //
        this.userName = document.getElementById('userName');
        this.email = document.getElementById('email');
        this.phone = document.getElementById('phone');
        this.perfil = document.getElementById('idPerfil');
        this.servicioComunaDiv = document.getElementById('servicioComunaDiv');
        this.servicio = document.getElementById('idServicio');
        this.comuna = document.getElementById('idComuna');
        this.passwordHash = document.getElementById('passwordHash');
    }

    checkNumeros() {
        // this.prevenirCopyPasteQuerySelectorOne("#phone");
        this.checkNumeroTelefonoQuerySelectorOne('#phone', '#mensaje-telefono-usuario-solo-numeros', 3000);
    }

    prevenirCopyPasteQuerySelectorOne(selector) {
        const elemento = document.querySelector(selector);
        elemento.addEventListener('paste', (e) => {
            e.preventDefault();
            return false;
        });
    }

    checkNumeroTelefonoQuerySelectorOne(selector, idDivMensaje, tiempoMiliSeg) {
        const elemento = document.querySelector(selector);
        elemento.addEventListener('keypress', (e) => {
            let charCode = (e.which) ? e.which : e.keyCode;
            if ((charCode !== 8 && charCode !== 0) && (charCode < 48 || charCode > 57)) {
                e.preventDefault();
                let msg = 'Solo n&uacute;meros telef&oacute;nicos (Ej: 224123456, 981234567)';
                const divMensaje = document.querySelector(idDivMensaje);
                divMensaje.innerHTML = msg;
                setTimeout(() => {
                    divMensaje.innerHTML = '';
                }, 5000);
                // this.mostrarMensaje((msg), 'alert alert-danger mt-4 mb-5 text-center', idDivMensaje, tiempoMiliSeg);
                return false;
            }
        });
    }

    /*
    mostrarMensaje(mensaje, clases, idDivMensaje, tiempoMiliSeg = 5000) {
        const divMensaje = document.querySelector(idDivMensaje);
        this.limpiarMensaje(divMensaje, clases);
        divMensaje.innerHTML = mensaje;
        divMensaje.classList = clases;
        setTimeout(() => {
            this.limpiarMensaje(divMensaje, clases);
        }, tiempoMiliSeg);
    }

    limpiarMensaje(divMensaje, clases) {
        divMensaje.innerHTML = '';
        divMensaje.classList = '';
    }
  */
}