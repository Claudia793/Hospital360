$(function () {
    $("#btnCrearCuenta").on("click", crearCuenta);
});

function crearCuenta() {
    var sNombreCompleto = $("#txtNombreCompleto").val();
    var sCorreo = $("#txtCorreo").val();
    var sTelefono = $("#txtTelefono").val();
    var sTipoClinica = $("#ddlTipoClinica").val();
    var sNombreUsuario = $("#txtNombreUsuario").val();
    var sContrasena = $("#txtContrasena").val();
    var sConfirmarContrasena = $("#txtConfirmarContrasena").val();

    ocultarMensaje();

    if (!sNombreCompleto || !sCorreo || !sTelefono || !sNombreUsuario || !sContrasena || !sConfirmarContrasena) {
        mostrarMensaje("Completa todos los campos.", "danger");
        return;
    }

    if (sContrasena !== sConfirmarContrasena) {
        mostrarMensaje("Las contraseñas no coinciden.", "danger");
        return;
    }

    PageMethods.Registrar(
        sNombreCompleto, sCorreo, sTelefono, sTipoClinica,
        sNombreUsuario, sContrasena, sConfirmarContrasena,
        onExitoRegistro, onErrorRegistro
    );
}

function onExitoRegistro(resultado) {
    if (resultado.Exito) {
        mostrarMensaje("Cuenta creada correctamente. Redirigiendo al inicio de sesión...", "success");
        setTimeout(function () {
            window.location.href = "Login.aspx";
        }, 1500);
    } else {
        mostrarMensaje(resultado.Mensaje, "danger");
    }
}

function onErrorRegistro(error) {
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
