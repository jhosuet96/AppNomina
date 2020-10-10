using System;
using System.Collections.Generic;
using System.Text;
using AppNominas.Model;
using AppNominas.Repository.Interface;
using AppNominas.Repository.Repository;

namespace AppNominas.Repository.GenericRepository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private AppNominaContext _repoContext;
        private IEmployeeRepository _employee;
        private INominaRepository _nomina;
        private IRetencionISRRepository _retencionISR;
        private INominaGeneradaRepository _nominaGenerada;
        public RepositoryWrapper(AppNominaContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public IEmployeeRepository employee {
            get 
            {
                if (_employee == null)
                {
                    _employee = new EmployeeRepository(_repoContext);
                }
                return _employee;
            }
        }

        public INominaRepository nomina {
            get
            {
                if (_nomina == null)
                {
                    _nomina = new NominaRepository(_repoContext);
                }
                return _nomina;
            }
        }

        public IRetencionISRRepository retencionISR
        {
            get
            {
                if (_retencionISR==null)
                {
                    _retencionISR = new RetencionISRRepository(_repoContext);
                }
                return _retencionISR;
            }
        }

        public INominaGeneradaRepository nominaGenerada
        {
            get
            {
                if (_nominaGenerada == null)
                {
                    _nominaGenerada = new NominaGeneradaRepository(_repoContext);
                }
                return _nominaGenerada;
            }
        }
        public void Save()
        {
            
            _repoContext.SaveChanges();
        }
    }
}
