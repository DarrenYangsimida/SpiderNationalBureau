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

        public bool InsertProvinceData(Province data)
        {
            try
            {
                var exsitCount = _DBContext.Province.Where(d => d.ProvinceCode.Equals(data.ProvinceCode) && d.DeletedFlag.Equals(false)).Count();
                if (exsitCount == 0)
                {
                    data.DeletedFlag = false;
                    data.CreatedTime = DateTime.Now;
                    data.ModifiedTime = DateTime.Now;
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
                    data.ModifiedTime = DateTime.Now;
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
                    data.ModifiedTime = DateTime.Now;
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
                    data.ModifiedTime = DateTime.Now;
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
                    data.ModifiedTime = DateTime.Now;
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
