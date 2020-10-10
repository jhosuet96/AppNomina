using AppNominas.Model;
using AppNominas.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppNominas.Repository.Repository
{
    public class NominaRepository : Repository<Nomina>, INominaRepository
    {
        public NominaRepository(AppNominaContext context) : base(context)
        {
        }
    }
}
