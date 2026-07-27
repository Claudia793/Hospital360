$(function () {
    $("#btnIniciarSesion").on("click", iniciarSesion);

    $("#txtNombreUsuario, #txtContrasena").on("keypress", function (e) {
        if (e.which === 13) iniciarSesion();
    });

    // Mostrar/ocultar contraseña
    $("#btnTogglePass").on("click", function () {
        var $input = $("#txtContrasena");
        var $icon = $(this).find("i");
        if ($input.attr("type") === "password") {
            $input.attr("type", "text");
            $icon.removeClass("bi-eye").addClass("bi-eye-slash");
        } else {
            $input.attr("type", "password");
            $icon.removeClass("bi-eye-slash").addClass("bi-eye");
        }
    });
});

function iniciarSesion() {
    var sNombreUsuario = $("#txtNombreUsuario").val();
    var sContrasena = $("#txtContrasena").val();

    ocultarMensaje();

    if (!sNombreUsuario || !sContrasena) {
        mostrarMensaje("Completa todos los campos.", "danger");
        return;
    }

    PageMethods.IniciarSesion(sNombreUsuario, sContrasena, onExitoLogin, onErrorLogin);
}

function onExitoLogin(resultado) {
    if (resultado.Exito) {
        window.location.href = "Citas.aspx";
    } else {
        mostrarMensaje(resultado.Mensaje, "danger");
    }
}

function onErrorLogin(error) {
    mostrarMensaje("ERROR: No se pudo contactar al servidor (" + error.get_message() + ")", "danger");
}

function mostrarMensaje(sTexto, sTipo) {
    var sIcono = sTipo === "danger" ? '<i class="bi bi-exclamation-circle"></i> ' : '<i class="bi bi-check-circle"></i> ';
    $("#divMensaje")
        .removeClass("d-none alert-success alert-danger")
        .addClass("alert-" + sTipo)
        .html(sIcono + sTexto);
}

function ocultarMensaje() {
    $("#divMensaje").addClass("d-none");
}