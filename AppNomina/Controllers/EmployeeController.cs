using AppNominas.Model;
using AppNominas.Repository.GenericRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppNomina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IRepositoryWrapper _repo;
        NominaController nominaController;
        Nomina _Nomina;
        public EmployeeController(IRepositoryWrapper repo)
        {
            _Nomina = new Nomina();
            nominaController = new NominaController(repo);
            _repo = repo;
        }

        [HttpGet]
        [Route("GetAllActive")]
        [EnableCors("AllowOrigin")] ////Importante 3
        public IActionResult GetAll()
        {
            var empleadoA = _repo.employee.GetAll().Where(p => p.Activo == true);
            var empleadoI = _repo.employee.GetAll().Where(p => p.Activo == false);
            return Ok(new { empleadoA,empleadoI});
        }

       
        [HttpGet]
        [Route("GetById/{id:int}")]
        [EnableCors("AllowOrigin")]
        public IActionResult GetById(int id)
        {
            if (id > 0)
            {
                var employee = _repo.employee.GetByID(id);
                return Ok(new { employee });
            }
            else
            {
                return NotFound(id);
            }
        }


        [HttpPost]
        [Route("AddEmployee")]
        [EnableCors("AllowOrigin")]
        public IActionResult AddEmployee(Empleado empleado)
        {
            _repo.employee.Add(empleado);
           nominaController.GetSueldoBruto(empleado.Id);

            _repo.Save();


            return Ok(empleado);
        }

        [HttpPatch]
        [Route("UpdateEmployee")]
        [EnableCors("AllowOrigin")]
        public IActionResult Update(Empleado empleado)
        {
            try
            {
                int id = empleado.Id;
                var _nomina = _repo.nomina.FindByCondition(p => p.EmpleadoID == id);
                if (_nomina.Count() > 0)
                {
                    var nomina = _repo.nomina.FindByCondition(p => p.EmpleadoID == id).First();
                    if (nomina.SueldoBruto != empleado.SueldoBruto)
                    {
                        _repo.employee.Update(empleado);
                        _repo.Save();
                        nominaController.UpdateNomina(empleado.Id);
                    }
                    else
                    {
                        _repo.employee.Update(empleado);
                        _repo.Save();
                    }
                    return Ok(empleado);
                }
                else
                {
                    _repo.employee.Update(empleado);
                    _repo.Save(); 
                    return Ok(empleado);
                }
            }
            catch (System.Exception ex)
            {
                return NotFound(empleado);
            }
        }

        [HttpDelete]
        [Route("DeleteEmployee/{id:int}")]
        [EnableCors("AllowOrigin")]
        public IActionResult Delete(int id)
        { 
            if (id > 0)
            {
                var employee = _repo.employee.GetByID(id);
                // _repo.employee.Delete(employee);
                if (employee.Activo == false)
                {
                    employee.Activo = true;
                }
                else
                {
                    employee.Activo = false;
                }
                _repo.Save();
                return Ok(new { employee });
            }
            else
            {
                return NotFound(id);
            }
            
        }

    }
}
