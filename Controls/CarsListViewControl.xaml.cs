using System.Windows.Controls;
using PHILOBM.Models;

namespace PHILOBM.Controls;

public partial class CarsListViewControl : UserControl
{
    public CarsListViewControl()
    {
        InitializeComponent();
    }

    public void LoadCars(IEnumerable<Car> cars)
    {
        CarsListView.ItemsSource = cars;
    }
}
