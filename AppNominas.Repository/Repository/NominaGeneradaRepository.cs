using AppNominas.Model;
using AppNominas.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppNominas.Repository.Repository
{
    public class NominaGeneradaRepository:Repository<NominaGenerada>, INominaGeneradaRepository 
    {
        public NominaGeneradaRepository(AppNominaContext context) : base(context)
        {

        }
    }
}
