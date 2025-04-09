export class AppEditaRegistros {

    constructor(metodos, ui, api) {
        //
        this.metodos = metodos;
        this.ui = ui;
        this.api = api;
        //
        this.aspValidationSummaryAll = document.getElementById("aspValidationSummaryAll");
        //
        this.idRegistro = document.getElementById("idRegistro");
        this.idID = document.getElementById("idID");
        //
        this.idPerfil = document.getElementById("idPerfil");
        this.perfilName = document.getElementById("perfilName");
        //
        // Datos del establecimiento
        //
        this.idServicio = document.getElementById("ID_Servicio");
        this.idComuna = document.getElementById("ID_Comuna");
        this.idEstablecimiento = document.getElementById("ID_Establecimiento");
        this.administracion = document.getElementById("Administracion");
        // Datos del funcionario
        //
        this.rut = document.getElementById("Rut");
        this.dv = document.getElementById("DV");
        this.apellidoPaterno = document.getElementById("Apellido_Paterno");
        this.apellidoMaterno = document.getElementById("Apellido_Materno");
        this.nombres = document.getElementById("Nombre");
        this.sexo = document.getElementById("Sexo");
        this.fechaNacimiento = document.getElementById("fechaNacimientoFuncionarioId");
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
        this.fechaIngreso = document.getElementById("fechaIngresoFuncionarioId");
        this.bienios = document.getElementById("Bienios");
        this.tipoPrevision = document.getElementById("Tipo_Prevision");
        this.tipoIsapre = document.getElementById("Tipo_Isapre");
        this.sueldoBaseComunal = document.getElementById("SBase_Comunal");
        this.haberes = document.getElementById("Haberes");
        this.categoriaInit = this.categoria.value;
        //
        this.btnEnviarDatosComputadora = document.getElementById("btnEnviarDatosComputadora");
    }

    initApp() {
        //
    }

    domIsReadyApp() {
        //
        if (document.readyState === 'complete') {
            //
            this.metodos.formatSeparadorMiles(this.sueldoBaseComunal);
            this.metodos.formatSeparadorMiles(this.haberes);
        }
    }

    aspValidationSummaryAllCloseModal() {
        //
        this.btnEnviarDatosComputadora.addEventListener('click', e => {
            //

            let stringFull = this.aspValidationSummaryAll.innerHTML;
            let subString = "style=";

            if (stringFull.includes(subString)) {
                //
                console.log(this.aspValidationSummaryAll.innerHTML);
                console.log(stringFull.includes(subString));
                $('#exampleModal').modal('hide')
            } else {
                //
                console.log(this.aspValidationSummaryAll.innerHTML);
                console.log(stringFull.includes(subString));
                $('#exampleModal').modal('hide');
            }
        });

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

    getEstablecimientoComunaChange() {
        //
        this.idComuna.addEventListener('change', e => {
            //
            let idComuna = e.target.value;
            this.api.fetchEstablecimientoByIdComunaChange(idComuna)
                .then(data => {
                    let dataObject = data.resultado;
                    this.EstablecimientoComunaChangeSelectList(dataObject, this.idEstablecimiento)
                        .then(data => {
                            let dataObject = data.resultado;
                            this.ContratoSelectList(dataObject, this.tipoContrato, idLey);
                        })
                });
        });
    }

    getContratoCategoriaNivelCarreraProfesionByLeyCange() {
        //
        let idProfesion = this.profesion.value;

        this.ley.addEventListener('change', e => {
            //
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

                                    let idCategoria = this.categoriaInit;
                                    this.api.fetchProfesionByIdLey(idLey, idCategoria)
                                        .then(data => {
                                            let dataObject = data.resultado;
                                            this.CategoriaProfesionSelectList(dataObject, this.profesion, idLey);

                                            /*
                                            if (idLey != "Ley 19.378") {
                                                this.profesion.value = idProfesion;
                                            } else if (idLey == "Ley 19.664") {
                                                this.profesion.value = 'MEDICO';
                                            } else {
                                                this.profesion.remove(1);
                                            }
                                            */
                                             
                                        });
                                });
                        });

                });

        })

    }

    getCategoriaChange() {
        //
        this.categoria.addEventListener('change', e => {
            //
            let idCategoria = e.target.value
            console.log("idCategoria: " + idCategoria);

            let idLey = this.ley.value;
            console.log("idLey: " + this.ley.value);
            //
            this.api.fetchProfesionByIdCategoriaChange(idCategoria, idLey)
                .then(data => {
                    let dataObject = data.resultado;
                    this.CategoriaProfesionSelectList(dataObject, this.profesion, idLey);
                });
        });

    }

    getEspecialidadChange() {
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

    getAsignacionChoferAmbulanciaChange() {
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

    /* ######################################################################################################################### */

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

    EstablecimientoComunaChangeSelectList(dataObject, objectHtmlSelect) {
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

    /*
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
    }*/

    CategoriaProfesionSelectList(dataObject, objectHtmlSelect, idLey) {
        //
        let objectHS = objectHtmlSelect;
        objectHS.length = 0;

        console.log("CategoriaProfesionSelectList Ley: " + idLey);
        console.log("this.ley.value: " + this.ley.value);
        console.log("this.profesion.value: " + this.profesion.value);
        console.log("this.profesion.text: " + this.profesion.text);
        console.log("this.profesion.innerHTML: " + this.profesion.innerHTML);
        console.log("this.profesion.innerText: " + this.profesion.innerText);

        if (idLey == "Ley 19.664") {
            let defaultOption = document.createElement('option');
            defaultOption.value = 'MEDICO';
            defaultOption.text = 'MEDICO';
            objectHS.add(defaultOption);
            objectHS.selectedIndex = '';

            let idProfesionMedico = 'MEDICO'
            this.api.fetchEspecialidadByIdProfesionChange(idProfesionMedico)
                .then(data => {
                    let dataObject = data.resultado;
                    this.ProfesionEspecialidadSelectList(dataObject, this.especialidad, idProfesionMedico);
                });

        } else {
            //
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

    baseComunalFormat() {
        //
        this.sueldoBaseComunal.addEventListener('change', e => {
            //
            this.metodos.formatSeparadorMiles(this.sueldoBaseComunal);
        });
    }

    totalHaberesFormat() {
        //
        this.haberes.addEventListener('change', e => {
            //
            this.metodos.formatSeparadorMiles(this.haberes);
        });
    }

}