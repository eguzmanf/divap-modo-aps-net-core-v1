export class FuncionesGenerales {

    constructor() {

    }

    // ######################################### Función para prevenir el copy paste en inputs ########################
    prevenirCopyPasteQuerySelectorAll(selectores) {
        const elementos = document.querySelectorAll(selectores);
        elementos.forEach(elemento => {
            elemento.addEventListener('paste', (e) => {
                e.preventDefault();
                return false;
            });
        });
    }

    prevenirCopyPasteQuerySelectorOne(selector) {
        const elemento = document.querySelector(selector);
        elemento.addEventListener('paste', (e) => {
            e.preventDefault();
            return false;
        });
    }

    // ######################################### Función para check si es un numero(s)  ###############################
    isNumeric(num) {
        return !isNaN(num);
    }

    isNumericV2(num) {
        return Number.isFinite(num);
    }

    isNumericRegexPositiveAndNegative(value) {
        return /^-?\d+$/.test(value);
    }

    isNumericRegexOnlyPositive(value) {
        return /^\d+$/.test(value);
    }

    isNumber(num) {
        if (typeof num === 'number') {
            return true;
        } else {
            return false;
        }
    }

    isNumberRegex(num) {
        if(num.match(/^-?\d+$/)) {
            //valid integer (positive or negative)
            return true;
        }else if(num.match(/^\d+\.\d+$/)){
            //valid float
            return true;
        }else{
            //not valid number
            return false;
        }
    }

    isInteger(num) {
        return Number.isInteger();
    }

    // ######################################### Función para check si hay numero(s) en un string #####################
    hasNumber(myString) {
        return /\d/.test(myString);
    }

    // ######################################### Función para check números enteros y float ###########################
    checkNumerosEnterosQuerySelectorAll(selectores, idDivMensaje, tiempoMiliSeg) {
        const elementos = document.querySelectorAll(selectores);
        elementos.forEach(elemento => {
            elemento.addEventListener('keypress', (e) => {
                let charCode = (e.which) ? e.which : e.keyCode;
                if ((charCode !== 8 && charCode !== 0) && (charCode < 48 || charCode > 57)) {
                    e.preventDefault();
                    this.mostrarMensaje('Solo números enteros (Ejs: 1, 3, 18, 85, 255)', 'alert alert-danger mt-4 mb-5 text-center', idDivMensaje, tiempoMiliSeg);
                    return false;
                }
            });
        });
    }

    checkNumerosEnterosQuerySelectorOne(selector, idDivMensaje, tiempoMiliSeg) {
        const elemento = document.querySelector(selector);
        elemento.addEventListener('keypress', (e) => {
            let charCode = (e.which) ? e.which : e.keyCode;
            if ((charCode !== 8 && charCode !== 0) && (charCode < 48 || charCode > 57)) {
                e.preventDefault();
                this.mostrarMensaje('Solo números enteros (Ejs: 1, 3, 18, 85, 255)', 'alert alert-danger mt-4 mb-5 text-center', idDivMensaje, tiempoMiliSeg);
                return false;
            }
        });
    }

    checkNumeroTelefonoQuerySelectorOne(selector, idDivMensaje, tiempoMiliSeg) {
        const elemento = document.querySelector(selector);
        elemento.addEventListener('keypress', (e) => {
            let charCode = (e.which) ? e.which : e.keyCode;
            if ((charCode !== 8 && charCode !== 0) && (charCode < 48 || charCode > 57)) {
                e.preventDefault();
                this.mostrarMensaje('Solo números telefónicos (Ej: 224123456, 981234567)', 'alert alert-danger mt-4 mb-5 text-center', idDivMensaje, tiempoMiliSeg);
                return false;
            }
        });
    }

    checkNumeroTelefonoQuerySelectorOneText(selector, idDivMensaje, tiempoMiliSeg) {
        const elemento = document.querySelector(selector);
        elemento.addEventListener('keypress', (e) => {
            let charCode = (e.which) ? e.which : e.keyCode;
            if ((charCode !== 8 && charCode !== 0) && (charCode < 48 || charCode > 57)) {
                e.preventDefault();
                let msg = 'Solo Tel&eacute;fono (Ej: 224123456 o 981234567)';
                const divMensaje = document.querySelector(idDivMensaje);
                divMensaje.innerHTML = msg;
                setTimeout(() => {
                    divMensaje.innerHTML = '';
                }, 5000);
                return false;
            }
        });
    }

    checkNumerosFloatQuerySelectorAll(selectores, idDivMensaje, tiempoMiliSeg) {
        const elementos = document.querySelectorAll(selectores);
        elementos.forEach(elemento => {
            elemento.addEventListener('keypress', (e) => {
                let charCode = (e.which) ? e.which : e.keyCode;
                if ((charCode != 46 || elemento.value.indexOf('.') != -1) && (charCode < 48 || charCode > 57)) {
                    e.preventDefault();
                    this.mostrarMensaje('Solo números enteros (Ejs: 1, 3, 18, 85, 255) o decimales con punto (Ejs: 1.78, 85.7, 120.09)', 'alert alert-danger mt-4 mb-5 text-center', idDivMensaje, tiempoMiliSeg);
                    return false;
                }
            });
        });
    }

    checkNumerosFloatQuerySelectorOne(selector, idDivMensaje, tiempoMiliSeg) {
        const elemento = document.querySelector(selector);
        elemento.addEventListener('keypress', (e) => {
            let charCode = (e.which) ? e.which : e.keyCode;
            if ((charCode != 46 || elemento.value.indexOf('.') != -1) && (charCode < 48 || charCode > 57)) {
                e.preventDefault();
                this.mostrarMensaje('Solo números enteros (Ejs: 1, 3, 18, 85, 255) o decimales con punto (Ejs: 1.78, 85.7, 120.09)', 'alert alert-danger mt-4 mb-5 text-center', idDivMensaje, tiempoMiliSeg);
                return false;
            }
        });
    }

    checkNumerosSlashQuerySelectorOne(selector, idDivMensaje, tiempoMiliSeg) {
        const elemento = document.querySelector(selector);
        elemento.addEventListener('keypress', (e) => {
            let charCode = (e.which) ? e.which : e.keyCode;
            if ((charCode != 47 || elemento.value.indexOf('/') != -1) && (charCode < 48 || charCode > 57)) {
                e.preventDefault();
                this.mostrarMensaje('Solo números enteros (Ejs: 1, 3, 18, 85, 255) o números con slash (Ejs: 16/2)', 'alert alert-danger mt-4 mb-5 text-center', idDivMensaje, tiempoMiliSeg);
                return false;
            }
        });
    }

    // ######################################### Función fadeout y fadeIn ############################################
    // Native fadeOut ==> not working
    fadeOut(el, ms) {
        if(ms) {
            el.style.opacity = '1';
            el.style.transition = `opacity ${ms} ms`;
            el.addEventListener('transitionend', function(event) {
              el.style.display = 'none';
            }, false);
        }
        el.style.opacity = '0';
    }

    // Native fadeIn
    fadeIn(elem, ms) {
        elem.style.opacity = 0;
        if(ms) {
          let opacity = 0;
          const timer = setInterval(function() {
                opacity += 50 / ms;
                if (opacity >= 1) {
                  clearInterval(timer);
                  opacity = 1;
                }
                elem.style.opacity = opacity;
          }, 50);
        } else {
          elem.style.opacity = 1;
        }
    }

    fadeOut2(elem, ms) {
        elem.style.margin = "0 0 10px 0";
        elem.style.opacity = 1;
        // elem.style.WebkitTransition = `all ${ms}ms`;    // Code for Safari 3.1 to 6.0
        elem.style.transition = `all ${ms}ms`;          // Standard syntax
        setTimeout(() => {
            elem.style.opacity = 0;
            elem.style.margin = "0 0 0 0";
            elem.style.marginBottom = "0px";
        }, ms);
    }

    // ######################################### Función para mostrar mensajes ########################################
    mostrarMensaje(mensaje, clases, idDivMensaje, tiempoMiliSeg = 5000) {
        const divMensaje = document.querySelector(idDivMensaje);
        this.limpiarMensaje(divMensaje, clases);
        divMensaje.innerHTML = mensaje;
        divMensaje.classList = clases;
        setTimeout(() => {
            this.limpiarMensaje(divMensaje,clases);
        }, tiempoMiliSeg);
    }

    limpiarMensaje(divMensaje, clases) {
        divMensaje.innerHTML = '';
        divMensaje.classList = '';
    }

    // ######################################### Función para transformar string tipo título ##########################
    titleCase(str) {
        let title;
        if(str) {
            title = str.toLowerCase().replace(/(^|\s)\S/g,  (firstLetter) => firstLetter.toUpperCase());
            title = title.replace(/\s+/g, ' ');
            title = title.trim();
        } else {
            title = '';
        }
        return title;
    }

    titleCaseV2(str) {
        str = str.toLowerCase().split(' ');
        for (let i = 0; i < str.length; i++) {
            str[i] = str[i].charAt(0).toUpperCase() + str[i].slice(1);
        }
        return this.onlySingleSpace(str.join(' ').trim());
    }

    // ######################################### Función para transformar string con caracteres y números #############
    stringCharacterNumbers(str) {
        str = str.toLowerCase().split(' ');
        for (let i = 0; i < str.length; i++) {
            str[i] = str[i].toUpperCase();
        }
        return this.onlySingleSpace(str.join(' ').trim());
    }

    // ######################################### Función un solo espacio ##############################################
    onlySingleSpace(str) {
        return str.replace(/\s+/g," ");
    }

    // ######################################### Función: Validador de RUT chileno ####################################
    checkRut(run) {
        let Fn = {
            // Valida el rut con su cadena completa "XXXXXXXX-X"
            validaRut : function (run) {
                    var rutCompleto = run.replace(/\./g, '');

                    if ( !/^[0-9]+[-|‐]{1}[0-9kK]{1}$/.test( rutCompleto ) )
                        return false;

                    var tmp 	= rutCompleto.split('-');
                    var digv	= tmp[1];
                    var rut 	= tmp[0];

                    if ( digv == 'K' ) digv = 'k';

                    return (Fn.dv(rut) == digv );
            },
            dv : function(T) {
                    var M=0,S=1;

                    for(;T;T=Math.floor(T/10))
                        S=(S+T%10*(9-M++%6))%11;

                    return S?S-1:'k';
            }
        }
        return Fn.validaRut(run);
    }

    // ############################################## Función para validar mail #######################################
    checkEmail(email) {
        let regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        return regex.test(String(email).toLowerCase());
    }

    // ############################################## Función para validar números telefónicos ########################
    checkTelefono(telefono) {
        let regex = /([2-9]{1})(\d{8})/g;
        let valorBooleano = regex.test(telefono);
        if(telefono.length === 9 && valorBooleano === true) {
            return true;
        } else {
            return false;
        }
    }

    // ############################################## Función para validar StringTitleCase ############################
    checkStringTitleCase(string) {
        if(string.length > 1) {
            let regex = /^([a-zA-ZáéíóúÁÉÍÓÚäëïöüÄËÏÖÜàèìòùÀÈÌÒÙñÑ]+\s)*[a-zA-ZáéíóúÁÉÍÓÚäëïöüÄËÏÖÜàèìòùÀÈÌÒÙñÑ]+$/;
            return regex.test(string);
        } else {
            return false;
        }
    }

    // ############################################## Función para validar StringTitleCase con números ############################
    checkStringTitleCaseNumbers(string) {
        if(string.length > 0) {
            let regex = /^([0-9a-zA-ZáéíóúÁÉÍÓÚäëïöüÄËÏÖÜàèìòùÀÈÌÒÙñÑ]+\s)*[0-9a-zA-ZáéíóúÁÉÍÓÚäëïöüÄËÏÖÜàèìòùÀÈÌÒÙñÑ]+$/;
            return regex.test(string);
        } else {
            return false;
        }
    }

    // ############################################## Función para validar String con números ############################
    checkStringNumbers(string) {
        if(string.length > 0) {
            let regex = /^([0-9]+\s)*[0-9]+$/;
            return regex.test(string);
        } else {
            return false;
        }
    }

    // ############################################## Función para validar StringTitleCase con números ############################
    checkStringCharacterNumbers(string) {
        if(string.length > 0) {
            let regex = /^([0-9a-zA-ZáéíóúÁÉÍÓÚäëïöüÄËÏÖÜàèìòùÀÈÌÒÙñÑ#\\$%(\@)\;[\]\&!?¡¿/:\-,.]+\s)*[0-9a-zA-ZáéíóúÁÉÍÓÚäëïöüÄËÏÖÜàèìòùÀÈÌÒÙñÑ#\\$%(\@)\;[\]\&!?¡¿/:\-,.]+$/;
            return regex.test(string);
        } else {
            return false;
        }
    }

    // ############################################## Función para validar fechas YYYY-MM-DD ############################
    checkDateFormat(string) {
        if(string.length > 0) {
            let regex = /([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))/;
            return regex.test(string);
        } else {
            return false;
        }
    }

    // ############################################## Función para validar fechas YYYY-MM-DD HH:MM:SS ############################
    checkDateTimeFormat(string) {
        if(string.length > 0) {
            // let regex = /^\d\d\d\d-(0?[1-9]|1[0-2])-(0?[1-9]|[12][0-9]|3[01]) (00|[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]):([0-9]|[0-5][0-9])$/;
            let regex = /[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1]) (2[0-3]|[01][0-9]):[0-5][0-9]:[0-5][0-9]/;
            return regex.test(string);
        } else {
            return false;
        }
    }

    // ############################################## Función Modal ############################
    modalConfirm = (callback) => {

        $(".launchid").on("click", function() {
            $("#exampleModal").modal('show');
            // console.log('show');
        });

        $("#saveid").on("click", function() {
            callback(true);
            $("#exampleModal").modal('hide');
            // console.log('hide-saveid');
        });

        $("#closeid").on("click", function() {
            callback(false);
            $("#exampleModal").modal('hide');
            // console.log('hide-closeid');
        });
    };

    // ############################################## Numero Flotante o con decimales ############################
    roundNumberMathFloat(value, decimals) {
        return Number(Math.round(value+'e'+decimals)+'e-'+decimals);
    }

    // ############### llena un campo select a partir de la selección de otro campo select ########################
    objectHtmlSelect(dataObject, objectHtmlSelect, objetcName) {
        //
        let objectHS = objectHtmlSelect;
        objectHS.length = 0;

        let defaultOption = document.createElement('option');
        defaultOption.value = '0';
        defaultOption.text = 'Seleccionar ' + objetcName + '...';
        objectHS.add(defaultOption);
        objectHS.selectedIndex = 0;

        let option;
        Object.keys(dataObject).forEach(function(key) {
            option = document.createElement('option');
            option.text = dataObject[key];
            option.value = key;
            objectHS.add(option);
        });
    }

    objectHtmlSelectList(dataObject, objectHtmlSelect, objetcName) {
        //
        let objectHS = objectHtmlSelect;
        objectHS.length = 0;

        let defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.text = '---Seleccionar ' + objetcName + '---';
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

     // ############### Funciones para vaciar un campo select (option) ########################

    vaciarSelect(select) {
        //
        var length = select.options.length;
        for (var i = length - 1; i >= 0; i--) {
            select.options[i] = null;
        }
    }

    vaciarSelectV2 = (select) => {
        for (let i = select.options.length; i >= 0; i--) {
            select.remove(i);
        }
    };

    vaciarSelectV3(select) {
        var i, L = select.options.length - 1;
        for (i = L; i >= 0; i--) {
            select.remove(i);
        }
    }

    vaciarSelectV4(select) {
        while (select.options.length > 0) {
            select.remove(0);
        }
    }

    vaciarSelectV5(select) {
        while (select.options.length) {
            select.remove(0);
        }
    }

    vaciarSelectV6() {
        document.querySelectorAll('#idComuna option').forEach(option => option.remove());
    }

     // ############### Funciones para formatear un numero con separador de miles ########################

    formatSeparadorMiles(input) {
        var num = input.value.replace(/\./g, '');
        if (!isNaN(num)) {
            num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
            num = num.split('').reverse().join('').replace(/^[\.]/, '');
            input.value = num;
        } else {
            // alert('Solo se permiten numeros');
            let value = input.value;
            let numbers = value.replace(/[^0-9]/g, "");
            input.value = numbers;
            num = input.value.replace(/[^\d\.]*/g, '');
            num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
            num = num.split('').reverse().join('').replace(/^[\.]/, '');
            // console.log(num);
            input.value = num;
        }
    }

}


