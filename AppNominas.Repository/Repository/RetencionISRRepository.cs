using AppNominas.Repository;
using AppNominas.Model;
using AppNominas.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppNominas.Repository.Repository
{
   public class RetencionISRRepository: Repository<RetencionISR>, IRetencionISRRepository
    {
        public RetencionISRRepository(AppNominaContext context) : base(context)
        {

        }
    }
}
