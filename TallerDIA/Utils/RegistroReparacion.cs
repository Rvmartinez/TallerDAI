﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TallerDIA.Models;

namespace TallerDIA.Utils
{
    public class RegistroReparacion : ObservableCollection<Reparacion>
    {

        public RegistroReparacion()
            : this(new List<Reparacion>())
        {
        }


        public RegistroReparacion(IEnumerable<Reparacion> reparaciones)
            : base(reparaciones)
        {
        }


        public void AddRange(IEnumerable<Reparacion> vs)
        {
            foreach (Reparacion v in vs)
            {
                this.Add(v);
            }
        }


        public Reparacion[] ToArray()
        {
            var toret = new Reparacion[this.Count];

            this.CopyTo(toret, 0);
            return toret;
        }

        public override string ToString()
        {
            var toret = new StringBuilder();

            foreach (Reparacion v in this)
            {
                toret.AppendLine(v.ToString());
            }

            return toret.ToString();
        }
    }
}
