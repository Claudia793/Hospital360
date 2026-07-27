<%@ Page Title="Iniciar sesión" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PL_Hospital360.frmLogin" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Hospital360 - Iniciar sesión</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <link rel="stylesheet" href="Content/theme.css" />
    <link rel="stylesheet" href="Content/Sitio.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

        <div class="login-shell">
            <!-- Panel izquierdo (oculto en móvil) -->
            <div class="login-left">
                <div class="logo-row">
                    <div class="logo-box white">+</div>
                    <div>
                        <p class="logo-title white">HOSPITAL<span class="verde">360</span></p>
                        <p class="logo-sub white">Gestión Clínica</p>
                    </div>
                </div>

                <div>
                    <h2>La gestión <span class="verde">inteligente</span><br />para clínicas modernas</h2>
                    <p class="lead">Administra pacientes, expedientes, membresías, inventario y finanzas desde una sola plataforma segura.</p>

                    <div class="stat-grid">
                        <div class="stat-mini"><i class="bi bi-people-fill verde" style="font-size:20px;"></i>
                            <div><p class="val">487</p><p class="lbl">Pacientes</p></div></div>
                        <div class="stat-mini"><i class="bi bi-calendar3 verde" style="font-size:20px;"></i>
                            <div><p class="val">23</p><p class="lbl">Citas hoy</p></div></div>
                        <div class="stat-mini"><i class="bi bi-gem verde" style="font-size:20px;"></i>
                            <div><p class="val">200</p><p class="lbl">Membresías</p></div></div>
                        <div class="stat-mini"><i class="bi bi-shield-check verde" style="font-size:20px;"></i>
                            <div><p class="val">✓</p><p class="lbl">100% seguro</p></div></div>
                    </div>
                </div>

                <p class="login-footer">© 2026 Hospital360 · Tu clínica, en buenas manos.</p>
            </div>

            <!-- Panel derecho -->
            <div class="login-right">
                <div class="login-form-wrap">
                    <div class="login-mobile-logo">
                        <div class="logo-row">
                            <div class="logo-box">+</div>
                            <div>
                                <p class="logo-title">HOSPITAL<span class="verde">360</span></p>
                                <p class="logo-sub">Gestión Clínica</p>
                            </div>
                        </div>
                    </div>

                    <h1>Bienvenido de vuelta</h1>
                    <p class="text-muted-c" style="font-size:14px; margin:4px 0 24px;">Inicia sesión en tu cuenta</p>

                    <div id="divMensaje" class="alert-box d-none"></div>

                    <div class="field">
                        <label>Correo electrónico o usuario</label>
                        <div class="input-icon">
                            <i class="bi bi-envelope icon-left"></i>
                            <input type="text" id="txtNombreUsuario" placeholder="tu@correo.com" />
                        </div>
                    </div>

                    <div class="field">
                        <label>Contraseña</label>
                        <div class="input-icon">
                            <i class="bi bi-shield-lock icon-left"></i>
                            <input type="password" id="txtContrasena" class="has-toggle" placeholder="••••••••" />
                            <button type="button" id="btnTogglePass" class="toggle-pass"><i class="bi bi-eye"></i></button>
                        </div>
                    </div>

                    <div class="forgot-row"><a href="#">¿Olvidaste tu contraseña?</a></div>

                    <button type="button" id="btnIniciarSesion" class="btn-primary w-100">Iniciar sesión</button>

                    <p class="text-center mt-3" style="font-size:14px; color:var(--muted);">
                        ¿No tienes cuenta? <a href="Registro.aspx" style="color:var(--blue); font-weight:700; text-decoration:none;">Crear cuenta</a>
                    </p>

                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="JavaScript/InicioSesion.js"></script>
</body>
</html>
