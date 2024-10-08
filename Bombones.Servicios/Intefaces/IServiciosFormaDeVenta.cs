﻿using Bombones.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombones.Servicios.Intefaces
{
    public interface IServiciosFormaDeVenta
        {
            void Borrar(int formaDeVentaId);
            bool Existe(FormaDeVenta formaDeVenta);
            List<FormaDeVenta> GetLista();
            void Guardar(FormaDeVenta formaDeVenta);
        }
   
}
