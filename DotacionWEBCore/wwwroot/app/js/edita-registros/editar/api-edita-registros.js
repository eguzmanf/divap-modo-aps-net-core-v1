export class ApiEditaRegistros {

    constructor(urlGetComunasByServicioChange, urlGetEstablecimientosByComunasChange, urlGetContrato, urlGetCategorias, urlNivelCarrera, urlGetProfesion, urlGetTipoCategoriaChange, urlGetTipoEspecialidadProfesionChange, urlGetAsignacionChoferAmbulanciaChange) {
        this.urlGetComunasByServicioChange = urlGetComunasByServicioChange;
        this.urlGetEstablecimientosByComunasChange = urlGetEstablecimientosByComunasChange;
        this.urlGetContrato = urlGetContrato;
        this.urlGetCategorias = urlGetCategorias;
        this.urlNivelCarrera = urlNivelCarrera;
        this.urlGetProfesion = urlGetProfesion;
        this.urlGetTipoCategoriaChange = urlGetTipoCategoriaChange;
        this.urlGetTipoEspecialidadProfesionChange = urlGetTipoEspecialidadProfesionChange;
        this.urlGetAsignacionChoferAmbulanciaChange = urlGetAsignacionChoferAmbulanciaChange;
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

    async fetchEstablecimientoByIdComunaChange(idComuna) {
        let enviarDatos = { idComuna: idComuna };

        const obtenerDatos = await fetch(this.urlGetEstablecimientosByComunasChange, {
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

        const obtenerDatos = await fetch(this.urlGetAsignacionChoferAmbulanciaChange, {
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