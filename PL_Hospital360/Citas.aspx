<%@ Page Title="Panel de control" Language="C#" AutoEventWireup="true" CodeBehind="Citas.aspx.cs" Inherits="PL_Hospital360.Citas" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Hospital360 - Panel de control</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

        <!-- Header -->
        <header class="h360-header sticky-top">
            <div class="container-fluid px-4 py-3 d-flex align-items-center justify-content-between">
                <p class="h360-brand mb-0 fs-4">HOSPITAL<span style="color:var(--h360-green);">360</span></p>
                <div class="d-flex align-items-center gap-3">
                    <div class="h360-avatar"><%= InicialesUsuario %></div>
                    <div class="d-none d-sm-block">
                        <p class="mb-0 small fw-semibold"><%= System.Web.HttpUtility.HtmlEncode(NombreUsuarioSesion) %></p>
                        <p class="mb-0" style="color:var(--h360-muted);font-size:.75rem;">Usuario</p>
                    </div>
                    <button type="button" id="btnAbrirAuditoria" class="btn-h360 btn-h360-outline" data-bs-toggle="offcanvas" data-bs-target="#panelAuditoria"><i class="bi bi-clock-history"></i> Auditoría</button>
                    <a href="#" id="lnkCerrarSesion" class="btn-h360 btn-h360-outline">Salir</a>
                </div>
            </div>
        </header>

        <div class="container-fluid px-4 py-4">

            <h2 class="h360-brand mb-3" style="font-size:1.25rem;">Panel de control</h2>

            <div id="divMensaje" class="alert d-none" role="alert"></div>

            <!-- KPIs -->
            <div class="row g-3 mb-4">
                <div class="col-6 col-lg-3">
                    <div class="h360-card p-3 d-flex justify-content-between align-items-start">
                        <div>
                            <p class="h360-label mb-1">Total de registros</p>
                            <p class="h360-kpi-value mb-0" id="lblTotalRegistros">0</p>
                        </div>
                        <div class="h360-kpi-icon" style="background:rgba(26,58,143,.1);">📋</div>
                    </div>
                </div>
                <div class="col-6 col-lg-3">
                    <div class="h360-card p-3 d-flex justify-content-between align-items-start">
                        <div>
                            <p class="h360-label mb-1">Citas activas</p>
                            <p class="h360-kpi-value mb-0" id="lblCitasActivas">0</p>
                        </div>
                        <div class="h360-kpi-icon" style="background:rgba(6,182,212,.12);">📅</div>
                    </div>
                </div>
                <div class="col-6 col-lg-3">
                    <div class="h360-card p-3 d-flex justify-content-between align-items-start">
                        <div>
                            <p class="h360-label mb-1">Ingresos del mes</p>
                            <p class="h360-kpi-value mb-0" id="lblIngresos">₡0</p>
                        </div>
                        <div class="h360-kpi-icon" style="background:rgba(34,197,94,.12);">💲</div>
                    </div>
                </div>
                <div class="col-6 col-lg-3">
                    <div class="h360-card p-3 d-flex justify-content-between align-items-start">
                        <div>
                            <p class="h360-label mb-1">Membresías activas</p>
                            <p class="h360-kpi-value mb-0" id="lblPremium">0</p>
                        </div>
                        <div class="h360-kpi-icon" style="background:rgba(139,92,246,.12);">👑</div>
                    </div>
                </div>
            </div>

            <!-- Charts: ingresos y semana actual -->
            <div class="row g-3 mb-4">
                <div class="col-lg-6">
                    <div class="h360-card p-3">
                        <p class="fw-bold mb-2 h360-brand" style="font-size:.9rem;">Ingresos mensuales (₡, últimos 12 meses)</p>
                        <div style="position:relative;height:220px;">
                            <canvas id="chartIngresos"></canvas>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="h360-card p-3">
                        <p class="fw-bold mb-2 h360-brand" style="font-size:.9rem;">Citas por día (semana actual)</p>
                        <div style="position:relative;height:220px;">
                            <canvas id="chartCitasSemana"></canvas>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Charts: estado y tipo/profesional -->
            <div class="row g-3 mb-4">
                <div class="col-lg-5">
                    <div class="h360-card p-3 h-100">
                        <p class="fw-bold mb-2 h360-brand" style="font-size:.9rem;">Citas por estado</p>
                        <div style="position:relative;height:220px;">
                            <canvas id="chartEstado"></canvas>
                        </div>
                        <table class="table table-sm mt-3 mb-0" id="tablaEstado"><tbody></tbody></table>
                    </div>
                </div>
                <div class="col-lg-7">
                    <div class="h360-card p-3 h-100">
                        <div class="d-flex justify-content-between align-items-center mb-2">
                            <p class="fw-bold mb-0 h360-brand" style="font-size:.9rem;" id="tituloChartTercero">Citas por tipo de atención</p>
                            <div class="btn-group btn-group-sm">
                                <button type="button" id="btnVerPorTipo" class="btn btn-outline-primary active">Por tipo</button>
                                <button type="button" id="btnVerPorProfesional" class="btn btn-outline-primary">Por profesional</button>
                            </div>
                        </div>
                        <div style="position:relative;height:220px;">
                            <canvas id="chartTercero"></canvas>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Filtros -->
            <div class="h360-card p-3 mb-4">
                <p class="fw-bold small mb-3">🔎 Filtros de búsqueda</p>
                <div class="row g-2">
                    <div class="col-lg-4">
                        <input type="text" id="txtBuscar" class="form-control h360-input" placeholder="Buscar por nombre..." />
                    </div>
                    <div class="col-lg-2 col-6">
                        <select id="ddlEstadoCita" class="form-select h360-input"><option value="">Estado de cita</option></select>
                    </div>
                    <div class="col-lg-2 col-6">
                        <select id="ddlProfesional" class="form-select h360-input"><option value="">Profesional</option></select>
                    </div>
                    <div class="col-lg-2 col-6">
                        <select id="ddlTipoCita" class="form-select h360-input"><option value="">Tipo de atención</option></select>
                    </div>
                    <div class="col-lg-1 col-6">
                        <select id="ddlMembresia" class="form-select h360-input"><option value="">Membresía</option></select>
                    </div>
                    <div class="col-lg-1">
                        <input type="date" id="txtFecha" class="form-control h360-input" />
                    </div>
                </div>
                <div class="mt-2 text-end">
                    <button type="button" id="btnFiltrar" class="btn-h360 btn-h360-primary">Filtrar</button>
                    <button type="button" id="btnLimpiarFiltros" class="btn-h360 btn-h360-outline">Limpiar</button>
                </div>
            </div>

            <!-- Tabla -->
            <div class="h360-card">
                <div class="d-flex justify-content-between align-items-center px-3 py-3" style="border-bottom:1px solid var(--h360-border);">
                    <p class="fw-bold mb-0 small">Citas <span id="lblContador" class="fw-normal" style="color:var(--h360-muted);">(0 resultados)</span></p>
                    <button type="button" id="btnNuevaCita" class="btn-h360 btn-h360-primary">+ Nueva cita</button>
                </div>
                <div class="table-responsive">
                    <table class="table h360-table mb-0">
                        <thead>
                            <tr>
                                <th>Paciente</th><th>Teléfono</th><th>Tipo de atención</th>
                                <th>Membresía</th><th>Estado de cita</th><th>Fecha</th><th>Hora</th><th>Acciones</th>
                            </tr>
                        </thead>
                        <tbody id="tbodyCitas"></tbody>
                    </table>
                </div>
            </div>
        </div>

        <!-- Modal Nueva/Editar cita -->
        <div class="modal fade" id="modalCita" tabindex="-1">
            <div class="modal-dialog modal-lg modal-dialog-centered">
                <div class="modal-content h360-card" style="border:none;">
                    <div class="h360-modal-header px-4 py-3 d-flex justify-content-between align-items-center">
                        <div class="d-flex align-items-center gap-3">
                            <div class="h360-icon-badge" style="background:rgba(255,255,255,.15);">
                                <i class="bi bi-plus-lg fs-5"></i>
                            </div>
                            <div>
                                <h5 class="mb-0" id="tituloModalCita">Nuevo Registro</h5>
                                <p class="mb-0 small" style="color:#c7d6f7;">Completa los datos del paciente y la cita</p>
                            </div>
                        </div>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body px-4 py-3">
                        <input type="hidden" id="hdnIdCita" value="0" />

                        <p class="h360-section-title">
                            <span class="h360-icon-badge-sm" style="background:rgba(37,99,235,.12);color:var(--h360-blue2);"><i class="bi bi-person"></i></span>
                            DATOS DEL PACIENTE
                        </p>
                        <div class="row g-3 mb-4">
                            <div class="col-12">
                                <label class="h360-label">Nombre completo</label>
                                <div class="h360-input-icon">
                                    <i class="bi bi-person"></i>
                                    <input type="text" id="txtNombrePaciente" class="form-control h360-input" placeholder="Nombre del paciente" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <label class="h360-label">Fecha de nacimiento</label>
                                <input type="date" id="txtFechaNacimientoCita" class="form-control h360-input" />
                            </div>
                            <div class="col-md-6">
                                <label class="h360-label">Teléfono</label>
                                <input type="text" id="txtTelefonoCita" class="form-control h360-input" placeholder="8888-0000" />
                            </div>
                            <div class="col-12">
                                <label class="h360-label">Correo electrónico</label>
                                <div class="h360-input-icon">
                                    <i class="bi bi-envelope"></i>
                                    <input type="email" id="txtCorreoCita" class="form-control h360-input" placeholder="paciente@correo.com" />
                                </div>
                            </div>
                        </div>

                        <p class="h360-section-title">
                            <span class="h360-icon-badge-sm" style="background:rgba(34,197,94,.12);color:var(--h360-green);"><i class="bi bi-calendar3"></i></span>
                            DATOS DE LA CITA
                        </p>
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label class="h360-label">Tipo de atención</label>
                                <select id="ddlTipoCitaForm" class="form-select h360-input"></select>
                            </div>
                            <div class="col-md-6">
                                <label class="h360-label">Tipo de membresía</label>
                                <select id="ddlMembresiaForm" class="form-select h360-input">
                                    <option value="">Ninguna</option>
                                </select>
                            </div>
                            <div class="col-md-6">
                                <label class="h360-label">Estado de la cita</label>
                                <select id="ddlEstadoCitaForm" class="form-select h360-input"></select>
                            </div>
                            <div class="col-md-6">
                                <label class="h360-label">Profesional que atenderá</label>
                                <select id="ddlProfesionalForm" class="form-select h360-input"></select>
                            </div>
                            <div class="col-md-6">
                                <label class="h360-label">Fecha</label>
                                <input type="date" id="txtFechaCita" class="form-control h360-input" />
                            </div>
                            <div class="col-md-6">
                                <label class="h360-label">Hora</label>
                                <input type="time" id="txtHoraCita" class="form-control h360-input" />
                            </div>
                            <div class="col-12">
                                <label class="h360-label">Observaciones</label>
                                <textarea id="txtObservacionesCita" class="form-control h360-input" rows="3" placeholder="Notas adicionales sobre la cita o el paciente..."></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer" style="background:var(--h360-input-bg);border-top:1px solid var(--h360-border);">
                        <button type="button" class="btn-h360 btn-h360-outline" data-bs-dismiss="modal">Cancelar</button>
                        <button type="button" id="btnGuardarCita" class="btn-h360 btn-h360-primary"><i class="bi bi-plus-lg"></i> Registrar</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal Eliminar -->
        <div class="modal fade" id="modalEliminar" tabindex="-1">
            <div class="modal-dialog modal-dialog-centered modal-sm">
                <div class="modal-content h360-card text-center p-4" style="border:none;">
                    <h5 class="h360-brand mb-2">Eliminar cita</h5>
                    <p class="small mb-3" style="color:var(--h360-muted);">
                        ¿Estás seguro de que deseas eliminar la cita de
                        <strong id="lblEliminarNombre" style="color:var(--h360-navy);"></strong>?
                        Esta acción no se puede deshacer.
                    </p>
                    <div class="d-flex gap-2">
                        <button type="button" class="btn-h360 btn-h360-outline flex-fill" data-bs-dismiss="modal">Cancelar</button>
                        <button type="button" id="btnConfirmarEliminar" class="btn-h360 btn-h360-danger flex-fill">Eliminar</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Panel Auditoría -->
        <div class="offcanvas offcanvas-end" tabindex="-1" id="panelAuditoria" style="width:min(720px, 100%);">
            <div class="offcanvas-header h360-modal-header">
                <div>
                    <h5 class="offcanvas-title mb-0">Auditoría</h5>
                    <p class="mb-0 small" style="color:#c7d6f7;">Historial de tus inicios de sesión y de las acciones que realizaste.</p>
                </div>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="offcanvas" aria-label="Cerrar"></button>
            </div>
            <div class="offcanvas-body">
                <div id="divMensajeAuditoria" class="alert d-none" role="alert"></div>

                <div class="h360-card">
                    <div class="d-flex justify-content-between align-items-center px-3 py-3" style="border-bottom:1px solid var(--h360-border);">
                        <p class="fw-bold mb-0 small">Historial <span id="lblContadorAuditoria" class="fw-normal" style="color:var(--h360-muted);">(0 resultados)</span></p>
                        <button type="button" id="btnActualizarAuditoria" class="btn-h360 btn-h360-outline"><i class="bi bi-arrow-clockwise"></i> Actualizar</button>
                    </div>
                    <div class="table-responsive">
                        <table class="table h360-table mb-0">
                            <thead>
                                <tr>
                                    <th>#</th><th>Acción</th><th>Módulo</th><th>Descripción</th><th>Fecha y hora</th>
                                </tr>
                            </thead>
                            <tbody id="tbodyAuditoria"></tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.4/dist/chart.umd.min.js"></script>
    <script src="JavaScript/Citas.js"></script>
</body>
</html>
