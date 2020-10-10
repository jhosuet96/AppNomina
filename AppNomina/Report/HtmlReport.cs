using AppNominas.Repository.GenericRepository;
using AppNominas.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNomina.Report
{
    public class HtmlReport
    {

        private IRepositoryWrapper _repo;
        private NominaProcess nominaProcess;
        public HtmlReport(IRepositoryWrapper repo)
        {
            nominaProcess = new NominaProcess(repo);
            _repo = repo;
        }

        public string GetHtmlString(DateTime fecha)
        {
            var search = nominaProcess.getAllPayrollGenerated(fecha);
            var sb = new StringBuilder();
            sb.Append(@"<!DOCTYPE html>
                <html>
                    <head>
                <style>
                    .header {
                        text-align: center;
                        color: red;
                        padding-bottom:35px;
                    }
                    table {
                        width: 80%;
                        border-collapse: collapse;
                    }

                    td, th {
                        border: 1px solid gray;
                        padding: 15px;
                        font-size: 22px;
                        text-align: center;
                    }

                    table th {
                        background-color: gray;
                        color: white;
                    }
                </style>
                    </head>
                <body>
                    <div class='header'><h1>Reporte de Nomina</h1></br>
                    <table align='center'>
                        <tr>
                            <th>Nombre Empleado</th>
                            <th>Fecha Nomina Generada</th>
                            <th>Fecha Proximo Corte</th>
                            <th>Sueldo Neto</th>
                            <th>Dias Laborados</th> 
                            <th>Sueldo Generado</th>
                            <th>Estatus</th>
                        </tr>"
            );
            foreach (var item in search)
            {
                var format = sb.AppendFormat(@"<tr>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td>{3}</td>
                                        <td>{4}</td>
                                        <td>{5}</td>
                                        <td>{6}</td>
                                    </tr>", 
                                    item.Nominas.Empleados.Nombre + " " + item.Nominas.Empleados.Apellido,
                                    item.FechaNominaGenerada,
                                    item.FechaProximoCorte,
                                    item.Nominas.SueldoNeto,
                                    item.DiasLaborados,
                                    item.Nominas.Activo == true ? "Activo" : "Inactivo");
            }
            sb.Append(@"</table></body></html>");
            return sb.ToString();
        }

    }
}
