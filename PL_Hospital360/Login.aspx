<%@ Page Title="Iniciar sesión" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PL_Hospital360.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Hospital360 - Iniciar sesión</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="Content/site.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

        <div class="d-flex" style="min-height:100vh;">
            <!-- Panel izquierdo -->
            <div class="h360-login-left d-none d-lg-flex flex-column justify-content-between p-5" style="width:50%;">
                <div class="d-flex align-items-center gap-2">
                    <div class="d-flex align-items-center justify-content-center rounded-3" style="width:2.5rem;height:2.5rem;background:rgba(255,255,255,.15);">
                        <span class="fw-bold fs-4">+</span>
                    </div>
                    <div>
                        <p class="h360-brand mb-0 fs-4">HOSPITAL<span style="color:var(--h360-green);">360</span></p>
                        <p class="mb-0 small" style="color:rgba(255,255,255,.6);">Gestión Clínica</p>
                    </div>
                </div>

                <div>
                    <h2 class="h360-brand mb-3" style="font-size:2.25rem;line-height:1.2;">
                        La gestión <span style="color:var(--h360-green);">inteligente</span><br />para clínicas modernas
                    </h2>
                    <p style="color:#c7d6f7;">
                        Administra pacientes, citas, membresías y reportes desde una sola plataforma segura.
                    </p>
                    <div class="row g-3 mt-3">
                        <div class="col-6"><div class="h360-stat-pill d-flex align-items-center gap-2"><i class="bi bi-calendar3" style="color:var(--h360-green);"></i><span class="small" style="color:#a9c0ef;">Citas y agenda</span></div></div>
                        <div class="col-6"><div class="h360-stat-pill d-flex align-items-center gap-2"><i class="bi bi-gem" style="color:var(--h360-green);"></i><span class="small" style="color:#a9c0ef;">Membresías</span></div></div>
                        <div class="col-6"><div class="h360-stat-pill d-flex align-items-center gap-2"><i class="bi bi-bar-chart-fill" style="color:var(--h360-green);"></i><span class="small" style="color:#a9c0ef;">Reportes</span></div></div>
                        <div class="col-6"><div class="h360-stat-pill d-flex align-items-center gap-2"><i class="bi bi-shield-check" style="color:var(--h360-green);"></i><span class="small" style="color:#a9c0ef;">100% seguro</span></div></div>
                    </div>
                </div>

                <p class="small mb-0" style="color:#a9c0ef;">© 2026 Hospital360 · Tu clínica, en buenas manos.</p>
            </div>

            <!-- Panel derecho -->
            <div class="d-flex align-items-center justify-content-center flex-grow-1 p-4">
                <div style="max-width:24rem;width:100%;">
                    <h1 class="h360-brand" style="font-size:1.6rem;">Bienvenido de vuelta</h1>
                    <p style="color:var(--h360-muted);">Inicia sesión en tu cuenta</p>

                    <div id="divMensaje" class="d-none align-items-center gap-2 rounded-3 px-3 py-2 mb-2 small" style="background:rgba(239,68,68,.08);border:1px solid rgba(239,68,68,.2);color:var(--h360-red);"></div>

                    <div class="mb-3">
                        <label class="h360-label">Usuario o correo electrónico</label>
                        <input type="text" id="txtNombreUsuario" class="form-control h360-input" placeholder="tu@correo.com" />
                    </div>
                    <div class="mb-2">
                        <label class="h360-label">Contraseña</label>
                        <div class="position-relative">
                            <input type="password" id="txtContrasena" class="form-control h360-input" placeholder="••••••••" style="padding-right:2.6rem;" />
                            <button type="button" id="btnTogglePass" class="btn btn-sm position-absolute top-0 end-0 mt-1 me-1 border-0" style="color:var(--h360-muted);"><i class="bi bi-eye"></i></button>
                        </div>
                    </div>

                    <div class="text-end mb-3">
                        <a href="#" class="small fw-semibold text-decoration-none" style="color:var(--h360-blue);">¿Olvidaste tu contraseña?</a>
                    </div>

                    <button type="button" id="btnIniciarSesion" class="btn-h360 btn-h360-primary w-100">Iniciar sesión</button>

                    <p class="text-center small mt-3" style="color:var(--h360-muted);">
                        ¿No tienes cuenta?
                        <a href="Registro.aspx" class="fw-bold text-decoration-none" style="color:var(--h360-blue);">Crear cuenta</a>
                    </p>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="JavaScript/InicioSesion.js"></script>
</body>
</html>
