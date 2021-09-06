export class ApiCargaSimple {

    constructor(urlGetComunasByServicioChange, urlGetComunasDelServicio, urlGetComunas, urlGetEstablecimiento, urlGetContrato, urlGetCategorias, urlNivelCarrera, urlGetProfesion, urlGetTipoCategoriaChange, urlGetTipoEspecialidadProfesionChange, urlGetAsignacionChoferAmbulancia) {
        this.urlGetComunasByServicioChange = urlGetComunasByServicioChange;
        this.urlGetComunasDelServicio = urlGetComunasDelServicio;
        this.urlGetComunas = urlGetComunas;
        this.urlGetEstablecimiento = urlGetEstablecimiento;
        this.urlGetContrato = urlGetContrato;
        this.urlGetCategorias = urlGetCategorias;
        this.urlNivelCarrera = urlNivelCarrera;
        this.urlGetProfesion = urlGetProfesion;
        this.urlGetTipoCategoriaChange = urlGetTipoCategoriaChange;
        this.urlGetTipoEspecialidadProfesionChange = urlGetTipoEspecialidadProfesionChange;
        this.urlGetAsignacionChoferAmbulancia = urlGetAsignacionChoferAmbulancia;
    }

    async fetchComunaDelServicioSalud(idServicio) {
        let enviarDatos = { idServicio: idServicio };

        const obtenerDatos = await fetch(this.urlGetComunasDelServicio, {
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

    async fetchComunaByIdDelServicio(idServicio) {
        let enviarDatos = { idServicio: idServicio };

        const obtenerDatos = await fetch(this.urlGetComunas, {
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

    async fetchComunaByIdServicioChange(idServicio) {
        let enviarDatos = { idServicio: idServicio };

        const obtenerDatos = await fetch(this.urlGetComunasByServicioChange, {
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

    async fetchEstablecimientoByIdComuna(idComuna) {
        let enviarDatos = { idComuna: idComuna };

        const obtenerDatos = await fetch(this.urlGetEstablecimiento, {
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

    async fetchContratosByIdLey(idLey) {
        let enviarDatos = { Ley: idLey };

        const obtenerDatos = await fetch(this.urlGetContrato, {
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

    async fetchCategoriaByIdLey(idLey) {
        let enviarDatos = { Ley: idLey };

        const obtenerDatos = await fetch(this.urlGetCategorias, {
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

    async fetchNivelCarreraByIdLey(idLey) {
        let enviarDatos = { Ley: idLey };

        const obtenerDatos = await fetch(this.urlNivelCarrera, {
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

    async fetchProfesionByIdLey(idLey, idCategoria) {
        let enviarDatos = { Ley: idLey, idCategoria: idCategoria };

        const obtenerDatos = await fetch(this.urlGetProfesion, {
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

    async fetchProfesionByIdCategoriaChange(idCategoria, idLey) {
        let enviarDatos = { Ley: idLey, idCategoria: idCategoria };

        const obtenerDatos = await fetch(this.urlGetTipoCategoriaChange, {
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

    async fetchEspecialidadByIdProfesionChange(idProfesion) {
        let enviarDatos = { idProfesion: idProfesion };

        const obtenerDatos = await fetch(this.urlGetTipoEspecialidadProfesionChange, {
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

    async fetchAsignacionChoferAmbulanciaByIdCargoChange(idCargo) {
        let enviarDatos = { idCargo: idCargo };

        const obtenerDatos = await fetch(this.urlGetAsignacionChoferAmbulancia, {
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

}