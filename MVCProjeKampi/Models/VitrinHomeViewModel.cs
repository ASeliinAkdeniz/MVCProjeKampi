using System.Collections.Generic;
using EntityLayer.Concrete;
using PagedList;

namespace MVCProjeKampi.Models
{
    public class VitrinHomeViewModel
    {
        public IPagedList<Heading> Headings { get; set; }        // orta akış
        public List<Heading> Trending { get; set; }        // gündem
        public List<Category> Categories { get; set; }     // aktif kategoriler
        public Dictionary<int, int> EntryCounts { get; set; }     // başlık -> entry sayısı
        public Dictionary<int, string> LatestEntry { get; set; }  // başlık -> son entry metni
        public Dictionary<int, string> CategoryNames { get; set; }// kategori -> ad
        public int TotalHeadings { get; set; }
        public int TotalEntries { get; set; }
        public int TotalWriters { get; set; }
    }
}