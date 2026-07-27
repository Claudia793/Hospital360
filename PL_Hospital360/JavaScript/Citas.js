$(function () {
    cargarCatalogos();
    filtrarCitas();

    $("#btnFiltrar").on("click", filtrarCitas);
    $("#lnkCerrarSesion").on("click", function (e) {
        e.preventDefault();
        cerrarSesion();
    });
});

function cargarCatalogos() {
    PageMethods.CargarCatalogos(onExitoCatalogos, onErrorCarga);
}

function onExitoCatalogos(catalogos) {
    llenarCombo("#ddlEstadoCita", catalogos.EstadosCita);
    llenarCombo("#ddlProfesional", catalogos.Profesionales);
    llenarCombo("#ddlTipoCita", catalogos.TiposCita);
    llenarCombo("#ddlMembresia", catalogos.TiposMembresia);
}

function llenarCombo(sSelector, lista) {
    var $combo = $(sSelector);
    (lista || []).forEach(function (item) {
        $combo.append($("<option></option>").val(item.Id).text(item.Nombre));
    });
}

function filtrarCitas() {
    var sNombrePaciente = $("#txtBuscar").val();
    var sFecha = $("#txtFecha").val();
    var iIdEstadoCita = valorComboONull("#ddlEstadoCita");
    var iIdProfesional = valorComboONull("#ddlProfesional");
    var iIdTipoCita = valorComboONull("#ddlTipoCita");
    var iIdTipoMembresia = valorComboONull("#ddlMembresia");

    ocultarMensaje();

    PageMethods.FiltrarCitas(
        sNombrePaciente, sFecha, iIdEstadoCita, iIdProfesional, iIdTipoCita, iIdTipoMembresia,
        onExitoFiltro, onErrorCarga
    );
}

function valorComboONull(sSelector) {
    var sValor = $(sSelector).val();
    return sValor ? parseInt(sValor, 10) : null;
}

function onExitoFiltro(lista) {
    var $tbody = $("#tbodyCitas");
    $tbody.empty();

    if (!lista || lista.length === 0) {
        $tbody.append("<tr><td colspan='8' class='text-center text-muted'>No se encontraron citas.</td></tr>");
        return;
    }

    lista.forEach(function (cita) {
        var $fila = $("<tr></tr>");
        [cita.NombrePaciente, cita.Telefono, cita.NombreTipoCita, cita.NombreMembresia,
         cita.NombreEstado, cita.NombreProfesional, cita.Fecha, cita.Hora].forEach(function (valor) {
            $fila.append($("<td></td>").text(valor || ""));
        });
        $tbody.append($fila);
    });
}

function cerrarSesion() {
    PageMethods.CerrarSesion(function () {
        window.location.href = "Login.aspx";
    }, onErrorCarga);
}

function onErrorCarga(error) {
    mostrarMensaje("ERROR: No se pudo contactar al servidor (" + error.get_message() + ")", "danger");
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
