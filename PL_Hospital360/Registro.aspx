<%@ Page Title="Crear cuenta" Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="PL_Hospital360.Registro" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Hospital360 - Crear cuenta</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

        <div class="d-flex align-items-center justify-content-center py-5" style="min-height:100vh;">
            <div style="max-width:34rem;width:100%;" class="px-3">

                <div class="text-center mb-4">
                    <p class="h360-brand mb-0 fs-3">HOSPITAL<span style="color:var(--h360-green);">360</span></p>
                    <h1 class="h360-brand mt-2" style="font-size:1.5rem;">Crear cuenta nueva</h1>
                    <p style="color:var(--h360-muted);">Completa el formulario para registrarte</p>
                </div>

                <div class="h360-card p-4">
                    <div id="divMensaje" class="alert d-none" role="alert"></div>

                    <div class="row g-3">
                        <div class="col-sm-6">
                            <label class="h360-label">Nombre completo</label>
                            <input type="text" id="txtNombreCompleto" class="form-control h360-input" placeholder="María López Solano" />
                        </div>
                        <div class="col-sm-6">
                            <label class="h360-label">Correo electrónico</label>
                            <input type="email" id="txtCorreo" class="form-control h360-input" placeholder="tu@correo.com" />
                        </div>
                        <div class="col-sm-6">
                            <label class="h360-label">Teléfono</label>
                            <input type="text" id="txtTelefono" class="form-control h360-input" placeholder="8888-0000" />
                        </div>
                        <div class="col-sm-6">
                            <label class="h360-label">Cédula</label>
                            <input type="text" id="txtCedula" class="form-control h360-input" placeholder="9 dígitos" maxlength="9" />
                        </div>
                        <div class="col-sm-6">
                            <label class="h360-label">Tipo de clínica</label>
                            <select id="ddlTipoClinica" class="form-select h360-input">
                                <option value="Medicina General">Medicina General</option>
                                <option value="Clínica estética">Clínica estética</option>
                                <option value="Odontología">Odontología</option>
                                <option value="Fisioterapia">Fisioterapia</option>
                            </select>
                        </div>
                        <div class="col-sm-6">
                            <label class="h360-label">Nombre de usuario</label>
                            <input type="text" id="txtNombreUsuario" class="form-control h360-input" placeholder="usuario" />
                        </div>
                        <div class="col-sm-6">
                            <label class="h360-label">Contraseña</label>
                            <input type="password" id="txtContrasena" class="form-control h360-input" placeholder="Mínimo 6 caracteres" />
                        </div>
                        <div class="col-sm-6">
                            <label class="h360-label">Confirmar contraseña</label>
                            <input type="password" id="txtConfirmarContrasena" class="form-control h360-input" placeholder="Repite tu contraseña" />
                        </div>
                        <div class="col-12">
                            <button type="button" id="btnCrearCuenta" class="btn-h360 btn-h360-primary w-100">Crear mi cuenta</button>
                        </div>
                    </div>
                </div>

                <p class="text-center small mt-3" style="color:var(--h360-muted);">
                    ¿Ya tienes cuenta?
                    <a href="Login.aspx" class="fw-bold text-decoration-none" style="color:var(--h360-blue);">Iniciar sesión</a>
                </p>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="JavaScript/Registro.js"></script>
</body>
</html>
