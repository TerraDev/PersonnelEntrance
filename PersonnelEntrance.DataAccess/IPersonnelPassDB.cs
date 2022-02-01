using PersonnelEntrance.DomainModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonnelEntrance.DataAccess
{
    public interface IPersonnelPassDB
    {
        public IEnumerable<PersonnelPass> SelectAll();

        public PersonnelPass Select(Guid id);

        public int Insert(PersonnelPass personnelPass);

        public int Update(Guid id, PersonnelPass personnelPass);

        public int Delete(Guid id);

        public int GetCount(String employeeName);
    }
}
