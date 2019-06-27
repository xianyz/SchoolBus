using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolBusWXWeb.Models.PmsData;
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

        public async Task<twxuser> GetTwxuserAsync(string pkid)
        {
            try
            {
                return await _schoolBusRepository.GetTwxuserBypkidAsync(pkid);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        public async Task<int> DoRegister(RegisterModel user)
        {
            var Record = await _schoolBusRepository.GetTwxuserBytOpenidAsync(user.wxid);
            if (Record != null)
            {
                var userRecord = await _schoolBusRepository.GetCardBypkidAsync(Record.fk_card_id);
                if (userRecord != null && userRecord.fstatus == 1)
                {
                    return 1;// 已经注册过
                }
            }
            else
            {
                var cardRecord = await _schoolBusRepository.GetCardByCode(user.cardNum);
            }
            return 0;
        }
    }
}
