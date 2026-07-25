<%@ Page Title="Citas" Language="C#" AutoEventWireup="true" CodeBehind="Citas.aspx.cs" Inherits="PL_Hospital360.Citas" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Hospital360 - Citas</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="Content/Sitio.css" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

        <nav class="navbar navbar-dark bg-primario mb-4">
            <div class="container-fluid">
                <span class="navbar-brand">HOSPITAL<span class="verde">360</span></span>
                <a href="#" id="lnkCerrarSesion" class="text-white">Salir</a>
            </div>
        </nav>

        <div class="container">
            <div class="card shadow-sm p-3 mb-3">
                <div class="row g-2">
                    <div class="col-md-2">
                        <input type="text" id="txtBuscar" class="form-control" placeholder="Buscar por nombre..." />
                    </div>
                    <div class="col-md-2">
                        <select id="ddlEstadoCita" class="form-select"><option value="">Estado de cita</option></select>
                    </div>
                    <div class="col-md-2">
                        <select id="ddlProfesional" class="form-select"><option value="">Profesional</option></select>
                    </div>
                    <div class="col-md-2">
                        <select id="ddlTipoCita" class="form-select"><option value="">Tipo de atención</option></select>
                    </div>
                    <div class="col-md-2">
                        <select id="ddlMembresia" class="form-select"><option value="">Membresía</option></select>
                    </div>
                    <div class="col-md-1">
                        <input type="date" id="txtFecha" class="form-control" />
                    </div>
                    <div class="col-md-1">
                        <button type="button" id="btnFiltrar" class="btn btn-primary w-100">Filtrar</button>
                    </div>
                </div>
            </div>

            <div id="divMensaje" class="alert d-none" role="alert"></div>

            <div class="card shadow-sm">
                <table class="table tabla-registros mb-0">
                    <thead>
                        <tr>
                            <th>Paciente</th>
                            <th>Teléfono</th>
                            <th>Tipo de atención</th>
                            <th>Membresía</th>
                            <th>Estado de cita</th>
                            <th>Profesional</th>
                            <th>Fecha</th>
                            <th>Hora</th>
                        </tr>
                    </thead>
                    <tbody id="tbodyCitas">
                    </tbody>
                </table>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="JavaScript/Citas.js"></script>
</body>
</html>
