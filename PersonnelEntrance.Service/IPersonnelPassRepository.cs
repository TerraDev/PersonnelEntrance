using PersonnelEntrance.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonnelEntrance.Service
{
    public interface IPersonnelPassRepository
    {
        public IEnumerable<PersonnelPassDTO> RetrieveAllPersonnelPass();

        public PersonnelPassDTO RetrievePersonnelPass(Guid id);

        public Validator<PersonnelPassDTO> CreatePersonnelPass(PersonnelPassDTO personnelPassDTO);

        public Validator<PersonnelPassDTO> UpdatePersonnelPass(Guid id, PersonnelPassDTO personnelPassDTO);

        public int DeletePersonnelPass(Guid id);
    }
}
