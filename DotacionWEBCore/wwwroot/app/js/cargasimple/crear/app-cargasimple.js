export class AppCargaSimple {

    constructor(metodos, ui, api) {
        //
        this.metodos = metodos;
        this.ui = ui;
        this.api = api;
        //
        // this.form = document.getElementById("idCargaSimpleForm");
        this.form = document.getElementById("id_Carga_Simple_Form");
        this.validationSummary = document.getElementById("validationSummary");
        this.tieneError = document.getElementById("tieneError");
        //
        this.backErrorEstablecimiento = document.getElementById("backErrorEstablecimiento");
        this.backErrorComuna = document.getElementById("backErrorComuna");
        this.backErrorServicio = document.getElementById("backErrorServicio");
        //
        this.backErrorLey = document.getElementById("backErrorLey");
        this.backErrorTipoContrato = document.getElementById("backErrorTipoContrato");
        this.backErrorCategoria = document.getElementById("backErrorCategoria");
        this.backErrorNivelCarrera = document.getElementById("backErrorNivelCarrera");
        this.backErrorProfesion = document.getElementById("backErrorProfesion");
        this.backErrorEspecialidad = document.getElementById("backErrorEspecialidad");
        this.backErrorCargo = document.getElementById("backErrorCargo");
        this.backErrorFuncionesChofer = document.getElementById("backErrorFuncionesChofer");
        //
        this.idPerfil = document.getElementById("idPerfil");
        // Datos del establecimiento
        //
        this.idServicio = document.getElementById("ID_Servicio");
        this.idComuna = document.getElementById("ID_Comuna");
        this.establecimiento = document.getElementById("CodigoNuevo");
        this.administracion = document.getElementById("Administracion");
        // Datos del funcionario
        //
        this.run = document.getElementById("Rut");
        this.dv = document.getElementById("DV");
        this.apellidoPaterno = document.getElementById("Apellido_Paterno");
        this.apellidoMaterno = document.getElementById("Apellido_Materno");
        this.nombres = document.getElementById("Nombres");
        this.sexo = document.getElementById("Sexo");
        this.fechaNacimiento = document.getElementById("Fecha_Nacimiento");
        this.nacionalidad = document.getElementById("Nacionalidad");
        // Datos del contrato
        //
        this.ley = document.getElementById("Ley");
        this.tipoContrato = document.getElementById("Tipo_contrato");
        this.categoria = document.getElementById("Categoria");
        this.nivelCarrera = document.getElementById("Nivel_Carrera");
        this.profesion = document.getElementById("Profesion");
        this.especialidad = document.getElementById("Especialidad");
        this.cargo = document.getElementById("Cargo");
        this.asignacionChofer = document.getElementById("Funciones_Chofer");
        this.jornada = document.getElementById("Jornada");
        this.aniosServicio = document.getElementById("Anos_Servicio");
        this.fechaIngreso = document.getElementById("Fecha_Ingreso");
        this.bienios = document.getElementById("Bienios");
        this.tipoPrevision = document.getElementById("Tipo_Prevision");
        this.tipoIsapre = document.getElementById("Tipo_Isapre");
        this.sueldoBaseComunal = document.getElementById("SBase_Comunal");
        this.haberes = document.getElementById("Haberes");
    }

    initApp() {
        //
        // console.log(this.validationSummary.innerHTML);
        // console.log(this.tieneError.value);
        // console.log(this.backErrorEstablecimiento.value);
        //
        if (this.tieneError.value == 1) {
            console.log("[ ################### (^) ################### ]")
            console.log("Id Perfil: " + this.idPerfil.value);
            console.log("Ley: " + this.backErrorLey.value);
            console.log("Tipo Contrato: " + this.backErrorTipoContrato.value);
            console.log("Categoria: " + this.backErrorCategoria.value);
            console.log("Nivel Carrera: " + this.backErrorNivelCarrera.value);
            console.log("Profesion: " + this.backErrorProfesion.value);
            console.log("Especialidad: " + this.backErrorEspecialidad.value);
            console.log("Cargo o Función: " + this.backErrorCargo.value);
            console.log("Funciones Chofer: " + this.backErrorFuncionesChofer.value);
            //
            let idLey = this.backErrorLey.value;
            this.api.fetchContratosByIdLey(idLey)
                .then(data => {
                    let dataObject = data.resultado;
                    this.ContratoSelectList(dataObject, this.tipoContrato, idLey);
                    this.tipoContrato.value = this.backErrorTipoContrato.value;

                    this.api.fetchCategoriaByIdLey(idLey)
                        .then(data => {
                            let dataObject = data.resultado;
                            this.CategoriaSelectList(dataObject, this.categoria, idLey);
                            this.categoria.value = this.backErrorCategoria.value;

                            this.api.fetchNivelCarreraByIdLey(idLey)
                                .then(data => {
                                    let dataObject = data.resultado;
                                    this.NivelCarreraSelectList(dataObject, this.nivelCarrera, idLey);
                                    this.nivelCarrera.value = this.backErrorNivelCarrera.value;

                                    let idCategoria = this.backErrorCategoria.value;
                                    this.api.fetchProfesionByIdLey(idLey, idCategoria)
                                        .then(data => {
                                            let dataObject = data.resultado;
                                            this.CategoriaProfesionSelectList(dataObject, this.profesion, idLey);
                                            this.profesion.value = this.backErrorProfesion.value;

                                            let idProfesion = this.backErrorProfesion.value;
                                            this.api.fetchEspecialidadByIdProfesionChange(idProfesion)
                                                .then(data => {
                                                    let dataObject = data.resultado;
                                                    this.ProfesionEspecialidadSelectList(dataObject, this.especialidad, idProfesion);
                                                    this.especialidad.value = this.backErrorEspecialidad.value;

                                                    let idCargo = this.backErrorCargo.value;
                                                    this.api.fetchAsignacionChoferAmbulanciaByIdCargoChange(idCargo)
                                                        .then(data => {
                                                            let dataObject = data.resultado;
                                                            this.CargoAsignacionChoferAmbulanciaSelectList(dataObject, this.asignacionChofer, idCargo);
                                                            this.asignacionChofer.value = this.backErrorFuncionesChofer.value;
                                                        });
                                                });
                                        });
                                });
                        });
                });

        } else if (this.tieneError.value == 0) {
            //
            this.resetForm(this.form);

            if (this.idPerfil.value == '3') {
                this.idServicio.value = "";
            }

            this.administracion.value = "";
            this.dv.value = "";
            this.sexo.value = "";
            this.nacionalidad.value = "";
            this.ley.value = "";
            this.categoria.value = "";
            this.cargo.value = "";
            this.bienios.value = "";
            this.tipoPrevision.value = "";
            this.tipoIsapre.value = "";
        }
    }

    domIsReadyApp() {
        //
        // console.log(this.validationSummary.innerHTML);
        if (document.readyState === 'complete') {
            //
            console.log("[ ################### (^) ################### ]")
            console.log("Id Perfil: " + this.idPerfil.value);
            console.log(this.tieneError.value == 1 ? "Hay Errores: Si" : "Hay Errores: No");
            console.log("Id Establecimiento: " + this.backErrorEstablecimiento.value);
            console.log("Id Comuna: " + this.backErrorComuna.value);
            console.log("Id Servicio: " + this.backErrorServicio.value);
            console.log("[ ################### (^) ################### ]")
            
            if (this.idPerfil.value != 3) {
                //
                let idServicio = this.idServicio.value;
                let idPerfil = this.idPerfil.value;
                this.api.fetchComunaDelServicioSalud(idServicio)
                    .then(data => {
                        let idComunaIndex = data.resultado[0].value;
                        this.api.fetchComunaByIdDelServicio(idServicio)
                            .then(data => {
                                let dataObject = data.resultado;
                                this.ComunaSelectList(dataObject, this.idComuna, idPerfil, idComunaIndex, this.backErrorComuna);
                                //
                                let idComuna = this.idComuna.value;
                                this.api.fetchEstablecimientoByIdComuna(idComuna)
                                    .then(data => {
                                        let dataObject = data.resultado;
                                        this.EstablecimientoSelectList(dataObject, this.establecimiento, this.backErrorEstablecimiento);
                                    });
                            });
                    });

            } else if(this.idPerfil.value == 3) {
                //
                if (this.tieneError.value == 1) {
                    //
                    let idServicio = this.idServicio.value;
                    let idComunaIndex = null;
                    this.api.fetchComunaByIdDelServicio(idServicio)
                        .then(data => {
                            let dataObject = data.resultado;
                            this.ComunaSelectList(dataObject, this.idComuna, idPerfil, idComunaIndex, this.backErrorComuna);
                            //
                            let idComuna = this.idComuna.value;
                            this.api.fetchEstablecimientoByIdComuna(idComuna)
                                .then(data => {
                                    let dataObject = data.resultado;
                                    this.EstablecimientoSelectList(dataObject, this.establecimiento, this.backErrorEstablecimiento);
                                });

                        });
                }
            }
        }
    }

    getComunaServicioChange() {
        //
        this.idServicio.addEventListener('change', e => {
            //
            let idServicio = e.target.value;
            this.api.fetchComunaByIdServicioChange(idServicio)
                .then(data => {
                    let dataObject = data.resultado;
                    this.ComunaServicioChangeSelectList(dataObject, this.idComuna);
                });
        });
    }

    getEstablecimiento() {
        //
        this.idComuna.addEventListener('change', e => {
            //
            let idComuna = e.target.value;
            this.api.fetchEstablecimientoByIdComuna(idComuna)
                .then(data => {
                    let dataObject = data.resultado;
                    this.backErrorEstablecimiento.value = "";
                    this.EstablecimientoSelectList(dataObject, this.establecimiento, this.backErrorEstablecimiento);
                });
        });
    }

    getContrato() {
        //
        this.ley.addEventListener('change', e => {
            let idLey = e.target.value;
            this.api.fetchContratosByIdLey(idLey)
                .then(data => {
                    let dataObject = data.resultado;
                    this.ContratoSelectList(dataObject, this.tipoContrato, idLey);

                    this.api.fetchCategoriaByIdLey(idLey)
                        .then(data => {
                            let dataObject = data.resultado;
                            this.CategoriaSelectList(dataObject, this.categoria, idLey);

                            this.api.fetchNivelCarreraByIdLey(idLey)
                                .then(data => {
                                    let dataObject = data.resultado;
                                    this.NivelCarreraSelectList(dataObject, this.nivelCarrera, idLey);

                                    let idCategoria = this.categoria.value;
                                    this.api.fetchProfesionByIdLey(idLey, idCategoria)
                                        .then(data => {
                                            let dataObject = data.resultado;
                                            this.CategoriaProfesionSelectList(dataObject, this.profesion, idLey);
                                        });
                                })
                        });
                });
        });
    }

    getCategoria() {
        //
        this.categoria.addEventListener('change', e => {
            //
            let idCategoria = e.target.value
            let idLey = this.ley.value;
            //
            this.api.fetchProfesionByIdCategoriaChange(idCategoria, idLey)
                .then(data => {
                    let dataObject = data.resultado;
                    this.CategoriaProfesionSelectList(dataObject, this.profesion, idLey);
                });
        });

    }

    getEspecialidad() {
        //
        this.profesion.addEventListener('change', e => {
            //
            let idProfesion = e.target.value;
            //
            this.api.fetchEspecialidadByIdProfesionChange(idProfesion)
                .then(data => {
                    let dataObject = data.resultado;
                    this.ProfesionEspecialidadSelectList(dataObject, this.especialidad, idProfesion);
                });
        });

    }

    getAsignacionChoferAmbulancia() {
        //
        this.cargo.addEventListener('change', e => {
            //
            let idCargo = e.target.value;
            //
            this.api.fetchAsignacionChoferAmbulanciaByIdCargoChange(idCargo)
                .then(data => {
                    let dataObject = data.resultado;
                    this.CargoAsignacionChoferAmbulanciaSelectList(dataObject, this.asignacionChofer, idCargo);
                });

        });
    }

    ContratoSelectList(dataObject, objectHtmlSelect, idLey) {
        //
        let objectHS = objectHtmlSelect;
        objectHS.length = 0;

        if (idLey == "Ley 19.378") {
            let defaultOption = document.createElement('option');
            defaultOption.value = '';
            defaultOption.text = 'Seleccionar...';
            objectHS.add(defaultOption);
            objectHS.selectedIndex = '';
        }
        
        let option;
        Object.keys(dataObject).forEach(function (key) {
            option = document.createElement('option');
            option.text = dataObject[key].text;
            option.value = dataObject[key].value;
            objectHS.add(option);
        });
    }

    CategoriaSelectList(dataObject, objectHtmlSelect, idLey) {
        //
        let objectHS = objectHtmlSelect;
        objectHS.length = 0;

        if (idLey == "Ley 19.378") {
            let defaultOption = document.createElement('option');
            defaultOption.value = '';
            defaultOption.text = 'Seleccionar...';
            objectHS.add(defaultOption);
            objectHS.selectedIndex = '';
        }

        let option;
        Object.keys(dataObject).forEach(function (key) {
            option = document.createElement('option');
            option.text = dataObject[key].text;
            option.value = dataObject[key].value;
            objectHS.add(option);
        });
    }

    NivelCarreraSelectList(dataObject, objectHtmlSelect, idLey) {
        //
        let objectHS = objectHtmlSelect;
        objectHS.length = 0;

        if (idLey == "Ley 19.378") {
            let defaultOption = document.createElement('option');
            defaultOption.value = '';
            defaultOption.text = 'Seleccionar...';
            objectHS.add(defaultOption);
            objectHS.selectedIndex = '';
        }

        let option;
        Object.keys(dataObject).forEach(function (key) {
            option = document.createElement('option');
            if (dataObject[key].value == 0) {
                option.text = "N/A";
                option.value = "N/A";
            } else {
                //
                option.text = dataObject[key].text;
                option.value = dataObject[key].value;
            }
            objectHS.add(option);
        });
    }

    CategoriaProfesionSelectList(dataObject, objectHtmlSelect, idLey) {
        //
        let objectHS = objectHtmlSelect;
        objectHS.length = 0;

        let defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.text = 'Seleccionar...';
        objectHS.add(defaultOption);
        objectHS.selectedIndex = '';

        let option;
        Object.keys(dataObject).forEach(function (key) {
            option = document.createElement('option');
            option.text = dataObject[key].text;
            option.value = dataObject[key].value;
            objectHS.add(option);
        });
    }

    ProfesionEspecialidadSelectList(dataObject, objectHtmlSelect, idProfesion) {
        //
        let objectHS = objectHtmlSelect;
        objectHS.length = 0;

        if (idProfesion == "MEDICO") {
            let defaultOption = document.createElement('option');
            defaultOption.value = '';
            defaultOption.text = 'Seleccionar...';
            objectHS.add(defaultOption);
            objectHS.selectedIndex = '';
        }

        let option;
        Object.keys(dataObject).forEach(function (key) {
            option = document.createElement('option');
            option.text = dataObject[key].text;
            option.value = dataObject[key].value;
            objectHS.add(option);
        });
    }

    CargoAsignacionChoferAmbulanciaSelectList(dataObject, objectHtmlSelect, idCargo) {
        //
        let objectHS = objectHtmlSelect;
        objectHS.length = 0;

        if (idCargo == "CHOFER") {
            let defaultOption = document.createElement('option');
            defaultOption.value = '';
            defaultOption.text = 'Seleccionar...';
            objectHS.add(defaultOption);
            objectHS.selectedIndex = '';
        }

        let option;
        Object.keys(dataObject).forEach(function (key) {
            option = document.createElement('option');
            option.text = dataObject[key].text;
            option.value = dataObject[key].value;
            objectHS.add(option);
        });
    }

    ComunaSelectList(dataObject, objectHtmlSelect, idPerfil, idComunaIndex, backError) {
        //
        let objectHS = objectHtmlSelect;
        objectHS.length = 0;

        if (idPerfil != "2") {
            let defaultOption = document.createElement('option');
            defaultOption.value = '';
            defaultOption.text = 'Seleccionar...';
            objectHS.add(defaultOption);
            objectHS.selectedIndex = '';
        }

        let option;
        Object.keys(dataObject).forEach(function (key) {
            option = document.createElement('option');
            option.text = dataObject[key].text;
            option.value = dataObject[key].value;
            if (option.value == idComunaIndex) {
                option.selected = true;
                option.selectedIndex = idComunaIndex;
            }
            objectHS.add(option);
        });

        if (backError.value != "" && backError.value.length > 0) {
            //
            objectHS.value = backError.value;
        }
    }

    ComunaServicioChangeSelectList(dataObject, objectHtmlSelect) {
        //
        let objectHS = objectHtmlSelect;
        objectHS.length = 0;

        let defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.text = 'Seleccionar...';
        objectHS.add(defaultOption);
        objectHS.selectedIndex = '';

        let option;
        Object.keys(dataObject).forEach(function (key) {
            option = document.createElement('option');
            option.text = dataObject[key].text;
            option.value = dataObject[key].value;
            objectHS.add(option);
        });
    }

    EstablecimientoSelectList(dataObject, objectHtmlSelect, backError) {
        //
        let objectHS = objectHtmlSelect;
        objectHS.length = 0;

        let defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.text = 'Seleccionar...';
        objectHS.add(defaultOption);
        objectHS.selectedIndex = '';

        let option;
        Object.keys(dataObject).forEach(function (key) {
            option = document.createElement('option');
            option.text = dataObject[key].text;
            option.value = dataObject[key].value;
            objectHS.add(option);
        });

        if (backError.value != "" && backError.value.length > 0) {
            //
            objectHS.value = backError.value;
        }
    }

    baseComunalFormat() {
        //
        this.sueldoBaseComunal.addEventListener('input', (e) => {
            //
            this.metodos.formatSeparadorMiles(this.sueldoBaseComunal);
        });
    }

    totalHaberesFormat() {
        //
        this.haberes.addEventListener('input', (e) => {
            //
            this.metodos.formatSeparadorMiles(this.haberes);
        });
    }

    checkNumerosEnterosRun() {
        this.run.addEventListener('keypress', (e) => {
            let charCode = (e.which) ? e.which : e.keyCode;
            if ((charCode !== 8 && charCode !== 0) && (charCode < 48 || charCode > 57)) {
                e.preventDefault();
                return false;
            }
        });
    }

    onlyNumCopyPasteRun() {
        this.run.addEventListener('input', (e) => {
            //
            let value = e.target.value;
            let numbers = value.replace(/[^0-9]/g, "");
            e.target.value = numbers;
        });
    }

    onlyNumCopyPasteJornada() {
        this.jornada.addEventListener('input', (e) => {
            //
            let value = e.target.value;
            let numbers = value.replace(/[^0-9]/g, "");
            e.target.value = numbers;
        });
    }

    onlyNumCopyPasteAniosServicio() {
        this.aniosServicio.addEventListener('input', (e) => {
            //
            let value = e.target.value;
            let numbers = value.replace(/[^0-9]/g, "");
            e.target.value = numbers;
        });
    }

    resetForm(form) {
        //
        form.reset();
    }
}
