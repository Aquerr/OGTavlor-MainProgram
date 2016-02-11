using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGTavlor_MainProgram
{
    public class CustomerEntity : TableEntity
    {
        public CustomerEntity(string lastName, string firstName)
        {
            this.PartitionKey = lastName;
            this.RowKey = firstName;
        }

        public CustomerEntity() { }


      //  public int ArtworkId { get; set; }
      //  public string Title { get; set; }
      //  public string Artist { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
    }
}