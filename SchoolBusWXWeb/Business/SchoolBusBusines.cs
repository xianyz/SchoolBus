using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolBusWXWeb.Models.SchollBusModels;
using SchoolBusWXWeb.Repository;

namespace SchoolBusWXWeb.Business
{
    public class SchoolBusBusines : ISchoolBusBusines
    {
        private readonly ISchoolBusRepository _schoolBusRepository;
        public SchoolBusBusines(ISchoolBusRepository schoolBusRepository)
        {
            _schoolBusRepository = schoolBusRepository;
        }

        public async Task<twxuser> GetTwxuserAsync()
        {
            try
            {
                return await _schoolBusRepository.GetTwxuserAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
         
        }
    }
}
