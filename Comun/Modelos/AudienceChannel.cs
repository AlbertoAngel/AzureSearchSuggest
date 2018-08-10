using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comun.Modelos
{
    [SerializePropertyNamesAsCamelCase]
    public partial class AudienceChannel
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable]
        public string id { get; set; }

        [IsSearchable, IsFilterable, IsSortable]
        public string Name { get; set; }

        [IsSearchable, IsFilterable, IsSortable, IsFacetable]
        public string ChannelId { get; set; }

        [IsSearchable, IsFilterable, IsSortable, IsFacetable]
        public string KindAdsScore { get; set; }

        [IsSearchable, IsFilterable, IsSortable, IsFacetable]
        public string Price { get; set; }

        [IsSearchable, IsFilterable, IsSortable, IsFacetable]
        public string IsPremium { get; set; }
    }
}
