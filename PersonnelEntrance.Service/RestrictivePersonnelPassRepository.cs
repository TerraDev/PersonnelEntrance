using PersonnelEntrance.DomainModel;
using PersonnelEntrance.DTO;
using PersonnelEntrance.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonnelEntrance.Service
{
    public class RestrictivePersonnelPassRepository : PersonnelPassRepository
    {
        public RestrictivePersonnelPassRepository(IPersonnelPassDB passDB) : base(passDB)
        {

        }

        public override Validator<PersonnelPassDTO> CreatePersonnelPass(PersonnelPassDTO personnelPassDTO)
        {
            int repCount = RetrieveCount(personnelPassDTO.personnelName);
            if (repCount < Global_info.MaxEmployeeRecordCount)
                return base.CreatePersonnelPass(personnelPassDTO);
            else
            {
                Validator<PersonnelPassDTO> validator= new Validator<PersonnelPassDTO>();
                personnelPassDTO.passTime = DateManager.ShortenDate(personnelPassDTO.passTime);
                validator.item = personnelPassDTO;
                validator.isValid = false;
                validator.errorMessage = $"Can't create record, because there's currently" +
                   $" {repCount} records for ${validator.item.personnelName}." +
                   $" Maximum is ${repCount}.";
                return validator;
            }
        }


        // WARNING: This method has a bug: updating an 
        // employee record that is repeated four times
        // isn't possible (even just updating the passtime).
        public override Validator<PersonnelPassDTO> UpdatePersonnelPass(Guid id, PersonnelPassDTO personnelPassDTO)
        {
            int repCount = RetrieveCount(personnelPassDTO.personnelName);
            if (repCount < Global_info.MaxEmployeeRecordCount)
                return base.UpdatePersonnelPass(id, personnelPassDTO);
            else
            {
                Validator<PersonnelPassDTO> validator = new Validator<PersonnelPassDTO>();
                personnelPassDTO.passTime = DateManager.ShortenDate(personnelPassDTO.passTime);
                validator.item = personnelPassDTO;
                validator.isValid = false;
                validator.errorMessage = $"Can't update record, because there's currently" +
                    $" {repCount} records for ${validator.item.personnelName}." +
                    $" Maximum is ${repCount}.";
                return validator;
            }
        }

        private int RetrieveCount(string PersonnelName)
        {
            return _passDB.GetCount(PersonnelName);
        }

    }
}
