using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    internal class Files
    {
       public string name { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string size { get; set; }
        public string fullname { get; set; }  
        public List<Files> SortNameAZ(List<Files> f)
        {
            f=f.OrderBy(f=>f.name).ToList();
            return f;
        }
        public List<Files> SortNameZA(List<Files> f)
        {
            f = f.OrderByDescending(f => f.name).ToList();
            return f;
        }
        public List<Files> Size0N(List<Files> f)
        {
            f = f.OrderBy(f => f.size).ToList();
            return f;
        }
        public List<Files> SizeN0(List<Files> f)
        {
            f = f.OrderByDescending(f => f.size).ToList();
            return f;
        }
        public List<Files> PopulateList(ListView list)
        {
            List<Files> files = new List<Files>();  
            foreach(ListViewItem item in list.Items)
            {
                Files f= new Files();   
                f.name = item.SubItems[0].Text;
                f.date= item.SubItems[1].Text;
                f.type= item.SubItems[2].Text;
                f.size= item.SubItems[3].Text;
                f.fullname= item.SubItems[4].Text;
                files.Add(f);
            }
            return files;
        }
        protected private string FormatString(string s)
        {
            string result;
            if (s != null && s.Length > 0)
            {
                string nameLow = s.ToLower();
                string nameUpper = s.ToUpper();
                //nameLow[1..] is used to take digits of a variable from the the second one till the last one
                result = nameUpper[0] + nameLow[1..];
                return result;
            }
            return result = "None";

        }
        public List<Files> Search(string query, List<Files> f)
        {
            f=f.Where(x=>x.name.Contains(FormatString(query))|| x.name.Contains(query)).ToList();
            return f;
        }
    }
}
