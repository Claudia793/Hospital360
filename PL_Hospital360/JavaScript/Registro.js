$(function () {
    $("#btnCrearCuenta").on("click", crearCuenta);
});

function crearCuenta() {
    var sNombreCompleto = $("#txtNombreCompleto").val().trim();
    var sCorreo = $("#txtCorreo").val().trim();
    var sTelefono = $("#txtTelefono").val().trim();
    var sCedula = $("#txtCedula").val().trim();
    var sTipoClinica = $("#ddlTipoClinica").val();
    var sNombreUsuario = $("#txtNombreUsuario").val().trim();
    var sContrasena = $("#txtContrasena").val();
    var sConfirmarContrasena = $("#txtConfirmarContrasena").val();

    ocultarMensaje();

    if (!sNombreCompleto || !sCorreo || !sTelefono || !sCedula || !sNombreUsuario || !sContrasena || !sConfirmarContrasena) {
        mostrarMensaje("Completa todos los campos.", "danger");
        return;
    }

    if (!/^[A-Za-zÀ-ÿ ]+$/.test(sNombreCompleto)) {
        mostrarMensaje("El nombre completo solo puede contener letras y espacios.", "danger");
        return;
    }

    if (!/^[0-9]{9}$/.test(sCedula)) {
        mostrarMensaje("La cédula debe tener exactamente 9 dígitos numéricos.", "danger");
        return;
    }

    if (sContrasena !== sConfirmarContrasena) {
        mostrarMensaje("Las contraseñas no coinciden.", "danger");
        return;
    }

    PageMethods.Registrar(
        sNombreCompleto, sCorreo, sTelefono, sCedula, sTipoClinica,
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
