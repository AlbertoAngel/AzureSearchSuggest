using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comun.Modelos
{    

    [SerializePropertyNamesAsCamelCase]
    public partial class User
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable]
        public string id { get; set; }

        [IsSearchable, IsFilterable, IsSortable, IsRetrievable(true)]
        public string UserName { get; set; }

        [IsSearchable, IsFilterable, IsSortable]
        public string Name { get; set; }

        [IsSearchable, IsFilterable, IsSortable]
        public string Email { get; set; }

        [IsSearchable, IsFilterable, IsSortable, IsFacetable]
        public string Role { get; set; }
    }
}
