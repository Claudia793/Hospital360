var ESTADO_BADGE = {
    "Pendiente": "badge-amber",
    "Confirmada": "badge-blue",
    "Finalizada": "badge-green",
    "Cancelada": "badge-red",
    "No asistió": "badge-muted"
};
var MEMBRESIA_BADGE = { "Premium": "badge-purple", "Plus": "badge-amber", "Básica": "badge-blue" };

var COLOR_ESTADO = {
    "Confirmada": "#2563eb",
    "Pendiente": "#f59e0b",
    "Finalizada": "#22c55e",
    "Cancelada": "#ef4444",
    "No asistió": "#8b5cf6"
};
var COLOR_ESTADO_DEFECTO = "#5a6e99";
var PALETA_CATEGORICA = ["#2563eb", "#f59e0b", "#06b6d4", "#8b5cf6", "#22c55e", "#ef4444"];

var modalCita = null;
var modalEliminar = null;
var oListaActual = [];
var idAEliminar = null;
var datosPorTipo = [];
var datosPorProfesional = [];
var chartTercero = null;
var chartIngresos = null;
var chartCitasSemana = null;
var chartEstado = null;
var vistaActualTercero = "tipo";

$(function () {
    modalCita = new bootstrap.Modal(document.getElementById("modalCita"));
    modalEliminar = new bootstrap.Modal(document.getElementById("modalEliminar"));

    cargarCatalogos();
    filtrarCitas();

    $("#btnFiltrar").on("click", filtrarCitas);
    $("#btnLimpiarFiltros").on("click", limpiarFiltros);
    $("#btnNuevaCita").on("click", abrirModalNuevaCita);
    $("#btnGuardarCita").on("click", guardarCita);
    $("#btnConfirmarEliminar").on("click", confirmarEliminar);
    $("#btnVerPorTipo").on("click", function () { alternarChartTercero("tipo"); });
    $("#btnVerPorProfesional").on("click", function () { alternarChartTercero("profesional"); });
    $("#lnkCerrarSesion").on("click", function (e) {
        e.preventDefault();
        cerrarSesion();
    });
});

// ---------- Catálogos ----------
function cargarCatalogos() {
    PageMethods.CargarCatalogos(onExitoCatalogos, onErrorCarga);
}

function onExitoCatalogos(catalogos) {
    llenarCombo("#ddlEstadoCita", catalogos.EstadosCita);
    llenarCombo("#ddlProfesional", catalogos.Profesionales);
    llenarCombo("#ddlTipoCita", catalogos.TiposCita);
    llenarCombo("#ddlMembresia", catalogos.TiposMembresia);

    llenarCombo("#ddlEstadoCitaForm", catalogos.EstadosCita);
    llenarCombo("#ddlProfesionalForm", catalogos.Profesionales);
    llenarCombo("#ddlTipoCitaForm", catalogos.TiposCita);
    llenarCombo("#ddlMembresiaForm", catalogos.TiposMembresia);
}

function llenarCombo(sSelector, lista) {
    var $combo = $(sSelector);
    (lista || []).forEach(function (item) {
        $combo.append($("<option></option>").val(item.Id).text(item.Nombre));
    });
}

// ---------- Carga / filtro ----------
function obtenerFiltrosActuales() {
    return {
        sNombrePaciente: $("#txtBuscar").val(),
        sFecha: $("#txtFecha").val(),
        iIdEstadoCita: valorComboONull("#ddlEstadoCita"),
        iIdProfesional: valorComboONull("#ddlProfesional"),
        iIdTipoCita: valorComboONull("#ddlTipoCita"),
        iIdTipoMembresia: valorComboONull("#ddlMembresia")
    };
}

function filtrarCitas() {
    var f = obtenerFiltrosActuales();

    ocultarMensaje();

    PageMethods.FiltrarCitas(
        f.sNombrePaciente, f.sFecha, f.iIdEstadoCita, f.iIdProfesional, f.iIdTipoCita, f.iIdTipoMembresia,
        onExitoFiltro, onErrorCarga
    );

    pintarGraficos(f);
}

function limpiarFiltros() {
    $("#txtBuscar").val("");
    $("#ddlEstadoCita, #ddlProfesional, #ddlTipoCita, #ddlMembresia").val("");
    $("#txtFecha").val("");
    filtrarCitas();
}

function valorComboONull(sSelector) {
    var sValor = $(sSelector).val();
    return sValor ? parseInt(sValor, 10) : null;
}

function onExitoFiltro(lista) {
    oListaActual = lista || [];
    pintarTabla(oListaActual);
    pintarKPIs(oListaActual);
}

function onErrorCarga(error) {
    mostrarMensaje("ERROR: No se pudo contactar al servidor (" + error.get_message() + ")", "danger");
}

// ---------- Tabla ----------
function pintarTabla(lista) {
    var $tbody = $("#tbodyCitas");
    $tbody.empty();
    $("#lblContador").text("(" + lista.length + " resultados)");

    if (lista.length === 0) {
        $tbody.append("<tr><td colspan='8' class='text-center py-4' style='color:var(--h360-muted);'>No se encontraron citas con esos filtros.</td></tr>");
        return;
    }

    lista.forEach(function (cita) {
        var $fila = $("<tr></tr>");

        $fila.append($("<td></td>").html("<strong>" + escaparHtml(cita.NombrePaciente) + "</strong>"));
        $fila.append($("<td></td>").text(cita.Telefono || ""));
        $fila.append($("<td></td>").html("<span class='badge-h360 badge-blue'>" + escaparHtml(cita.NombreTipoCita) + "</span>"));

        var sBadgeMembresia = MEMBRESIA_BADGE[cita.NombreMembresia] || "badge-muted";
        $fila.append($("<td></td>").html(cita.NombreMembresia ? "<span class='badge-h360 " + sBadgeMembresia + "'>" + escaparHtml(cita.NombreMembresia) + "</span>" : "<span class='small' style='color:var(--h360-muted);'>Ninguna</span>"));

        var sBadgeEstado = ESTADO_BADGE[cita.NombreEstado] || "badge-blue";
        $fila.append($("<td></td>").html("<span class='badge-h360 " + sBadgeEstado + "'>" + escaparHtml(cita.NombreEstado) + "</span>"));

        $fila.append($("<td></td>").text(cita.Fecha || ""));
        $fila.append($("<td></td>").html("<strong style='color:var(--h360-blue);'>" + (cita.Hora || "") + "</strong>"));

        var $acciones = $("<td></td>");
        $("<button type='button' class='btn btn-sm p-1 me-1' title='Editar'>✏️</button>")
            .on("click", function () { abrirModalEditar(cita.IdCita); })
            .appendTo($acciones);
        $("<button type='button' class='btn btn-sm p-1' title='Eliminar'>🗑️</button>")
            .on("click", function () { pedirEliminar(cita.IdCita, cita.NombrePaciente); })
            .appendTo($acciones);
        $fila.append($acciones);

        $tbody.append($fila);
    });
}

function escaparHtml(sTexto) {
    return $("<div></div>").text(sTexto || "").html();
}

function pintarKPIs(lista) {
    var iActivas = lista.filter(function (r) { return r.NombreEstado === "Pendiente" || r.NombreEstado === "Confirmada"; }).length;
    var iMembresiasActivas = lista.filter(function (r) { return !!r.NombreMembresia; }).length;

    $("#lblTotalRegistros").text(lista.length);
    $("#lblCitasActivas").text(iActivas);
    $("#lblPremium").text(iMembresiasActivas);
    // "lblIngresos" se actualiza aparte, con el reporte real de ingresos (ver pintarGraficos).
}

// ---------- Modal Nueva/Editar ----------
function abrirModalNuevaCita() {
    limpiarFormularioCita();
    $("#tituloModalCita").text("Nuevo Registro");
    $("#btnGuardarCita").html("<i class='bi bi-plus-lg'></i> Registrar");
    modalCita.show();
}

function abrirModalEditar(idCita) {
    PageMethods.ObtenerCita(idCita, function (cita) {
        if (!cita.Encontrada) {
            mostrarMensaje("No se encontró la cita solicitada.", "danger");
            return;
        }

        limpiarFormularioCita();
        $("#tituloModalCita").text("Editar Registro");
        $("#btnGuardarCita").html("<i class='bi bi-check-lg'></i> Guardar cambios");
        $("#hdnIdCita").val(cita.IdCita);
        $("#txtNombrePaciente").val(cita.NombrePaciente);
        $("#txtTelefonoCita").val(cita.Telefono);
        $("#txtCorreoCita").val(cita.Correo);
        $("#txtFechaCita").val(cita.Fecha);
        $("#txtHoraCita").val(cita.Hora);
        $("#ddlTipoCitaForm").val(cita.IdTipoCita);
        $("#ddlMembresiaForm").val(cita.IdTipoMembresia || "");
        $("#ddlEstadoCitaForm").val(cita.IdEstadoCita);
        $("#ddlProfesionalForm").val(cita.IdProfesional);
        $("#txtObservacionesCita").val(cita.Observaciones);
        $("#txtFechaNacimientoCita").val(cita.FechaNacimiento);

        modalCita.show();
    }, onErrorCarga);
}

function limpiarFormularioCita() {
    $("#hdnIdCita").val(0);
    $("#txtNombrePaciente, #txtTelefonoCita, #txtCorreoCita, #txtFechaCita, #txtHoraCita, #txtObservacionesCita, #txtFechaNacimientoCita").val("");
    $("#ddlTipoCitaForm, #ddlMembresiaForm, #ddlEstadoCitaForm, #ddlProfesionalForm").val("");
}

function guardarCita() {
    var iIdCita = parseInt($("#hdnIdCita").val(), 10) || 0;
    var sNombrePaciente = $("#txtNombrePaciente").val().trim();
    var sTelefono = $("#txtTelefonoCita").val().trim();
    var sCorreo = $("#txtCorreoCita").val().trim();
    var sFecha = $("#txtFechaCita").val();
    var sHora = $("#txtHoraCita").val();
    var iIdTipoCita = valorComboONull("#ddlTipoCitaForm");
    var iIdTipoMembresia = valorComboONull("#ddlMembresiaForm");
    var iIdEstadoCita = valorComboONull("#ddlEstadoCitaForm");
    var iIdProfesional = valorComboONull("#ddlProfesionalForm");
    var sObservaciones = $("#txtObservacionesCita").val().trim();
    var sFechaNacimiento = $("#txtFechaNacimientoCita").val();

    if (!sNombrePaciente || !sTelefono || !sCorreo || !sFecha || !sHora ||
        !iIdTipoCita || !iIdEstadoCita || !iIdProfesional || !sFechaNacimiento) {
        mostrarMensaje("Completa todos los campos obligatorios.", "danger");
        return;
    }

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(sCorreo)) {
        mostrarMensaje("El correo no tiene un formato válido.", "danger");
        return;
    }

    var hoy = new Date(); hoy.setHours(0, 0, 0, 0);

    if (new Date(sFechaNacimiento + "T00:00:00") > hoy) {
        mostrarMensaje("La fecha de nacimiento no puede ser futura.", "danger");
        return;
    }

    if (iIdCita === 0 && new Date(sFecha + "T00:00:00") < hoy) {
        mostrarMensaje("La fecha de la cita no puede ser en el pasado.", "danger");
        return;
    }

    ocultarMensaje();

    var alTerminar = function (resultado) {
        if (resultado.Exito) {
            modalCita.hide();
            mostrarMensaje("Cita guardada correctamente.", "success");
            filtrarCitas();
            pintarGraficos();
        } else {
            mostrarMensaje(resultado.Mensaje, "danger");
        }
    };

    if (iIdCita === 0) {
        PageMethods.CrearCita(
            sNombrePaciente, sTelefono, sCorreo, sFecha, sHora,
            iIdTipoCita, iIdTipoMembresia, iIdEstadoCita, iIdProfesional,
            sObservaciones, sFechaNacimiento,
            alTerminar, onErrorCarga
        );
    } else {
        PageMethods.ActualizarCita(
            iIdCita, sNombrePaciente, sTelefono, sCorreo, sFecha, sHora,
            iIdTipoCita, iIdTipoMembresia, iIdEstadoCita, iIdProfesional,
            sObservaciones, sFechaNacimiento,
            alTerminar, onErrorCarga
        );
    }
}

// ---------- Eliminar ----------
function pedirEliminar(idCita, sNombre) {
    idAEliminar = idCita;
    $("#lblEliminarNombre").text(sNombre);
    modalEliminar.show();
}

function confirmarEliminar() {
    PageMethods.EliminarCita(idAEliminar, function (resultado) {
        modalEliminar.hide();
        if (resultado.Exito) {
            mostrarMensaje("Cita eliminada correctamente.", "success");
            filtrarCitas();
            pintarGraficos();
        } else {
            mostrarMensaje(resultado.Mensaje, "danger");
        }
    }, onErrorCarga);
}

// ---------- Gráficos ----------
// Todos los gráficos respetan los mismos filtros que la grilla (nombre,
// fecha, estado, profesional, tipo de atención, membresía): se vuelven a
// pedir cada vez que se filtra, y actualizan la instancia existente en
// vez de recrear el canvas.
function pintarGraficos(filtro) {
    filtro = filtro || obtenerFiltrosActuales();

    PageMethods.ReporteIngresosPorMes(
        filtro.sNombrePaciente, filtro.sFecha, filtro.iIdEstadoCita, filtro.iIdProfesional, filtro.iIdTipoCita, filtro.iIdTipoMembresia,
        function (lista) {
            var etiquetas = lista.map(function (i) { return i.Etiqueta; });
            var valores = lista.map(function (i) { return i.Cantidad; });

            if (!chartIngresos) {
                chartIngresos = new Chart(document.getElementById("chartIngresos"), {
                    type: "line",
                    data: {
                        labels: etiquetas,
                        datasets: [{
                            data: valores, borderColor: "#1a3a8f", backgroundColor: "rgba(26,58,143,.15)",
                            fill: true, tension: 0.4
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: { legend: { display: false } },
                        scales: { y: { beginAtZero: true, ticks: { callback: function (v) { return "₡" + v.toLocaleString("es-CR"); } } } }
                    }
                });
            } else {
                chartIngresos.data.labels = etiquetas;
                chartIngresos.data.datasets[0].data = valores;
                chartIngresos.update();
            }

            var iMesActual = valores.length > 0 ? valores[valores.length - 1] : 0;
            $("#lblIngresos").text("₡" + iMesActual.toLocaleString("es-CR"));
        }, onErrorCarga);

    PageMethods.ReporteCitasSemanaActual(
        filtro.sNombrePaciente, filtro.iIdEstadoCita, filtro.iIdProfesional, filtro.iIdTipoCita, filtro.iIdTipoMembresia,
        function (lista) {
            var etiquetas = lista.map(function (i) { return i.Etiqueta; });
            var valores = lista.map(function (i) { return i.Cantidad; });

            if (!chartCitasSemana) {
                chartCitasSemana = new Chart(document.getElementById("chartCitasSemana"), {
                    type: "bar",
                    data: { labels: etiquetas, datasets: [{ data: valores, backgroundColor: "#1a3a8f", borderRadius: 8 }] },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: { legend: { display: false } },
                        scales: { y: { beginAtZero: true, ticks: { precision: 0 } } }
                    }
                });
            } else {
                chartCitasSemana.data.labels = etiquetas;
                chartCitasSemana.data.datasets[0].data = valores;
                chartCitasSemana.update();
            }
        }, onErrorCarga);

    PageMethods.ReporteCitasPorEstado(
        filtro.sNombrePaciente, filtro.sFecha, filtro.iIdEstadoCita, filtro.iIdProfesional, filtro.iIdTipoCita, filtro.iIdTipoMembresia,
        function (lista) {
            var etiquetas = lista.map(function (i) { return i.Etiqueta; });
            var valores = lista.map(function (i) { return i.Cantidad; });
            var colores = etiquetas.map(function (e) { return COLOR_ESTADO[e] || COLOR_ESTADO_DEFECTO; });

            if (!chartEstado) {
                chartEstado = new Chart(document.getElementById("chartEstado"), {
                    type: "doughnut",
                    data: { labels: etiquetas, datasets: [{ data: valores, backgroundColor: colores }] },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: { legend: { position: "bottom" } }
                    }
                });
            } else {
                chartEstado.data.labels = etiquetas;
                chartEstado.data.datasets[0].data = valores;
                chartEstado.data.datasets[0].backgroundColor = colores;
                chartEstado.update();
            }

            var $tbody = $("#tablaEstado tbody");
            $tbody.empty();
            lista.forEach(function (item) {
                $tbody.append("<tr><td>" + escaparHtml(item.Etiqueta) + "</td><td class='text-end'>" + item.Cantidad + "</td></tr>");
            });
        }, onErrorCarga);

    PageMethods.ReporteCitasPorTipoCita(
        filtro.sNombrePaciente, filtro.sFecha, filtro.iIdEstadoCita, filtro.iIdProfesional, filtro.iIdTipoCita, filtro.iIdTipoMembresia,
        function (lista) {
            datosPorTipo = lista;
            if (vistaActualTercero === "tipo") actualizarChartTercero(lista);
        }, onErrorCarga);

    PageMethods.ReporteCitasPorProfesional(
        filtro.sNombrePaciente, filtro.sFecha, filtro.iIdEstadoCita, filtro.iIdProfesional, filtro.iIdTipoCita, filtro.iIdTipoMembresia,
        function (lista) {
            datosPorProfesional = lista;
            if (vistaActualTercero === "profesional") actualizarChartTercero(lista);
        }, onErrorCarga);
}

function actualizarChartTercero(lista) {
    var etiquetas = lista.map(function (i) { return i.Etiqueta; });
    var valores = lista.map(function (i) { return i.Cantidad; });
    var colores = etiquetas.map(function (_, idx) { return PALETA_CATEGORICA[idx % PALETA_CATEGORICA.length]; });

    if (!chartTercero) {
        chartTercero = new Chart(document.getElementById("chartTercero"), {
            type: "bar",
            data: { labels: etiquetas, datasets: [{ data: valores, backgroundColor: colores, borderRadius: 4 }] },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: { legend: { display: false } },
                scales: { y: { beginAtZero: true, ticks: { precision: 0 } } }
            }
        });
    } else {
        chartTercero.data.labels = etiquetas;
        chartTercero.data.datasets[0].data = valores;
        chartTercero.data.datasets[0].backgroundColor = colores;
        chartTercero.update();
    }
}

function alternarChartTercero(vista) {
    vistaActualTercero = vista;
    var lista = vista === "tipo" ? datosPorTipo : datosPorProfesional;

    $("#tituloChartTercero").text(vista === "tipo" ? "Citas por tipo de atención" : "Citas por profesional");
    $("#btnVerPorTipo").toggleClass("active", vista === "tipo");
    $("#btnVerPorProfesional").toggleClass("active", vista === "profesional");

    actualizarChartTercero(lista);
}

// ---------- Sesión / mensajes ----------
function cerrarSesion() {
    PageMethods.CerrarSesion(function () {
        window.location.href = "Login.aspx";
    }, onErrorCarga);
}

function mostrarMensaje(sTexto, sTipo) {
    $("#divMensaje")
        .removeClass("d-none alert-success alert-danger")
        .addClass("alert-" + sTipo)
        .text(sTexto);
}

function ocultarMensaje() {
    $("#divMensaje").addClass("d-none");
}
