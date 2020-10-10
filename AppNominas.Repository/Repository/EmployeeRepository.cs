using AppNominas.Model;
using AppNominas.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppNominas.Repository.Repository
{
    public class EmployeeRepository : Repository<Empleado>, IEmployeeRepository
    {
        public EmployeeRepository(AppNominaContext context) : base(context)
        {
        }
    }
}
