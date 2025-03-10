using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScannerApp1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void TreeView_Loaded(object sender, RoutedEventArgs e)
    {
        string path = @"E:\Mi_scanner";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        TreeView? treeView = sender as TreeView;
        if (treeView != null)
        {
            TreeViewItem rootItem = new TreeViewItem { Header = "DIRECTORIO PRINCIPAL" };
            foreach (var directory in Directory.GetDirectories(path))
            {
                TreeViewItem item = new TreeViewItem { Header = System.IO.Path.GetFileName(directory) };
                rootItem.Items.Add(item);
            }
            treeView.Items.Add(rootItem);
        }
    }

    private void TreeView_RightClick(object sender, MouseButtonEventArgs e)
    {
        if (treeView.SelectedItem is TreeViewItem selectedItem)
        {
            if (selectedItem.Header.ToString() == "DIRECTORIO PRINCIPAL")
            {
                ContextMenu menu = (ContextMenu)FindResource("DirectoryContextMenu");
                menu.PlacementTarget = treeView;
                menu.IsOpen = true;
            }
        }
    }

    private void AgregarNuevoDirectorio_Click(object sender, RoutedEventArgs e)
    {
        if (treeView.SelectedItem is TreeViewItem selectedItem && selectedItem.Header.ToString() == "DIRECTORIO PRINCIPAL")
        {
            string path = @"E:\Mi_scanner";
            string nuevoDirectorio = "NuevoDirectorio_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

            string fullPath = System.IO.Path.Combine(path, nuevoDirectorio);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            // Agregar el nuevo directorio al TreeView
            TreeViewItem nuevoItem = new TreeViewItem { Header = nuevoDirectorio };
            selectedItem.Items.Add(nuevoItem);
            selectedItem.IsExpanded = true;
        }
    }

}
