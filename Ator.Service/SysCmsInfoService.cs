using Ator.Entity.Sys;
using Ator.IService;
using Ator.Model;
using Ator.Model.Api.TimeLine;
using Ator.Repository;
using Ator.Utility.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ator.Service
{
    public class SysCmsInfoService : ISysCmsInfoService
    {
        UnitOfWork _unitOfWork;
        public SysCmsInfoService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<KeyValuePair<string, string>> GetInfoList()
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
            var all = _unitOfWork.SysCmsInfoRepository.Load().OrderByDescending(o => o.InfoTop).ThenBy(o => o.Sort).ThenByDescending(o => o.InfoPublishTime).ToList();
            foreach (var item in all)
            {
                data.Add(new KeyValuePair<string, string>(item.SysCmsInfoId, item.InfoTitle));
            }
            return data;
        }

        /// <summary>
        /// 获取时间轴数据
        /// </summary>
        /// <returns></returns>
        public List<YearData> GetTimeLineData()
        {
            var data = _unitOfWork.SysCmsInfoRepository.Load(o => o.SysCmsColumnId == "a1962d915dde48ebaaa1b590a51cde75" && o.Status == 1).OrderByDescending(o => o.InfoPublishTime);
            List<YearData> returnData = new List<YearData>();
            foreach (var item in data)
            {
                var thisyear = ((DateTime)item.InfoPublishTime).Year.ToString();//年
                var thismonth = ((DateTime)item.InfoPublishTime).Month.ToString().PadLeft(2, '0');//月
                var thisday = ((DateTime)item.InfoPublishTime).ToString("MM月dd日 HH:mm");//日数据
                //不存在年份
                var thisYearData = returnData.FirstOrDefault(o => o.year == thisyear);
                if(thisYearData == null)
                {
                    thisYearData = new YearData() { year = thisyear};
                    returnData.Add(thisYearData);
                }
                var thisMonthData = thisYearData.months.FirstOrDefault(o => o.month == thismonth);
                //不存在月份数据
                if(thisMonthData == null)
                {
                    thisMonthData = new MonthData() { month = thismonth };
                    thisYearData.months.Add(thisMonthData);
                }
                //添加日数据
                thisMonthData.data.Add(new DayData
                {
                    content = item.InfoContent,
                    create_time = thisday
                });
            }
            return returnData;
        }
    }
}
