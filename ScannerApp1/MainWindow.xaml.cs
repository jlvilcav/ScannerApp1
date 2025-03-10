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

    //private void TreeView_Loaded(object sender, RoutedEventArgs e)
    //{
    //    string path = @"E:\Mi_scanner";
    //    if (!Directory.Exists(path))
    //    {
    //        Directory.CreateDirectory(path);
    //    }

    //    TreeView? treeView = sender as TreeView;
    //    if (treeView != null)
    //    {
    //        TreeViewItem rootItem = new TreeViewItem { Header = "DIRECTORIO PRINCIPAL" };
    //        foreach (var directory in Directory.GetDirectories(path))
    //        {
    //            TreeViewItem item = new TreeViewItem { Header = System.IO.Path.GetFileName(directory), ContextMenu = (ContextMenu)FindResource("DirectoryContextMenu") };
    //            // Cargar las subcarpetas dentro del directorio raíz

    //            rootItem.Items.Add(item);
    //        }

    //        treeView.Items.Add(rootItem);
    //    }
    //}

    private void TreeView_Loaded(object sender, RoutedEventArgs e)
    {
        string path = @"E:\Mi_scanner\DIRECTORIO PRINCIPAL";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        TreeView? treeView = sender as TreeView;
        if (treeView != null)
        {
            // Crear el nodo raíz
            TreeViewItem rootItem = new TreeViewItem { Header = "DIRECTORIO PRINCIPAL", ContextMenu = (ContextMenu)FindResource("DirectoryContextMenu") };

            // Cargar las subcarpetas dentro del directorio raíz
            CargarSubCarpetas(path, rootItem);

            // Agregar la raíz al TreeView
            treeView.Items.Clear();
            treeView.Items.Add(rootItem);
        }
    }

    // Método recursivo para cargar subcarpetas
    private void CargarSubCarpetas(string ruta, TreeViewItem parentItem)
    {
        try
        {
            foreach (var directory in Directory.GetDirectories(ruta))
            {
                TreeViewItem newItem = new TreeViewItem
                {
                    Header = System.IO.Path.GetFileName(directory),
                    ContextMenu = (ContextMenu)FindResource("DirectoryContextMenu")
                };

                // Llamada recursiva para agregar subcarpetas
                CargarSubCarpetas(directory, newItem);

                parentItem.Items.Add(newItem);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar subcarpetas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
        if (treeView.SelectedItem is TreeViewItem selectedItem)
        {
            string parentPath = GetFullPath(selectedItem);

            // Solicitar nombre al usuario
            string nombreDirectorio = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el nombre del nuevo directorio:", "Nuevo Directorio", "NombreDirectorio");

            // Validar que el usuario ingresó un nombre válido
            if (string.IsNullOrWhiteSpace(nombreDirectorio))
            {
                MessageBox.Show("Debe ingresar un nombre válido para el directorio.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //string path = @"E:\Mi_scanner";
            //string fullPath = System.IO.Path.Combine(path, nombreDirectorio);
            string fullPath = System.IO.Path.Combine(parentPath, nombreDirectorio);

            if (Directory.Exists(fullPath))
            {
                MessageBox.Show("El directorio ya existe. Ingrese un nombre diferente.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Crear el nuevo directorio
            Directory.CreateDirectory(fullPath);

            // Agregar el nuevo directorio al TreeView
            TreeViewItem nuevoItem = new TreeViewItem { Header = nombreDirectorio, ContextMenu = (ContextMenu)FindResource("DirectoryContextMenu") };
            selectedItem.Items.Add(nuevoItem);
            selectedItem.IsExpanded = true;
        }


    }

    // Método para obtener la ruta completa de un TreeViewItem
    private string GetFullPath(TreeViewItem item)
    {
        string basePath = @"E:\Mi_scanner";
        Stack<string> pathStack = new Stack<string>();

        while (item != null)
        {
            if (item.Header != null)
            {
                pathStack.Push(item.Header.ToString());
            }
            item = item.Parent as TreeViewItem;
        }

        return System.IO.Path.Combine(basePath, System.IO.Path.Combine(pathStack.ToArray()));
    }

}
