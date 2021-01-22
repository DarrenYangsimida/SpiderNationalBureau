using SpiderNationalBureau.DataModel_MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpiderNationalBureau.Repository
{
    public class BaseRepository
    {
        private readonly SpiderDbContext _DBContext;

        public BaseRepository(SpiderDbContext DBContext)
        {
            _DBContext = DBContext;
        }

        /// <summary>
        /// 获取省份数据
        /// </summary>
        /// <returns></returns>
        public List<Province> GetProvinces()
        {
            try
            {
                List<Province> dataProvinces = _DBContext.Province
                                               .Where(d => d.DeletedFlag.Equals(false))
                                               .OrderBy(d => d.ProvinceCode).ToList();
                return dataProvinces;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return new List<Province>();
            }
        }

        /// <summary>
        /// 获取身份数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetProvinceCount()
        {
            try
            {
                return _DBContext.Province.Where(d => d.DeletedFlag.Equals(false)).Count();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// 获取城市数据
        /// </summary>
        /// <param name="ProvinceCode"></param>
        /// <returns></returns>
        public List<City> GetCities(string ProvinceCode)
        {
            try
            {
                List<City> dataCitys = _DBContext.City
                                       .Where(d => d.ProvinceCode.Equals(long.Parse(ProvinceCode)) && d.DeletedFlag.Equals(false))
                                       .OrderBy(d => d.CityCode).ToList();
                return dataCitys;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return new List<City>();
            }
        }

        /// <summary>
        /// 获取城市数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetCityCount()
        {
            try
            {
                return _DBContext.City.Where(d => d.DeletedFlag.Equals(false)).Count();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// 获取区域数据
        /// </summary>
        /// <param name="CityCode"></param>
        /// <returns></returns>
        public List<District> GetDistricts(string CityCode)
        {
            try
            {
                List<District> dataDistricts = _DBContext.District
                                               .Where(d => d.CityCode.Equals(long.Parse(CityCode)) && d.DeletedFlag.Equals(false))
                                               .OrderBy(d => d.DistrictCode).ToList();
                return dataDistricts;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return new List<District>();
            }
        }

        /// <summary>
        /// 获取区域数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetDistrictCount()
        {
            try
            {
                return _DBContext.District.Where(d => d.DeletedFlag.Equals(false)).Count();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// 获取街道/镇数据
        /// </summary>
        /// <param name="DistrictCode"></param>
        /// <returns></returns>
        public List<Street> GetStreets(string DistrictCode)
        {
            try
            {
                List<Street> dataStreets = _DBContext.Street
                                           .Where(d => d.DistrictCode.Equals(long.Parse(DistrictCode)) && d.DeletedFlag.Equals(false))
                                           .OrderBy(d => d.StreetCode).ToList();
                return dataStreets;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return new List<Street>();
            }
        }

        /// <summary>
        /// 获取街道/镇数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetStreetCount()
        {
            try
            {
                return _DBContext.Street.Where(d => d.DeletedFlag.Equals(false)).Count();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// 获取社区数据
        /// </summary>
        /// <param name="StreetCode"></param>
        /// <returns></returns>
        public List<Community> GetCommunities(string StreetCode)
        {
            try
            {
                List<Community> dataCommunitys = _DBContext.Community
                                                 .Where(d => d.StreetCode.Equals(long.Parse(StreetCode)) && d.DeletedFlag.Equals(false))
                                                 .OrderBy(d => d.CommunityCode).ToList();
                return dataCommunitys;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return new List<Community>();
            }
        }

        /// <summary>
        /// 获取社区数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetCommunityCount()
        {
            try
            {
                return _DBContext.Community.Where(d => d.DeletedFlag.Equals(false)).Count();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// 写入省份数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertProvinceData(Province data)
        {
            try
            {
                var exsitCount = _DBContext.Province.Where(d => d.ProvinceCode.Equals(data.ProvinceCode)
                                                             && d.DeletedFlag.Equals(false)).Count();
                if (exsitCount == 0)
                {
                    data.DeletedFlag = false;
                    data.CreatedTime = DateTime.Now;
                    data.ModifiedTime = (DateTime?)DateTime.Now;
                    _DBContext.Province.Add(data);
                    _DBContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 写入城市数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertCityData(City data)
        {
            try
            {
                var exsitCount = _DBContext.City.Where(d => d.CityCode.Equals(data.CityCode)
                                                         && d.ProvinceCode.Equals(data.ProvinceCode)
                                                         && d.DeletedFlag.Equals(false)).Count();
                if (exsitCount == 0)
                {
                    data.DeletedFlag = false;
                    data.CreatedTime = DateTime.Now;
                    data.ModifiedTime = (DateTime?)DateTime.Now;
                    _DBContext.City.Add(data);
                    _DBContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 写入区域数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertDistrictData(District data)
        {
            try
            {
                var exsitCount = _DBContext.District.Where(d => d.DistrictCode.Equals(data.DistrictCode)
                                                             && d.CityCode.Equals(data.CityCode)
                                                             && d.DeletedFlag.Equals(false)).Count();
                if (exsitCount == 0)
                {
                    data.DeletedFlag = false;
                    data.CreatedTime = DateTime.Now;
                    data.ModifiedTime = (DateTime?)DateTime.Now;
                    _DBContext.District.Add(data);
                    _DBContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 写入街道/镇数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertStreetData(Street data)
        {
            try
            {
                var exsitCount = _DBContext.Street.Where(d => d.StreetCode.Equals(data.StreetCode)
                                                           && d.DistrictCode.Equals(data.DistrictCode)
                                                           && d.DeletedFlag.Equals(false)).Count();
                if (exsitCount == 0)
                {
                    data.DeletedFlag = false;
                    data.CreatedTime = DateTime.Now;
                    data.ModifiedTime = (DateTime?)DateTime.Now;
                    _DBContext.Street.Add(data);
                    _DBContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 写入社区数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertCommunityData(Community data)
        {
            try
            {
                var exsitCount = _DBContext.Community.Where(d => d.CommunityCode.Equals(data.CommunityCode)
                                                              && d.StreetCode.Equals(data.StreetCode)
                                                              && d.DeletedFlag.Equals(false)).Count();
                if (exsitCount == 0)
                {
                    data.DeletedFlag = false;
                    data.CreatedTime = DateTime.Now;
                    data.ModifiedTime = (DateTime?)DateTime.Now;
                    _DBContext.Community.Add(data);
                    _DBContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                return false;
            }
        }

    }
}
