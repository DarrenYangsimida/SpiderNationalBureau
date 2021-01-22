using Microsoft.Extensions.DependencyInjection;
using SpiderNationalBureau.DataModel_MySql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using SpiderNationalBureau.BLL;
using SpiderNationalBureau.Repository;
using System.Threading;

namespace SpiderNationalBureau
{
    public partial class Spider : Form
    {
        private readonly static CancellationTokenSource tokenSource = new CancellationTokenSource();
        private readonly CancellationToken token = tokenSource.Token;
        private readonly ManualResetEvent resetEvent = new ManualResetEvent(true);
        private readonly BaseRepository repository;
        private readonly string NationalBureauUrlPrefix = "http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2020/";

        public Spider(ServiceProvider serviceProvider)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            dataGridView1.AutoGenerateColumns = false;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView3.AutoGenerateColumns = false;
            dataGridView4.AutoGenerateColumns = false;
            dataGridView5.AutoGenerateColumns = false;

            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
            dataGridView2.CellClick += new DataGridViewCellEventHandler(dataGridView2_CellClick);
            dataGridView3.CellClick += new DataGridViewCellEventHandler(dataGridView3_CellClick);
            dataGridView4.CellClick += new DataGridViewCellEventHandler(dataGridView4_CellClick);

            repository = new BaseRepository(serviceProvider.GetService<SpiderDbContext>());
        }

        #region 抓取数据

        /// <summary>
        /// 开始抓取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            label6.Text = "正在抓取数据，请等待操作完成......";
            var task = Task.Run(() =>
            {
                GetProvinceData();
            });
            _ = Task.Run(() =>
            {
                //等待执行完成
                Task.WaitAll(task);
                if (task.IsCompleted)
                {
                    MessageBox.Show("数据抓取完成，请点击显示\"抓取数据\"查看");
                    button1.Enabled = true;
                    button2.Enabled = true;
                    label6.Text = "抓取状态......";
                }
            });
        }

        /// <summary>
        /// 抓取省份信息
        /// </summary>
        private void GetProvinceData()
        {
            try
            {
                var provinceUrl = $"{NationalBureauUrlPrefix}index.html";
                var html = CommonBLL.GetHtmlByUrl(provinceUrl);
                if (!string.IsNullOrWhiteSpace(html))
                {
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);
                    HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//tr[@class='provincetr']");
                    if (htmlNodes != null)
                    {
                        foreach (var item in htmlNodes)
                        {
                            var aNodes = item.SelectNodes("//a");
                            if (aNodes != null)
                            {
                                foreach (var a in aNodes)
                                {
                                    var provinceData = new Province();
                                    var cityUrl = a.Attributes["href"].Value.ToString();
                                    var provinceCodeOrg = cityUrl.Substring(0, 2);
                                    if (CommonBLL.IsNumber(provinceCodeOrg))
                                    {
                                        provinceData.ProvinceCode = long.Parse(provinceCodeOrg.AddedDigitsRight(12));
                                        provinceData.ProvinceName = CommonBLL.GetChineseWord(a.InnerText);
                                        var result = repository.InsertProvinceData(provinceData);
                                        if (result)
                                        {
                                            GetCityData(provinceUrl.GetUrlPrefix(cityUrl), provinceData.ProvinceCode);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"抓取省份数据出错！Error Message: {ex.Message}");
            }
        }

        /// <summary>
        /// 抓取城市信息
        /// </summary>
        /// <param name="cityUrl"></param>
        /// <param name="provinceCode"></param>
        private void GetCityData(string cityUrl, long provinceCode)
        {
            try
            {
                var html = CommonBLL.GetHtmlByUrl(cityUrl);
                if (!string.IsNullOrWhiteSpace(html))
                {
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);
                    HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//tr[@class='citytr']");
                    if (htmlNodes != null)
                    {
                        foreach (var item in htmlNodes)
                        {
                            var aNodes = item.ChildNodes;
                            if (aNodes != null && aNodes.Count >= 2)
                            {
                                var districtUrl = aNodes[0].FirstChild.Attributes["href"].Value;
                                if (!string.IsNullOrWhiteSpace(districtUrl))
                                {
                                    var cityData = new City
                                    {
                                        CityCode = long.Parse(CommonBLL.GetCodeNum(aNodes[0].FirstChild.InnerText)),
                                        CityName = CommonBLL.GetChineseWord(aNodes[1].FirstChild.InnerText),
                                        ProvinceCode = provinceCode
                                    };
                                    var result = repository.InsertCityData(cityData);
                                    if (result)
                                    {
                                        GetDistrictData(cityUrl.GetUrlPrefix(districtUrl), cityData.CityCode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"抓取城市数据出错！Error Message: {ex.Message}");
            }
        }

        /// <summary>
        /// 抓取区域信息
        /// </summary>
        /// <param name="districtUrl"></param>
        /// <param name="cityCode"></param>
        private void GetDistrictData(string districtUrl, long cityCode)
        {
            try
            {
                var html = CommonBLL.GetHtmlByUrl(districtUrl);
                if (!string.IsNullOrWhiteSpace(html))
                {
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);
                    HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//tr[@class='countytr']");
                    if (htmlNodes != null)
                    {
                        foreach (var item in htmlNodes)
                        {
                            var aNodes = item.ChildNodes;
                            if (aNodes != null && aNodes.Count >= 2)
                            {
                                if (aNodes[0].ChildNodes == null || !aNodes[0].FirstChild.Name.Equals("a"))
                                {
                                    var districtData = new District
                                    {
                                        DistrictCode = long.Parse(CommonBLL.GetCodeNum(aNodes[0].InnerText)),
                                        DistrictName = CommonBLL.GetChineseWord(aNodes[1].InnerText),
                                        CityCode = cityCode
                                    };
                                    _ = repository.InsertDistrictData(districtData);
                                }
                                else
                                {
                                    var streetUrl = aNodes[0].FirstChild.Attributes["href"].Value;
                                    if (!string.IsNullOrWhiteSpace(streetUrl))
                                    {
                                        var districtData = new District
                                        {
                                            DistrictCode = long.Parse(CommonBLL.GetCodeNum(aNodes[0].FirstChild.InnerText)),
                                            DistrictName = CommonBLL.GetChineseWord(aNodes[1].FirstChild.InnerText),
                                            CityCode = cityCode
                                        };
                                        var result = repository.InsertDistrictData(districtData);
                                        if (result)
                                        {
                                            GetStreetData(districtUrl.GetUrlPrefix(streetUrl), districtData.DistrictCode);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"抓取区域数据出错！Error Message: {ex.Message}");
            }
        }

        /// <summary>
        /// 抓取街道/镇信息
        /// </summary>
        /// <param name="streetUrl"></param>
        /// <param name="districtCode"></param>
        private void GetStreetData(string streetUrl, long districtCode)
        {
            try
            {
                var html = CommonBLL.GetHtmlByUrl(streetUrl);
                if (!string.IsNullOrWhiteSpace(html))
                {
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);
                    HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//tr[@class='towntr']");
                    if (htmlNodes != null)
                    {
                        foreach (var item in htmlNodes)
                        {
                            var aNodes = item.ChildNodes;
                            if (aNodes != null && aNodes.Count >= 2)
                            {
                                if (aNodes[0].ChildNodes == null || !aNodes[0].FirstChild.Name.Equals("a"))
                                {
                                    var streetData = new Street
                                    {
                                        StreetCode = long.Parse(CommonBLL.GetCodeNum(aNodes[0].InnerText)),
                                        StreetName = CommonBLL.GetChineseWord(aNodes[1].InnerText),
                                        DistrictCode = districtCode
                                    };
                                    _ = repository.InsertStreetData(streetData);
                                }
                                else
                                {
                                    var communityUrl = aNodes[0].FirstChild.Attributes["href"].Value;
                                    if (!string.IsNullOrWhiteSpace(communityUrl))
                                    {
                                        var streetData = new Street
                                        {
                                            StreetCode = long.Parse(CommonBLL.GetCodeNum(aNodes[0].FirstChild.InnerText)),
                                            StreetName = CommonBLL.GetChineseWord(aNodes[1].FirstChild.InnerText),
                                            DistrictCode = districtCode
                                        };
                                        var result = repository.InsertStreetData(streetData);
                                        if (result)
                                        {
                                            GetCommunityData(streetUrl.GetUrlPrefix(communityUrl), streetData.StreetCode);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"抓取街道/镇数据出错！Error Message: {ex.Message}");
            }
        }

        /// <summary>
        /// 抓取社区信息
        /// </summary>
        /// <param name="communityUrl"></param>
        /// <param name="streetCode"></param>
        private void GetCommunityData(string communityUrl, long streetCode)
        {
            try
            {
                var html = CommonBLL.GetHtmlByUrl(communityUrl);
                if (!string.IsNullOrWhiteSpace(html))
                {
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);
                    HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//tr[@class='villagetr']");
                    if (htmlNodes != null)
                    {
                        foreach (var item in htmlNodes)
                        {
                            var tdNodes = item.ChildNodes;
                            if (tdNodes != null && tdNodes.Count >= 3)
                            {
                                var communityData = new Community
                                {
                                    CommunityCode = long.Parse(CommonBLL.GetCodeNum(tdNodes[0].InnerText)),
                                    CommunityName = CommonBLL.GetChineseWord(tdNodes[2].InnerText),
                                    StreetCode = streetCode
                                };
                                _ = repository.InsertCommunityData(communityData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"抓取社区数据出错！Error Message: {ex.Message}");
            }
        }

        #endregion

        #region 显示数据

        /// <summary>
        /// 显示抓取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            int provinceCount = repository.GetProvinceCount();
            int cityCount = repository.GetCityCount();
            int districtCount = repository.GetDistrictCount();
            int streetCount = repository.GetStreetCount();
            int communityCount = repository.GetCommunityCount();
            int allCount = provinceCount + cityCount + districtCount + streetCount + communityCount;
            label13.Text = allCount.ToString();
            label14.Text = provinceCount.ToString();
            label15.Text = cityCount.ToString();
            label16.Text = districtCount.ToString();
            label17.Text = streetCount.ToString();
            label18.Text = communityCount.ToString();

            var dataProvinces = repository.GetProvinces();
            BindingSource bs = new BindingSource
            {
                DataSource = CommonBLL.ListToDataTable(dataProvinces)
            };
            dataGridView1.DataSource = bs;
        }

        /// <summary>
        /// 省 - 点击事件 - 加载 city 数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.DataSource = null;
            dataGridView3.DataSource = null;
            dataGridView4.DataSource = null;
            dataGridView5.DataSource = null;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Selected)
                {
                    if (item.Cells[0].Value != null)
                    {
                        var selectedProvinceCode = item.Cells[0].Value.ToString();
                        if (CommonBLL.IsNumber(selectedProvinceCode))
                        {
                            var dataCitys = repository.GetCities(selectedProvinceCode);
                            BindingSource bs = new BindingSource
                            {
                                DataSource = CommonBLL.ListToDataTable(dataCitys)
                            };
                            dataGridView2.DataSource = bs;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 城市 - 点击事件 - 加载 district 数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView3.DataSource = null;
            dataGridView4.DataSource = null;
            dataGridView5.DataSource = null;
            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                if (item.Selected)
                {
                    if (item.Cells[0].Value != null)
                    {
                        var selectedCityCode = item.Cells[0].Value.ToString();
                        if (CommonBLL.IsNumber(selectedCityCode))
                        {
                            var dataDistricts = repository.GetDistricts(selectedCityCode);
                            BindingSource bs = new BindingSource
                            {
                                DataSource = CommonBLL.ListToDataTable(dataDistricts)
                            };
                            dataGridView3.DataSource = bs;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 区域 - 点击事件 - 加载 street 数据 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView4.DataSource = null;
            dataGridView5.DataSource = null;
            foreach (DataGridViewRow item in dataGridView3.Rows)
            {
                if (item.Selected)
                {
                    if (item.Cells[0].Value != null)
                    {
                        var selectedDistrictsCode = item.Cells[0].Value.ToString();
                        if (CommonBLL.IsNumber(selectedDistrictsCode))
                        {
                            var dataStreets = repository.GetStreets(selectedDistrictsCode);
                            BindingSource bs = new BindingSource
                            {
                                DataSource = CommonBLL.ListToDataTable(dataStreets)
                            };
                            dataGridView4.DataSource = bs;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 街道/镇 - 点击事件 - 加载 community 数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView5.DataSource = null;
            foreach (DataGridViewRow item in dataGridView4.Rows)
            {
                if (item.Selected)
                {
                    if (item.Cells[0].Value != null)
                    {
                        var selectedStreetCode = item.Cells[0].Value.ToString();
                        if (CommonBLL.IsNumber(selectedStreetCode))
                        {
                            var dataCommunitys = repository.GetCommunities(selectedStreetCode);
                            BindingSource bs = new BindingSource
                            {
                                DataSource = CommonBLL.ListToDataTable(dataCommunitys)
                            };
                            dataGridView5.DataSource = bs;
                        }
                    }
                }
            }
        }

        #endregion

    }
}
