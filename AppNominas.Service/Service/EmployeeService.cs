using AppNominas.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppNominas.Service.Service
{
    public class EmployeeService
    {
        private IRepositoryWrapper _repo;
        public EmployeeService(IRepositoryWrapper repo)
        {
            _repo = repo;
        }
        public int getEmployeeId(int id) {
            var employee = _repo.employee.GetByID(id);
            return employee.Id;
        }
    }
}
