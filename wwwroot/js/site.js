// Write your JavaScript code.
//=============== Costos =====================
//Funciones para regresar el costo total de costos variables, fijos y otros e ingresos
$(document).ready(function () {
    $.ajax({
        url: "/Recetas/CalcularTotalVariable",
        method: "GET",
        success: function (result) {
            $("#txtTotalVariable").val(currencyFormat(result));
            $("#ddTotalVariable").text('$ ' + currencyFormat(result));
        }
    });
});

$(document).ready(function () {
    $.ajax({
        url: "/Recetas/CalcularTotalIngresos",
        method: "GET",
        success: function (result) {            
            $("#ddTotalIngresos").text('$ ' + currencyFormat(result));
        }
    });
});

$(document).ready(function () {
    $.ajax({
        url: "/Costo/CalcularTotalFijo",
        method: "GET",
        success: function (result) {
            $("#txtTotalFijo").val(result);
            $("#ddTotalFijo").text('$ ' + currencyFormat(result));
        }
    });
});

$(document).ready(function () {
    $.ajax({
        url: "/Costo/CalcularTotalOtro",
        method: "GET",
        success: function (result) {
            $("#txtTotalOtro").val(currencyFormat(result));
            $("#ddTotalOtro").text('$ ' + currencyFormat(result));
        }
    });
});

$(document).ready(function () {
    $.ajax({
        url: "/Costo/RecuperarTotales",
        method: "GET",
        success: function (result) {
            if (result >= 0) {                
                $("#ddPuntoEquilibrio").css('color', 'green');
            } else {
                $("#ddPuntoEquilibrio").css('color', 'red');
            }           
            $("#ddPuntoEquilibrio").text('$ ' + currencyFormat(result));
        }
    });
});

function currencyFormat(num) {
    return num.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
}

// ================ Recetas
// ================================= Index
var $currentFilter = $("#currentFilter");

switch ($currentFilter.val()) {
    case "nombre":
        $("#nombreReceta").attr("checked", "checked");
        break;
    case "tipo_receta":
        $("#tipoReceta").attr("checked", "checked");
        break;
    case "clasificacion_receta":
        $("#clasificacionReceta").attr("checked", "checked");
        break;
    case "area_produccion":
        $("#areaProduccion").attr("checked", "checked");
        break;
    default:
        $("#nombreReceta").attr("checked", "checked");
        break;
}

// ================================= Create
$.ajax({
    url: "/Recetas/ListAreasProduccion", success: function (result) {
        $.each(result, function (i, item) {
            $('#AreaProduccion').append($('<option>', {
                value: item.id,
                text: item.nombre
            }));
        });
    }
});
//
$.ajax({
    url: "/Recetas/ListTiposReceta", success: function (result) {
        //<a class="dropdown-item" href="#">Tipo de receta 1</a>
        $.each(result, function (i, item) {
            $('#listaTipoReceta')
                .append('<a class="dropdown-item" href="#">' + item + '</a>');
        });

        $('#listaTipoReceta a').click(function () {
            $("#TipoReceta").val($(this).html())
        });
    }
});

$.ajax({
    url: "/Recetas/ListClasificaciones", success: function (result) {
        //<a class="dropdown-item" href="#">Tipo de receta 1</a>
        $.each(result, function (i, item) {
            $('#listaClasificaciones')
                .append('<a class="dropdown-item" href="#">' + item + '</a>');
        });
        $('#listaClasificaciones a').click(function () {
            $("#Clasificacion").val($(this).html())
        });
    }
});

$("#btnContinuarDatos").click(function () {
    $('#tabRecetas li:nth-child(2) a').removeClass("disabled");
    $('#tabRecetas li:nth-child(2) a').tab('show');
});
$("#btnContinuarIngredientes").click(function () {
    $('#tabRecetas li:nth-child(3) a').removeClass("disabled");
    $('#tabRecetas li:nth-child(3) a').tab('show');
});


// Objeto que retiene la receta
var Receta = {
    nombre: "",
    metodoPreparacion: "",
    porciones: 1,
    tipoReceta: "",
    clasificacion: "",
    areaProduccion: 0,
    insumos: [],
    recetas: [],

}

var insumoPorConfigurar;


$("#txtBuscarInsumoModal").keyup(function (e) {
    $.ajax({
        url: "/Recetas/ListInsumos",
        data: {
            queryString: $("#txtBuscarInsumoModal").val()
        },
        success: function (data) {
            $("#addInsumosTableModal tbody").html("");
            for (var i = 0; i < data.length; i++){
                $("#addInsumosTableModal tbody").append('<tr><td class="insumoNameTD">' + data[i].nombre + '</td><td><a href="#" class="btn btn-success addInsumoBtnModal" data-model=\'' + JSON.stringify(data[i]) + '\'>Agregar</a></td></tr>');
            }            
            $(".addInsumoBtnModal").click(function (e) {
                insumoPorConfigurar = $(this).data("model");
                $("#configInsumoModal").modal("show");

                var idNombreUnidad = insumoPorConfigurar.unidad.nombre;
                var nombreUnidad = asignarNombreUnidad(idNombreUnidad);

                $("#nombreInsumoConfig").val(insumoPorConfigurar.nombre);
                $("#medidaInsumoConfig").val(nombreUnidad);
            });
        }
    });
});

function asignarNombreUnidad(idNomrbeUnidad) {
    var nombreUnidad;
    switch (idNomrbeUnidad) {
        case 0:
            nombreUnidad = "Kilogramo";
            break;
        case 1:
            nombreUnidad = "Gramo";
            break;
        case 2:
            nombreUnidad = "Litro";
            break;
        case 3:
            nombreUnidad = "Mililitro";
            break;
        case 4:
            nombreUnidad = "Rollo";
            break;
        case 5:
            nombreUnidad = "Pieza";
            break;
        default:
            break;
    }
    return nombreUnidad;
}

$("#addInsumoConfigurado").click(function () {
    var insumoSeleccionado = insumoPorConfigurar.id;
    var presentacionSeleccionada;    
    var precioUnitarioInsumo;
    $.ajax({
        url: "/Presentaciones/ObtenerPresentaciones",
        method: "GET",
        data: { idInsumo: insumoSeleccionado },
        success: function (result) {    
            $.each(result, function (index, value) {
                presentacionSeleccionada = value["idPresentacion"];
                precioUnitarioInsumo = value["precioUnitario"]
            });            
            var insumoConfigurado = {                
                id: insumoPorConfigurar.id,
                nombre: insumoPorConfigurar.nombre,
                unidadBaseId: insumoPorConfigurar.idUnidadBase,
                nombreUnidad: insumoPorConfigurar.unidad.nombre,
                costoUnitario: precioUnitarioInsumo,
                pesoNeto: $("#cantidadInsumoConfig").val(),
                costoTotal: (precioUnitarioInsumo * parseFloat($("#cantidadInsumoConfig").val())).toFixed(2)
            }

            Receta.insumos.push(insumoConfigurado);
            $(".modal").modal("hide");
            refreshDataTableInsumos();            
        }
    });
});

function refreshDataTableInsumos() {
    var $table = $("#tableInsumos tbody");
    $table.html("");    
    $.each(Receta.insumos, function (key, value) {
        $table.append('<tr><th scope="row">' + value.nombre + '</th><td>' + value.pesoNeto + '</td><td>' + asignarNombreUnidad(value.nombreUnidad) + '</td><td>$' + value.costoUnitario + '</td><td>$' + value.costoTotal + '</td><td><a href="#" class="btn btn-danger tableInsumosEliminar" data-id="'+ key +'">-</a></td></tr>')
    });
    actualizarPrecios();

    $(".tableInsumosEliminar").click(function () {
        Receta.insumos.splice($(this).data("id"), 1);
        refreshDataTableInsumos();
        actualizarPrecios();
    });
}


$("#txtBuscarRecetaModal").keyup(function (e) {
    $.ajax({
        url: "/Recetas/ListRecetas",
        data: {
            queryString: $("#txtBuscarRecetaModal").val()
        },
        success: function (data) {
            
            $("#addRecetasTableModal tbody").html("");
            for (var i = 0; i < data.length; i++) {
                $("#addRecetasTableModal tbody").append('<tr><td class="recetaNameTD">' + data[i].nombre + '</td><td><input type="number" class="form-control" value="1"></td><td><a href="#" class="btn btn-success addRecetaBtnModal" data-model=\'' + JSON.stringify(data[i]) + '\'>Agregar</a></td></tr>');
            }

            $(".addRecetaBtnModal").click(function (e) {
                
                var RecetaAReceta = {
                    IdRecetaHijo: $(this).data("model").id,
                    CostoUnitario: $(this).data("model").costoUnitario,
                    nombre: $(this).data("model").nombre,
                    Porciones: parseInt($($(this).parent().siblings()[1]).children()[0].value)
                }
                Receta.recetas.push(RecetaAReceta);
                $(".modal").modal("hide");
                refreshDataTableRecetas();
            });
        }
    });
});

function refreshDataTableRecetas() {
    var $table = $("#tableRecetas tbody");
    $table.html("");
    $.each(Receta.recetas, function (key, value) {
        $table.append('<tr><th scope="row">' + value.nombre + '</th><td>$' + value.CostoUnitario + '</td><td>' + value.Porciones + '</td><td><a href="#" class="btn btn-danger tableRecetaEliminar" data-id="' + key + '">-</a></td></tr>')
    });
    actualizarPrecios();

    $(".tableRecetaEliminar").click(function () {
        Receta.recetas.splice($(this).data("id"), 1);
        refreshDataTableRecetas();
        actualizarPrecios();
    });
}
// ============== Funciones AgregarCompras ===========================
//Función para que las tarjetas reaccionen al click
$(document).on('click', '.singleCard', function () {
    
    $(this).toggleClass(agregarArr(this));
    if (!botonActivo && arrSeleccionados.length > 0) {
        $(".botonF1").toggle(55);
        botonActivo = true;
    } else if (botonActivo && arrSeleccionados.length == 0) {
        $(".botonF1").toggle(55);
        botonActivo = false;
    }
    $(this).addClass("singleCard");
});
//Metodo para filtrar categorias
$(".filtro").click(function () {
    var categoria = $(this).attr("id");

    if (filtrosSeleccionados.indexOf(categoria) < 0) {
        filtrosSeleccionados.push(categoria);
    } else {
        filtrosSeleccionados.splice(filtrosSeleccionados.indexOf(categoria), 1);
    }

    $("#row").empty();

    $.each(model, function (index, value) {

        if (filtrosSeleccionados.indexOf(value["insumo"]["categoria"]["nombre"]) >= 0 || filtrosSeleccionados.length == 0) {
            $("#row").append(
                '<div class="col-md-2 col-sm-12 offset-sm-0 divCard">' +
                '<div class="singleCard" id="' + value["insumo"]["id"] + '-' + value["presentacion"]["idPresentacion"] + '">' +
                '<div class="cardSobrePuesta"></div>' +
                '<div class="cardInsumo">' +
                '<div class="imgText">' +
                '<div class="img">' +
                '<img src="' + value["insumo"]["rutaImagen"] +'"/>' +
                '</div>' +
                '<h2>' + value["insumo"]["nombre"] + '</h2>' +
                '<span>Presentación: <span>' + value["presentacion"]["nombre"] + '</span></span>' +
                '<br />' +
                '</div>' +
                '</div>' +
                '</div>' +
                '</div>'
            );
        }

    });
});

//Metodo de busqueda de insumos-presentaciones por escritura de letras
$("#txtBuscar").keyup(function () {
    var busqueda = $(this).val();
    $("#row").empty();
    $.each(model, function (index, valor) {
        if (valor["insumo"]["nombre"].indexOf(busqueda) !== -1) {
            $("#row").append(
                '<div class="col-md-2 col-sm-12 offset-sm-0 divCard">' +
                    '<div class="singleCard" id="' + valor["insumo"]["id"] + '-' + valor["presentacion"]["idPresentacion"] +'">' +
                        '<div class="cardSobrePuesta"></div>' +
                        '<div class="cardInsumo">' +
                            '<div class="imgText">' +
                                '<div class="img">' +
                                    '<img src="'+ valor["insumo"]["rutaImagen"] +'"/>' +
                                 '</div>' +
                                '<h2>' + valor["insumo"]["nombre"] + '</h2>' +
                                '<span>Presentación: <span>'+valor["presentacion"]["nombre"]+ '</span></span>' +
                                '<br />' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                 '</div>'
            );
        }
    });
});
//Metodo complementario de la función de selección de tarjetas
function agregarArr(elementPadre) {
    var elementDiv = $(elementPadre).find($(".cardSobrePuesta"));

    var id = $(elementPadre).attr("id");
    if (arrSeleccionados.indexOf(id) < 0) {
        $(elementDiv).toggle(55);
        arrSeleccionados.push(id);
        $(".spanBtn").text(arrSeleccionados.length);
    } else {
        $(elementDiv).toggle(55);
        arrSeleccionados.splice(arrSeleccionados.indexOf(id), 1);
        $(".spanBtn").text(arrSeleccionados.length);
    }
    if (arrSeleccionados.length > 1 || arrSeleccionados.length == 0) {
        $(".spanBtn2").text("s");
    } else {
        $(".spanBtn2").text("");
    }
}
//Función que declara la vantana emergente para registrar las compras
alertify.dialog('genericDialog', function () {
    return {
        main: function (content) {
            this.setContent(content);
        },
        setup: function () {
            return {
                options: {
                    title: "Registrar las compras:",
                    basic: true,
                    maximizable: false,
                    resizable: false,
                    padding: false
                }
            };
            
        },
        build: function () {
            $(this.elements.dialog).css("max-width", "800px");
            $(this.elements.header).css("color", "black");

        },
        settings: {
            selector: undefined
        },
        hooks: {
            onclose: function () {
                var arrDiv = $("#modalAgregarCompras").find(".targetLarge");
                $.each(arrDiv, function (index, valor) {
                    $(valor).remove();
                });
            }
        }
    };
});
//Función que obtiene del servidor la información de las tarjetas completas de la ventana emergente
$("#btnAgregarCompras").click(function () {
    $.ajax({
        url: "/Insumos/ObtenerInsumosIds",
        method: "GET",
        data: { arrJSON: arrSeleccionados.toString() },
        success: function (result) {
            var valor = JSON.parse(result);
            var arrTargetsTemp = []
            var suma = 0;
            if (!Array.isArray(valor)) {
                var options = "";
                $.each(valor["marcas"], function (index, value) {
                    options += '<option>' + value + '</option>';
                });
                suma = suma + parseInt(valor["precio"]);
                arrTargetsTemp.push(valor["id"]);
                $('#modalAgregarCompras').append(
                    '<div class="col-md-11 col-sm-12 targetLarge" id="div' + valor["id"] + '">' +
                    '<form id="form-' + valor["id"] + '">' +
                    '<div class="row">' +
                    '<div class="imgTarget col-md-3">' +
                    '<img src="' + valor["rutaImagen"] + '">' +
                    '</div>' +
                    '<div class="textTarget col-md-4">' +
                    '<h6>' + valor["nombre"] + '</h6>' +
                    '<span>Presentación: <span id="presentacion">' + valor["presentacion"] + '</span></span>' +
                    '<span>Unidad por presentación: <span id="presentacionUnidad">' + valor["unidadPresentacion"] + '</span></span>' +
                    '<span>Unidad: <span id="unidad">' + valor["unidad"] + '</span></span>' +
                    '</div>' +
                    '<div class="textTarget col-md-4">' +
                    '<span>Precio:</span>' +
                    '<input type="text" id="precio' + valor["id"] + '" value="' + valor["precio"] + '" name="precio" />' +
                    '<span>Cantidad:</span>' +
                    '<input type="number" class="inputCantidad" id="cant' + valor["id"] + '"  name="cantidad" value="1" min="1" />' +
                    '<span>Marca:</span>' +
                    '<select id="sct' + valor["id"] + '">' +
                    options +
                    '</select>' +
                    '</div>' +
                    '<div class="btnsTarget col-md-1">' +
                    '<div class="btn btnTarget">X</div>' +
                    '</div>' +
                    '</div>' +
                    '</form>' +
                    '</div>');
                alertify.genericDialog($('#modalAgregarCompras')[0]);
            } else {
                $.each(valor, function (index, value) {
                    var options = "";
                    $.each(value["marcas"], function (index2, value2) {
                        options += '<option>' + value2 + '</option>';
                    });
                    suma = suma + parseInt(value["precio"]);
                    arrTargetsTemp.push(value["id"]);
                    $('#modalAgregarCompras').append(
                        '<div class="col-md-11 col-sm-12 targetLarge" id="div' + value["id"] + '">' +
                        '<form id="form-' + value["id"] + '">' +
                        '<div class="row">' +
                        '<div class="imgTarget col-md-3">' +
                        '<img src="' + value["rutaImagen"] + '">' +
                        '</div>' +
                        '<div class="textTarget col-md-4">' +
                        '<h6>' + value["nombre"] + '</h6>' +
                        '<span>Presentación: <span id="presentacion">' + value["presentacion"] + '</span></span>' +
                        '<span>Unidad por presentación: <span id="presentacionUnidad">' + value["unidadPresentacion"] + '</span></span>' +
                        '<span>Unidad: <span id="unidad">' + value["unidad"] + '</span></span>' +
                        '</div>' +
                        '<div class="textTarget col-md-4">' +
                        '<span>Precio:</span>' +
                        '<input type="text" id="precio' + value["id"] + '" value="' + value["precio"] + '" name="precio" />' +
                        '<span>Cantidad:</span>' +
                        '<input type="number" class="inputCantidad" id="cant' + valor["id"] + '" value="1" name="cantidad" min="1" />' +
                        '<span>Marca:</span>' +
                        '<select id="sct' + value["id"] + '">' +
                        options +
                        '</select>' +
                        '</div>' +
                        '<div class="btnsTarget col-md-1">' +
                        '<div class="btn btnTarget">X</div>' +
                        '</div>' +
                        '</div>' +
                        '</form>' +
                        '</div>');
                });
                alertify.genericDialog($('#modalAgregarCompras')[0]);
            }
            //Función que remueve una tarjeta de la ventana emergente
            $(".btnTarget").click(function (event) {
                event.preventDefault();
                var padreTarget = $($($($(this).parent()).parent()).parent()).parent();
                var idDiv = padreTarget.attr("id");
                var inputCant = padreTarget.find("input[name='cantidad']");
                var inputPrecio = padreTarget.find("input[name='precio']");
                suma += -(inputCant.val() * inputPrecio.val());
                $("#total").text(suma);
                idDiv = idDiv.substring(3);
                padreTarget.remove();
                arrTargetsTemp.splice(idDiv, 1);
            });
            
            $("#total").text(suma);
            //Detecta cambios en las cantidades de insumos para actualizar el total de compra
            $(".inputCantidad").change(function () {
                var arrCant = $("#modalAgregarCompras").find(".inputCantidad");
                var arrPrecio = $("#modalAgregarCompras").find("input[name='precio']");
                suma = 0;
                $.each(arrCant, function (index, valor) {
                    suma += $(valor).val() * $(arrPrecio[index]).val();
                });
                $("#total").text(suma);
            });
            //Función que hace una petición POST para actualizar el inventario según las compras (Falta agregar registro a costos)
            $("#btnRegistrarCompras").click(function () {
                var targets = $("#modalAgregarCompras").find(".targetLarge");
                $.each(targets, function (index, valor) {
                    var presentacionUnidad = $(valor).find("#presentacionUnidad").text();
                    var cantidad = $(valor).find("[name='cantidad']").val();
                    var id = $(this).attr("id").substring(3);
                    $.ajax({
                        url: "/Insumos/RegistrarCompras",
                        type: "POST",
                        data: {
                            idInsumo: id,
                            cantidadStock: (parseInt(presentacionUnidad) * parseInt(cantidad))
                        },
                        success: function (result) {
                            location.reload();
                        }
                    });
                });
                alert("Se actualizo correctamente el invetario");
            });
        }
    });
});

// ============== Termina: Funciones agregarCompras ===================


// ============== Fuciones para presentaciones ===================

//Metodo para filtrar categorias
$(".filtroPresentaciones").click(function () {
    var insumos = JSON.parse(model);
    var categoria = $(this).attr("id");

    if (filtrosSeleccionados.indexOf(categoria) < 0) {
        filtrosSeleccionados.push(categoria);
    } else {
        filtrosSeleccionados.splice(filtrosSeleccionados.indexOf(categoria), 1);
    }

    $(".row").empty();

    $.each(insumos, function (index, value) {

        if (filtrosSeleccionados.indexOf(value.categoria["nombre"]) >= 0 || filtrosSeleccionados.length == 0) {
            $(".row").append(
                '<div class="col-md-2 col-sm-12">' +
                    '<div class="insumoSeleccionado" id="' + value["id"] + '">'+
                        '<div class="cardInsumo">' +
                            '<div class="imgText">' +
                                '<div class="img">' +
                                    '<img src="' + value["rutaImagen"] + '">' +
                                 '</div>' +
                                    '<h2 class="nombrePresentacion">' + value["nombre"] + '</h2>' +
                                '</div>' +
                        '</div>'+
                    '</div>' + 
                '</div>'
            );
        }

    });
});

//Metodo de busqueda de insumos-presentaciones por escritura de letras
$("#txtBuscarPresentaciones").keyup(function () {
    var insumos = JSON.parse(model);
    var busqueda = $(this).val();

    $(".row").empty();
    $.each(insumos, function (index, value) {

        if (value["nombre"].includes(busqueda)) {
            $(".row").append(
                '<div class="col-md-2 col-sm-12">' +
                    '<div class="cardInsumo insumoInventario" id="' + value["id"] +'">' +
                        '<div class="imgText">' +
                            '<div class="img">' +
                                '<img src="' + value["rutaImagen"] + '">' +
                            '</div>' +
                            '<h2 class="nombrePresentacion">' + value["nombre"] + '</h2>' +
                        '</div>' +
                    '</div>' +
                '</div>'
            );
        }
    });
});

/* Función para calcular el precio unitario de acuerdo a lo ingresado en los campos
 * de la cantidad de unidades en la presentación y el costo de la presentación*/
function calcularPrecioUnidad() {
    var precioPresentacion = $("#presentacionPrecio").val();
    var unidades = $("#unidades").val();
    var precioUnidad = precioPresentacion / unidades;
    $("#precioUnitario").val(precioUnidad.toFixed(2));
}

$("#unidades").keyup(function () {
    calcularPrecioUnidad();
});

$("#presentacionPrecio").keyup(function () {
    calcularPrecioUnidad();
});

/* Función para encontrar las presentaciones asociadas al insumo seleccionado*/
$(document).on('click','.insumoSeleccionado',function () {
    var insumoSeleccionado = $(this).attr("id");
    $('#btnAgregarPre').attr('href', '/Presentaciones/Create?id=' + insumoSeleccionado);
    $.ajax({
        url: "/Presentaciones/ObtenerPresentaciones",
        method: "GET",
        data: { idInsumo: insumoSeleccionado },
        success: function (result) {            
            $.each(result, function (index, value) {                
                var presentacionSeleccionada = value["idPresentacion"];
                $("#presentaciones").append(
                    "<tr>" +
                    "<td>" + value["nombre"] + "</td >" +
                    "<td>"+
                    '<a href="/Presentaciones/Edit/' + presentacionSeleccionada + '" asp-route-id="@item.Id" class="btn btn-sm btn-secondary">Editar</a> | ' +
                    '<a href="/Presentaciones/Details/' + presentacionSeleccionada + '" asp-route-id="@item.idPresentacion" class="btn btn-sm btn-info">Detalles</a> | ' +
                    '<a href="/Presentaciones/Delete/' + presentacionSeleccionada + '" asp-route-id="@item.Id" class="btn btn-sm btn-danger">Eliminar</a>' +
                    "</td>"+
                    "</tr>");            
            });
            alertify.dialogoPresentaciones($("#modalPresentaciones")[0]);
        }
    });
});

alertify.dialog('dialogoPresentaciones', function () {
    return {
        main: function (content) {
            this.setContent(content);
        },
        setup: function () {
            return {
                options: {
                    title: "Presentaciones:",
                    basic: true,
                    maximizable: false,
                    resizable: false,
                    padding: false
                }
            };
        },
        build: function () {
            $(this.elements.dialog).css("max-width", "800px");
            $(this.elements.header).css("color", "black");

        },
        settings: {
            selector: undefined
        },
        hooks: {
            onclose: function () {
                $("#presentaciones").empty();
            }
        }
    };
});
// ============== Termina fuciones para presentaciones===================

$("#costos-tab").click(actualizarPrecios);
$("#btnContinuarIngredientes").click(actualizarPrecios);

function actualizarPrecios() {
    var costoRecetaTotal = 0;
    var costoRecetaUnitario, costoRecetaOtros, precioSugerido, precioDefinido, precioIVA;
    $.each(Receta.insumos, function (key, value) {
        if (typeof (value.costoTotal) === 'string') {
            costoRecetaTotal = costoRecetaTotal + parseFloat(value.costoTotal);
        } else {
            costoRecetaTotal += value.costoTotal;
        }        
    });

    $.each(Receta.recetas, function (key, value) {
        if (typeof (value.costoTotal) === 'string') {
            costoRecetaTotal = costoRecetaTotal + parseFloat(value.CostoUnitario);
        } else {
            costoRecetaTotal += value.CostoUnitario;
        }  
    });

    costoRecetaUnitario = costoRecetaTotal / Receta.porciones;
    //se agrega el 10% solo como representacion de la variación de precios,
    //en futuras iteraciones este porcentaje se obtendrá del modulo de ingresos y egresos
    costoRecetaOtros = costoRecetaUnitario * 1.1;
    //Se agrega el 10% por variación de gastos
    precioSugerido = costoRecetaOtros * 1.1
    precioDefinido = precioSugerido;
    precioIVA = precioDefinido * 1.16; //16% de IVA
    costoRecetaTotal = roundNumber(costoRecetaTotal, 2);
    costoRecetaUnitario = roundNumber(costoRecetaUnitario, 2);
    costoRecetaOtros = roundNumber(costoRecetaOtros, 2);
    precioSugerido = roundNumber(precioSugerido, 2);
    precioDefinido = roundNumber(precioDefinido, 2);
    precioIVA = roundNumber(precioIVA, 2);

    $("#CostoUnitario").val(costoRecetaUnitario);
    $("#CostoOtrosConUtilidad").val(costoRecetaOtros);
    $("#PrecioSugerido").val(precioSugerido);
    //$("#PrecioDefinidoPorUsuario").val(precioDefinido);
    $("#PrecioVentaConIva").val(precioIVA);

}

function roundNumber(numero, numeroDecimales) {
    return (Math.round((numero * 1000) / 10) / 100).toFixed(numeroDecimales);
}

$("#PrecioDefinidoPorUsuario").keyup(function (e) {
    var nuevoValor = parseFloat($("#PrecioDefinidoPorUsuario").val());
    var precionIva = nuevoValor * 1.16;
    precionIva = roundNumber(precionIva, 2);
    $("#PrecioVentaConIva").val(precionIva);
});

$("#crearRecetaForm").submit(function (e) {
    e.preventDefault();
    var insumosParam = [];
    var recetasParam = [];
    $.each(Receta.insumos, function (i, value) {
        var viewInsumo = {
            IdInsumo: value.id,
            IdUnidad: value.unidadBaseId,
            PesoNeto: value.pesoNeto
        }
        insumosParam.push(viewInsumo);
    });
    $.each(Receta.recetas, function (i, value) {
        var viewReceta = {
            IdRecetaHijo: value.IdRecetaHijo,
            Porciones: value.Porciones
        }
        recetasParam.push(viewReceta);
    });


    $.ajax({
        url: "/Recetas/CreateAjax",
        method: "POST",
        data: {
            
            Nombre: $("#Nombre").val(),
            MetodoPreparacion: $("#MetodoPreparacion").val(),
            Porciones: parseInt($("#Porciones").val()),
            TipoReceta: $("#TipoReceta").val(),
            Clasificacion: $("#Clasificacion").val(),
            AreaProduccionId: $("#AreaProduccion").val(),
            CostoUnitario: parseFloat($("#CostoUnitario").val()),
            CostoOtrosConUtilidad: parseFloat($("#CostoOtrosConUtilidad").val()),
            PrecioSugerido: parseFloat($("#PrecioSugerido").val()),
            PrecioDefinidoPorUsuario: parseFloat($("#PrecioDefinidoPorUsuario").val()),
            PrecioVentaConIva: parseFloat($("#PrecioVentaConIva").val()),
            Activo: true,
            
            insumos: insumosParam,
            recetas: recetasParam
        },
        success: function (data) {
            if (data == "complete") {
                $("#submitReceta").addClass("disabled");
                $("#alertSuccess").css("display", "block");
            }
        }
    });
});

if ($("#action").val() == "edit") {
    var idReceta = $("#Id").val();    
    function loadRecetasFromServer() {
        $.ajax({
            url: "/Recetas/GetDataFromReceta",
            method: "GET",
            data: {
                Id: idReceta
            },
            success: function (data) {  
                console.log(data);
                    var costoUnitario = 0; 
                    for (var i = 0; i < data[1].length; i++) {
                        var Presentaciones = data[1];
                        costoUnitario = Presentaciones[i].precioUnitario;
                        var insumoConfigurado = {
                            id: data[0].insumosReceta[i].insumo.id,
                            nombre: data[0].insumosReceta[i].insumo.nombre,
                            unidadBaseId: data[0].insumosReceta[i].IdUnidad,
                            nombreUnidad: data[0].insumosReceta[i].insumo.unidad.nombre,
                            costoUnitario: costoUnitario,
                            pesoNeto: data[0].insumosReceta[i].pesoNeto,
                            costoTotal: roundNumber(data[0].insumosReceta[i].pesoNeto * costoUnitario, 2)
                        }
                        Receta.insumos.push(insumoConfigurado);                       
                    }               
                
                $.each(data[0].recetasIntegradoras, function (key, value) {
                    var RecetaAReceta = {
                        IdRecetaHijo: value.recetaHijo.id,
                        CostoUnitario: value.recetaHijo.costoUnitario,
                        nombre: value.recetaHijo.nombre,
                        Porciones: parseInt(value.porciones)
                    }
                    Receta.recetas.push(RecetaAReceta);
                });

                refreshDataTableInsumos();
                refreshDataTableRecetas();
                actualizarPrecios();
            }
        })
    }

    loadRecetasFromServer();
}

$("#editRecetaForm").submit(function (e) {
    e.preventDefault();
    var insumosParam = [];
    var recetasParam = [];
    $.each(Receta.insumos, function (i, value) {
        var viewInsumo = {
            IdInsumo: value.id,
            IdUnidad: value.unidadBaseId,
            PesoNeto: value.pesoNeto
        }
        insumosParam.push(viewInsumo);
    });
    $.each(Receta.recetas, function (i, value) {
        var viewReceta = {
            IdRecetaHijo: value.IdRecetaHijo,
            Porciones: value.Porciones
        }
        recetasParam.push(viewReceta);
    });


    $.ajax({
        url: "/Recetas/EditAjax",
        method: "POST",
        data: {
            Id: $("#Id").val(), 
            Nombre: $("#Nombre").val(),
            MetodoPreparacion: $("#MetodoPreparacion").val(),
            Porciones: parseInt($("#Porciones").val()),
            TipoReceta: $("#TipoReceta").val(),
            Clasificacion: $("#Clasificacion").val(),
            AreaProduccionId: $("#AreaProduccion").val(),
            CostoUnitario: parseFloat($("#CostoUnitario").val()),
            CostoOtrosConUtilidad: parseFloat($("#CostoOtrosConUtilidad").val()),
            PrecioSugerido: parseFloat($("#PrecioSugerido").val()),
            PrecioDefinidoPorUsuario: parseFloat($("#PrecioDefinidoPorUsuario").val()),
            PrecioVentaConIva: parseFloat($("#PrecioVentaConIva").val()),
            Activo: true,

            insumos: insumosParam,
            recetas: recetasParam
        },
        success: function (data) {
            console.log(data);
            if (data == "complete") {
                $("#submitEditReceta").addClass("disabled");
                $("#alertSuccess").css("display", "block");
            }
        }
    });
});


/**==================================================
 *  Insumos Create and Edit
 *  =================================================
 * */

function calcularPrecioAjustado() {
    var precio = $("#Precio").val();
    var merma = $("#Merma").val();
    var unidad = 1;
    var precioAjustado = 0;
    if (precio == "" || precio == null || !$.isNumeric(precio)) {
        precio = 0;
    }
    if (merma == "" || merma == null || !$.isNumeric(merma)) {
        merma = 0;
    }
    precioAjustado = precio / (unidad - merma);
    $("#PrecioAjustado").val(precioAjustado.toFixed(2));
}

$("#Precio").keyup(function () {
    calcularPrecioAjustado();
});

$("#Merma").keyup(function () {
    calcularPrecioAjustado();
});

function validarTamañoString(string, minSize, maxSize) {
    if (string.length > minSize && string.length < maxSize) {
        return true;
    } else {
        return false;
    }
}

function validarCategoria() {
    $("#error_create_insumo_categoria").text("");
    var nuevaCategoria = $("#nueva_categoria").val();
    var selectCategoria = $("#select_categoria").val();
    var stringError = "";
    if (nuevaCategoria == "" && selectCategoria == null) {
        stringError = "Por favor seleccione una categoría o ingrese una nueva."
    } else {
        if (selectCategoria != null && nuevaCategoria != "") {
            if (validarTamañoString(nuevaCategoria, 1, 64)) {
                return true;
            } else {
                stringError = "Debe de contener mínimo 1 y máximo 64 caracteres"
            }
            //stringError = "Ha seleccionado e ingresado una categoría, solo puede elegir una opción."
        } else {
            if (selectCategoria != null) {
                return true;
            } else {
                if (validarTamañoString(nuevaCategoria, 1, 64)) {
                    return true;
                } else {
                    stringError = "Debe de contener mínimo 1 y máximo 64 caracteres"
                }
            }
        }
    }
    $("#error_create_insumo_categoria").text(stringError);
    return false;
}

$("#form_submit_insumo").submit(function (e) {
    if (!validarCategoria()) {
        e.preventDefault();
    }
});

//Eliminar cuando se modifique la página de Inicio
$("#portada").ready(function () {
    if ($("#portada").length > 0) {
        $(".container-home").removeClass("container-home");
    }
});

// Funciones para el inventario

/**
* Obtiene la información de un insumo seleccionado
*/
$(document).on('click', '.insumoInventario', function () {
    var insumoSeleccionado = $(this).attr("id");
    alert(insumoSeleccionado)
    $.ajax({
        url: "/Insumos/ObtenerDatosInsumo",
        method: "GET",
        data: { idInsumo: insumoSeleccionado },
        success: function (result) {
            alert(result);
            var marcas = result[0]["marcas"];
            var presentaciones = result[0]["presentaciones"];

            $("#imagenInsumo").attr("src", result[0]["imagen"]);
            $("#nombreInsumo").append(result[0]["insumoNombre"]);

            $("#unidad").append(result[0].unidad["simbolo"]);
            $("#stockMin").append(result[0]["stockMin"]);
            $("#stock").append(result[0]["stock"]);

            $("#ultimaCompra").append(result[0]["ultimaFecha"]);
            $("#bajasMerma").append(result[0]["bajasMerma"]);

            $.each(marcas, function (index, value) {
                $("#tbodyMarcas").append("<tr><td>" + value["nombre"] +"<tr><td>");   
            });

            $.each(presentaciones, function (index, value) {
                $("#tbodyPresentaciones").append("<tr><td>" + value["nombre"] + "<tr><td>");
            });

            $("#txtCantidadMerma").attr("max", result[0]["stock"]);
            $(".btnDescontar").attr("id", result[0]["id"]);

            alertify.dialogoInventario($("#modalDatosInsumo")[0]).set({ transition: 'zoom', message: 'Transition effect: zoom' }).show(); 
        }
    });
});

/**
* Asocia a los objetos de la clase btnDescontar un método que reduce el stock del insumo en el inventario. 
*/
$(".btnDescontar").click(function (event) {
    var idInsumo = $(this).attr("id");
    var nombreInsumo = $("#nombreInsumo").text();
    var cantidadMerma = $("#txtCantidadMerma").val();
    var insumoCantidad = $("#stock").text();
    
    if (parseInt(insumoCantidad) < cantidadMerma || cantidadMerma < 0) {
        $("#errorMerma").text(" *Ingrese una cantidad válida");
        $("#errorMerma").attr("style", "color: red;");
        event.preventDefault();
    } else {
        alertify.confirm('Descontar inventario', '¿Seguro que desea descontar ' + cantidadMerma + ' ' + nombreInsumo + ' del inventario?', function () {
            $.ajax({
                url: "/Insumos/DescontarInventario",
                method: "POST",
                data: { idInsumo: idInsumo, cantidad: cantidadMerma },
                success: function (result) {
                        localStorage.setItem("Status", result.OperationStatus)
                        location.reload();
                    
                }
            });  
        });

        
    }
});

$(document).ready(function () {
    if (localStorage.getItem("Status")) {
        alertify.success('Insumos descontados');
        localStorage.clear();
    }
});

/**
* Asocia a los objetos de la clase filtro un método que de acuerdo con el modelo y el filtro seleccionado
 * muestra las tarjetas correspondientes. 
*/
$(".filtroInventario").click(function () {
    var insumos = JSON.parse(modelo);
    var categoria = $(this).attr("id");

    if (filtrosSeleccionados.indexOf(categoria) < 0) {
        filtrosSeleccionados.push(categoria);
    } else {
        filtrosSeleccionados.splice(filtrosSeleccionados.indexOf(categoria), 1);
    }

    $("#tarjetas").empty();
    
    $.each(insumos, function (index, value) {
        if (value["cantidad"] <= value["stockMinimo"]) {
            color = "#E74C3C";
        }
        else if ((value["cantidad"] - 2) <= value["stockMinimo"]) {
            color = "#F1948A";
        }
        else {
            color = "white";
        }

        if (filtrosSeleccionados.indexOf(value.categoria["nombre"]) >= 0 || filtrosSeleccionados.length == 0) {
            $("#tarjetas").append(
                '<div class="col-md-2 col-sm-12">'+
                '<div class="cardInsumo insumoInventario" id="' + value["id"] + '" style="background-color:'+ color +';">' +
                        '<div class="imgText">' + 
                            '<div class="img">' +
                                '<img src="'+ value["rutaImagen"]+'">'+
                            '</div>'+
                                '<h2>' + value["nombre"] +'</h2>'+
                                '<span>Stock min: <span>'+ value["stockMinimo"]+'</span></span>'+
                                '<span>Stock: <span>'+value["cantidad"]+'</span></span>'+
                                '<br />'+
                            '</div>'+
                        '</div>'+
                    '</div>'
            );
        }
        
    });
});

/**
* Genera una ventana modal para los datos de un insumo seleccionado. 
*/
alertify.dialog('dialogoInventario', function () {
    return {
        main: function (content) {
            this.setContent(content);
        },
        setup: function () {
            return {
                options: {
                    title: "Detalles insumo:",
                    basic: true,
                    maximizable: false,
                    resizable: false,
                    padding: true
                }
            };
        },
        build: function () {
            $(this.elements.dialog).css("max-width", "40%");
            $(this.elements.header).css("color", "black");

        },
        settings: {
            selector: undefined
        },
        hooks: {
            onclose: function () {
                $("#imagenInsumo").empty();
                $("#nombreInsumo").empty();
                $("#unidad").empty();
                $("#stockMin").empty();
                $("#stock").empty();
                $("#tbodyPresentaciones").empty();
                $("#tbodyMarcas").empty();
                $("#txtCantidadMerma").removeAttr("max");
                $("#btnDescontar").removeAttr("id");
                $("#ultimaCompra").empty();
                $("#bajasMerma").empty();
            }
        }
    };
});

//Metodo de busqueda de insumos en el inventario por escritura de letras
$("#txtBuscarInventario").keyup(function () {
    var insumos = JSON.parse(modelo);
    var busqueda = $(this).val();

    $("#tarjetas").empty();
    $.each(insumos, function (index, value) {
        if (value["cantidad"] <= value["stockMinimo"]) {
            color = "#E74C3C";
        }
        else if ((value["cantidad"] - 2) <= value["stockMinimo"]) {
            color = "#F1948A";
        }
        else {
            color = "white";
        }

        if (value["nombre"].indexOf(busqueda) !== -1) {
            $("#tarjetas").append(
                '<div class="col-md-2 col-sm-12">' +
                '<div class="cardInsumo insumoInventario" id="' + value["id"] + '" style="background-color:' + color + ';">' +
                '<div class="imgText">' +
                '<div class="img">' +
                '<img src="' + value["rutaImagen"] + '">' +
                '</div>' +
                '<h2>' + value["nombre"] + '</h2>' +
                '<span>Stock min: <span>' + value["stockMinimo"] + '</span></span>' +
                '<span>Stock: <span>' + value["cantidad"] + '</span></span>' +
                '<br />' +
                '</div>' +
                '</div>' +
                '</div>'
            );
        }
    });
});