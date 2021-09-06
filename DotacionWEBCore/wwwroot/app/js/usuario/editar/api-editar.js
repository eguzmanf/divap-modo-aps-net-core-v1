export class ApiEditarUsuario {

    constructor(urlGetComuna, urlGetJsonUsuario, urlGetComunaDelServicio) {
        this.urlGetComuna = urlGetComuna;
        this.urlGetJsonUsuario = urlGetJsonUsuario;
        this.urlGetComunaDelServicio = urlGetComunaDelServicio;
        this.perfil = document.getElementById('idPerfil');
    }

    async fetchObtenerDatosUsuario(idUsuario) {
        let enviarDatos = { idUsuario: idUsuario };

        const obtenerDatos = await fetch(this.urlGetJsonUsuario, {
            method: 'POST',
            body: JSON.stringify(enviarDatos),
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Content-Type': 'application/json'
            }
        });

        const resultado = await obtenerDatos.json();

        return {
            resultado
        };
    }

    async fetchComunasByIdServicio(idservicio) {
        //
        if (this.perfil.value == "2") {
            //
            let enviarDatos = { idservicio: idservicio, perfilCom: this.perfil.value };

            const obtenerDatos = await fetch(this.urlGetComuna, {
                method: 'POST',
                body: JSON.stringify(enviarDatos),
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    'Content-Type': 'application/json'
                }
            });

            const resultado = await obtenerDatos.json();

            return {
                resultado
            };
        } else {
            return;
        }
    }

    async fetchComunaByIdDelSarvicio(idServicio) {
        //
        if (this.perfil.value == "1") {
            //
            let enviarDatos = { idServicio: idServicio, perfilComServ: this.perfil.value };

            const obtenerDatos = await fetch(this.urlGetComunaDelServicio, {
                method: 'POST',
                body: JSON.stringify(enviarDatos),
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    'Content-Type': 'application/json'
                }
            });

            const resultado = await obtenerDatos.json();

            return {
                resultado
            };
        } else {
            return;
        }
        
    }
}