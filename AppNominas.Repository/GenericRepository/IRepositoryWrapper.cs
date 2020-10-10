using AppNominas.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppNominas.Repository.GenericRepository
{
    public interface IRepositoryWrapper
    {
        IEmployeeRepository employee { get; }
        INominaRepository nomina { get; }
        IRetencionISRRepository retencionISR { get; }
        INominaGeneradaRepository nominaGenerada { get; }

        void Save();
    }
}
