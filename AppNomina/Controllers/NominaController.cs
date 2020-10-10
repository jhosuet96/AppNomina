using AppNominas.Model;
using AppNominas.Repository.GenericRepository;
using AppNominas.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Wkhtmltopdf.NetCore;
namespace AppNomina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NominaController : ControllerBase
    {
        private IRepositoryWrapper _repo;
        private NominaProcess nominaProcess;
        IQueryable<Nomina> _nomina;
        List<NominaGenerada> nominaG;
        //private TemplateGenerator template;
        public NominaController(IRepositoryWrapper repo)
        {
            nominaProcess = new NominaProcess(repo);
            nominaG = new List<NominaGenerada>();
            _repo = repo;
        }


        [HttpGet]
        [Route("GetSueldoBruto/{id:int}")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetSueldoBruto(int id)//Id Empleado
        {
            var isnomina = _repo.nomina.FindByCondition(p => p.EmpleadoID == id);
            if (isnomina.Count() > 0)
            {
                int isnominaCount = isnomina.Count();
                return Ok(new { isnomina, isnominaCount });                
            }
            else
            {
                List<RetencionISR> _retencionISR = new List<RetencionISR>();
                var employee = _repo.employee.GetByID(id);
                var retencionISR = _repo.retencionISR.GetAll();
                foreach (var item in retencionISR)
                {
                    _retencionISR.Add(item);
                }

                var nomina = nominaProcess.getCalculators(employee.Id, employee.SueldoBruto, _retencionISR);
                Add(nomina);
                return Ok(new {nomina});
            }
        }

        [HttpPatch]
        [Route("UpdateNomina/{id:int}")]
        [EnableCors("AllowOrigin")] ////Importante 3
        public IActionResult UpdateNomina(int id)
        {
            var isnomina = _repo.nomina.FindByCondition(p => p.EmpleadoID == id).First();
            if (isnomina.Id > 0)
            {
                List<RetencionISR> _retencionISR = new List<RetencionISR>();
                var employee = _repo.employee.GetByID(id);
                var retencionISR = _repo.retencionISR.GetAll();
                foreach (var item in retencionISR)
                {
                    _retencionISR.Add(item);
                }
                var _nomina = nominaProcess.getCalculators(id, employee.SueldoBruto, _retencionISR);                             
                _nomina.Id = isnomina.Id;
                _repo.nomina.Update(_nomina);
                _repo.Save();
                return Ok(new { _nomina });
            }
            else
            {
                return NotFound(_nomina);
            }

        }

        [HttpGet]
        [Route("GetNominaGenerated")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetNominaGenerated(DateTime fecha)
        {
            DateTime fechaProximoCorte = Convert.ToDateTime(fecha);
            var select = nominaProcess.getAllNominaEmployee().Where(p=> p.Activo == true);
            foreach (var item in select)
            {
                var _nominaG = nominaProcess.PayrollGenerated(item.Id, fechaProximoCorte, item.FechaNominaGenerada, item.SueldoNeto);
                nominaG.Add(_nominaG);

                item.FechaNominaGenerada = _nominaG.FechaNominaGenerada;
                _repo.nomina.Update(item);
                _repo.nominaGenerada.Add(_nominaG);               
            }
           _repo.Save();
            return Ok(new {nominaG });
        }


        //ORIGINAL

        //[HttpGet]
        //[Route("reportPdf/{id:int}")]
        //[EnableCors("AllowOrigin")]
        //public IActionResult reportPdf(DateTime fecha)
        //{
        //    var html = nominaProcess.getAllPayrollGenerated(fecha);
        //    var pdf = _generatePdf.GetPDF(html);
        //    var pdfStream = new System.IO.MemoryStream();
        //    pdfStream.Write(pdf, 0, pdf.Length);
        //    pdfStream.Position = 0;
        //    return File(pdfStream, "application/pdf");
        //}

        [HttpPost]
        [Route("AddNominaGenerated")]
        [EnableCors("AllowOrigin")]
        public IActionResult AddNominaGenerated(DateTime fechaProximoCorte)
        {
            var select = nominaProcess.getAllNominaEmployee().Where(p => p.Activo == true);

            foreach (var item in select)
            {
                var _nominaG = nominaProcess.PayrollGenerated(item.Id, fechaProximoCorte, item.FechaNominaGenerada, item.SueldoNeto);
                _repo.nominaGenerada.Add(_nominaG);
                _repo.Save();
            }
            return Ok(new { select });
        }



        [HttpGet]
        [Route("getNominaEmployeeSearch")]
        [EnableCors("AllowOrigin")]
        public IActionResult getNominaEmployeeSearch(string search)
        {
            _nomina = nominaProcess.BuscarNomina(search);
            return Ok(_nomina);
        }

        [HttpGet]
        [Route("GetAllNominaEmployee")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetAllNominaEmployee()
        {
            _nomina = nominaProcess.getAllNominaEmployee();
            return Ok(_nomina);
        }

        
        [HttpPost]
        [Route("Add")]
        [EnableCors("AllowOrigin")]
        public IActionResult Add(Nomina nomina)
        {
            //nomina.FechaNominaGenerada = nomina.FechaCreacion;
            _repo.nomina.Add(nomina);
            _repo.Save();
            return Ok(nomina);
        }
    }
}
