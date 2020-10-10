using AppNominas.Model;
using AppNominas.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppNominas.Service
{
    public class NominaProcess
    {

        int i = 0;
        IQueryable<Nomina> _nomina;
        IQueryable<NominaGenerada> _nominaG;
        private IRepositoryWrapper _repo;


        public NominaProcess(IRepositoryWrapper repo)
        {
            _repo = repo;
        }
                
        private decimal _RetencionISR(decimal sueldoBruto, List<RetencionISR> retenciones)
        {            
            sueldoBruto *= 12;
            foreach (var item in retenciones)
            {
                if (sueldoBruto >= 416220.01M)
                {
                    if (sueldoBruto >= item.LimiteInferior && sueldoBruto <= item.LimiteSuperior)
                    {
                        sueldoBruto -= item.LimiteInferior;

                        if (item.PorcentajeExcedente != 0)
                        {
                            sueldoBruto *= item.PorcentajeExcedente;
                        }

                        if (item.TasaFija != 0)
                        {
                            sueldoBruto += item.TasaFija;
                        }

                        i = item.Id;
                        sueldoBruto /= 12;
                        return sueldoBruto;
                    }
                }
                else
                {
                    i = item.Id;
                    return 0;
                }
                
            }
            return 0;
        }

        public Nomina getCalculators(int idE,decimal SueldoBruto, List<RetencionISR> retenciones) { 
            decimal RetencionAFP = (decimal)0.0287;
            decimal RetencionARS = (decimal)0.0304;

            RetencionAFP *= SueldoBruto;
            RetencionARS *= SueldoBruto;
            decimal SueldoImponible = SueldoBruto - (RetencionAFP + RetencionARS);
            decimal RetencionISR = _RetencionISR(SueldoImponible, retenciones);
            decimal TotalRetencion = (RetencionAFP + RetencionARS + RetencionISR);
            decimal SueldoNeto = SueldoBruto - TotalRetencion;
            DateTime fecha = DateTime.Now;

            return new Nomina
            { 
                SueldoBruto = SueldoBruto,
                RetencionAFP = RetencionAFP,
                RetencionARS = RetencionARS,
                SueldoImponible = SueldoImponible,
                RetencionIsrId = i,
                TotalRetencion = TotalRetencion,
                SueldoNeto = SueldoNeto,
                Activo = true,
                EmpleadoID = idE,
                RetencionISR = RetencionISR,
                FechaCreacion = fecha,  
                FechaNominaGenerada= fecha,
            };
        }

        public NominaGenerada PayrollGenerated(int id, DateTime fechaProximoCorte, DateTime fechaNominaGenerada, decimal sueldoNeto)
        {
            //fechaProximoCorte = new DateTime(2020,08,30);
            int diasLaborados = Convert.ToInt32((fechaProximoCorte - fechaNominaGenerada).TotalDays);
            if (diasLaborados >= 1)
            {
                decimal Sueldo = sueldoNeto / 30;
                decimal sueldoGenerado = (Sueldo * diasLaborados);
                //fechaNominaGenerada = fechaProximoCorte;

                return new NominaGenerada
                {
                    NominasId = id,
                    FechaNominaGenerada = fechaNominaGenerada,
                    FechaProximoCorte = fechaProximoCorte,
                    DiasLaborados = diasLaborados,
                    //SueldoNeto = sueldoNeto,
                    SueldoGenerado = sueldoGenerado
                };
            }
            else
            {
                return null;
            }

        }

        public IQueryable<NominaGenerada> getAllPayrollGenerated(DateTime fechaProximoCorte)
        {
            _nominaG = _repo.nominaGenerada.GetAll().Where(p => p.FechaProximoCorte == fechaProximoCorte).Select(
                o=> new NominaGenerada
                {
                    FechaNominaGenerada = o.FechaNominaGenerada,
                    FechaProximoCorte= o.FechaProximoCorte,
                    DiasLaborados= o.DiasLaborados,
                    SueldoGenerado = o.SueldoGenerado,
                    Nominas =new Nomina
                    {
                        SueldoBruto =o.Nominas.SueldoBruto,
                        Activo = o.Nominas.Activo,
                        Empleados = new Empleado
                        {
                            Nombre = o.Nominas.Empleados.Nombre,
                            Apellido=o.Nominas.Empleados.Apellido
                        }
                    }
                });
             return _nominaG;
        }


        public IQueryable<Nomina> getAllNominaEmployee()
        {
            _nomina = _repo.nomina.GetAll()
           .Select(n => new Nomina
           {
               Id = n.Id,
               SueldoBruto = n.SueldoBruto,
               RetencionAFP = n.RetencionAFP,
               RetencionARS = n.RetencionARS,
               SueldoImponible = n.SueldoImponible,
               RetencionISR = n.RetencionISR,
               TotalRetencion = n.TotalRetencion,
               SueldoNeto = n.SueldoNeto,
               Activo = n.Activo,
               EmpleadoID = n.Empleados.Id,
               RetencionIsrId = n.RetencionIsrId,
               FechaCreacion = Convert.ToDateTime(n.FechaCreacion),//.ToString("dd.MM.yyyy HH:mm:ss")
               FechaNominaGenerada = Convert.ToDateTime(n.FechaNominaGenerada),
               Empleados = new Empleado
               {
                   Id = n.Empleados.Id,
                   Nombre = n.Empleados.Nombre,
                   Apellido = n.Empleados.Apellido,
                   Sexo = n.Empleados.Sexo,
                   SueldoBruto = n.Empleados.SueldoBruto,
                   Activo = n.Empleados.Activo
               }
           });

            return _nomina;
        }


        public IQueryable<Nomina> BuscarNomina(string findBy)
        {
            if (string.IsNullOrEmpty(findBy) || findBy == "null")
            {
                getAllNominaEmployee().Where(r => r.Activo == true);
            }
            else
            {
                _nomina = getAllNominaEmployee().Where(o =>
                o.SueldoBruto.ToString().Contains(findBy) ||
                o.RetencionAFP.ToString().Contains(findBy) ||
                o.RetencionARS.ToString().Contains(findBy) ||
                o.SueldoImponible.ToString().Contains(findBy) ||
                o.RetencionISR.ToString().Contains(findBy) ||
                o.TotalRetencion.ToString().Contains(findBy) ||
                o.SueldoNeto.ToString().Contains(findBy) ||
                o.Activo.ToString().Contains(findBy) ||
                o.Empleados.Nombre.ToString().Contains(findBy) ||
                o.Empleados.Apellido.ToString().Contains(findBy) ||
                o.Empleados.Sexo.ToString().Contains(findBy) &&
                o.Activo == true
                );
            }
            return _nomina;
        }
    }
}

