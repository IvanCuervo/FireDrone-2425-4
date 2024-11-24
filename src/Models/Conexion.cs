using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Conexion
    {
        //Id del dron
        public int dronId { get; set; }
        //Driver del dron
        public string? driver { get; set; }
        //Puntos del plan de vuelo
        public string? puntos { get; set; }
    }
}
