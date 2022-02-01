using PersonnelEntrance.DataAccess;
using PersonnelEntrance.DomainModel;
using PersonnelEntrance.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonnelEntrance.Service
{
    public class PersonnelPassRepository : IPersonnelPassRepository
    {
        protected readonly IPersonnelPassDB _passDB;

        public PersonnelPassRepository(IPersonnelPassDB personnelPassDB)
        {
            _passDB = personnelPassDB;
        }

        public virtual Validator<PersonnelPassDTO> CreatePersonnelPass(PersonnelPassDTO personnelPassDTO)
        {
            Guid newId = Guid.NewGuid();
            personnelPassDTO.passTime = DateManager.ShortenDate(personnelPassDTO.passTime);
            Validator<PersonnelPassDTO> Validator = new Validator<PersonnelPassDTO>();
            Validator.item = personnelPassDTO;
    
            int rowsEffected = _passDB.Insert(new PersonnelPass
            {
                employeeName = personnelPassDTO.personnelName,
                passTime = personnelPassDTO.passTime,
                passType = (PassTypeEnum)personnelPassDTO.passType,
                passId = newId
            });

            if (rowsEffected > 0)
            {
                personnelPassDTO.passId = newId;
                Validator.isValid = true;
                return Validator;
            }
            else
            {
                Validator.isValid = false;
                Validator.errorMessage = "Couldn't create personnel record.";
                return Validator;
            }
        }

        public int DeletePersonnelPass(Guid id)
        {
            return _passDB.Delete(id);
        }

        public IEnumerable<PersonnelPassDTO> RetrieveAllPersonnelPass()
        {
            IEnumerable<PersonnelPass> personnelPasses = _passDB.SelectAll();
            if (personnelPasses == null)
                return null;
            else
            {
                ICollection<PersonnelPassDTO> passes = new List<PersonnelPassDTO>();
                foreach (var personnelPass in personnelPasses)
                {
                    passes.Add(new PersonnelPassDTO
                    {
                        passId = personnelPass.passId,
                        personnelName = personnelPass.employeeName,
                        passTime = personnelPass.passTime,
                        passType = (PassEnumDTO)personnelPass.passType
                    });
                }
                return passes;
            }
        }

        public PersonnelPassDTO RetrievePersonnelPass(Guid id)
        {
            PersonnelPass personnelPass = _passDB.Select(id);
            if (personnelPass != null)
                return new PersonnelPassDTO
                {
                    passTime = personnelPass.passTime,
                    passType = (PassEnumDTO)personnelPass.passType,
                    personnelName = personnelPass.employeeName,
                    passId = personnelPass.passId,
                };
            else
                return null;
        }

        public virtual Validator<PersonnelPassDTO> UpdatePersonnelPass(Guid id, 
            PersonnelPassDTO personnelPassDTO)
        {
            personnelPassDTO.passId = id;
            Validator<PersonnelPassDTO> Validator = new Validator<PersonnelPassDTO>();
            Validator.item = personnelPassDTO;
            personnelPassDTO.passTime = DateManager.ShortenDate(personnelPassDTO.passTime);

            int rowsEffected = _passDB.Update(id, new PersonnelPass
            {
                employeeName = personnelPassDTO.personnelName,
                passTime = personnelPassDTO.passTime,
                passType = (PassTypeEnum)personnelPassDTO.passType,
            });

            if (rowsEffected > 0)
            {
                Validator.isValid = true;
                return Validator;
            }
            else // rowsEffected == 0
            {
                Validator.isValid = false;
                Validator.errorMessage = "This record doesn't exist";
                return Validator;
            }
        }
    }
}
