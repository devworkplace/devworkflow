using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.Models.TO
{
    public class PermissaoExibicao
    {
        public Grupo grupo { get; set; }
        public Formulario formulario { get; set; }
    }
}