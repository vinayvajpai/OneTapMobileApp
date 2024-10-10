using OneTapMobile.Models;
using OneTapMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microcharts;
using Microcharts.Forms;
using Entry = Microcharts.ChartEntry;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using OneTapMobile.Global;
using System.ComponentModel;

namespace OneTapMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CampaignOverviewView : ContentPage
    {
        CampaignOverviewViewModel m_viewmodel;
        public CampaignOverviewView(Campaign Selecteditem)
        {
            try
            {
                InitializeComponent();
                BindingContext = m_viewmodel = new CampaignOverviewViewModel(Selecteditem);
                m_viewmodel.IsBusy = false;
                string HTMLPageContent = Helper.ReadHtmlFileContent("LineChart.HTML");
                var browser = new WebView();
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = HTMLPageContent;
                ChartWebView.Source = htmlSource;
                m_viewmodel.PropertyChanged += VM_propertyChanged;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void VM_propertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ChartDataList")
            {
                PlotChart();
            }
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                IsBusy = false;
                if (m_viewmodel != null)
                {
                    m_viewmodel.FirstTimeCalled = true;
                    m_viewmodel.IsTap = false;
                    m_viewmodel.nav = this.Navigation;
                    _ = m_viewmodel.GetCampData();
                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void PlotChart()
        {
            if (m_viewmodel.ChartDataList.Count != 0)
            {
                m_viewmodel.NoGrpah = false;
                m_viewmodel.ShowGraph = true;

                string FormattedString = "ddd";

                if (m_viewmodel.SelectedReport_type == "week")
                {
                    FormattedString = "dd/MM";
                }
                else if (m_viewmodel.SelectedReport_type == "month")
                {
                    FormattedString = "MMM/yy";
                }
                else
                {
                    FormattedString = "yyyy";
                }

                List<string> CLicks = m_viewmodel.ChartDataList.Select(c => c.clicks).ToList();
                List<string> Labels = m_viewmodel.ChartDataList.Select(c => c.label.ToString(FormattedString)).ToList();
                var ClicksArray = "['" + string.Join("','", CLicks) + "']";
                var LabelsArray = "['" + string.Join("','", Labels) + "']";

                string ShowChart = string.Empty;

                // string ShowChart = "var dom = document.getElementById('container');var myChart = echarts.init(dom, null, {renderer: 'canvas',useDirtyRect: false}); var app = {};var option;option = { xAxis: {type: 'category',boundaryGap: false,data:"+ LabelsArray + "},yAxis: {type: 'value'},series: [{data: "+ ClicksArray + ",type: 'line',smooth: true}]};if (option && typeof option === 'object') {myChart.setOption(option);}window.addEventListener('resize', myChart.resize);";
                if (Device.RuntimePlatform == Device.Android)
                    ShowChart = "var dom = document.getElementById('container');var myChart = echarts.init(dom, null, {renderer: 'canvas',useDirtyRect: false});var app = {};var option;option = {color: ['#8826C7'],title: {text: 'Clicks'},tooltip: {trigger: 'axis',axisPointer: {type: 'cross',label: {backgroundColor: '#6a7985'}}},toolbox: {},grid: {left: '3%',right: '4%',bottom: '3%',containLabel: true},xAxis: [{type: 'category',boundaryGap: false,data:" + LabelsArray + "}],yAxis: [{type: 'value'}],series: [{name: 'Clicks',type: 'line',stack: 'Total',smooth: true,lineStyle: {width: 2},showSymbol: true,areaStyle: {pacity: 0.9,color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{offset: 0,color: 'rgb(136, 38, 199)'},{offset: 1,color: 'rgb(255, 245, 221)'}])},emphasis: {focus: 'series'},data:" + ClicksArray + "}]};if (option && typeof option === 'object') {myChart.setOption(option);}";
                else
                    ShowChart = "var dom = document.getElementById('container');var myChart = echarts.init(dom, null, {renderer: 'canvas',useDirtyRect: false});var app = {};var option;option = {color: ['#8826C7'],title: {text: 'Clicks'},tooltip: {trigger: 'axis',axisPointer: {type: 'cross',label: {backgroundColor: '#6a7985'}}},toolbox: {},grid: {left: '3%',right: '4%',bottom: '3%',containLabel: true},xAxis: [{type: 'category',boundaryGap: false,data:" + LabelsArray + ",axisLabel: { fontSize: 15}}],yAxis: [{type: 'value', axisLabel: { fontSize: 15}}],series: [{name: 'Clicks',type: 'line',stack: 'Total',smooth: true,lineStyle: {width: 2},showSymbol: true,areaStyle: {pacity: 0.9,color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{offset: 0,color: 'rgb(136, 38, 199)'},{offset: 1,color: 'rgb(255, 245, 221)'}])},emphasis: {focus: 'series'},data:" + ClicksArray + "}]};if (option && typeof option === 'object') {myChart.setOption(option);}window.addEventListener('resize', myChart.resize);";

                        ChartWebView.EvaluateJavaScriptAsync(ShowChart);
            }
            else
            {
                m_viewmodel.NoGrpah = true;
                m_viewmodel.ChartHeight = 20;
                m_viewmodel.ShowGraph = false;
            }
        }

        private void CampstatusSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (m_viewmodel.CampToggedCount != 1 && !m_viewmodel.FirstTimeCalled)
            {
                m_viewmodel.ChangeCampstatusCmd();
            }
            else
            {
                if (m_viewmodel.CampToggedCount == 2 &&  m_viewmodel.FirstTimeCalled)
                {
                    m_viewmodel.ChangeCampstatusCmd();
                }

                m_viewmodel.CampToggedCount ++ ;
                m_viewmodel.FirstTimeCalled = false;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            m_viewmodel.CampToggedCount = 0;
        }

    }
}