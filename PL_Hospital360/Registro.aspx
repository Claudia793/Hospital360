<%@ Page Title="Crear cuenta" Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="PL_Hospital360.Registro" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Hospital360 - Crear cuenta</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="Content/Sitio.css" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

        <div class="registro-container card shadow-sm p-4">
            <h2 class="text-center">Crear cuenta nueva</h2>

            <div id="divMensaje" class="alert d-none" role="alert"></div>

            <div class="mb-3">
                <label class="form-label">Nombre completo</label>
                <input type="text" id="txtNombreCompleto" class="form-control" />
            </div>
            <div class="row">
                <div class="col mb-3">
                    <label class="form-label">Correo electrónico</label>
                    <input type="email" id="txtCorreo" class="form-control" />
                </div>
                <div class="col mb-3">
                    <label class="form-label">Teléfono</label>
                    <input type="text" id="txtTelefono" class="form-control" />
                </div>
            </div>
            <div class="mb-3">
                <label class="form-label">Tipo de clínica</label>
                <select id="ddlTipoClinica" class="form-select">
                    <option value="Medicina General">Medicina General</option>
                    <option value="Clínica estética">Clínica estética</option>
                    <option value="Odontología">Odontología</option>
                    <option value="Fisioterapia">Fisioterapia</option>
                </select>
            </div>
            <div class="mb-3">
                <label class="form-label">Nombre de usuario</label>
                <input type="text" id="txtNombreUsuario" class="form-control" />
            </div>
            <div class="row">
                <div class="col mb-3">
                    <label class="form-label">Contraseña</label>
                    <input type="password" id="txtContrasena" class="form-control" />
                </div>
                <div class="col mb-3">
                    <label class="form-label">Confirmar contraseña</label>
                    <input type="password" id="txtConfirmarContrasena" class="form-control" />
                </div>
            </div>

            <button type="button" id="btnCrearCuenta" class="btn btn-primary w-100">Crear mi cuenta</button>

            <p class="text-center mt-3">¿Ya tienes cuenta? <a href="Login.aspx">Iniciar sesión</a></p>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="JavaScript/Registro.js"></script>
</body>
</html>
