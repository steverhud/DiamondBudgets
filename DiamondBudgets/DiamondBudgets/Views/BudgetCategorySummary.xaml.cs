using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OxyPlot;
using OxyPlot.Series;
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
            var plotModel = new PlotModel
            {
                IsLegendVisible = true,
                LegendBackground = OxyColor.FromRgb(125, 125, 125),
            };

            var ps = new PieSeries
            {
                FontSize = 20,
                StrokeThickness = 0,
                InsideLabelPosition = .45,
                AngleSpan = 360,
                StartAngle = 270,
                Diameter = .85,
                InnerDiameter = .35,
                AreInsideLabelsAngled = true,
            };

            // http://www.nationsonline.org/oneworld/world_population.htm
            // http://en.wikipedia.org/wiki/Continent
            //ps.Slices.Add(new PieSlice("Africa", 1030));
            //ps.Slices.Add(new PieSlice("Americas", 929) { IsExploded = true });
            //ps.Slices.Add(new PieSlice("Asia", 4157));
            //ps.Slices.Add(new PieSlice("Europe", 739));
            //ps.Slices.Add(new PieSlice("Oceania", 35));

            IEnumerable<BudgetCategory> budgetCategories = await bcManager.GetBudgeCategorysAsync();

            foreach (BudgetCategory budget in budgetCategories)
                ps.Slices.Add(new PieSlice(budget.Category, (double)budget.Amount));

            var myController = new PlotController();

            plotModel.Series.Add(ps);
            pieChart.Model = plotModel;
            pieChart.Controller = myController;

        }

    }
}
