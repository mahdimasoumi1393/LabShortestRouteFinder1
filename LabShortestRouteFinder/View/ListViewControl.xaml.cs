using LabShortestRouteFinder.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LabShortestRouteFinder.View
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListViewControl : UserControl
    {
        public ListViewControl()
        {
            InitializeComponent();

            //// Set DataContext to RouteViewModel if not done in XAML
            //if (DataContext == null)
            //{
            //    DataContext = new RouteViewModel();
            //}
        }
        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var dataGrid = sender as DataGrid;

            if (e.EditAction == DataGridEditAction.Commit)
            {
                // Temporarily detach event handler to avoid recursion
                dataGrid.RowEditEnding -= DataGrid_RowEditEnding;

                try
                {
                    // Commit the edit to update the bound source
                    dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                }
                finally
                {
                    // Reattach the event handler
                    dataGrid.RowEditEnding += DataGrid_RowEditEnding;
                }
            }
        }

    }
}
