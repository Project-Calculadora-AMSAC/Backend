using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.User.Domain.Model.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates
{
    public class Estimacion
    {
        public int EstimacionId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public AuthUser AuthUser { get; private set; }
        public int ProyectoId { get; private set; }
        public Proyecto Proyecto { get; private set; }
        public string CodPam { get; private set; }
        public DateTime FechaEstimacion { get; private set; }
        public List<SubEstimacion> SubEstimaciones { get;  set; } = new List<SubEstimacion>(); // 🔹 Inicializar lista

        private Estimacion()
        {
            SubEstimaciones = new List<SubEstimacion>();

        } // Constructor privado para EF Core

        public Estimacion(Guid usuarioId, int proyectoId, string codPam, DateTime fechaEstimacion)
        {
            UsuarioId = usuarioId;
            ProyectoId = proyectoId;
            CodPam = codPam ?? throw new ArgumentNullException(nameof(codPam));
            FechaEstimacion = fechaEstimacion;
            SubEstimaciones = new List<SubEstimacion>();
        }

        public Estimacion(Guid usuarioId, int proyectoId, string codPam, List<SubEstimacion> subEstimaciones)
        {
            UsuarioId = usuarioId;
            ProyectoId = proyectoId;
            CodPam = codPam ?? throw new ArgumentNullException(nameof(codPam));
            FechaEstimacion = DateTime.UtcNow;

            if (subEstimaciones == null || !subEstimaciones.Any())
                throw new ArgumentException("Debe haber al menos una subestimación.");

            foreach (var sub in subEstimaciones)
            {
                sub.SetEstimacion(this);
            }

            SubEstimaciones = new List<SubEstimacion>(subEstimaciones);
          
        }

     
        public void EnsureSubEstimacionesInitialized()
        {
            if (SubEstimaciones == null)
                SubEstimaciones = new List<SubEstimacion>();
        }

        public void AgregarSubEstimacion(SubEstimacion subEstimacion)
        {
            if (subEstimacion == null)
            {
                Console.WriteLine("ERROR: Intentando agregar una SubEstimacion NULL.");
                throw new ArgumentNullException(nameof(subEstimacion), "SubEstimacion no puede ser NULL.");
            }

            if (SubEstimaciones == null) 
            {
                Console.WriteLine("WARNING: SubEstimaciones estaba NULL. Se inicializa.");
                SubEstimaciones = new List<SubEstimacion>();
            }

            Console.WriteLine($"DEBUG: Agregando SubEstimacion con TipoPamId {subEstimacion.TipoPam.Id} y cantidad {subEstimacion.Cantidad}");
            SubEstimaciones.Add(subEstimacion);
        }



    }
}
