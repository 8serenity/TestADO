using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestADO
{
    //    Id int identity primary key not null,
    //UserId int not null foreign key references Users(Id),
    //FilePath varchar(1000) not null,
    //Filesize float not null,
    //FileExtension varchar(50),
    //DateCreated datetime not null default getdate()
    public class UserFile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FilePath { get; set; }
        public double Filesize { get; set; }
        public string FileExtension { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
