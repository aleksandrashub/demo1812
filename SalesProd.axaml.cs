using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using shubenok1212.Models;
using System.Collections.Generic;
using System.Linq;

namespace shubenok1212;

public partial class SalesProd : Window
{
    public List<Sale> sales = new List<Sale>();


    public SalesProd()
    {
        InitializeComponent();
    }
    public SalesProd(Prod pr)
    {
        InitializeComponent();
        sales = Helper.context.Sales.Where(x => x.IdProd == pr.IdProd).Include(x => x.IdProdNavigation).ToList();


    }


    private void nazad_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}