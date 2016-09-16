using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using OxyPlot;
//using OxyPlot.Series;
using Xamarin.Forms;

namespace DiamondBudgets
{
    public partial class BudgetCategorySummary : ContentPage
    {
        BudgetCategoryManager bcManager;
        public BudgetCategorySummary()
        {
            InitializeComponent();

            bcManager = BudgetCategoryManager.DefaultManager;
        }
        async protected override void OnAppearing()
        {
           // var plotModel = new PlotModel
           // {
           //     IsLegendVisible = true,
           //     LegendBackground = OxyColor.FromRgb(125, 125, 125),
           // };

           // var ps = new PieSeries
           // {
           //     FontSize = 20,
           //     StrokeThickness = 0,
           //     InsideLabelPosition = .45,
           //     AngleSpan = 360,
           //     StartAngle = 270,
           //     Diameter = .85,
           //     InnerDiameter = .35,
           //     AreInsideLabelsAngled = true,
           // };

           // // http://www.nationsonline.org/oneworld/world_population.htm
           // // http://en.wikipedia.org/wiki/Continent
           // //ps.Slices.Add(new PieSlice("Africa", 1030));
           // //ps.Slices.Add(new PieSlice("Americas", 929) { IsExploded = true });
           // //ps.Slices.Add(new PieSlice("Asia", 4157));
           // //ps.Slices.Add(new PieSlice("Europe", 739));
           // //ps.Slices.Add(new PieSlice("Oceania", 35));

           // IEnumerable<BudgetCategory> budgetCategories = await bcManager.GetBudgeCategorysAsync(entityType:"Budget");

           // foreach (BudgetCategory budget in budgetCategories)
           //     ps.Slices.Add(new PieSlice(budget.Category, (double)budget.Amount));

           // var myController = new PlotController();

           // //TwoColorAreaSeries example
           // var s1 = new TwoColorAreaSeries


           //{
           //     Color = OxyColors.Tomato,
           //     Color2 = OxyColors.LightBlue,
           //     MarkerFill = OxyColors.Tomato,
           //     MarkerFill2 = OxyColors.LightBlue,
           //     StrokeThickness = 2,
           //     MarkerType = MarkerType.Circle,
           //     MarkerSize = 3,
           // };

           // s1.Points.AddRange(new[] { new DataPoint(0, 3), new DataPoint(1, 5), new DataPoint(2, 1), new DataPoint(3, 0), new DataPoint(4, 3) });
           // //s1.Points2.AddRange(new[] { new DataPoint(0, -3), new DataPoint(1, -1), new DataPoint(2, 0), new DataPoint(3, -6), new DataPoint(4, -4) });

           // plotModel.Series.Add(s1);
           // //plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
           // //plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });


           // //plotModel.Series.Add(ps);
           // pieChart.Model = plotModel;
           // pieChart.Controller = myController;

        }

    }
}
